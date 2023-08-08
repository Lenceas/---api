namespace MorningStar.Api.Controllers.v1
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    [Authorize]
    public class TestWebController : ControllerBase
    {
        private readonly ITestService _testService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TestWebController(ITestService testService)
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
        public async Task<ApiResult<PageViewModel<TestWebModel>>> GetPage(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                return new ApiResult<PageViewModel<TestWebModel>>()
                {
                    Code = 200,
                    Success = true,
                    Data = await _testService.GetPage(pageIndex, pageSize),
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<PageViewModel<TestWebModel>> { Errors = ex.ToString() };
            }
        }
    }
}