namespace MorningStar.Service
{
    /// <summary>
    /// 用户接口类
    /// </summary>
    public interface IUserService
    {
        #region 公共

        #endregion

        #region 业务
        /// <summary>
        /// 初始化用户数据
        /// </summary>
        void InitDatas();

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">用户登录WebModel</param>
        /// <returns></returns>
        Task<TokenWebModel> Login(LoginWebModel model);
        #endregion
    }
}
