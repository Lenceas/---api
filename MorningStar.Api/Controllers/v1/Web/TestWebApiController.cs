namespace MorningStar.Api
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    public class TestWebApiController : BaseApiController
    {
        private readonly ITestService _testService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TestWebApiController(ITestService testService)
        {
            _testService = testService;
        }

        /// <summary>
        /// 获取测试数据分页
        /// </summary>
        /// <param name="pageIndex">当前页,默认1</param>
        /// <param name="pageSize">页大小,默认10</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageViewModel<TestWebModel>), 200)]
        public async Task<IActionResult> GetPage(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                return ApiResult(await _testService.GetPage(pageIndex, pageSize));
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex.ToString());
            }
        }
    }
}