using Autofac;
using Autofac.Extensions.DependencyInjection;
using MorningStar;
using MorningStar.Repository;

var builder = WebApplication.CreateBuilder(args);

#region 将服务添加到容器中

// 注册 AppSettings 服务
builder.Services.AddSingleton(new AppSettings());

// 注册 Serilog 服务
builder.Services.AddSerilogSetup(builder.Environment, out Serilog.ILogger log);
log.Information("************ 开始注册容器服务 ************");
Console.WriteLine();
log.Information("容器服务：【Serilog】已注册！");

// 注册 HttpContext 服务
builder.Services.AddHttpContextAccessor();
log.Information("容器服务：【HttpContext】已注册！");

// 注册 MiniProfiler 服务
builder.Services.AddMiniProfilerSetup(builder);
log.Information("容器服务：【MiniProfiler】已注册！");

// 注册 Authorization 服务
builder.Services.AddAuthorizationSetup();
log.Information("容器服务：【Authorization】已注册！");

// 注册 Swagger 服务
builder.Services.AddSwaggerSetup();
log.Information("容器服务：【Swagger】已注册！");

// 注册 MemoryCache 服务
builder.Services.AddMemoryCacheSetup();
log.Information("容器服务：【MemoryCache】已注册！");

// 注册 Redis 服务
builder.Services.AddRedisSetup();
log.Information("容器服务：【Redis】已注册！");

// 注册 IPRateLimit 服务
builder.Services.AddIPRateLimitSetup(builder.Configuration);
log.Information("容器服务：【IPRateLimit】已注册！");

// 注册 SqlSugar 服务
builder.Services.AddSqlSugarSetup();
log.Information("容器服务：【SqlSugar】已注册！");

// 注册 SqlSugar Repository 服务
builder.Services.AddScoped(typeof(SqlSugarRepository<>));
log.Information("容器服务：【SqlSugar Repository】已注册！");

// 注册 MongoDB 服务
builder.Services.AddMongoDBSetup();
log.Information("容器服务：【MongoDB】已注册！");

// 注册 AutoMapper 服务
builder.Services.AddAutoMapperSetup();
log.Information("容器服务：【AutoMapper】已注册！");

// 注册 Cors 服务
builder.Services.AddCorsSetup();
log.Information("容器服务：【Cors】已注册！");

// 注册 Autofac 服务
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacModuleRegister());
    });
log.Information("容器服务：【Autofac】已注册！");

builder.WebHost.UseUrls("http://*:8079");

builder.Services.AddControllers()
                .AddMvcOptions(options =>
                {
                    // 添加模型验证过滤器
                    options.Filters.Add(new ModelStateValidationFilter());
                })
                // Json 序列化配置
                .AddNewtonsoftJson(options =>
                {
                    // long 类型序列化时转 string
                    options.SerializerSettings.Converters.AddLongTypeConverters();
                    // 时间格式化
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // 序列化属性名全部小写
                    //options.SerializerSettings.ContractResolver = new LowerCasePropertyNameContractResolver();
                    // 忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

builder.Services.AddEndpointsApiExplorer();

Console.WriteLine();
log.Information("************ 容器服务注册完毕 ************");
Console.WriteLine();

#endregion 将服务添加到容器中

var app = builder.Build();

#region 配置HTTP请求管道

log.Information("************ 开始启用中间件 **************");
Console.WriteLine();

// 是否开发环境
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

// 初始化全局 App 实例
App.Initialize(app.Services, app.Services.GetRequiredService<IHttpContextAccessor>(), app.Environment);
log.Information("中间件：【App】已启用！");

// 启用 静态文件 中间件
app.UseStaticFiles();
log.Information("中间件：【StaticFiles】已启用！");

// 启用 路由 中间件
app.UseRouting();
log.Information("中间件：【Routing】已启用！");

// 启用 认证 中间件
app.UseAuthentication();
log.Information("中间件：【Authentication】已启用！");

// 启用 授权 中间件
app.UseAuthorization();
log.Information("中间件：【Authorization】已启用！");

// 启用 IPRateLimit 中间件
app.UseIPRateLimitMiddleware();
log.Information("中间件：【IPRateLimit】已启用！");

// 启用 MiniProfiler 中间件
app.UseMiniProfilerMiddleware();
log.Information("中间件：【MiniProfiler】已启用！");

// 启用 Swagger 中间件
app.UseSwaggerMiddleware();
log.Information("中间件：【Swagger】已启用！");

// 启用 Cors 中间件
app.UseCorsMiddleware();
log.Information("中间件：【Cors】已启用！");

// CodeFirst 生成数据
app.UseSeedDataMilddleware(log);

// 启用 映射控制器 中间件
app.MapControllers();

Console.WriteLine();
log.Information("************ 中间件启用完毕 **************");
Console.WriteLine();

app.Run();

#endregion 配置HTTP请求管道