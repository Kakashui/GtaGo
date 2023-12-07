using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using GTANetworkAPI;
using System.Linq;
using Whistler.Helpers;
using System.Collections.Concurrent;
using System.Threading;

namespace Whistler.SDK
{
    public static class MySQL
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MySQL));
        private static ConcurrentQueue<MySqlCommand> _queueCommands = new ConcurrentQueue<MySqlCommand>();

        private static Thread _queryThread { get; set; }

        private static string Connection = null;

        public static bool Debug = false;

        private static void ThreadQuery()
        {
            using (MySqlConnection connection = new MySqlConnection(Connection))
            {
                connection.Open();
                while (true)
                {
                    try
                    {
                        //Test();
                        while (_queueCommands.Count > 0 && _queueCommands.TryDequeue(out MySqlCommand cmd))
                        {
                            cmd.Connection = connection;
                            cmd.ExecuteNonQuery();
                        }
                        Thread.Sleep(200);
                    }
                    catch (Exception e)
                    {
                        if(connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        _logger.WriteError(e.ToString());
                    }
                }
            }
        }

        public static void Init()
        {
            if (Connection is string) return;
            Connection = 
                $"Host={Main.ServerConfig.MySQL.Server};" +
                $"Port={Main.ServerConfig.MySQL.Port};" +
                $"User={Main.ServerConfig.MySQL.User};" +
                $"Password={Main.ServerConfig.MySQL.Password};" +
                $"Database={Main.ServerConfig.MySQL.DataBase};" +
                $"{Main.ServerConfig.MySQL.SSL}";
            _queryThread = new Thread(ThreadQuery);
            _queryThread.IsBackground = true;
            _queryThread.Start();
        }

        /// <summary>
        /// Тест соединения с базой
        /// </summary>
        /// <returns>True - если все хорошо</returns>
        public static bool Test()
        {
            _logger.WriteInfo("Testing connection...");
            try
            {
                using(MySqlConnection conn = new MySqlConnection(Connection))
                {
                    conn.Open();
                    _logger.WriteInfo("Connection is successful!");
                    conn.Close();
                }
                return true;
            }
            catch (ArgumentException ex)
            {
                _logger.WriteError($"Сonnection string contains an error\n{ex}");
                return false;
            }
            catch (MySqlException me)
            {
                switch (me.Number)
                {
                    case 1042:
                        _logger.WriteError("Unable to connect to any of the specified MySQL hosts");
                        break;
                    case 0:
                        _logger.WriteError("Access denied");
                        break;
                    default:
                        _logger.WriteError($"({me.Number}) {me}");
                        break;
                }
                return false;
            }
        }

        public static void Query(string command)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(command);
                _queueCommands.Enqueue(cmd);
            }
            catch (Exception e)
            {
                var trace = new System.Diagnostics.StackTrace();
                _logger.WriteError(e.ToString() + $"\n command: {command} \n stacktrace: {trace}");
            }
        }

        /// <summary>
        /// Выполнить запрос без ответа
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        public static void Query(string command, params object[] args)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(command);
                _queueCommands.Enqueue(cmd.LoadArguments(args));
            }
            catch(Exception e)
            {
                var trace = new System.Diagnostics.StackTrace();
                _logger.WriteError(e.ToString() + $"\n command: {command} \n stacktrace: {trace}");
            }
        }

        public static void Query(MySqlCommand command)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(Connection);
                connection.Open();
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                var trace = new System.Diagnostics.StackTrace();
                _logger.WriteError(e.ToString() + $"\n command: {command} \n stacktrace: {trace}");
            }
        }

        public static void QuerySync(string command)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(command);
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                var trace = new System.Diagnostics.StackTrace();
                _logger.WriteError(e.ToString() + $"\n command: {command} \n stacktrace: {trace}");
            }
        }

        /// <summary>
        /// Выполнить запрос без добавления в асинхронную очередь
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        public static void QuerySync(string command, params object[] args)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(command);
                    cmd.Connection = connection;
                    cmd.LoadArguments(args);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                var trace = new System.Diagnostics.StackTrace();
                _logger.WriteError(e.ToString() + $"\n command: {command} \n stacktrace: {trace}");
            }
        }

        public static async Task QueryAsync(MySqlCommand command)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(Connection);
                await connection.OpenAsync();
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                var trace = new System.Diagnostics.StackTrace();
                _logger.WriteError(e.ToString() + $"\n command: {command} \n stacktrace: {trace}");
            }
        }

        /// <summary>
        /// Отправить запрос и считать ответ
        /// </summary>
        /// <param name="command">Передаем заранее составленную команду</param>
        /// <returns>Ответ базы данных в формате таблицы</returns>
        private static DataTable QueryRead(MySqlCommand command)
        {
            if (Debug) _logger.WriteDebug("Query to DB:\n" + command.CommandText);
            using (MySqlConnection connection = new MySqlConnection(Connection))
            {
                connection.Open();

                command.Connection = connection;

                DbDataReader reader = command.ExecuteReader();
                DataTable result = new DataTable();
                result.Load(reader);

                return result;
            }
        }

        public static DataTable QueryRead(string command)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {
                return QueryRead(cmd);
            }
        }

        /// <summary>
        /// Отправить запрос и считать ответ
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DataTable QueryRead(string command, params object[] args)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {
                cmd.LoadArguments(args);
                return QueryRead(cmd);
            }
        }

        /// <summary>
        /// Асинхронная версия Read
        /// </summary>
        /// <param name="command">Передаем заранее составленную команду</param>
        /// <returns>Ответ базы данных в формате таблицы</returns>
        private static async Task<DataTable> QueryReadAsync(MySqlCommand command)
        {
            if (Debug) _logger.WriteDebug("Query to DB:\n" + command.CommandText);
            using (MySqlConnection connection = new MySqlConnection(Connection))
            {
                await connection.OpenAsync();

                command.Connection = connection;

                DbDataReader reader = await command.ExecuteReaderAsync();
                DataTable result = new DataTable();
                result.Load(reader);

                return result;
            }
        }

        public static async Task<DataTable> QueryReadAsync(string command, params object[] args)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {
                cmd.LoadArguments(args);
                return await QueryReadAsync(cmd);
            }
        }

        public static async Task<DataTable> QueryReadAsync(string command)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {
                return await QueryReadAsync(cmd);
            }
        }

        public static string ConvertTime(DateTime dateTime)
        {
            return dateTime.ToString("s");
        }

        private static MySqlCommand LoadArguments(this MySqlCommand cmd, params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                cmd.Parameters.AddWithValue($"@prop{i}", args[i]);
            }
            return cmd;
        }
    }
}