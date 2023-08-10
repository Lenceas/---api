namespace MorningStar.Extension
{
    /// <summary>
    /// Cors 容器服务
    /// </summary>
    public static class CorsSetup
    {
        /// <summary>
        /// 注册【Cors】容器服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddCorsSetup(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                if (AppSettings.Get("Cors:EnableAllIPs").Equals("False"))
                {
                    options.AddPolicy(AppSettings.Get("Cors:PolicyName"), policy =>
                    {
                        policy
                            .WithOrigins(AppSettings.Get("Cors:IPs").Split(','))
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
                }
                else
                {
                    options.AddPolicy(AppSettings.Get("Cors:PolicyName"), policy =>
                    {
                        policy
                        // 允许来自任何源的跨域请求。这是一个比较宽松的设置，但可能会增加安全风险，因为任何站点都可以向您的 API 发起请求。
                            .SetIsOriginAllowed((host) => true)
                        // 允许任何 HTTP 方法（如 GET、POST、PUT、DELETE 等）的跨域请求。这意味着客户端可以使用任何方法来访问您的 API。
                            .AllowAnyMethod()
                        // 允许任何 HTTP 标头的跨域请求。这允许客户端在请求中包含任何标头信息。
                            .AllowAnyHeader();
                    });
                }
            });

            Console.WriteLine("容器服务：【Cors】已注册！");
        }
    }
}
