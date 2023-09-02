using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace MorningStar.Common
{
    /// <summary>
    /// 验证码生成器
    /// </summary>
    public static class VerifyCodeGenerator
    {
        private static readonly Color[] Colors = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown,
        Color.Brown,Color.DarkBlue};
        private static readonly char[] Chars = { '2','3','4','5','6','8','9',
       'A','B','C','D','E','F','G','H','J','K', 'L','M','N','P','R','S','T','W','X','Y' };

        /// <summary>
        /// 生成随机几位数的验证码
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string GenCode(int num)
        {
            var code = string.Empty;
            var r = new Random();
            for (int i = 0; i < num; i++)
            {
                code += Chars[r.Next(Chars.Length)].ToString();
            }
            return code;
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="CodeLength">验证码长度</param>
        /// <param name="Width">宽度</param>
        /// <param name="Height">高度</param>
        /// <param name="FontSize">字体大小</param>
        /// <returns></returns>
        public static (string code, byte[] bytes) CreateValidateGraphic(int CodeLength, int Width, int Height, int FontSize)
        {
            var code = GenCode(CodeLength);
            var r = new Random();
            using var image = new Image<Rgba32>(Width, Height);
            // 字体
            var font = GetFont(FontSize);
            image.Mutate(ctx =>
            {
                // 白底背景
                ctx.Fill(Color.White);

                // 画验证码
                for (int i = 0; i < code.Length; i++)
                {
                    ctx.DrawText(code[i].ToString()
                        , font
                        , Colors[r.Next(Colors.Length)]
                        , new PointF(20 * i + 10, r.Next(2, 12)));
                }

                // 画干扰线
                for (int i = 0; i < 6; i++)
                {
                    var pen = Pens.DashDot(Colors[r.Next(Colors.Length)], 1);
                    var p1 = new PointF(r.Next(Width), r.Next(Height));
                    var p2 = new PointF(r.Next(Width), r.Next(Height));

                    ctx.DrawLine(pen, p1, p2);
                }

                // 画噪点
                for (int i = 0; i < 60; i++)
                {
                    var pen = Pens.DashDot(Colors[r.Next(Colors.Length)], 1);
                    var p1 = new PointF(r.Next(Width), r.Next(Height));
                    var p2 = new PointF(p1.X + 1f, p1.Y + 1f);

                    ctx.DrawLine(pen, p1, p2);
                }
            });
            using var ms = new MemoryStream();

            //  格式 自定义
            image.SaveAsPng(ms);
            return (code, ms.ToArray());
        }

        /// <summary>
        /// 获取字体
        /// </summary>
        /// <param name="FontSize">字体大小</param>
        /// <returns></returns>
        public static Font GetFont(int FontSize)
        {
            // 获取默认字体
            Font font;
            FontCollection fonts = new();
            //加载系统默认字体
            if (SystemFonts.Families != null && SystemFonts.Families.Any())
            {
                font = SystemFonts.CreateFont(SystemFonts.Families.First().Name, FontSize, FontStyle.Bold);
            }
            else // 如果系统默认字体加载失败，则指定字体
            {
                // 字体的路径，也就是可以使用配置文件来指定字体
                var fontPath = $"{App.WebHostEnvironment()?.WebRootPath ?? string.Empty}/Fonts/arial.ttf";
                FontFamily fontfamily = fonts.Add(fontPath);
                font = new Font(fontfamily, FontSize, FontStyle.Bold);
            }
            return font;
        }
    }
}
