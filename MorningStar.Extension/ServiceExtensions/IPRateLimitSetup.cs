using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;

namespace MorningStar.Extension
{
    /// <summary>
    /// IPRateLimit 容器服务
    /// </summary>
    public static class IPRateLimitSetup
    {
        /// <summary>
        /// 注册【IPRateLimit】容器服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void AddIPRateLimitSetup(this IServiceCollection services, IConfiguration Configuration)
        {
            // 从配置文件中获取名为 "IpRateLimiting" 的配置节点，并将其绑定到 IpRateLimitOptions 类型的实例。
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            // 注册 IIpPolicyStore 接口的单例实现，用于存储IP策略。使用 MemoryCacheIpPolicyStore，它将使用内存缓存来存储IP限制策略。
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            // 注册 IRateLimitCounterStore 接口的单例实现，用于存储请求计数器。使用 MemoryCacheRateLimitCounterStore 以在内存中存储请求计数。
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            // 注册用于处理速率限制的策略：确保在高并发情况下正确处理请求计数器。
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            // 注册 IRateLimitConfiguration 接口的单例实现，它将保存速率限制的配置信息。这可以确保在应用程序的不同部分共享相同的配置。
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}