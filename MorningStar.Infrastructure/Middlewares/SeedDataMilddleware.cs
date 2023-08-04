using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace MorningStar.Infrastructure
{
    /// <summary>
    /// Sqlsugar CodeFirst 中间件
    /// </summary>
    public static class SeedDataMilddleware
    {
        /// <summary>
        /// 启用【Sqlsugar CodeFirst】中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseSeedDataMilddleware(this IApplicationBuilder app)
        {
            try
            {
                var db = app.ApplicationServices.GetService<SqlSugarScope>() ?? throw new Exception("未获取到SqlSugarScope实例！");

                Console.WriteLine("************ 开始自动初始化数据 **********");
                Console.WriteLine();

                Console.WriteLine("开始数据库连接检测...");
                if (db.CopyNew().DbMaintenance.CreateDatabase())
                    Console.WriteLine("数据库连接成功！");

                Console.WriteLine();
                Console.WriteLine("开始初始化表数据...");
                Console.WriteLine();

                #region 表
                // todo:
                #endregion

                Console.WriteLine();
                Console.WriteLine("表数据初始化完成！");
                Console.WriteLine();

                Console.WriteLine("************ 自动初始化数据完成 **********");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                throw new Exception("中间件：【SqlSugar CodeFirst】启用错误：" + ex.Message);
            }

            Console.WriteLine("中间件：【SqlSugar CodeFirst】已启用！");
        }
    }
}
