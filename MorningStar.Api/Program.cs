using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region 将服务添加到容器中
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "晨星博客 接口文档",
        Description = "晨星博客 - 卢杰晟的个人博客",
        Contact = new OpenApiContact
        {
            Name = "Lenceas",
            Url = new Uri("http://lujiesheng.cn/")
        }
    });
});
builder.WebHost.UseUrls("http://*:8079");
#endregion

var app = builder.Build();

#region 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion