using Microsoft.AspNetCore.Mvc;
using static MorningStar.Infrastructure.CustomApiVersion;

namespace MorningStar.Api.Controllers.v1
{
    /// <summary>
    /// 天气预报接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <summary>
        /// 构造函数
        /// </summary>
        public WeatherForecastController()
        {

        }

        /// <summary>
        /// 获取天气预报数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="myService"></param>
        [HttpGet]
        public void GetServices([FromServices] IMyService myService)
        {
            myService.ShowCode();
        }
    }
}