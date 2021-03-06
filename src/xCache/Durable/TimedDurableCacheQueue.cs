﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Timers;

namespace xCache.Durable
{
    public class TimedDurableCacheQueue : IDurableCacheQueue
    {
        private readonly ConcurrentDictionary<Guid, Timer> _timers;
        private readonly IDurableCacheRefreshHandler _handler;
        private readonly Timer _cleanup;

        public TimedDurableCacheQueue(IDurableCacheRefreshHandler handler, TimeSpan cleanupInterval)
        {
            _handler = handler;
            _timers = new ConcurrentDictionary<Guid, Timer>();

            _cleanup = new Timer
            {
                AutoReset = true,
                Interval = cleanupInterval.TotalMilliseconds
            };

            _cleanup.Elapsed += (s, e) =>
            {
                var count = 0;

                foreach (var item in _timers)
                {
                    if (!item.Value.Enabled)
                    {
                        Timer timer = null;
                        if (_timers.TryRemove(item.Key, out timer))
                        {
                            timer.Close();
                            timer.Dispose();
                            count++;
                        }
                        else
                        {
                            Trace.TraceWarning("Unable to remove timer from dictionary for key {0}", item.Key);
                        }
                    }
                }

                var timerCountAfterCleanup = _timers.Count;
            };

            _cleanup.Start();
        }

        public void Purge()
        {
            foreach (var timer in _timers)
            {
                timer.Value.Enabled = false;
            }
        }


        public void ScheduleRefresh(DurableCacheRefreshEvent refreshEvent)
        {
            var timer = new Timer
            {
                AutoReset = true,
                Interval = refreshEvent.RefreshTime.TotalMilliseconds,
            };

            timer.Elapsed += (s, e) =>
            {
                _handler.Handle(refreshEvent);
                if (refreshEvent.UtcLifetime <= DateTime.UtcNow.Add(refreshEvent.RefreshTime))
                {
                    timer.Stop();
                }
            };

            timer.Start();

            if (!_timers.TryAdd(Guid.NewGuid(), timer))
            {
                Trace.TraceWarning("Unable to add timer to dictionary for key {0}", refreshEvent.Key);
            };
        }
    }
}
