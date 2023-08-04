namespace MorningStar.Api.Controllers.v1
{
    /// <summary>
    /// 基础权限接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    [Authorize]
    public class BaseAuthController : ControllerBase
    {
        public BaseAuthController()
        {

        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">用户登录WebModel</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ApiResult<TokenWebModel> Login(LoginWebModel model)
        {
            try
            {
                if (!model.Account.Equals("admin") || !model.PassWord.Equals("admin888"))
                    return new ApiResult<TokenWebModel>() { Errors = "用户名或密码错误！" };

                var jwtToken = JwtToken.GenerateJwtToken(99999, "Lenceas");

                return new ApiResult<TokenWebModel>()
                {
                    Success = true,
                    Data = new TokenWebModel()
                    {
                        UserID = 99999,
                        UserName = "Lenceas",
                        Token = jwtToken,
                        ExpiredMinuteTime = Convert.ToInt32(AppSettings.Get("Jwt:ExpiryInMinutes"))
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<TokenWebModel>()
                {
                    Code = 500,
                    Errors = ex.ToString()
                };
            }
        }
    }
}
