using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whistler.Helpers;

namespace Whistler.SDK
{
    public class WhistlerTimer
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(WhistlerTimer));
        public string ID { get; }
        public int MS { get; set; }
        public DateTime Next { get; private set; }

        public Action action { get; set; } = null;
        public Func<Task> taskAction { get; set; } = null;

        public bool isOnce { get; set; }
        public bool isTask { get; set; }
        public bool isFinished { get; set; }

        public WhistlerTimer(Action action_, string id_, int ms_, bool isonce_ = false, bool istask_ = false)
        {
            action = action_;
            taskAction = null;

            ID = id_;
            MS = ms_;
            Next = DateTime.Now.AddMilliseconds(MS);

            isOnce = isonce_;
            isTask = istask_;
            isFinished = false;
        }

        public WhistlerTimer(Func<Task> action_, string id_, int ms_, bool isonce_ = false, bool istask_ = false)
        {
            taskAction = action_;
            action = null;

            ID = id_;
            MS = ms_;
            Next = DateTime.Now.AddMilliseconds(MS);

            isOnce = isonce_;
            isTask = istask_;
            isFinished = false;
        }

        public void Elapsed(DateTime now)
        {
            try
            {
                if (this == null || !Timers.TimersData.Values.Contains(this)) return;
                if (isFinished) return;
                if (Next > now) return;

                if (isOnce) isFinished = true;
                Next = now.AddMilliseconds(MS);
                _logger.WriteDebug($"Timer.{ID}.Invoke");

                if (isTask)
                {
                    Task.Factory.StartNew(() =>
                    {
                        if (action != null)
                        {
                            try
                            {
                                action.Invoke();
                            }
                            catch (Exception e)
                            {
                                _logger.WriteError($"Elapsed({ID},MS:{MS},ONCE:{isOnce},From:{action.Target?.ToString()}) Task #1 Exception: {e.ToString()}");
                            }
                            return;
                        }
                        if (taskAction != null)
                        {
                            try
                            {
                                taskAction.Invoke();
                            }
                            catch (Exception e)
                            {
                                _logger.WriteError($"Elapsed({ID},MS:{MS},ONCE:{isOnce},From:{taskAction.Target?.ToString()}) Task #2 Exception: {e.ToString()}");
                            }
                        }
                        
                    }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
                else NAPI.Task.Run(action);
                _logger.WriteDebug($"Timer.{ID}.Completed");
            }
            catch (Exception e)
            {
                _logger.WriteError($"Elapsed({ID},MS:{MS},ONCE:{isOnce},From:{action.Target?.ToString()}) Exception: {e.ToString()}");
            }
        }
    }
}
