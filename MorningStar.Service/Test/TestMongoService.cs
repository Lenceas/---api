namespace MorningStar.Service
{
    /// <summary>
    /// 测试Mongo实现类
    /// </summary>
    public class TestMongoService : MongoRepository<TestMongoEntity>, ITestMongoService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TestMongoService() : base(nameof(TestMongoEntity))
        {

        }

        #region 公共

        #endregion

        #region 业务
        /// <summary>
        /// 初始化测试Mongo数据
        /// </summary>
        public void InitDatas()
        {
            var list = GetAllListAsync().GetAwaiter().GetResult();
            if (list.Count == 0)
            {
                for (int i = 1; i < 25; i++)
                    list.Add(new TestMongoEntity() { ID = i });
                if (list.Count != 0)
                    InsertRangeAsync(list).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// 获取测试Mongo数据分页
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public async Task<PageViewModel<TestMongoEntity>> GetPage(int pageIndex, int pageSize)
        {
            var sort = Builders<TestMongoEntity>.Sort.Ascending(_ => _.OrderIndex);
            return await GetPageAsync(pageIndex, pageSize, sort);
        }

        /// <summary>
        /// 获取测试Mongo数据详情
        /// </summary>
        /// <param name="id">测试数据ID</param>
        /// <returns></returns>
        public async Task<TestMongoEntity> GetDetail(long id)
        {
            return await GetByIdAsync(id) ?? throw new Exception("无效的测试Mongo数据ID！");
        }

        /// <summary>
        /// 保存测试Mongo数据
        /// </summary>
        /// <param name="model">保存测试Mongo数据WebModel</param>
        /// <returns></returns>
        public async Task SaveTest(SaveTestMongoWebModel model)
        {
            if (model.ID == 0)// 新增
            {
                var list = await GetAllListAsync();
                if (list.Count == 0)
                    await InsertAsync(new TestMongoEntity() { ID = 1 });
                else
                {
                    var maxID = list?.Max(_ => _.ID) ?? 0;
                    await InsertAsync(new TestMongoEntity() { ID = maxID + 1 });
                }
            }
            else// 编辑
            {
                _ = await GetByIdAsync(model.ID) ?? throw new Exception("无效的测试Mongo数据ID！");
                // todo:
            }
        }

        /// <summary>
        /// 删除测试数据
        /// </summary>
        /// <param name="id">测试数据ID</param>
        /// <returns></returns>
        public async Task DeleteTest(long id)
        {
            var en = await GetByIdAsync(id) ?? throw new Exception("无效的测试Mongo数据ID！");
            await DeleteAsync(en.ID);
        }
        #endregion
    }
}
