using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MorningStar.Extension
{
    /// <summary>
    /// 基类控制器，提供通用的 API 返回方法
    /// </summary>
    [Authorize]
    [CustomRoute(ApiVersions.v1)]
    [Produces("application/json")]
    public class BaseApiController : ControllerBase
    {
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
        /// <param name="errorMessage">错误信息</param>
        /// <param name="errorCode">错误状态码，默认为 400</param>
        /// <returns>API 返回结果</returns>
        protected static IActionResult ApiErrorResult(string errorMessage, int errorCode = 400)
        {
            return new OkObjectResult(new ApiResult(errorMessage, errorCode));
        }

        /// <summary>
        /// 返回一个失败的 API 结果
        /// </summary>
        /// <typeparam name="T">返回数据的类型</typeparam>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="errorCode">错误状态码，默认为 400</param>
        /// <returns>API 返回结果</returns>
        protected static IActionResult ApiErrorTResult<T>(string errorMessage, int errorCode = 400)
        {
            return new OkObjectResult(new ApiResult<T>(errorMessage, errorCode));
        }
    }
}