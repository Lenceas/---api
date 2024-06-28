using MongoDB.Bson.Serialization.Attributes;
using System.Reflection;

namespace MorningStar.Repository
{
    /// <summary>
    /// MongoDB 仓储实现类
    /// </summary>
    /// <typeparam name="T">泛型实体</typeparam>
    public class MongoRepository<T> : IMongoRepository<T> where T : class, new()
    {
        private readonly IMongoCollection<T> _collection;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collectionName">集合名称（表名称）</param>
        /// <param name="db">IMongoDatabase 操作实例</param>
        public MongoRepository(string collectionName, IMongoDatabase? db = null)
        {
            db ??= App.GetService<IMongoDatabase>();
            // 自动截取表名，保留前面的部分
            _collection = db.GetCollection<T>(collectionName.Replace("MongoEntity", ""));
        }

        /// <summary>
        /// 根据主键匹配
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(long id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
        {
            var query = _collection.Find(filter);
            return await query.SingleOrDefaultAsync();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter, SortDefinition<T>? orderBy = null)
        {
            var query = _collection.Find(filter);
            if (orderBy != null) query = query.Sort(orderBy);
            return await query.ToListAsync();
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public async Task<PageViewModel<T>> GetPageAsync(int pageIndex, int pageSize, SortDefinition<T>? orderBy = null)
        {
            var totalCount = await _collection.CountDocumentsAsync(_ => true);
            var query = _collection.Find(_ => true);
            if (orderBy != null) query = query.Sort(orderBy);
            var items = await query.Skip((pageIndex - 1) * pageSize)
                                   .Limit(pageSize)
                                   .ToListAsync();
            return new PageViewModel<T>
            {
                ViewModelList = items,
                TotalCount = (int)totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllListAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">需新增的实体</param>
        /// <returns></returns>
        public async Task InsertAsync(T entity)
        {
            var en = SortEntityProperties(entity);
            await _collection.InsertOneAsync(en);
        }

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entities">需批量新增的实体</param>
        /// <returns></returns>
        public async Task InsertRangeAsync(List<T> entities)
        {
            var list = SortEntityProperties(entities);
            await _collection.InsertManyAsync(list);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="entity">需更新的实体</param>
        /// <returns></returns>
        public async Task UpdateAsync(long id, T entity)
        {
            UpdateMTime(entity);
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">需批量更新的实体</param>
        /// <param name="idSelector">使用泛型约束和泛型委托，使用示例：_=>_.ID </param>
        /// <returns></returns>
        public async Task UpdateRangeAsync(List<T> entities, Func<T, long> idSelector)
        {
            var bulkWrites = new List<WriteModel<T>>();
            foreach (var entity in entities)
            {
                UpdateMTime(entity);
                var filter = Builders<T>.Filter.Eq("_id", idSelector(entity));
                bulkWrites.Add(new ReplaceOneModel<T>(filter, entity));
            }
            if (bulkWrites.Count != 0)
                await _collection.BulkWriteAsync(bulkWrites);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        public async Task DeleteRangeAsync(List<long> ids)
        {
            var bulkWrites = new List<WriteModel<T>>();
            foreach (var id in ids)
            {
                var filter = Builders<T>.Filter.Eq("_id", id);
                bulkWrites.Add(new DeleteOneModel<T>(filter));
            }
            if (bulkWrites.Count != 0)
            {
                // 用于指定在执行批量写入操作时，是否要按照指定的顺序执行这些操作（false：并行删除）
                var bulkWriteOptions = new BulkWriteOptions { IsOrdered = false };
                await _collection.BulkWriteAsync(bulkWrites, bulkWriteOptions);
            }
        }

        /// <summary>
        /// 更新 MTime 属性
        /// </summary>
        /// <param name="entity">需更新的实体</param>
        private static void UpdateMTime(T entity)
        {
            var mtimeProperty = entity.GetType().GetProperty(nameof(BaseMySqlEntity.MTime));
            if (mtimeProperty != null && mtimeProperty.PropertyType == typeof(DateTime))
                mtimeProperty.SetValue(entity, DateTime.UtcNow);
        }

        /// <summary>
        /// 自定义排序（解决实体基类BsonElement(Order = 1)不生效永远排在子类前面的问题）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        private static T SortEntityProperties(T entity)
        {
            var properties = typeof(T).GetProperties()
                .Select(prop => new
                {
                    PropertyInfo = prop,
                    Order = prop.GetCustomAttribute<BsonElementAttribute>()?.Order ?? 0
                })
                .OrderBy(prop => prop.Order)
                .ToList();
            T sortedEntity = Activator.CreateInstance<T>();
            foreach (var propInfo in properties)
                propInfo.PropertyInfo.SetValue(sortedEntity, propInfo.PropertyInfo.GetValue(entity));
            return sortedEntity;
        }

        /// <summary>
        /// 自定义排序（解决实体基类BsonElement(Order = 1)不生效永远排在子类前面的问题）
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns></returns>
        public static List<T> SortEntityProperties(List<T> entities)
        {
            var sortedEntities = new List<T>();
            foreach (var entity in entities)
            {
                var properties = typeof(T).GetProperties()
                    .Select(prop => new
                    {
                        PropertyInfo = prop,
                        Order = prop.GetCustomAttribute<BsonElementAttribute>()?.Order ?? 0
                    })
                    .OrderBy(prop => prop.Order)
                    .ToList();
                T sortedEntity = Activator.CreateInstance<T>();
                foreach (var propInfo in properties)
                    propInfo.PropertyInfo.SetValue(sortedEntity, propInfo.PropertyInfo.GetValue(entity));
                sortedEntities.Add(sortedEntity);
            }
            return sortedEntities;
        }
    }
}