namespace MorningStar.Model
{
    /// <summary>
    /// Token令牌信息
    /// </summary>
    public class TokenWebModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 过期时间（单位：分钟）
        /// </summary>
        public int ExpiredMinuteTime { get; set; }

        /// <summary>
        /// 具体过期时间
        /// </summary>
        public DateTime ExpiredMinuteTimeStr { get { return DateTime.Now.AddMinutes(ExpiredMinuteTime); } }
    }
}
