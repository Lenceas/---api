namespace MorningStar.Model
{
    /// <summary>
    /// 通用返回类（不带泛型T）
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 构造一个成功的 ApiResult 实例
        /// </summary>
        /// <param name="data">返回的数据</param>
        public ApiResult(object data)
        {
            Code = 200;
            Success = true;
            Data = data;
        }

        /// <summary>
        /// 构造一个失败的 ApiResult 实例
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="errorCode">错误状态码，默认为 400</param>
        public ApiResult(string errorMessage, int errorCode = 400)
        {
            Code = errorCode;
            Success = false;
            Errors = errorMessage;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = 400;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 返回数据
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public object? Errors { get; set; }

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 通用返回类（带泛型T）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 构造一个成功的 ApiResult 实例
        /// </summary>
        /// <param name="data">返回的数据</param>
        public ApiResult(T data)
        {
            Code = 200;
            Success = true;
            Data = data;
        }

        /// <summary>
        /// 构造一个失败的 ApiResult 实例
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="errorCode">错误状态码，默认为 400</param>
        public ApiResult(string errorMessage, int errorCode = 400)
        {
            Code = errorCode;
            Success = false;
            Errors = errorMessage;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = 400;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 返回数据
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public object? Errors { get; set; }

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}