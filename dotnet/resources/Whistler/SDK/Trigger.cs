using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models;

namespace Whistler.SDK
{
    /// <summary>
    /// Небезопасные вызовы, которые будут работать только в Main thread'е.
    /// </summary>
    internal class Trigger
    {
        public static ExtPlayer GetPlayerById(int id)
        {
            if (Thread.CurrentThread.Name != "Main") return null;

            List<Player> players = NAPI.Pools.GetAllPlayers();
            foreach (Player player in players)
            {
                if (player == null || player.Value != id) continue;
                if (player is ExtPlayer ExtPlayer) return ExtPlayer;
                return null;
            }
            return null;
        }

        public static ExtVehicle GetVehicleById(int id)
        {
            if (Thread.CurrentThread.Name != "Main") return null;

            List<Vehicle> vehicles = NAPI.Pools.GetAllVehicles();
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle == null || vehicle.Value != id) continue;
                if (vehicle is ExtVehicle extVehicle) return extVehicle;
                return null;
            }
            return null;
        }

        public static List<ExtVehicle> GetVehiclesByNumberPlate(string numberPlate)
        {
            if (Thread.CurrentThread.Name != "Main") return null;

            List<Vehicle> vehicles = NAPI.Pools.GetAllVehicles();
            List<ExtVehicle> foundVehicles = new List<ExtVehicle>();
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle == null || !vehicle.NumberPlate.Contains(numberPlate)) continue;
                if (vehicle is ExtVehicle extVehicle) foundVehicles.Add(extVehicle);
            }
            return foundVehicles;
        }

        public static ExtPlayer GetPlayerByName(string name)
        {
            if (Thread.CurrentThread.Name != "Main") return null;
            if (string.IsNullOrEmpty(name)) return null;

            List<Player> players = NAPI.Pools.GetAllPlayers();
            if (players == null) return null;

            foreach (Player player in players)
            {
                if (player == null || player.Name != name) continue;
                if (player is ExtPlayer ExtPlayer) return ExtPlayer;
                return null;
            }
            return null;
        }

        public static ExtPlayer GetPlayerByUuid(int? uuid)
        {
            if (uuid == null || uuid < 0) return null;

            List<ExtPlayer> players = GetAllPlayers();
            if (players == null) return null;

            foreach (ExtPlayer player in players)
            {
                if (player == null) continue;
                if (player.Character == null) continue;
                if (player.Character.UUID != uuid) continue;
                return player;
            }
            return null;
        }

        public static ExtVehicle GetVehicleByUuid(int? uuid)
        {
            if (Thread.CurrentThread.Name != "Main") return null;
            if (uuid == null || uuid < 0) return null;

            List<ExtVehicle> vehicles = GetAllVehicles();
            if (vehicles == null) return null;

            foreach (ExtVehicle vehicle in vehicles)
            {
                if (vehicle == null) continue;
                if (vehicle.Data == null) continue;
                if (vehicle.Data.ID != uuid) continue;
                return vehicle;
            }
            return null;
        }

        public static List<ExtPlayer> GetAllPlayers()
        {
            if (Main.AllPlayers == null) return new List<ExtPlayer>();
            return Main.AllPlayers.ToList();
        }
        
        public static List<ExtVehicle> GetAllVehicles()
        {
            if (Main.AllVehicles == null || !Main.AllVehicles.Any()) return new List<ExtVehicle>();
            return Main.AllVehicles.Values.ToList();
        }
    }
}
