using AspNetCoreRateLimit;

namespace MorningStar.Extension
{
    /// <summary>
    /// IPRateLimit 中间件
    /// </summary>
    public static class IPRateLimitMiddleware
    {
        /// <summary>
        /// 启用【IPRateLimit】中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseIPRateLimitMiddleware(this IApplicationBuilder app)
        {
            app.UseIpRateLimiting();
        }
    }
}