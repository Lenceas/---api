using Castle.DynamicProxy;

namespace MorningStar.Api
{
    public class MyInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"Intercept before，Method：{invocation.Method.Name}");
            invocation.Proceed();
            Console.WriteLine($"Intercept after，Method：{invocation.Method.Name}");
        }
    }
}
