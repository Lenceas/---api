var builder = WebApplication.CreateBuilder(args);

#region 将服务添加到容器中
Console.WriteLine("开始注册容器服务...");
Console.WriteLine();
builder.WebHost.UseUrls("http://*:8079");
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacModuleRegister());
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSetup();
Console.WriteLine();
Console.WriteLine("容器服务注册完成！");
Console.WriteLine();
#endregion

var app = builder.Build();

#region 配置HTTP请求管道
Console.WriteLine("开始启用中间件...");
Console.WriteLine();
if (app.Environment.IsDevelopment()) { }
app.UseSwaggerMiddleware();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
Console.WriteLine();
Console.WriteLine("中间件启用完毕！");
Console.WriteLine();
app.Run();
#endregion