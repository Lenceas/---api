namespace MorningStar.Api
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    [AllowAnonymous]
    public class TestWebApiController : BaseApiController
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _mCache;
        private readonly IDistributedCache _dCache;
        private readonly ITestService _testService;
        private readonly ITestMongoService _testMongoService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TestWebApiController(Serilog.ILogger logger, IMapper mapper, IMemoryCache mCache, IDistributedCache dCache, ITestService testService, ITestMongoService testMongoService)
        {
            _logger = logger;
            _mapper = mapper;
            _mCache = mCache;
            _dCache = dCache;
            _testService = testService;
            _testMongoService = testMongoService;
        }

        #region 公共
        /// <summary>
        /// 获取内存缓存数据（过期时间：2s）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetMemoryCache()
        {
            try
            {
                return ApiResult(await _mCache.GetOrCreateAsync("MemoryCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2);
                    await Task.Delay(TimeSpan.FromSeconds(0));
                    return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }) ?? string.Empty);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "TestWeb/GetMemoryCache");
                return ApiErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取Redis缓存数据（过期时间：2s；滑动过期时间：2s）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetRedisCache()
        {
            try
            {
                var r = await _dCache.GetStringAsync("Redis");
                if (string.IsNullOrEmpty(r))
                {
                    r = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    await _dCache.SetStringAsync("Redis", r
                        , new DistributedCacheEntryOptions()
                        {
                            // 设置缓存项的绝对过期时间相对于当前时间的间隔
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2),
                            // 设置滑动过期时间：
                            // 1、滑动过期时间是指从最近一次访问缓存项开始的一段时间，在这段时间内如果缓存项没有被访问，它将过期。
                            // 2、如果在滑动过期时间内访问了缓存项，过期时间会被重置。
                            SlidingExpiration = TimeSpan.FromSeconds(2),
                        });
                }
                return ApiResult(r);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "TestWeb/GetRedisCache");
                return ApiErrorResult(ex.Message);
            }
        }
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
                _logger.Error(ex, "TestWeb/GetPage");
                return ApiErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取测试Mongo数据分页
        /// </summary>
        /// <param name="pageIndex">当前页,默认1</param>
        /// <param name="pageSize">页大小,默认10</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageViewModel<TestMongoPageWebModel>), 200)]
        public async Task<IActionResult> GetMongoPage(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var r = await _testMongoService.GetPage(pageIndex, pageSize);
                return ApiTResult(new PageViewModel<TestMongoPageWebModel>()
                {
                    PageIndex = r.PageIndex,
                    PageSize = r.PageSize,
                    TotalCount = r.TotalCount,
                    ViewModelList = _mapper.Map<List<TestMongoPageWebModel>>(r.ViewModelList)
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "TestWeb/GetMongoPage");
                return ApiErrorResult(ex.Message);
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
                _logger.Error(ex, "TestWeb/GetDetail");
                return ApiErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取测试Mongo数据详情
        /// </summary>
        /// <param name="id">测试Mongo数据ID</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TestMongoDetailWebModel), 200)]
        public async Task<IActionResult> GetMongoDetail([Required] long id)
        {
            try
            {
                return ApiTResult(_mapper.Map<TestMongoDetailWebModel>(await _testMongoService.GetDetail(id)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "TestWeb/GetMongoDetail");
                return ApiErrorResult(ex.Message);
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
                _logger.Error(ex, "TestWeb/SaveTest");
                return ApiErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 保存测试Mongo数据
        /// </summary>
        /// <param name="model">保存测试Mongo数据WebModel</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SaveMongoTest([FromBody] SaveTestMongoWebModel model)
        {
            try
            {
                await _testMongoService.SaveTest(model);
                return ApiResult(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "TestWeb/SaveMongoTest");
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
                _logger.Error(ex, "TestWeb/DeleteTest");
                return ApiErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 删除测试Mongo数据
        /// </summary>
        /// <param name="id">测试Mongo数据ID</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteMongoTest([Required] long id)
        {
            try
            {
                await _testMongoService.DeleteTest(id);
                return ApiResult(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "TestWeb/DeleteMongoTest");
                return ApiErrorResult(ex.Message);
            }
        }
        #endregion
    }
}