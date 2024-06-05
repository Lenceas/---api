namespace MorningStar.Extension
{
    /// <summary>
    /// Cors 中间件
    /// </summary>
    public static class CorsMiddleware
    {
        /// <summary>
        /// 启用【Cors】中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseCorsMiddleware(this IApplicationBuilder app)
        {
            app.UseCors(AppSettings.Get("Cors:PolicyName"));
        }
    }
}