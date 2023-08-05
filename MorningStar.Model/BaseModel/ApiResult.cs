namespace MorningStar.Model
{
    /// <summary>
    /// 通用返回类（不带泛型T）
    /// </summary>
    public class ApiResult
    {
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
