using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MorningStar.Extension
{
    /// <summary>
    /// Serilog 容器服务
    /// </summary>
    public static class SerilogSetup
    {
        /// <summary>
        /// 注册【Serilog】容器服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="log"></param>
        public static void AddSerilogSetup(this IServiceCollection services, IWebHostEnvironment webHostEnvironment, out Serilog.ILogger log)
        {
            var logger = log = new LoggerConfiguration()
                               // 设置日志记录的最低级别。
                               .MinimumLevel.Information()
                               // 可以对特定命名空间或类的日志级别进行覆盖。
                               //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                               // 使用此方法可以在日志消息中添加上下文信息，如请求ID、用户信息等。
                               .Enrich.FromLogContext()
                               // 将日志消息输出到控制台。
                               .WriteTo.Console()
                               // 将日志消息写入文件。你可以指定文件路径、文件名、文件滚动策略等。
                               .WriteTo.File(Path.Combine($"{webHostEnvironment.WebRootPath}/Logs/", "MorningStar.log"),
                                             rollingInterval: RollingInterval.Day,
                                             rollOnFileSizeLimit: true,
                                             retainedFileCountLimit: 7,
                                             outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                               // 将日志消息发送到 Seq 日志服务器。
                               //.WriteTo.Seq()
                               // 将日志消息发送到 Elasticsearch 服务器。
                               //.WriteTo.Elasticsearch()
                               // 将日志消息通过 HTTP POST 请求发送到指定的 URL。
                               //.WriteTo.Http()
                               // 通过自定义的方式将日志消息输出到其他输出源。
                               //.WriteTo.Custom()
                               // 添加附加属性到日志消息中，用于结构化日志。
                               //.WriteTo.WithProperty()
                               // 添加异常详细信息到日志消息中，用于更好的错误追踪。
                               //.WriteTo.WithExceptionDetails()
                               // 根据条件筛选哪些日志消息将被记录。
                               //.WriteTo.ByIncludingOnly()
                               .CreateLogger();

            // 注册 Serilog.ILogger 单例（使用方式：构造函数注入 or App.GetService<Serilog.ILogger>();）
            services.AddSingleton(logger);

            // 配置应用程序的日志记录器
            services.AddLogging(builder =>
            {
                // 清除默认提供程序
                builder.ClearProviders();
                // 添加自定义提供程序
                builder.AddSerilog(logger, dispose: true);
            });
        }
    }
}
