namespace MorningStar.Extension
{
    /// <summary>
    /// Redis 容器服务
    /// </summary>
    public static class RedisSetup
    {
        /// <summary>
        /// 注册【Redis】容器服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddRedisSetup(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                var redisConnectStr = ConfigHelper.RedisConnectionString;
                if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").Equals("Production"))
                    redisConnectStr = (Environment.GetEnvironmentVariable("REDIS_CONNECTSTR") ?? string.Empty).Replace("\"", "");
                if (string.IsNullOrEmpty(redisConnectStr)) throw new Exception("容器服务：【Redis】注册错误：redisConnectStr为空！");
                // 连接字符串
                options.Configuration = redisConnectStr;
                // 键名前缀
                options.InstanceName = ConfigHelper.RedisInstanceName;
            });
        }
    }
}