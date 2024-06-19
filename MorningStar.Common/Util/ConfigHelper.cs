namespace MorningStar.Common
{
    public static class ConfigHelper
    {
        #region 数据库配置

        /// <summary>
        /// Mongo数据库连接字符串
        /// </summary>
        public static string MongoConnectionString
        {
            get
            {
                return AppSettings.Get("DataBase:Mongo:ConnectionString");
            }
        }

        /// <summary>
        /// Mongo数据库名称
        /// </summary>
        public static string MongoDatabaseName
        {
            get
            {
                return AppSettings.Get("DataBase:Mongo:DatabaseName");
            }
        }

        #endregion 数据库配置

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

        #endregion 用户权限配置
    }
}