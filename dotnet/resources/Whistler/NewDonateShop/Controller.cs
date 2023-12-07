using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.NewDonateShop
{
    internal class Controller
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Controller));
        private static string _timer = null;
        private static int _updateTimeMin = 1;
        private static string _connection = null;

        public static void Initialize()
        {
            if (_timer != null) Timers.Stop(_timer);

            MySQLInitialize();
            _timer = Timers.StartTask(_updateTimeMin * 60 * 1000, SyncDonations);
            _logger.WriteInfo("Donations Sync successfully initialized");
        }

        private static void MySQLInitialize()
        {
            if (_connection is string) return;

            _connection =
                $"Host={Main.ServerConfig.MySQL.Server};" +
                $"Port={Main.ServerConfig.MySQL.Port};" +
                $"User={Main.ServerConfig.MySQL.User};" +
                $"Password={Main.ServerConfig.MySQL.Password};" +
                $"Database=donates_db;" +
                $"{Main.ServerConfig.MySQL.SSL}";
        }

        private static async Task SyncDonations()
        {
            if (!Main.ServerConfig.Main.DonateChecker) return;

            _logger.WriteDebug("Starting syncing donations.");
            using MySqlConnection connection = new MySqlConnection(_connection);
            if (connection == null) return;

            await connection.OpenAsync();
            MySqlCommand command = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM `donate` WHERE `status`=1"
            };
            if (command == null)
            {
                _logger.WriteError("Error while creating MySqlCommand");
                await connection.CloseAsync();
                return;
            }

            MySqlDataReader reader = await command.ExecuteReaderAsync();
            using DataTable result = new DataTable();
            result.Load(reader);
            await reader.CloseAsync();

            int id;
            string login;
            int donatePoints;
            int orderId;
            ExtPlayer target;
            DateTime now = DateTime.Now;
            foreach (DataRow row in result.Rows)
            {
                id = Convert.ToInt32(row["id"]);
                login = row["username"].ToString();
                login = login.ToLower();
                donatePoints = Convert.ToInt32(row["count"]);

                if (!Main.Usernames.Contains(login) || donatePoints <= 0)
                {
                    command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = "UPDATE `donate` SET `status`=3 WHERE `id`=@id"
                    };
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();

                    _logger.WriteError($"Login {login} is not found or donate amount is less or equals 0.");
                    continue;
                }

                orderId = Convert.ToInt32(row["orderid"]);
                target = SafeTrigger.GetPlayerByLogin(login);
                if (target != null && target.AddMCoins(donatePoints)) Notify.SendSuccess(target, $"Вы успешно пополнили свой аккаунт на сумму {donatePoints} донат.валюты.");
                else
                {
                    command = new MySqlCommand
                    {
                        CommandText = "UPDATE `accounts` SET `mcoins`=`mcoins`+@coins WHERE `login`=@login"
                    };
                    command.Parameters.AddWithValue("@coins", donatePoints);
                    command.Parameters.AddWithValue("@login", login);
                    await MySQL.QueryAsync(command);
                }

                command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = "INSERT INTO `donate_log` VALUES (@id,@orderId,@login,@amount,@date)"
                };
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@amount", donatePoints);
                command.Parameters.AddWithValue("@date", now);
                await command.ExecuteNonQueryAsync();

                command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = "UPDATE `donate` SET `status`=2 WHERE `id`=@id"
                };
                command.Parameters.AddWithValue("@id", id);
                await command.ExecuteNonQueryAsync();

                _logger.WriteDebug($"Successfully added {donatePoints} donate points to {login}");
            }

            await connection.CloseAsync();
            _logger.WriteDebug("Donations successfully synced.");
        }
    }
}
