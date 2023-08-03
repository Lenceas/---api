using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using static MorningStar.Infrastructure.CustomApiVersion;

namespace MorningStar.Infrastructure
{
    /// <summary>
    /// Swagger 容器服务
    /// </summary>
    public static class SwaggerSetup
    {
        /// <summary>
        /// 注册【Swagger】容器服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // 遍历出全部的版本，做文档信息展示
                typeof(ApiVersions).GetEnumNames().OrderBy(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Title = "晨星博客 接口文档",
                        Version = version,
                        Description = "晨星博客 HTTP API " + version,
                        Contact = new OpenApiContact
                        {
                            Name = "Lenceas",
                            Url = new Uri("http://lujiesheng.cn/")
                        }
                    });
                    c.OrderActionsBy(o => o.RelativePath);
                });

                // Api XML
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "MorningStar.Api.xml");
                c.IncludeXmlComments(xmlPath, true);

                // Model XML
                var xmlModelPath = Path.Combine(AppContext.BaseDirectory, "MorningStar.Model.xml");
                c.IncludeXmlComments(xmlModelPath);
            });

            Console.WriteLine($"容器服务：【Swagger】已注册！");
        }
    }
}
