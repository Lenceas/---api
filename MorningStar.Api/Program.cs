using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region ��������ӵ�������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "���ǲ��� �ӿ��ĵ�",
        Description = "���ǲ��� - ¬���ɵĸ��˲���",
        Contact = new OpenApiContact
        {
            Name = "Lenceas",
            Url = new Uri("https://lujiesheng.cn/")
        }
    });
});
builder.WebHost.UseUrls("http://*:8079");
#endregion

var app = builder.Build();

#region ����HTTP����ܵ�
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion