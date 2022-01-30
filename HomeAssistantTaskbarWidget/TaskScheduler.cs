﻿using System;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace HomeAssistantTaskbarWidget
{
    public class TaskScheduler : ITaskScheduler
    {
        private int _interval  = 30000;
        private Func<Task> _task;
        private Timer _timer;
        private ILogger _logger;

        public TaskScheduler(ILogger logger)
        {
            _logger = logger;
        }

        public TaskScheduler SetInterval(int? seconds)
        {
            if (seconds.HasValue)
            {
                _interval = seconds.Value * 1000;
            }

            return this;
        }

        public TaskScheduler SetTaskHandler(Func<Task> task)
        {
            _task = task;

            return this;
        }

        public TaskScheduler Start()
        {
            _timer = new Timer();
            _timer.Interval = _interval;
            _timer.Elapsed += Handler;
            _timer.Start();

            Task.Run(() => {
                Task.Delay(2000);
                Handler(null, null);
            });

            return this;
        }

        private async void Handler(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_task == null)
                    throw new ArgumentException("Undefined task in scheduler");

                await _task();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                Stop();
            }
        }

        public TaskScheduler Stop()
        {
            _timer?.Stop();

            return this;
        }
    }
}
