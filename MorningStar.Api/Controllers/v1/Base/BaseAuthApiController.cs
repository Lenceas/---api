namespace MorningStar.Api
{
    /// <summary>
    /// 基础权限接口
    /// </summary>    
    public class BaseAuthApiController : BaseApiController
    {
        private readonly Serilog.ILogger _logger;
        private readonly IDistributedCache _dCache;
        private readonly IUserService _userService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dCache"></param>
        /// <param name="userService"></param>
        public BaseAuthApiController(Serilog.ILogger logger, IDistributedCache dCache, IUserService userService)
        {
            _logger = logger;
            _dCache = dCache;
            _userService = userService;
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
                _logger.Error(ex, "BaseAuth/Login");
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
                await _dCache.RemoveAsync($"{ConfigHelper.LoginRedisKey}{1024}");
                return ApiTResult(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "BaseAuth/Logout");
                return ApiErrorResult(ex.Message);
            }
        }
    }
}
