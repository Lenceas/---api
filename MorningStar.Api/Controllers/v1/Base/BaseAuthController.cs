namespace MorningStar.Api
{
    /// <summary>
    /// 基础权限接口
    /// </summary>
    [CustomRoute(ApiVersions.v1)]
    public class BaseAuthController : BaseApiController
    {
        #region 构造函数

        private readonly IDistributedCache _dCache;
        private readonly IUserMongoService _userService;

        public BaseAuthController(
            Serilog.ILogger log,
            IDistributedCache dCache,
            IUserMongoService userService
        ) : base(log)
        {
            _dCache = dCache;
            _userService = userService;
        }

        #endregion

        #region 业务

        /// <summary>
        /// 获取登录图片验证码
        /// </summary>
        /// <param name="id">该次登录唯一标识</param>
        /// <returns>base64Code</returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetLoginVerifyCode([Required] string id)
        {
            try
            {
                var (code, base64Code) = _userService.GetLoginVerifyCode(4, 116, 46, 22);
                // 把唯一标识和验证码 缓存到内存并设置过期时间
                await _dCache.SetStringAsync($"{ConfigHelper.LoginCaptchaRedisKey}{id}", code, new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromSeconds(60) });
                return ApiTResult(base64Code);
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex, "获取登录图片验证码");
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">用户登录WebModel</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(TokenWebModel), 200)]
        public async Task<IActionResult> Login([FromBody] LoginWebModel model)
        {
            try
            {
                var r = await _userService.Login(model);
                await _dCache.SetStringAsync($"{ConfigHelper.LoginRedisKey}{r.UserID}", JsonConvert.SerializeObject(r),
                    new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(r.ExpiredMinuteTime)
                    });
                return ApiTResult(r);
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex, "用户登录");
            }
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // todo:从当前登录用户拿取用户ID
                await _dCache.RemoveAsync($"{ConfigHelper.LoginRedisKey}{1024}");
                return ApiTResult(true);
            }
            catch (Exception ex)
            {
                return ApiErrorResult(ex, "用户登出");
            }
        }

        #endregion
    }
}