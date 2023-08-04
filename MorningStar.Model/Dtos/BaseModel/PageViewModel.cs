namespace MorningStar.Model
{
    /// <summary>
    /// 分页Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageViewModel<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get { return TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1; } }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> ViewModelList { get; set; } = new List<T>();
    }
}
