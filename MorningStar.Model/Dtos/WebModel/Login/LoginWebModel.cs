namespace MorningStar.Model
{
    /// <summary>
    /// 用户登录WebModel
    /// </summary>
    public class LoginWebModel
    {
        /// <summary>
        /// 该次登录唯一标识
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public string CaptchaID { get; set; } = string.Empty;

        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "必填")
            , MinLength(5, ErrorMessage = "请输入长度不少于5位的账号!")
            , MaxLength(15, ErrorMessage = "请输入长度不多于15位的账号!")]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必填")
            , MinLength(8, ErrorMessage = "请输入长度不少于8位的密码!")
            , MaxLength(24, ErrorMessage = "请输入长度不多于24位的密码!")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "必填")
            , MinLength(4, ErrorMessage = "请输入4验证码！")
            , MaxLength(4, ErrorMessage = "请输入4验证码！")]
        public string Captcha { get; set; } = string.Empty;
    }
}
