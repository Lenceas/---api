using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MorningStar.Model
{
    /// <summary>
    /// Mongo实体基类
    /// </summary>
    public class BaseMongoEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.Int64)]
        public long ID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 排序下标, 越小越靠前 (只针对同级有效, 不夸层)
        /// </summary>
        public short OrderIndex { get; set; } = 999;

        /// <summary>
        /// 报表日期
        /// </summary>
        public DateTime ReportDate { get; set; } = DateTime.Today.ToUniversalTime();

        /// <summary>
        /// 修改人ID
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string MName { get; set; } = string.Empty;

        /// <summary>
        /// 修改时间(UTC)
        /// </summary>
        public DateTime MTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CName { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间(UTC)
        /// </summary>
        public DateTime CTime { get; set; } = DateTime.UtcNow;
    }
}
