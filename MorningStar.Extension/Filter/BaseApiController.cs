using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MorningStar.Extension
{
    /// <summary>
    /// 基类控制器，提供通用的 API 返回方法
    /// </summary>
    /// <param name="log"></param>
    [Authorize]
    [CustomRoute(ApiVersions.v1)]
    [Produces("application/json")]
    public abstract class BaseApiController(Serilog.ILogger log) : ControllerBase
    {
        protected readonly Serilog.ILogger _log = log;

        /// <summary>
        /// 返回一个成功的 API 结果
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns>API 返回结果</returns>
        protected static IActionResult ApiResult(object data)
        {
            return new OkObjectResult(new ApiResult(data));
        }

        /// <summary>
        /// 返回一个成功的 API 结果
        /// </summary>
        /// <typeparam name="T">返回数据的类型</typeparam>
        /// <param name="data">返回的数据</param>
        /// <returns>API 返回结果</returns>
        protected static IActionResult ApiTResult<T>(T data)
        {
            return new OkObjectResult(new ApiResult<T>(data));
        }

        /// <summary>
        /// 返回一个失败的 API 结果
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="messageTemplate">描述事件的消息模板。</param>
        /// <returns>API 返回结果</returns>
        protected IActionResult ApiErrorResult(Exception exception, string messageTemplate)
        {
            _log.Error(exception, $"【{messageTemplate}】接口调用错误：");
            return new BadRequestObjectResult(new ApiResult(exception.Message));
        }

        /// <summary>
        /// 返回一个失败的 API 结果
        /// </summary>
        /// <typeparam name="T">返回数据的类型</typeparam>
        /// <param name="exception">异常信息</param>
        /// <param name="messageTemplate">描述事件的消息模板。</param>
        /// <returns>API 返回结果</returns>
        protected IActionResult ApiErrorTResult<T>(Exception exception, string messageTemplate)
        {
            _log.Error(exception, $"【{messageTemplate}】接口调用错误：");
            return new BadRequestObjectResult(new ApiResult<T>(exception.Message));
        }
    }
}