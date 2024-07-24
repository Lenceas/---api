namespace MorningStar.Api
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    [CustomRoute(ApiVersions.v1)]
    [AllowAnonymous]
    public class TestWebController : BaseApiController
    {
        #region 构造函数

        private readonly IMapper _mapper;
        private readonly IMemoryCache _mCache;
        private readonly IDistributedCache _dCache;
        private readonly ITestMySqlService _testMySqlService;
        private readonly ITestMongoService _testMongoService;

        public TestWebController(
            Serilog.ILogger log,
            IMapper mapper,
            IMemoryCache mCache,
            IDistributedCache dCache,
            ITestMySqlService testMySqlService,
            ITestMongoService testMongoService
        ) : base(log)
        {
            _mapper = mapper;
            _mCache = mCache;
            _dCache = dCache;
            _testMySqlService = testMySqlService;
            _testMongoService = testMongoService;
        }

        #endregion

        #region 缓存

        /// <summary>
        /// 获取Memory缓存数据（过期时间：2s）
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
                return ApiErrorResult(ex, "获取Memory缓存数据");
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
                return ApiErrorResult(ex, "获取Redis缓存数据");
            }
        }

        #endregion

        #region MySql

        /// <summary>
        /// 获取测试MySql数据分页
        /// </summary>
        /// <param name="pageIndex">当前页,默认1</param>
        /// <param name="pageSize">页大小,默认10</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageViewModel<TestMySqlPageWebModel>), 200)]
        public async Task<IActionResult> GetMySqlPage(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var r = await _testMySqlService.GetPage(pageIndex, pageSize);
                return ApiTResult(new PageViewModel<TestMySqlPageWebModel>()
                {
                    PageIndex = r.PageIndex,
                    PageSize = r.PageSize,
                    TotalCount = r.TotalCount,
                    ViewModelList = _mapper.Map<List<TestMySqlPageWebModel>>(r.ViewModelList)
                });
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex, "获取测试MySql数据分页");
            }
        }

        /// <summary>
        /// 获取测试MySql数据详情
        /// </summary>
        /// <param name="id">测试数据ID</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TestMySqlDetailWebModel), 200)]
        public async Task<IActionResult> GetMySqlDetail([Required] long id)
        {
            try
            {
                return ApiTResult(_mapper.Map<TestMySqlDetailWebModel>(await _testMySqlService.GetDetail(id)));
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex, "获取测试MySql数据详情");
            }
        }

        /// <summary>
        /// 保存测试MySql数据
        /// </summary>
        /// <param name="model">保存测试数据WebModel</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SaveMySqlTest([FromBody] SaveTestMySqlWebModel model)
        {
            try
            {
                await _testMySqlService.SaveTest(model);
                return ApiResult(true);
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex, "保存测试MySql数据");
            }
        }

        /// <summary>
        /// 删除测试MySql数据
        /// </summary>
        /// <param name="id">测试数据ID</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteMySqlTest([Required] long id)
        {
            try
            {
                await _testMySqlService.DeleteTest(id);
                return ApiResult(true);
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex, "删除测试MySql数据");
            }
        }

        #endregion

        #region Mongo

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
                return ApiErrorResult(ex, "获取测试Mongo数据分页");
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
                return ApiErrorResult(ex, "获取测试Mongo数据详情");
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
                return ApiErrorResult(ex, "保存测试Mongo数据");
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
                return ApiErrorResult(ex, "删除测试Mongo数据");
            }
        }

        #endregion
    }
}