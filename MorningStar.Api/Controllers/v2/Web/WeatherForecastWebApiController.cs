namespace MorningStar.Api
{
    /// <summary>
    /// 天气预报接口
    /// </summary>
    [CustomRoute(ApiVersions.v2)]
    [AllowAnonymous]
    public class WeatherForecastWebApiController : BaseApiController
    {
        private readonly IMemoryCache _mCache;
        private readonly IDistributedCache _dCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WeatherForecastWebApiController(IMemoryCache memoryCache, IDistributedCache dCache)
        {
            _mCache = memoryCache;
            _dCache = dCache;
        }

        /// <summary>
        /// 获取天气预报数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<WeatherForecast>), 200)]
        public IActionResult Get()
        {
            try
            {
                string[] Summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };
                return ApiTResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray());
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex.Message);
            }
        }

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
                return ApiErrorResult(ex.Message);
            }
        }
    }
}