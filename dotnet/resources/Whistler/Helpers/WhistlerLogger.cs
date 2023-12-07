using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Whistler.Helpers
{
   
    class WhistlerLogger
    {
        private enum LogTypes
        {
            Error,
            Info,
            Warning,
            Debug,
            Player,
            Unhandled
        }

        private class LogModel
        {
            public LogModel(LogTypes type, string text)
            {
                Type = type;
                Text = text;
                Date = DateTime.Now;
            }
            public DateTime Date { get; set; }
            public LogTypes Type { get; set; }
            public string Text { get; set; }
        }

        private string _type;
        private static string _dirName;
        private static ConcurrentQueue<LogModel> _queue = new ConcurrentQueue<LogModel>();
        public WhistlerLogger(Type type, bool _canDebug = false)
        {
            _type = type.FullName;
        }
        static WhistlerLogger()
        {
            if (!Directory.Exists("WhistlerLogs"))
                Directory.CreateDirectory("WhistlerLogs");

            _dirName = $"WhistlerLogs/{DateTime.Now.ToString("yyyy_MM_dd")}";
            if (!Directory.Exists(_dirName))
                Directory.CreateDirectory(_dirName);
            Start();
        }
        public void WriteDebug(string text)
        {
            #if DEBUG
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(text);
                Console.ResetColor();
                _queue.Enqueue(new LogModel(LogTypes.Debug, text));
            #endif
        }
        public void WriteInfo(string text)
        {
            _queue.Enqueue(new LogModel(LogTypes.Info, text));
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public void WriteError(string text)
        {
            _queue.Enqueue(new LogModel(LogTypes.Error, text));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public void WriteUnhandled(string text)
        {
            _queue.Enqueue(new LogModel(LogTypes.Unhandled, text));
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public void WriteWarning(string text)
        {
            _queue.Enqueue(new LogModel(LogTypes.Warning, text));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public void WriteWarningOnlyFile(string text)
        {
            _queue.Enqueue(new LogModel(LogTypes.Warning, text));
        }
        public void WriteClient(string text)
        {
            _queue.Enqueue(new LogModel(LogTypes.Player, text));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        private static async Task Tick()
        {
            try
            {
                while (true)
                {
                    if (_queue.Count > 0)
                    {
                        if (_queue.TryDequeue(out LogModel log))
                        {
                            var path = _dirName;
                            switch (log?.Type)
                            {
                                case LogTypes.Error:
                                    path += "/Errors.log";
                                    break;
                                case LogTypes.Info:
                                    path += "/Infos.log";
                                    break;
                                case LogTypes.Warning:
                                    path += "/Warnings.log";
                                    break;
                                case LogTypes.Debug:
                                    path += "/Debugs.log";
                                    break;
                                case LogTypes.Player:
                                    path += "/Player.log";
                                    break;
                                case LogTypes.Unhandled:
                                    path += "/Unhandled.log";
                                    break;
                                default:
                                    path += "/Other.log";
                                    break;
                            }
                            using (var w = new StreamWriter(path, true))
                            {
                                await w.WriteLineAsync($"{log.Date}: {log.Type}\n{log.Text}");
                            }
                        }
                    }
                    await Task.Delay(500);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"wLogger: {e}");
            }
        }
        private static void Start()
        {
            Task.Factory.StartNew(Tick);           
        }
    }
}
