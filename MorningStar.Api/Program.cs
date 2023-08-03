var builder = WebApplication.CreateBuilder(args);

#region 将服务添加到容器中
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSetup();
builder.WebHost.UseUrls("http://*:8079");
#endregion

var app = builder.Build();

#region 配置HTTP请求管道
if (app.Environment.IsDevelopment()) { }
app.UseSwaggerMiddleware();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion