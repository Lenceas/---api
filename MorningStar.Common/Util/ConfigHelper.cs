namespace MorningStar.Common
{
    public static class ConfigHelper
    {
        #region 数据库配置

        /// <summary>
        /// MySql数据库连接字符串
        /// </summary>
        public static string MySqlConnectionString
        {
            get
            {
                return AppSettings.Get("DataBase:Mysql") ?? string.Empty;
            }
        }

        /// <summary>
        /// Mongo数据库连接字符串
        /// </summary>
        public static string MongoConnectionString
        {
            get
            {
                return AppSettings.Get("DataBase:Mongo:ConnectionString") ?? string.Empty;
            }
        }

        /// <summary>
        /// Mongo数据库名称
        /// </summary>
        public static string MongoDatabaseName
        {
            get
            {
                return AppSettings.Get("DataBase:Mongo:DatabaseName") ?? string.Empty;
            }
        }

        /// <summary>
        /// Redis数据库连接字符串
        /// </summary>
        public static string RedisConnectionString
        {
            get
            {
                return AppSettings.Get("DataBase:Redis:ConnectionString") ?? string.Empty;
            }
        }

        /// <summary>
        /// Redis键名前缀
        /// </summary>
        public static string RedisInstanceName
        {
            get
            {
                return AppSettings.Get("DataBase:Redis:InstanceName") ?? string.Empty;
            }
        }

        /// <summary>
        /// 登录RedisKey
        /// </summary>
        public static string LoginRedisKey
        {
            get
            {
                return AppSettings.Get("DataBase:Redis:TokenName") ?? string.Empty;
            }
        }

        /// <summary>
        /// 登录验证码RedisKey
        /// </summary>
        public static string LoginCaptchaRedisKey
        {
            get
            {
                return AppSettings.Get("DataBase:Redis:CaptchaName") ?? string.Empty;
            }
        }

        #endregion

        #region 用户权限配置

        /// <summary>
        /// JWT鉴权发行人
        /// </summary>
        public static string JwtIssuer
        {
            get
            {
                return AppSettings.Get("Jwt:Issuer") ?? string.Empty;
            }
        }

        /// <summary>
        /// JWT鉴权接收人
        /// </summary>
        public static string JwtAudience
        {
            get
            {
                return AppSettings.Get("Jwt:Audience") ?? string.Empty;
            }
        }

        /// <summary>
        /// JWT鉴权密钥
        /// </summary>
        public static string JwtSecretKey
        {
            get
            {
                return AppSettings.Get("Jwt:SecretKey") ?? string.Empty;
            }
        }

        /// <summary>
        /// JWT鉴权过期分钟,默认120分钟
        /// </summary>
        public static int JwtExpiryInMinutes
        {
            get
            {
                return Convert.ToInt32(AppSettings.Get("Jwt:ExpiryInMinutes") ?? "120");
            }
        }

        #endregion
    }
}