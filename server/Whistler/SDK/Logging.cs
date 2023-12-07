using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Whistler.SDK
{
    public class nLog
    {
        public nLog(string _reference = null, bool _canDebug = false)
        {
            if (_reference == null) _reference = "Logger";
            Reference = _reference;
            CanDebug = _canDebug;
        }
        public nLog(System.Type type, bool _canDebug = false)
        {
            Reference = type.FullName;
            CanDebug = _canDebug;
        }
        public string Reference { get; set; }
        public bool CanDebug { get; set; }

        public enum Type
        {
            Info,
            Warn,
            Error,
            Success
        };

        public void Write(string text, Type logType = Type.Info)
        {
            try
            {
                Console.ResetColor();
                Console.Write($"{DateTime.Now.ToString("HH':'mm':'ss.fff")}|");
                switch (logType)
                {
                    case Type.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Error");
                        break;
                    case Type.Warn:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Warn");
                        break;
                    case Type.Info:
                        Console.Write("Info");
                        break;
                    case Type.Success:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Succ");
                        break;
                    default:
                        return;
                }
                Console.ResetColor();
                Console.Write($"|{Reference}: {text}\n");
            }
            catch {}
        }
        public async Task WriteAsync(string text, Type logType = Type.Info)
        {
            await Task.Run(() =>
            {
                try
                {
                    Console.ResetColor();
                    Console.Write($"{DateTime.Now.ToString("HH':'mm':'ss.fff")}|");
                    switch (logType)
                    {
                        case Type.Error:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Error");
                            break;
                        case Type.Warn:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Warn");
                            break;
                        case Type.Info:
                            Console.Write("Info");
                            break;
                        case Type.Success:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Succ");
                            break;
                        default:
                            return;
                    }
                    Console.ResetColor();
                    Console.Write($"|{Reference}: {text}\n");

                }catch {}
            });
        }
        public void Debug(string text, Type logType = Type.Info)
        {
            try
            {
                if (!CanDebug) return;
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{DateTime.Now.ToString("HH':'mm':'ss.fff")}");
                Console.ResetColor();
                Console.Write($"|");
                switch (logType)
                {
                    case Type.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Error");
                        break;
                    case Type.Warn:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Warn");
                        break;
                    case Type.Info:
                        Console.Write("Info");
                        break;
                    case Type.Success:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Succ");
                        break;
                    default:
                        return;
                }
                Console.ResetColor();
                Console.Write($"|{Reference}: {text}\n");
            }
            catch { }
        }
        public async Task DebugAsync(string text, Type logType = Type.Info)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!CanDebug) return;
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{DateTime.Now.ToString("HH':'mm':'ss.fff")}");
                    Console.ResetColor();
                    Console.Write($"|");
                    switch (logType)
                    {
                        case Type.Error:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Error");
                            break;
                        case Type.Warn:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Warn");
                            break;
                        case Type.Info:
                            Console.Write("Info");
                            break;
                        case Type.Success:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Succ");
                            break;
                        default:
                            return;
                    }
                    Console.ResetColor();
                    Console.Write($"|{Reference}: {text}\n");
                }
                catch {}               
            });
        }

    }
}
