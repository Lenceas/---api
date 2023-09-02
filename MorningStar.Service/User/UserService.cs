using Microsoft.AspNetCore.Http;

namespace MorningStar.Service
{
    /// <summary>
    /// 用户实现类
    /// </summary>
    public class UserService : MongoRepository<UserEntity>, IUserService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserService() : base(nameof(UserEntity))
        {

        }

        #region 公共

        #endregion

        #region 业务
        /// <summary>
        /// 初始化用户数据
        /// </summary>
        public void InitDatas()
        {
            var en = GetSingleAsync(_ => _.ID == 1024).GetAwaiter().GetResult();
            if (en == null)
                InsertAsync(new UserEntity() { ID = 1024, UserName = "Lenceas", Account = "admin", Password = CommonHelper.MD5Encrytp("admin888") }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">用户登录WebModel</param>
        /// <returns></returns>
        public async Task<TokenWebModel> Login(LoginWebModel model)
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
                ExpiredMinuteTime = Convert.ToInt32(AppSettings.Get("Jwt:ExpiryInMinutes"))
            };
        }
        #endregion
    }
}
