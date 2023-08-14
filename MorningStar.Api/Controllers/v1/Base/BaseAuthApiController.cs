namespace MorningStar.Api
{
    /// <summary>
    /// 基础权限接口
    /// </summary>    
    public class BaseAuthApiController : BaseApiController
    {
        private readonly Serilog.ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseAuthApiController(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">用户登录WebModel</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(TokenWebModel), 200)]
        public IActionResult Login([FromBody] LoginWebModel model)
        {
            try
            {
                if (!model.Account.Equals("admin") || !model.PassWord.Equals("admin888"))
                    throw new Exception("用户名或密码错误！");

                var jwtToken = JwtToken.GenerateJwtToken(99999, "Lenceas");

                return ApiTResult(new TokenWebModel()
                {
                    UserID = 99999,
                    UserName = "Lenceas",
                    Token = jwtToken,
                    ExpiredMinuteTime = Convert.ToInt32(AppSettings.Get("Jwt:ExpiryInMinutes"))
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "BaseAuth/Login");
                return ApiErrorResult(ex.Message);
            }
        }
    }
}
