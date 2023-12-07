using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using Whistler.Helpers;

namespace Whistler.SDK
{
    public static class Timers
    {
        public static Dictionary<string, WhistlerTimer> timers = new Dictionary<string, WhistlerTimer>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Timers));
        private static Timer _timer;

        private static int delay;
        private static int clearDelay;

        public static void Init()
        {
            delay = Main.ServerConfig.Timers.Delay;
            _timer = new Timer(Logic, null, 0, delay);
        }
        private static void Logic(object state)
        {
            try
            {
                lock (timers)
                {
                    timers.Values.ToList().ForEach(timer => {
                        if (timer.Elapsed()) timers.Remove(timer.ID);
                    });
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Timers.Logic: {e}");
            }
        }

        
        /// <summary>
        /// Start() запускает таймер и возвращает случайный ID
        /// </summary>
        /// <param name="interval">Интервал срабатывания действия</param>
        /// <param name="action">Лямбда-выражение с действием</param>
        /// <returns>Уникальный ID таймера</returns>
        public static string Start(int interval, Action action)
        {
            string id = Guid.NewGuid().ToString();
            try
            {
                lock (timers)
                {
                    timers.Add(id, new WhistlerTimer(action, id, interval));
                    return id;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Timer.Start.{id}.Error: {e}");
                return null;
            }
        }

        /// <summary>
        /// Start() запускает таймер и возвращает случайный ID
        /// </summary>
        /// <param name="interval">Интервал срабатывания действия</param>
        /// <param name="action">Лямбда-выражение с действием</param>
        /// <returns>Уникальный ID таймера</returns>
        public static string Start(string key, int interval, Action action)
        {
            string id = Guid.NewGuid().ToString();
            try
            {
                lock (timers)
                {
                    timers.Add(id, new WhistlerTimer(action, id, interval));
                    return id;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Timer.Start.{id}.Error: {e}");
                return null;
            }
        }

        /// <summary>
        /// StartOnce() запускает таймер один раз и возвращает случайный ID
        /// </summary>
        /// <param name="interval">Интервал срабатывания действия</param>
        /// <param name="action">Лямбда-выражение с действием</param>
        /// <returns>Уникальный ID таймера</returns>
        public static string StartOnce(int interval, Action action)
        {
            string id = Guid.NewGuid().ToString();
            try
            {
                lock (timers)
                {
                    timers.Add(id, new WhistlerTimer(action, id, interval, true));
                    return id;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Timer.Start.{id}.Error: {e}");
                return null;
            }
        }


        /// <summary>
        /// StartTask() запускает таймер отдельной задачей и возвращает случайный ID
        /// </summary>
        /// <param name="interval">Интервал срабатывания действия</param>
        /// <param name="action">Лямбда-выражение с действием</param>
        /// <returns>Уникальный ID таймера</returns>
        public static string StartTask(int interval, Action action)
        {
            string id = Guid.NewGuid().ToString();
            try
            {
                lock (timers)
                {
                    timers.Add(id, new WhistlerTimer(action, id, interval, false, true));
                    return id;
                }
                
            }
            catch (Exception e)
            {
                _logger.WriteError($"Timer.Start.{id}.Error: {e}");
                return null;
            }
        }

        /// <summary>
        /// StartTask() запускает таймер отдельной задачей и возвращает ID
        /// </summary>
        /// <exception>
        /// Exception возникает при передаче уже существующего ID или значения null
        /// </exception>
        /// <param name="id">Уникальный идентификатор таймера</param>
        /// <param name="interval">Интервал срабатывания действия</param>
        /// <param name="action">Лямбда-выражение с действием</param>
        /// <returns>Уникальный ID таймера</returns>
        public static string StartTask(string id, int interval, Action action)
        {
            try
            {
                lock (timers)
                {
                    if (timers.ContainsKey(id)) throw new Exception("This id is already in use!");
                    if (id is null) throw new Exception("Id cannot be null");

                    timers.Add(id, new WhistlerTimer(action, id, interval, false, true));
                    return id;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Timer.Start.{id}.Error: {e}");
                return null;
            }
        }

        /// <summary>
        /// StartOnceTask() запускает таймер один раз отдельной задачей и возвращает ID
        /// </summary>
        /// <exception>
        /// Exception возникает при передаче уже существующего ID или значения null
        /// </exception>
        /// <param name="id">Уникальный идентификатор таймера</param>
        /// <param name="interval">Интервал срабатывания действия</param>
        /// <param name="action">Лямбда-выражение с действием</param>
        /// <returns>Уникальный ID таймера</returns>
        public static string StartOnceTask(string id, int interval, Action action)
        {
            try
            {
                if (id is null) throw new Exception("Id cannot be null");
                lock (timers)
                { 
                    if (timers.ContainsKey(id)) throw new Exception("This id is already in use!");

                    timers.Add(id, new WhistlerTimer(action, id, interval, true, true));
                    return id;
                }
               
            }
            catch (Exception e)
            {
                _logger.WriteError($"StartOnceTask {id}: {e}");
                return null;
            }
        }

        public static void Stop(string id)
        {
            if (id is null) _logger.WriteWarning("Trying to stop timer with NULL ID");
            else
            {
                lock (timers)
                {
                    if (timers.ContainsKey(id))
                    {
                        timers.Remove(id);
                    }
                }
            }      
        }
    }
}