using System;

namespace NITHdmis.MicroLibrary
{
    /// <summary>
    /// MicroStopwatch class
    /// </summary>
    public class MicroStopwatch : System.Diagnostics.Stopwatch
    {
        private readonly double _microSecPerTick =
            1000000D / System.Diagnostics.Stopwatch.Frequency;

        public long ElapsedMicroseconds
        {
            get
            {
                return (long)(ElapsedTicks * _microSecPerTick);
            }
        }

        public MicroStopwatch()
        {
            if (!System.Diagnostics.Stopwatch.IsHighResolution)
            {
                throw new Exception("On this system the high-resolution " +
                                    "performance counter is not available");
            }
        }
    }

    /// <summary>
    /// MicroTimer class
    /// </summary>
    public class MicroTimer
    {
        private long _ignoreEventIfLateBy = long.MaxValue;

        private bool _stopTimer = true;

        private System.Threading.Thread _threadTimer = null;

        private long _timerIntervalInMicroSec = 0;

        public bool Enabled
        {
            set
            {
                if (value)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }
            get
            {
                return (_threadTimer != null && _threadTimer.IsAlive);
            }
        }

        public long IgnoreEventIfLateBy
        {
            get
            {
                return System.Threading.Interlocked.Read(
                    ref _ignoreEventIfLateBy);
            }
            set
            {
                System.Threading.Interlocked.Exchange(
                    ref _ignoreEventIfLateBy, value <= 0 ? long.MaxValue : value);
            }
        }

        public long Interval
        {
            get
            {
                return System.Threading.Interlocked.Read(
                    ref _timerIntervalInMicroSec);
            }
            set
            {
                System.Threading.Interlocked.Exchange(
                    ref _timerIntervalInMicroSec, value);
            }
        }

        public MicroTimer()
        {
        }

        public MicroTimer(long timerIntervalInMicroseconds)
        {
            Interval = timerIntervalInMicroseconds;
        }

        public delegate void MicroTimerElapsedEventHandler(
                                                                                                     object sender,
                             MicroTimerEventArgs timerEventArgs);

        public event EventHandler<MicroTimerEventArgs> MicroTimerElapsed;

        public void Abort()
        {
            _stopTimer = true;

            if (Enabled)
            {
                _threadTimer.Abort();
            }
        }

        public void Start()
        {
            if (Enabled || Interval <= 0)
            {
                return;
            }

            _stopTimer = false;

            System.Threading.ThreadStart threadStart = delegate ()
            {
                NotificationTimer(ref _timerIntervalInMicroSec,
                                  ref _ignoreEventIfLateBy,
                                  ref _stopTimer);
            };

            _threadTimer = new System.Threading.Thread(threadStart);
            _threadTimer.Priority = System.Threading.ThreadPriority.Highest;
            _threadTimer.Start();
        }

        public void Stop()
        {
            _stopTimer = true;
        }

        public void StopAndWait()
        {
            StopAndWait(System.Threading.Timeout.Infinite);
        }

        public bool StopAndWait(int timeoutInMilliSec)
        {
            _stopTimer = true;

            if (!Enabled || _threadTimer.ManagedThreadId ==
                System.Threading.Thread.CurrentThread.ManagedThreadId)
            {
                return true;
            }

            return _threadTimer.Join(timeoutInMilliSec);
        }

        private void NotificationTimer(ref long timerIntervalInMicroSec,
                               ref long ignoreEventIfLateBy,
                               ref bool stopTimer)
        {
            int timerCount = 0;
            long nextNotification = 0;

            MicroStopwatch microStopwatch = new MicroStopwatch();
            microStopwatch.Start();

            while (!stopTimer)
            {
                long callbackFunctionExecutionTime =
                    microStopwatch.ElapsedMicroseconds - nextNotification;

                long timerIntervalInMicroSecCurrent =
                    System.Threading.Interlocked.Read(ref timerIntervalInMicroSec);
                long ignoreEventIfLateByCurrent =
                    System.Threading.Interlocked.Read(ref ignoreEventIfLateBy);

                nextNotification += timerIntervalInMicroSecCurrent;
                timerCount++;
                long elapsedMicroseconds;
                while ((elapsedMicroseconds = microStopwatch.ElapsedMicroseconds)
                        < nextNotification)
                {
                    System.Threading.Thread.SpinWait(10);
                }

                long timerLateBy = elapsedMicroseconds - nextNotification;

                if (timerLateBy >= ignoreEventIfLateByCurrent)
                {
                    continue;
                }

                MicroTimerEventArgs microTimerEventArgs =
                     new MicroTimerEventArgs(timerCount,
                                             elapsedMicroseconds,
                                             timerLateBy,
                                             callbackFunctionExecutionTime);
                MicroTimerElapsed(this, microTimerEventArgs);
            }

            microStopwatch.Stop();
        }
    }

    /// <summary>
    /// MicroTimer Event Argument class
    /// </summary>
    public class MicroTimerEventArgs : EventArgs
    {
        // Time it took to execute previous call to callback function (OnTimedEvent)
        public long CallbackFunctionExecutionTime { get; }

        // Time when timed event was called since timer started
        public long ElapsedMicroseconds { get; }

        // Simple counter, number times timed event (callback function) executed
        public int TimerCount { get; }

        // How late the timer was compared to when it should have been called
        public long TimerLateBy { get; }

        public MicroTimerEventArgs(int timerCount,
                                   long elapsedMicroseconds,
                                   long timerLateBy,
                                   long callbackFunctionExecutionTime)
        {
            TimerCount = timerCount;
            ElapsedMicroseconds = elapsedMicroseconds;
            TimerLateBy = timerLateBy;
            CallbackFunctionExecutionTime = callbackFunctionExecutionTime;
        }
    }
}