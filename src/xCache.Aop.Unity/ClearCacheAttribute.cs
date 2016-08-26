using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace xCache.Aop.Unity
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ClearCacheAttribute : HandlerAttribute
    {
        public string CacheKey { get; set; }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            var handler = new ClearCacheAttributeCallHandler(container)
            {
                Order = Order,
                CacheKey = CacheKey
            };

            return handler;
        }
    }
}
