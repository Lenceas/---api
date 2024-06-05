namespace MorningStar.IService
{
    /// <summary>
    /// 测试Mongo接口类
    /// </summary>
    public interface ITestMongoService
    {
        #region 业务

        /// <summary>
        /// 初始化测试Mongo数据
        /// </summary>
        void InitDatas();

        /// <summary>
        /// 获取测试Mongo数据分页
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        Task<PageViewModel<TestMongoEntity>> GetPage(int pageIndex, int pageSize);

        /// <summary>
        /// 获取测试Mongo数据详情
        /// </summary>
        /// <param name="id">测试Mongo数据ID</param>
        /// <returns></returns>
        Task<TestMongoEntity> GetDetail(long id);

        /// <summary>
        /// 保存测试Mongo数据
        /// </summary>
        /// <param name="model">保存测试Mongo数据WebModel</param>
        /// <returns></returns>
        Task SaveTest(SaveTestMongoWebModel model);

        /// <summary>
        /// 删除测试Mongo数据
        /// </summary>
        /// <param name="id">测试Mongo数据ID</param>
        /// <returns></returns>
        Task DeleteTest(long id);

        #endregion 业务
    }
}