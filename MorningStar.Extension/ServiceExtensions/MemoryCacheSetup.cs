namespace MorningStar.Extension
{
    /// <summary>
    /// MemoryCache 容器服务
    /// </summary>
    public static class MemoryCacheSetup
    {
        /// <summary>
        /// 注册【MemoryCache】容器服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddMemoryCacheSetup(this IServiceCollection services)
        {
            // 内存缓存
            services.AddMemoryCache();

            // 分布式内存缓存
            services.AddDistributedMemoryCache();
        }
    }
}