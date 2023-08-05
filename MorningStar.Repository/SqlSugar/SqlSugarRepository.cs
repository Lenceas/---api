using System.Reflection;

namespace MorningStar.Repository
{
    /// <summary>
    /// SqlSugar 仓储泛型基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlSugarRepository<T> : SimpleClient<T> where T : class, new()
    {
        public SqlSugarRepository(ISqlSugarClient? context = null) : base(context)
        {
            if (context == null)
            {
                var connectionString = AppSettings.Get("DataBase:Mysql");
                if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").Equals("Production"))
                    connectionString = (Environment.GetEnvironmentVariable("DATABASE_MYSQL") ?? string.Empty).Replace("\"", "");
                if (string.IsNullOrEmpty(connectionString)) throw new Exception("容器服务：【SqlSugar】注册错误：connectionString为空！");
                //Console.WriteLine(connectionString);
                var configConnection = new ConnectionConfig()
                {
                    DbType = DbType.MySql,
                    ConnectionString = connectionString,
                    IsAutoCloseConnection = true,
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        EntityNameService = (type, entity) =>
                        {
                            // 全局AOP全部禁止删除列(CodeFirst)
                            //entity.IsDisabledDelete = true;
                            // 全局AOP全部禁止更新+删除(CodeFirst) - 比上面优先级更高
                            entity.IsDisabledUpdateAll = true;
                        },
                        EntityService = (c, p) =>
                        {
                            // 建表技巧：自动Nullable 
                            if (p.IsPrimarykey == false && new NullabilityInfoContext().Create(c).WriteState is NullabilityState.Nullable)
                            {
                                p.IsNullable = true;
                            }
                            // 兼容 mysql longtext 字段类型 只需要在实体属性上面加是：[SugarColumn(ColumnDataType = "varchar(max)")]
                            if (p.DataType == "varchar(max)")
                            {
                                p.DataType = "longtext";
                            }
                        }
                    }
                };

                // Scope模式：SqlSugarClient  
                base.Context = new SqlSugarClient(configConnection,
                    db =>
                    {
                        // 单例参数配置，所有上下文生效
                        db.Aop.OnLogExecuting = (sql, pars) =>
                        {
                            // 输出sql
                            // Console.WriteLine(sql);
                        };
                    });
            }
            else
                base.Context = context;
        }
    }
}
