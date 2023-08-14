using MongoDB.Driver;

namespace MorningStar.Extension
{
    /// <summary>
    /// MongoDB 容器服务
    /// </summary>
    public static class MongoDBSetup
    {
        /// <summary>
        /// 注册【MongoDB】容器服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddMongoDBSetup(this IServiceCollection services)
        {
            // 注册 IMongoClient 单例
            services.AddSingleton<IMongoClient>(provider =>
            {
                var connectionString = AppSettings.Get("DataBase:Mongo:ConnectionString");
                if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").Equals("Production"))
                    connectionString = (Environment.GetEnvironmentVariable("DATABASE_MONGO") ?? string.Empty).Replace("\"", "");
                if (string.IsNullOrEmpty(connectionString)) throw new Exception("容器服务：【MongoDB】注册错误：connectionString为空！");
                //Console.WriteLine(connectionString);
                return new MongoClient(connectionString);
            });

            // 注册 IMongoDatabase 作用域
            services.AddScoped(provider =>
            {
                var client = provider.GetRequiredService<IMongoClient>();
                return client.GetDatabase(AppSettings.Get("DataBase:Mongo:DatabaseName"));
            });
        }
    }
}
