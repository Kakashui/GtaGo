using System;
using System.Collections.Generic;
using System.Text;
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

        public Action action { get; set; }

        public bool isOnce { get; set; }
        public bool isTask { get; set; }
        public bool isFinished { get; set; }

        public WhistlerTimer(Action action_, string id_, int ms_, bool isonce_ = false, bool istask_ = false)
        {
            action = action_;

            ID = id_;
            MS = ms_;
            Next = DateTime.Now.AddMilliseconds(MS);

            isOnce = isonce_;
            isTask = istask_;
            isFinished = false;
        }

        public bool Elapsed()
        {
            try
            {
                if (isFinished) return true;

                if (Next <= DateTime.Now)
                {
                    if (isOnce) isFinished = true;
                    else Next = DateTime.Now.AddMilliseconds(MS);
                    if (isTask) Task.Run(() => action.Invoke());
                    else WhistlerTask.Run(action);
                }

                return false;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Timer.Elapsed.{ID}.Error: {e}");
                return true;          
            }
        }
    }
}
