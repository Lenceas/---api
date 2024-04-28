namespace MorningStar.Model
{
    /// <summary>
    /// 测试表
    /// </summary>
    [Description("测试表"),
        SugarTable("Test", TableDescription = "测试表", IsCreateTableFiledSort = true)]
    public class TestMySqlEntity : BaseEntity
    {

    }
}
