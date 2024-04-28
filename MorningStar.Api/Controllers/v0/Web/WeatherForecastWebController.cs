namespace MorningStar.Api
{
    /// <summary>
    /// 天气预报接口
    /// </summary>
    /// <param name="logger"></param>
    [CustomRoute(ApiVersions.v0)]
    [AllowAnonymous]
    public class WeatherForecastWebController(
        Serilog.ILogger logger
        ) : BaseApiController
    {
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
                string[] Summaries =
                [
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                ];
                return ApiTResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "WeatherForecastWeb/Get");
                return ApiErrorResult(ex.Message);
            }
        }
    }
}