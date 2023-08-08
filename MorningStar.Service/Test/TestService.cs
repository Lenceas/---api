namespace MorningStar.Service
{
    /// <summary>
    /// 测试实现类
    /// </summary>
    public class TestService : SqlSugarRepository<TestEntity>, ITestService
    {
        public TestService()
        {

        }

        /// <summary>
        /// 初始化测试数据
        /// </summary>
        public void InitDatas()
        {
            var list = GetListAsync().GetAwaiter().GetResult();
            if (!list.Any())
            {
                for (int i = 1; i < 25; i++)
                    list.Add(new TestEntity() { ID = i });
                if (list.Any())
                    InsertRangeAsync(list).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// 获取测试数据分页
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public async Task<PageViewModel<TestWebModel>> GetPage(int pageIndex, int pageSize)
        {
            var result = new PageViewModel<TestWebModel>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ViewModelList = new List<TestWebModel>()
            };

            var filter = Expressionable.Create<TestEntity>().ToExpression();
            RefAsync<int> count = 0;
            var list = await AsQueryable()
                                    .OrderByDescending(_ => _.MTime)
                                    .OrderBy(_ => _.OrderIndex)
                                    .Where(filter)
                                    .ToPageListAsync(pageIndex, pageSize, count);
            result.TotalCount = count;
            result.ViewModelList = (from p in list
                                    select new TestWebModel()
                                    {
                                        ID = p.ID,
                                        Remark = p.Remark,
                                        OrderIndex = p.OrderIndex,
                                        ReportDate = p.ReportDate.ToLocalTime().ToString("yyyy-MM-dd"),
                                        MID = p.MID,
                                        MName = p.MName,
                                        MTime = p.MTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                                        CID = p.CID,
                                        CName = p.CName,
                                        CTime = p.CTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")
                                    }).ToList();

            return result;
        }
    }
}
