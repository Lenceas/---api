namespace MorningStar.IService
{
    /// <summary>
    /// 测试接口类
    /// </summary>
    public interface ITestService
    {
        #region 公共

        #endregion

        #region 业务
        /// <summary>
        /// 初始化测试数据
        /// </summary>
        void InitDatas();

        /// <summary>
        /// 获取测试数据分页
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        Task<PageViewModel<TestEntity>> GetPage(int pageIndex, int pageSize);

        /// <summary>
        /// 获取测试数据详情
        /// </summary>
        /// <param name="id">测试数据ID</param>
        /// <returns></returns>
        Task<TestEntity> GetDetail(long id);

        /// <summary>
        /// 保存测试数据
        /// </summary>
        /// <param name="model">保存测试数据WebModel</param>
        /// <returns></returns>
        Task SaveTest(SaveTestWebModel model);

        /// <summary>
        /// 删除测试数据
        /// </summary>
        /// <param name="id">测试数据ID</param>
        /// <returns></returns>
        Task DeleteTest(long id);
        #endregion
    }
}
