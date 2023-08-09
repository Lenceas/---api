namespace MorningStar.Api
{
    /// <summary>
    /// 天气预报接口
    /// </summary>
    [CustomRoute(ApiVersions.v2)]
    [AllowAnonymous]
    public class WeatherForecastWebApiController : BaseApiController
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WeatherForecastWebApiController()
        {

        }

        /// <summary>
        /// 获取天气预报数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<WeatherForecast>), 200)]
        public IActionResult Get()
        {
            try
            {
                string[] Summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };
                return ApiTResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray());
            }
            catch (Exception ex)
            {
                return ApiErrorTResult<List<WeatherForecast>>(ex.Message);
            }
        }
    }
}