namespace MorningStar.Model
{
    /// <summary>
    /// 保存测试Mongo数据WebModel
    /// </summary>
    public class SaveTestMongoWebModel
    {
        /// <summary>
        /// 测试数据ID 新增的时候传0
        /// </summary>
        [Required]
        public long ID { get; set; }
    }
}