namespace MorningStar.Api
{
    /// <summary>
    /// 基础权限接口
    /// </summary>    
    /// <remarks>
    /// 构造函数
    /// </remarks>
    /// <param name="logger"></param>
    /// <param name="dCache"></param>
    /// <param name="userService"></param>
    public class BaseAuthController(
        Serilog.ILogger logger,
        IDistributedCache dCache,
        IUserService userService
        ) : BaseApiController
    {
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
                var (code, base64Code) = userService.GetLoginVerifyCode(4, 116, 46, 22);
                // 把唯一标识和验证码 缓存到内存并设置过期时间
                await dCache.SetStringAsync($"{ConfigHelper.LoginCaptchaRedisKey}{id}", code, new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromSeconds(60) });
                return ApiTResult(base64Code);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "BaseAuth/GetLoginVerifyCode");
                return ApiErrorResult(ex.Message);
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
                var r = await userService.Login(model);
                await dCache.SetStringAsync($"{ConfigHelper.LoginRedisKey}{r.UserID}", JsonConvert.SerializeObject(r),
                    new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(r.ExpiredMinuteTime)
                    });
                return ApiTResult(r);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "BaseAuth/Login");
                return ApiErrorResult(ex.Message);
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
                await dCache.RemoveAsync($"{ConfigHelper.LoginRedisKey}{1024}");
                return ApiTResult(true);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "BaseAuth/Logout");
                return ApiErrorResult(ex.Message);
            }
        }
    }
}
