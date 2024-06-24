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
                                //.WriteTo.Console(new CompactJsonFormatter())
                                //.WriteTo.Console(new RenderedCompactJsonFormatter())
                                // 将日志消息异步写入文件。你可以指定文件路径、文件名、文件滚动策略等。
                                //.WriteTo.Async(_ => _.File(
                                //    Path.Combine($"{webHostEnvironment.WebRootPath}/Logs/", "MorningStar.log"),
                                //         rollingInterval: RollingInterval.Day,
                                //         rollOnFileSizeLimit: true,
                                //         outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss:fff} {Exception} {Level:u3} {Message:lj} {NewLine}"
                                //     ))
                                // 将日志消息写入 MySql 数据库
                                .WriteTo.Async(_ => _.MySQL(
                                    // 用于连接 MySQL 数据库的连接字符串。它包含数据库服务器地址、数据库名称、用户名、密码等信息。
                                    ConfigHelper.MySqlConnectionString,
                                    // 存储日志记录的 MySQL 数据库表的名称。如果没有指定，默认使用 "Logs"。
                                    "Logs",
                        // 设置要记录的最低日志级别。只有级别等于或高于此级别的日志事件才会被写入 MySQL。常见级别有 Verbose、Debug、Information、Warning、Error、Fatal。
                        Serilog.Events.LogEventLevel.Information,
                                    // 指定是否以 UTC 时间存储日志的时间戳。如果为 true，时间戳将转换为 UTC 时间。
                                    true,
                                    // 批量写入日志的大小。Serilog 会在内部缓存日志消息，并在达到此批量大小时写入数据库。
                                    10,
                        // 允许动态控制日志级别的开关，可以在运行时调整日志级别，而无需重启应用程序。
                        new Serilog.Core.LoggingLevelSwitch(Serilog.Events.LogEventLevel.Information)
                                    ))
                               // 将日志消息写入 Mongo 数据库 todo：还有问题写不进去！！！
                               //.WriteTo.Async(_ => _.MongoDB(
                               //    // 用于连接 MongoDB 数据库的连接字符串。它包含数据库服务器地址、数据库名称、用户名、密码等信息。
                               //    $"{ConfigHelper.MongoConnectionString}/{ConfigHelper.MongoDatabaseName}",
                               //    // 存储日志记录的 MongoDB 数据库集合名称。如果没有指定，默认使用 "log"。
                               //    "Logs",
                               //    // 设置要记录的最低日志级别。只有级别等于或高于此级别的日志事件才会被写入 MongoDB。常见级别有 Verbose、Debug、Information、Warning、Error、Fatal。
                               //    Serilog.Events.LogEventLevel.Information,
                               //    // 批量写入日志的数量限制。Serilog 会在内部缓存日志消息，并在达到此批量大小时写入数据库。
                               //    100,
                               //    // 批量日志写入的时间间隔。Serilog 会定期将缓存的日志消息写入 MongoDB。可以指定时间间隔，例如 TimeSpan.FromSeconds(5)。
                               //    TimeSpan.FromSeconds(10),
                               //    // 提供日志消息的格式化信息，通常用于文化相关的格式化。如果为 null，则使用系统默认的格式化设置。
                               //    null,
                               //     // 用于格式化日志事件的 JSON 格式化器。如果为 null，则使用默认的 JSON 格式化器。
                               //     new JsonFormatter(renderMessage: true)
                               //    ))
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