using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using static MorningStar.Infrastructure.CustomApiVersion;

namespace MorningStar.Infrastructure
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
                });
            });

            Console.WriteLine($"中间件：【Swagger】已启用！");
        }
    }
}
