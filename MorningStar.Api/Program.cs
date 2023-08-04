﻿var builder = WebApplication.CreateBuilder(args);

#region 将服务添加到容器中
Console.WriteLine("开始注册容器服务...");
Console.WriteLine();

// 注册 AppSettings 服务
builder.Services.AddSingleton(new AppSettings());

// 注册 Swagger 服务
builder.Services.AddSwaggerSetup();

// 注册 Authorization 服务
builder.Services.AddAuthorizationSetup();

// 注册 Autofac 服务
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacModuleRegister());
    });

builder.WebHost.UseUrls("http://*:8079");

builder.Services.AddControllers()
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
Console.WriteLine("容器服务注册完毕！");
Console.WriteLine();
#endregion

var app = builder.Build();

#region 配置HTTP请求管道
Console.WriteLine("开始启用中间件...");
Console.WriteLine();

if (app.Environment.IsDevelopment())
{

}

// 启用 Swagger 中间件
app.UseSwaggerMiddleware();

// 启用 静态文件 中间件
app.UseStaticFiles();

// 启用 认证 中间件
app.UseAuthentication();

// 启用 授权 中间件
app.UseAuthorization();

app.MapControllers();

Console.WriteLine();
Console.WriteLine("中间件启用完毕！");
Console.WriteLine();

app.Run();
#endregion