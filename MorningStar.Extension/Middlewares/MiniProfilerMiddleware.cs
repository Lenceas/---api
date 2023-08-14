namespace MorningStar.Extension
{
    /// <summary>
    /// MiniProfiler 中间件
    /// </summary>
    public static class MiniProfilerMiddleware
    {
        /// <summary>
        /// 启用【MiniProfiler】中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseMiniProfilerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiniProfiler();
        }
    }
}
