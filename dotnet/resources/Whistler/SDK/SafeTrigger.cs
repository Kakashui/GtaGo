using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Whistler.Core.Animations;
using Whistler.Core.Character;
using Whistler.Core.OldPets;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models;

namespace Whistler.SDK
{
    /// <summary>
    /// Безопасные вызовы Rage API, которые будут работать в любом потоке.
    /// </summary>
    internal class SafeTrigger
    {
        private static void ResetSharedDataMethod(ExtPlayer player, string key)
        {
            if (player == null || !player.IsLogged()) return;

            player.ResetSharedData(key);
        }

        private static void SetSharedDataMethod(ExtPlayer player, string key, object value)
        {
            if (player == null || !player.IsLogged()) return;

            player.SetSharedData(key, value);
        }

        private static void SetSharedDataMethod(ExtVehicle vehicle, string key, object value)
        {
            if (vehicle == null) return;

            vehicle.SetSharedData(key, value);
        }

        private static void SetDataMethod(ExtPlayer player, string key, object value)
        {
            if (player == null || !player.IsLogged()) return;

            player.SetData(key, value);
        }

        private static void SetDataMethod(ExtVehicle vehicle, string key, object value)
        {
            if (vehicle == null) return;

            vehicle.SetData(key, value);
        }

        private static void UpdateRotationMethod(ExtPlayer player, Vector3 rotation)
        {
            if (player == null || !player.IsLogged()) return;

            player.Rotation = rotation;
        }

        private static void UpdatePositionMethod(ExtPlayer player, Vector3 position)
        {
            if (player == null || !player.IsLogged()) return;

            player.Session.Position = position;
            player.Position = position;
        }

        private static void UpdateDimensionMethod(ExtPlayer player, uint dimension, bool withPet)
        {
            if (player == null) return;

            player.Session.Dimension = dimension;
            player.Dimension = dimension;
            if (!withPet) return;

            Core.Pets.Controller.Pet_UpdateDimension(player, dimension);
        }

        public static void SetSharedData(ExtPlayer player, string key, object value)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                SetSharedDataMethod(player, key, value);
                return;
            }
            NAPI.Task.Run(() =>
            {
                SetSharedDataMethod(player, key, value);
            });
        }

        public static void SetSharedData(ExtVehicle vehicle, string key, object value)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                SetSharedDataMethod(vehicle, key, value);
                return;
            }
            NAPI.Task.Run(() =>
            {
                SetSharedDataMethod(vehicle, key, value);
            });
        }

        // От этого нужно будет отказаться в ближайшее время, переведя всё на внутреннюю память мода.
        public static void SetData(ExtPlayer player, string key, object value)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                SetDataMethod(player, key, value);
                return;
            }
            NAPI.Task.Run(() =>
            {
                SetDataMethod(player, key, value);
            });
        }

        // От этого нужно будет отказаться в ближайшее время, переведя всё на внутреннюю память мода.
        public static void SetData(ExtVehicle vehicle, string key, object value)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                SetDataMethod(vehicle, key, value);
                return;
            }
            NAPI.Task.Run(() =>
            {
                SetDataMethod(vehicle, key, value);
            });
        }

        public static void ResetSharedData(ExtPlayer player, string key)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                ResetSharedDataMethod(player, key);
                return;
            }
            NAPI.Task.Run(() =>
            {
                ResetSharedDataMethod(player, key);
            });
        }

        public static void UpdateDimension(ExtPlayer player, uint dimension = 0, bool withPet = true)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                UpdateDimensionMethod(player, dimension, withPet);
                return;
            }
            NAPI.Task.Run(() =>
            {
                UpdateDimensionMethod(player, dimension, withPet);
            });
        }

        public static void UpdatePosition(ExtPlayer player, Vector3 position)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                UpdatePositionMethod(player, position);
                return;
            }
            NAPI.Task.Run(() =>
            {
                UpdatePositionMethod(player, position);
            });
        }

        public static void UpdateRotation(ExtPlayer player, Vector3 rotation)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                UpdateRotationMethod(player, rotation);
                return;
            }
            NAPI.Task.Run(() =>
            {
                UpdateRotationMethod(player, rotation);
            });
        }

        public static void SendChatMessage(ExtPlayer client, string message)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                client.SendChatMessage(message);
                return;
            }
            NAPI.Task.Run(() =>
            {
                if (client == null) return;
                client.SendChatMessage(message);
            });
        }

        public static void SendChatMessageToAll(string message)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                NAPI.Chat.SendChatMessageToAll(message);
                return;
            }
            NAPI.Task.Run(() =>
            {
                NAPI.Chat.SendChatMessageToAll(message);
            });
        }

        public static void StopAnimation(ExtPlayer client)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                if (client == null) return;
                client.StopAnimation();
                return;
            }
            NAPI.Task.Run(() =>
            {
                if (client == null) return;
                client.StopAnimation();
            });
        }

        public static void PlayAnimation(ExtPlayer client, string animDict, string animName, int flag)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                if (client == null) return;
                client.PlayAnimation(animDict, animName, flag);
                return;
            }
            NAPI.Task.Run(() =>
            {
                if (client == null) return;
                client.PlayAnimation(animDict, animName, flag);
            });
        }

        public static void ClientEvent(ExtPlayer client, string eventName, params object[] args)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                if (client == null) return;
                NAPI.ClientEvent.TriggerClientEvent(client, eventName, args);
                return;
            }
            NAPI.Task.Run(() =>
            {
                if (client == null) return;
                NAPI.ClientEvent.TriggerClientEvent(client, eventName, args);
            });
        }

        public static void ClientEventInRange(Vector3 pos, float range, string eventName, params object[] args)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                NAPI.ClientEvent.TriggerClientEventInRange(pos, range, eventName, args);
                return;
            }
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEventInRange(pos, range, eventName, args);
            });
        }

        public static void ClientEventInDimension(uint dim, string eventName, params object[] args)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                NAPI.ClientEvent.TriggerClientEventInDimension(dim, eventName, args);
                return;
            }
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEventInDimension(dim, eventName, args);
            });
        }

        public static void ClientEventToPlayers(ExtPlayer[] players, string eventName, params object[] args)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                NAPI.ClientEvent.TriggerClientEventToPlayers(players, eventName, args);
                return;
            }
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEventToPlayers(players, eventName, args);
            });
        }

        public static void ClientEventForAll(string eventName, params object[] args)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                NAPI.ClientEvent.TriggerClientEventForAll(eventName, args);
                return;
            }
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEventForAll(eventName, args);
            });
        }

        public static void ClientCefEventToAllPlayers(string storeFunction, object data)
        {
            if (Thread.CurrentThread.Name == "Main")
            {
                NAPI.ClientEvent.TriggerClientEventForAll("gui:setData", storeFunction, data);
                return;
            }
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEventForAll("gui:setData", storeFunction, data);
            });
        }

        /// <summary>
        /// Найти модель персонажа по его динамическому ID
        /// </summary>
        /// <param name="id">Динамический ID</param>
        /// <returns>ExtPlayer, если найден, в противном случае null</returns>
        public static ExtPlayer GetPlayerById(int id)
        {
            List<ExtPlayer> allPlayers = GetAllPlayers();
            foreach (ExtPlayer player in allPlayers)
            {
                if (player == null) continue;
                if (player.Session == null) continue;
                if (player.Session.Id != id) continue;
                return player;
            }
            return null;
        }

        /// <summary>
        /// Найти модель персонажа по логину аккаунта
        /// </summary>
        /// <param name="login">Логин аккаунта</param>
        /// <returns>ExtPlayer, если найден, в противном случае null</returns>
        public static ExtPlayer GetPlayerByLogin(string login)
        {
            List<ExtPlayer> allPlayers = GetAllPlayers();
            foreach (ExtPlayer player in allPlayers)
            {
                if (player == null) continue;
                if (player.Account == null) continue;
                if (player.Account.Login != login) continue;
                return player;
            }
            return null;
        }

        /// <summary>
        /// Найти модель транспортного средства по статичному ID
        /// </summary>
        /// <param name="dataId">Статичный ID</param>
        /// <returns>ExtVehicle, если найден, в противном случае null</returns>
        public static ExtVehicle GetVehicleByDataId(int dataId)
        {
            List<ExtVehicle> allVehicles = GetAllVehicles();
            foreach (ExtVehicle vehicle in allVehicles)
            {
                if (vehicle == null) continue;
                if (vehicle.Data == null) continue;
                if (vehicle.Data.ID != dataId) continue;
                return vehicle;
            }
            return null;
        }

        /// <summary>
        /// Найти модель транспортного средства по динамическому ID
        /// </summary>
        /// <param name="id">Динамический ID</param>
        /// <returns>ExtVehicle, если найден, в противном случае null</returns>
        public static ExtVehicle GetVehicleById(int id)
        {
            List<ExtVehicle> allVehicles = GetAllVehicles();
            foreach (ExtVehicle vehicle in allVehicles)
            {
                if (vehicle == null) continue;
                if (vehicle.Session == null) continue;
                if (vehicle.Session.Id != id) continue;
                return vehicle;
            }
            return null;
        }

        /// <summary>
        /// Получить список всех игроков (даже не авторизованных)
        /// </summary>
        /// <returns>Список игроков</returns>
        public static List<ExtPlayer> GetAllPlayers()
        {
            if (Main.AllPlayers == null) return new List<ExtPlayer>();
            return Main.AllPlayers.ToList();
        }

        /// <summary>
        /// Получить список всех транспортных средств
        /// </summary>
        /// <returns>Список транспортных средств</returns>
        public static List<ExtVehicle> GetAllVehicles()
        {
            if (Main.AllVehicles == null || !Main.AllVehicles.Any()) return new List<ExtVehicle>();
            return Main.AllVehicles.Values.ToList();
        }
    }
}
