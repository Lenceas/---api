namespace MorningStar.Service
{
    /// <summary>
    /// 测试接口类
    /// </summary>
    public interface ITestService
    {
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
        Task<PageViewModel<TestWebModel>> GetPage(int pageIndex, int pageSize);
    }
}
