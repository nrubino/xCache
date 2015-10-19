﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace xCache.Tests.Core
{
    public abstract class AopTests
    {
        protected IAop _aop = null;

        [Fact]
        public void TestFiveSecondTimeout()
        {
            var now = _aop.GetCurrentDateAsStringFiveSecondCache();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = _aop.GetCurrentDateAsStringFiveSecondCache();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = _aop.GetCurrentDateAsStringFiveSecondCache();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = _aop.GetCurrentDateAsStringFiveSecondCache();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        public async Task TestFiveSecondTimeoutAsync()
        {
            var now = await _aop.GetCurrentDataAsStringFiveSecondCacheAsync();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = await _aop.GetCurrentDataAsStringFiveSecondCacheAsync();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = await _aop.GetCurrentDataAsStringFiveSecondCacheAsync();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = await _aop.GetCurrentDataAsStringFiveSecondCacheAsync();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        public void TestFiveSecondTimeoutStruct()
        {
            var now = _aop.GetCurrentDateTimeFiveSecondCache();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = _aop.GetCurrentDateTimeFiveSecondCache();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = _aop.GetCurrentDateTimeFiveSecondCache();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = _aop.GetCurrentDateTimeFiveSecondCache();

            Assert.Equal(cached2, cached3);
        }

        [Fact]
        public async Task TestFiveSecondTimeoutStructAsync()
        {
            var now = await _aop.GetCurrentDateTimeFiveSecondCacheAsync();

            Thread.Sleep(new TimeSpan(0, 0, 2));

            var cached = await _aop.GetCurrentDateTimeFiveSecondCacheAsync();

            Assert.Equal(now, cached);

            Thread.Sleep(new TimeSpan(0, 0, 5));

            var cached2 = await _aop.GetCurrentDateTimeFiveSecondCacheAsync();

            Assert.NotEqual(now, cached2);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            var cached3 = await _aop.GetCurrentDateTimeFiveSecondCacheAsync();

            Assert.Equal(cached2, cached3);
        }
    }
}
