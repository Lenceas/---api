namespace MorningStar.Service
{
    /// <summary>
    /// 测试实现类
    /// </summary>
    public class TestService : SqlSugarRepository<TestEntity>, ITestService
    {
        public TestService()
        {

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitDatas()
        {

        }
    }
}
