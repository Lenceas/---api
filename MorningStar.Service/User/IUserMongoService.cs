namespace MorningStar.Service
{
    /// <summary>
    /// 用户接口类
    /// </summary>
    public interface IUserMongoService
    {
        #region 业务

        /// <summary>
        /// 初始化用户数据
        /// </summary>
        void InitDatas();

        /// <summary>
        /// 获取登录图片验证码
        /// </summary>
        /// <param name="codeLength">验证码长度</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="fontSize">字体大小</param>
        /// <returns>返回验证码和base64格式验证码图片字符串</returns>
        (string code, string base64Code) GetLoginVerifyCode(int codeLength, int width, int height, int fontSize);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">用户登录WebModel</param>
        /// <returns></returns>
        Task<TokenWebModel> Login(LoginWebModel model);

        #endregion
    }
}