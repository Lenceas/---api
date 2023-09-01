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
        #endregion
    }
}
