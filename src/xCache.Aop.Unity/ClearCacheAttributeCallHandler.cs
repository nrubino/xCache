using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using xCache.Extensions;

namespace xCache.Aop.Unity
{
    public class ClearCacheAttributeCallHandler : ICallHandler
    {
        public int Order { get; set; }
        public string CacheKey { get; set; }

        private readonly ICache _cache;
        private readonly ICacheKeyGenerator _keyGenerator;

        public ClearCacheAttributeCallHandler(IUnityContainer container)
        {
            _cache = container.Resolve<ICache>();
            _keyGenerator = container.Resolve<ICacheKeyGenerator>();
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var cacheKey = string.IsNullOrEmpty(CacheKey) ? _keyGenerator.GenerateKey(input) : CacheKey;

            typeof(CacheExtensions).GetMethod("RemoveFromCache")
                    .Invoke(null, new object[] {_cache, cacheKey});
            
            var arguments = new object[input.Inputs.Count];
            input.Inputs.CopyTo(arguments, 0);

            return new VirtualMethodReturn(input, getNext()(input, getNext).ReturnValue, arguments);
        }

    }
}
