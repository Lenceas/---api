namespace MorningStar.Service
{
    /// <summary>
    /// 用户实现类
    /// </summary>
    public class UserMongoService : MongoRepository<UserMongoEntity>, IUserMongoService
    {
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserMongoService() : base(nameof(UserMongoEntity))
        {
            _cache = App.GetService<IDistributedCache>();
        }

        #region 业务

        /// <summary>
        /// 初始化用户数据
        /// </summary>
        public void InitDatas()
        {
            var en = GetSingleAsync(_ => _.ID == 1024).GetAwaiter().GetResult();
            if (en == null)
                InsertAsync(new UserMongoEntity() { ID = 1024, UserName = "Lenceas", Account = "admin", Password = CommonHelper.MD5Encrytp("admin888") }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获取登录图片验证码
        /// </summary>
        /// <param name="codeLength">验证码长度</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="fontSize">字体大小</param>
        /// <returns>返回验证码和base64格式验证码图片字符串</returns>
        public (string code, string base64Code) GetLoginVerifyCode(int codeLength, int width, int height, int fontSize)
        {
            var (verificationCode, bytes) = VerifyCodeGenerator.CreateValidateGraphic(codeLength, width, height, fontSize);
            return (verificationCode, string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bytes)));
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">用户登录WebModel</param>
        /// <returns></returns>
        public async Task<TokenWebModel> Login(LoginWebModel model)
        {
            // 根据该次登录唯一标识和验证码验证Redis缓存里的值是否一致
            var captcha = await _cache.GetStringAsync($"{ConfigHelper.LoginCaptchaRedisKey}{model.CaptchaID}");
            if (!string.IsNullOrEmpty(captcha) && captcha.Equals(model.Captcha, StringComparison.OrdinalIgnoreCase))
            {
                var user = await GetSingleAsync(_ => _.Account.Equals(model.Account) && _.Password.Equals(CommonHelper.MD5Encrytp(model.Password))) ?? throw new Exception("账号或密码错误！");
                var jwtToken = JwtToken.GenerateJwtToken(user.ID, user.UserName);
                user.LastLoginTime = DateTime.UtcNow;
                user.LastLoginIPv4 = App.GetRemoteIpAddressToIPv4();
                user.LastLoginToken = jwtToken;
                await UpdateAsync(user.ID, user);
                return new TokenWebModel
                {
                    UserID = user.ID,
                    UserName = user.UserName,
                    RoleID = user.RoleID,
                    // todo:待赋值
                    RoleName = string.Empty,
                    Token = jwtToken,
                    ExpiredMinuteTime = ConfigHelper.JwtExpiryInMinutes
                };
            }
            else throw new Exception("验证码错误或已过期，请点击验证码刷新并重新登录！");
        }

        #endregion
    }
}