namespace MorningStar.Common
{
    public static  class ConfigHelper
    {
        #region 基础配置

        #endregion

        #region 用户权限配置
        /// <summary>
        /// 登录RedisKey
        /// </summary>
        public static string LoginRedisKey
        {
            get
            {
                return AppSettings.Get("Redis:TokenName");
            }
        }

        /// <summary>
        /// 登录验证码RedisKey
        /// </summary>
        public static string LoginCaptchaRedisKey
        {
            get
            {
                return AppSettings.Get("Redis:CaptchaName");
            }
        }
        #endregion
    }
}
