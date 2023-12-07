using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using Whistler.Helpers;
using System.Timers;

namespace Whistler.SDK
{
    public static class Timers
    {
        public static ConcurrentDictionary<string, WhistlerTimer> TimersData = new ConcurrentDictionary<string, WhistlerTimer>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Timers));
        private static Thread thread;

        private static int delay;

        public static void Init()
        {
            delay = Main.ServerConfig.Timers.Delay;
            thread = new Thread(Logic)
            {
                IsBackground = true,
                Name = "TimersThread"
            };
            thread.Start();
        }
        private static void Logic(object state)
        {
            WhistlerTimer timer;
            while (true)
            {
                try
                {
                    DateTime now = DateTime.Now;
                    foreach (string TimerId in TimersData.Keys.ToList())
                    {
                        if (!TimersData.ContainsKey(TimerId)) continue;

                        timer = TimersData[TimerId];
                        if (timer == null) continue;

                        if (!timer.isFinished)
                        {
                            timer.Elapsed(now);
                            continue;
                        }
                        if (timer.isFinished && TimersData.ContainsKey(timer.ID)) TimersData.TryRemove(timer.ID, out _);
                    }
                    Thread.Sleep(delay);
                }
                catch (Exception e)
                {
                    _logger.WriteError($"Logic While Exception: {e.ToString()}");
                }
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
                lock (TimersData)
                {
                    TimersData.TryAdd(id, new WhistlerTimer(action, id, interval));
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
                lock (TimersData)
                {
                    TimersData.TryAdd(id, new WhistlerTimer(action, id, interval));
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
                lock (TimersData)
                {
                    TimersData.TryAdd(id, new WhistlerTimer(action, id, interval, true));
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
                lock (TimersData)
                {
                    TimersData.TryAdd(id, new WhistlerTimer(action, id, interval, false, true));
                    return id;
                }
                
            }
            catch (Exception e)
            {
                _logger.WriteError($"Timer.Start.{id}.Error: {e}");
                return null;
            }
        }

        public static string StartTask(int interval, Func<Task> action)
        {
            string id = Guid.NewGuid().ToString();
            try
            {
                lock (TimersData)
                {
                    TimersData.TryAdd(id, new WhistlerTimer(action, id, interval, false, true));
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
                lock (TimersData)
                {
                    if (TimersData.ContainsKey(id)) throw new Exception("This id is already in use!");
                    if (id is null) throw new Exception("Id cannot be null");

                    TimersData.TryAdd(id, new WhistlerTimer(action, id, interval, false, true));
                    return id;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Timer.Start.{id}.Error: {e}");
                return null;
            }
        }

        public static string StartTask(string id, int interval, Func<Task> action)
        {
            try
            {
                lock (TimersData)
                {
                    if (TimersData.ContainsKey(id)) throw new Exception("This id is already in use!");
                    if (id is null) throw new Exception("Id cannot be null");

                    TimersData.TryAdd(id, new WhistlerTimer(action, id, interval, false, true));
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
                lock (TimersData)
                { 
                    if (TimersData.ContainsKey(id)) throw new Exception("This id is already in use!");

                    TimersData.TryAdd(id, new WhistlerTimer(action, id, interval, true, true));
                    return id;
                }
               
            }
            catch (Exception e)
            {
                _logger.WriteError($"StartOnceTask {id}: {e}");
                return null;
            }
        }

        public static string StartOnceTask(string id, int interval, Func<Task> action)
        {
            try
            {
                if (id is null) throw new Exception("Id cannot be null");
                lock (TimersData)
                {
                    if (TimersData.ContainsKey(id)) throw new Exception("This id is already in use!");

                    TimersData.TryAdd(id, new WhistlerTimer(action, id, interval, true, true));
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
                lock (TimersData)
                {
                    if (!TimersData.ContainsKey(id)) return;

                    TimersData[id].isFinished = true;
                    TimersData.TryRemove(id, out _);
                }
            }      
        }
    }
}