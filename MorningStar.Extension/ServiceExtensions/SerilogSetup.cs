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
        /// <param name="log"></param>
        public static void AddSerilogSetup(this IServiceCollection services, out Serilog.ILogger log)
        {
            var mysqlConnectionString = ConfigHelper.MySqlConnectionString;
            //var seqServerUrl = ConfigHelper.SeqServerUrl;
            //var seqApiKey = ConfigHelper.SeqApiKey;
            if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").Equals("Production"))
            {
                mysqlConnectionString = (Environment.GetEnvironmentVariable("DATABASE_MYSQL") ?? string.Empty).Replace("\"", "");
                //seqServerUrl = (Environment.GetEnvironmentVariable("DATABASE_SEQ_SERVERURL") ?? string.Empty).Replace("\"", "");
                //seqApiKey = (Environment.GetEnvironmentVariable("DATABASE_SEQ_APIKEY") ?? string.Empty).Replace("\"", "");
            }
            var logger = log = new LoggerConfiguration()
                               .ReadFrom.Configuration(AppSettings.Configuration)
                               // 设置日志记录的最低级别。
                               //.MinimumLevel.Information()
                               // 可以对特定命名空间或类的日志级别进行覆盖。
                               //.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                               //.MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                               //.MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                               //.MinimumLevel.Override("System.Net.Http.HttpClient", Serilog.Events.LogEventLevel.Warning)
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
                                           mysqlConnectionString,
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
                               //.WriteTo.Async(_ => _.Seq(
                               //   // 记录事件的Seq服务器的基本URL。
                               //   seqServerUrl,
                               //   // 将事件写入接收器所需的最低日志事件级别。
                               //   Serilog.Events.LogEventLevel.Information,
                               //    // 单批中要发布的最大事件数。
                               //    1000,
                               //    // 检查事件批之间的等待时间。
                               //    null,
                               //    // Seq API密钥，用于向Seq服务器验证客户端。
                               //    seqApiKey,
                               //    // 一组文件的路径，这些文件将用于缓冲事件，直到它们可以在网络上成功传输。将使用模式bufferBaseFilename*.json创建单个文件，该模式不应与同一目录中的任何其他文件名冲突。
                               //    null,
                               //    // 允许特定日期的缓冲区日志文件增长到的最大数据量（以字节为单位）。默认情况下，不会应用任何限制。
                               //    null,
                               //    // 事件的JSON表示在丢弃而不是发送到Seq服务器之前可能占用的最大大小（以字节为单位）。指定null表示无限制。默认值为265 KB。
                               //    262144,
                               //    // 如果提供，开关将根据Seq服务器对相应API密钥的级别设置进行更新。将相同的密钥传递给MinimumLevel。ControlledBy（）将使整个管道受到动态控制。不要使用此设置指定restrictdToMinimumLevel。
                               //    null,
                               //    // 用于构建将日志消息发送到Seq的HttpClient。
                               //    null,
                               //    // 用于存储失败请求的字节数的软限制。该限制是软的，因为任何单个错误有效载荷都可以超过它，但在这种情况下，只会保留单个错误有效负载。
                               //    null,
                               //    // 在等待将事件发送到Seq时，内存中保存的最大事件数。超过此限制，活动将被取消。默认值为100000。对耐用原木运输没有影响。
                               //    100000,
                               //    // Serilog。格式化。ITextFormatter，用于格式化（换行符分隔的CLEF/JSON）有效载荷。实验。
                               //    null
                               //    ))
                               // 将日志消息发送到 Elasticsearch 服务器。
                               //.WriteTo.Async(_ => _.Elasticsearch(new ElasticsearchSinkOptions()
                               //{
                               //    //
                               //    MinimumLevel = Serilog.Events.LogEventLevel.Information
                               //}))
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