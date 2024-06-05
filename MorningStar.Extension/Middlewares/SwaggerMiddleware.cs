using Swashbuckle.AspNetCore.SwaggerUI;

namespace MorningStar.Extension
{
    /// <summary>
    /// Swagger 中间件
    /// </summary>
    public static class SwaggerMiddleware
    {
        /// <summary>
        /// 启用【Swagger】中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // 根据版本名称倒序 遍历展示
                typeof(ApiVersions).GetEnumNames().OrderBy(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"晨星博客 {version}");
                    c.SupportedSubmitMethods(new[] { SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete });
                    c.RoutePrefix = string.Empty;

                    // 添加 MiniProfiler 显示JS
                    c.HeadContent = @$"<script async='async'
                        id='mini-profiler'
                        src='/profiler/includes.min.js?v=4.2.22+4563a9e1ab'
                        Data-version='4.2.22+4563a9e1ab'
                        Data-path='/profiler/'
                        Data-current-id='59e91ce8-3995-4c36-8a8c-a91f552259d1'
                        Data-ids='59e91ce8-3995-4c36-8a8c-a91f552259d1'
                        Data-position='Left'
                        Data-scheme='Light''
                        Data-authorized='true'
                        Data-max-traces='5'
                        Data-toggle-shortcut='Alt+P'
                        Data-trivial-milliseconds='2.0'
                        Data-ignored-duplicate-execute-types='Open,OpenAsync,Close,CloseAsync'>
                    </script>";
                });
            });
        }
    }
}