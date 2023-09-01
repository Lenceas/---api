using MongoDB.Driver;
using MorningStar.Model;
using System.Linq.Expressions;

namespace MorningStar.Repository
{
    /// <summary>
    /// MongoDB 仓储实现类
    /// </summary>
    /// <typeparam name="T">泛型实体</typeparam>
    public class MongoRepository<T> : IMongoRepository<T>
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
            _collection = db.GetCollection<T>(collectionName.Replace("Entity", ""));
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
            await _collection.InsertOneAsync(entity);
        }

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entities">需批量新增的实体</param>
        /// <returns></returns>
        public async Task InsertRangeAsync(List<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="entity">需更新的实体</param>
        /// <returns></returns>
        public async Task UpdateAsync(long id, T entity)
        {
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
                var filter = Builders<T>.Filter.Eq("_id", idSelector(entity));
                bulkWrites.Add(new ReplaceOneModel<T>(filter, entity));
            }
            if (bulkWrites.Any())
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
            if (bulkWrites.Any())
            {
                // 用于指定在执行批量写入操作时，是否要按照指定的顺序执行这些操作（false：并行删除）
                var bulkWriteOptions = new BulkWriteOptions { IsOrdered = false };
                await _collection.BulkWriteAsync(bulkWrites, bulkWriteOptions);
            }
        }
    }
}
