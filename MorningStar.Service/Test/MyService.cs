namespace MorningStar.Service
{
    public class MyService : IBaseService, IMyService
    {
        public void ShowCode()
        {
            Console.WriteLine($"MyService.ShowCode：{GetHashCode()}");
        }
    }

    public class MyServiceV2 : IBaseService, IMyService
    {
        public MyNameService? NameService { get; set; }

        public void ShowCode()
        {
            Console.WriteLine($"MyServiceV2.ShowCode：{GetHashCode()}，NameService是否为空：{NameService == null}");
        }
    }

    public class MyNameService
    {

    }
}
