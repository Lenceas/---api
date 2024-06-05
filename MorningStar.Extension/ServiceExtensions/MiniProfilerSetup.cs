using StackExchange.Profiling;
using StackExchange.Profiling.Storage;

namespace MorningStar.Extension
{
    /// <summary>
    /// MiniProfiler 容器服务
    /// </summary>
    public static class MiniProfilerSetup
    {
        /// <summary>
        /// 注册【MiniProfiler】容器服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddMiniProfilerSetup(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddMiniProfiler(options =>
            {
                // 路由基础地址, 默认是 /mini-profiler-resources
                options.RouteBasePath = "/profiler";
                // 设置授权策略，决定谁可以看到 MiniProfiler
                //options.ResultsAuthorize = request => !builder.Environment.IsProduction();
                // 设置需要显示MiniProfiler的位置，这里选择显示在左上角
                options.PopupRenderPosition = RenderPosition.Left;
                // 数据缓存时间
                if (options.Storage is MemoryCacheStorage memoryCacheStorage)
                    memoryCacheStorage.CacheDuration = TimeSpan.FromMinutes(1);
                // 设置性能分析数据在弹出式界面中是否显示每个步骤的执行时间，包括子步骤的执行时间。
                options.PopupShowTimeWithChildren = true;
            });
        }
    }
}