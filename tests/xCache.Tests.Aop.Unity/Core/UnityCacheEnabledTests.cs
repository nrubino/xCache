﻿using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using xCache.Aop.Unity;
using xCache.Aop.Unity.Durable;
using xCache.Durable;
using xCache.Tests.Core;

namespace xCache.Tests.Aop.Unity.Core
{
    public class UnityCacheEnabledTests : CacheEnabledTests
    {
        IUnityContainer _container;

        public UnityCacheEnabledTests()
        {    
            _container = new UnityContainer();

            //Register interception
            _container.AddNewExtension<Interception>();

            //Register xCache
            _container.RegisterType<ICache, MemoryCache>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICache, MemoryCache>("One", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICache, MemoryCache>("Two", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICache, DictionaryCache>("DictionaryCache", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICacheKeyGenerator,JsonCacheKeyGenerator>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDurableCacheQueue, TimedDurableCacheQueue>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory((c) => new TimedDurableCacheQueue(c.Resolve<IDurableCacheRefreshHandler>(), new TimeSpan(0,0,30))));
            _container.RegisterType<IDurableCacheRefreshHandler, UnityDurableCacheRefreshHandler>(new ContainerControlledLifetimeManager());

            //Register test class with interception
            _container.RegisterType<ICacheEnableObject, UnityCacheEnabledObject>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<InterfaceInterceptor>());

            _cached = _container.Resolve<ICacheEnableObject>();
        }

        protected override void PurgeDurableCacheQueue()
        {
            var queue = _container.Resolve<IDurableCacheQueue>();
            queue.Purge();
        }

        protected override void PurgeDictionaryCache()
        {
            //TODO figure out how to dispose this through unity
            var dictionary = (DictionaryCache)_container.Resolve<ICache>("DictionaryCache");
            dictionary.Purge();
        }
    }
}
