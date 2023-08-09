namespace MorningStar.Api
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    public class TestWebApiController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ITestService _testService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TestWebApiController(IMapper mapper, ITestService testService)
        {
            _mapper = mapper;
            _testService = testService;
        }

        #region 公共

        #endregion

        #region 业务
        /// <summary>
        /// 获取测试数据分页
        /// </summary>
        /// <param name="pageIndex">当前页,默认1</param>
        /// <param name="pageSize">页大小,默认10</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageViewModel<TestPageWebModel>), 200)]
        public async Task<IActionResult> GetPage(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var r = await _testService.GetPage(pageIndex, pageSize);
                return ApiTResult(new PageViewModel<TestPageWebModel>()
                {
                    PageIndex = r.PageIndex,
                    PageSize = r.PageSize,
                    TotalCount = r.TotalCount,
                    ViewModelList = _mapper.Map<List<TestPageWebModel>>(r.ViewModelList)
                });
            }
            catch (Exception ex)
            {
                return ApiErrorTResult<PageViewModel<TestPageWebModel>>(ex.Message);
            }
        }

        /// <summary>
        /// 获取测试数据详情
        /// </summary>
        /// <param name="id">测试数据ID</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TestDetailWebModel), 200)]
        public async Task<IActionResult> GetDetail([Required] long id)
        {
            try
            {
                return ApiTResult(_mapper.Map<TestDetailWebModel>(await _testService.GetDetail(id)));
            }
            catch (Exception ex)
            {
                return ApiErrorTResult<TestDetailWebModel>(ex.Message);
            }
        }

        /// <summary>
        /// 保存测试数据
        /// </summary>
        /// <param name="model">保存测试数据WebModel</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SaveTest([FromBody] SaveTestWebModel model)
        {
            try
            {
                await _testService.SaveTest(model);
                return ApiResult(true);
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 删除测试数据
        /// </summary>
        /// <param name="id">测试数据ID</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteTest([Required] long id)
        {
            try
            {
                await _testService.DeleteTest(id);
                return ApiResult(true);
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex.Message);
            }
        }
        #endregion
    }
}