using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.MP.RoyalBattle.Configs;
using Whistler.MP.RoyalBattle.Models;
using Whistler.SDK;

namespace Whistler.MP.RoyalBattle
{
    static class RoyalBattleService
    {
        private static Random _rnd = new Random();
        private static Dictionary<int, PlayerRatingDTO> _battleRating = new Dictionary<int, PlayerRatingDTO>();

        private static List<PlayerGo> _queuePlayers = new List<PlayerGo>();

        private static RoyalBattleModel battle;
        private static DateTime _battleDelay = DateTime.Now;

        public static void RegisterForBattle(PlayerGo player)
        {
            if (battle != null)
                return;
            if (DateTime.Now < _battleDelay)
                return;
            if (!_queuePlayers.Contains(player))
            {
                _queuePlayers.Add(player);
            }
            //if (_queuePlayers.Count >= Configurations.MaxPlayerInOneBattle)
            //    WhistlerTask.Run(CreateBattle, 30000);
            if (_queuePlayers.Count >= Configurations.MinPlayerInOneBattle)
                WhistlerTask.Run(CreateBattle, 120000);
        }
        public static void CreateBattle()
        {
            if (battle != null)
                return;
            if (_queuePlayers.Count == 0)
                return;
            battle = new RoyalBattleModel(new List<PlayerGo> (_queuePlayers));
            _queuePlayers = new List<PlayerGo>();
            battle.StartRoyalBattle();
        }
        public static void StopBattle(Player player, int minutes)
        {
            _battleDelay = DateTime.Now.AddMinutes(minutes);
            Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "royalb:1", 3000);
        }

        public static void EndBattle(Player winner)
        {
            WhistlerTask.Run(() =>
            {
                if (winner.IsLogged())
                {
                    MoneySystem.Wallet.MoneyAdd(winner.GetCharacter(), Configurations.MoneyForWinner, "Money_RoyalBattle");
                    Notify.Alert(winner, "royalb:2");
                    Chat.AdminToAll("royalb:3".Translate(winner.Name));
                }
            }, Configurations.TimeToSendToStartPosition + 1500);
            battle.Destroy();
            battle = null;
        }
        public static bool IsInBattle(PlayerGo player)
        {
            return battle?.IsInBattle(player) ?? false;
        }
        public static void InterractOpenBattleMenu(Player player)
        {
            if (battle != null)
            {
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "royalb:4", 3000);
                return;
            }
            if (DateTime.Now < _battleDelay)
            {
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "royalb:5".Translate((int)(_battleDelay - DateTime.Now).TotalMinutes), 3000);
                return;
            }
            player.TriggerEvent("royalBattle:openMenuEnterBattle", _queuePlayers.Contains(player), _queuePlayers.Count);
        }
        public static void InterractOpenBattleStatsMenu(Player player)
        {
            List<PlayerRatingDTO> currentStats = battle?.GetPlayers().Sorted(false) ?? new List<PlayerRatingDTO>();
            player.TriggerEvent("royalBattle:openBattleStats", JsonConvert.SerializeObject(new { currentMatch = currentStats, allTime = GetAllTimeStats() }));
        }
        public static void SearchPlayerInStats(Player player, string name)
        {
            player.TriggerEvent("royalBattle:sendSearchBattleStats", JsonConvert.SerializeObject(GetAllTimeStats(name)));
        }
        public static List<PlayerRatingDTO> GetAllTimeStats(string name = null)
        {
            return _battleRating.Values.Where(item => name == null || item.nickname.ToLower().Contains(name.ToLower())).ToList().Sorted(false).Where(item => item.place <= 50).ToList();
        }
        public static void OnExitRegisterZone(PlayerGo player)
        {
            if (_queuePlayers.Contains(player))
            {
                _queuePlayers.Remove(player);
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "royalb:6", 3000);
            }
        }

        public static void LoadBattleRating(int uuid, string name, int rating)
        {
            if (rating <= 0)
                return;
            if (_battleRating.ContainsKey(uuid))
                _battleRating[uuid].kills = rating;
            else
                _battleRating.Add(uuid, new PlayerRatingDTO(name, rating, -1));
        }
        public static void AddBattleRating(int uuid, int rating, bool save = false)
        {
            if (rating <= 0)
                return;
            if (_battleRating.ContainsKey(uuid))
                _battleRating[uuid].kills += rating;
            else
                _battleRating.Add(uuid, new PlayerRatingDTO(Main.PlayerNames.GetValueOrDefault(uuid), rating, -1));
            if (save)
                SDK.MySQL.Query("UPDATE `characters` SET `rbrating` = @prop0 WHERE `uuid` = @prop1", rating, uuid);
        }
        public static int GetBattleRating(int uuid)
        {
            if (_battleRating.ContainsKey(uuid))
                return _battleRating[uuid].kills;
            else
                return 0;
        }

        public static void DropThingOnPoint(Vector3 point, uint dimension)
        {
            var items = GetRandomThing();
            foreach (var item in items)
            {
                DropSystem.DropItem(item, point + new Vector3(0, 0, 0.05), dimension, false);
            }
        }

        public static bool PlayerDeath(PlayerGo player, PlayerGo killer, uint weapon)
        {
            return battle?.PlayerDeath(player, killer, weapon) ?? false;
        }
        public static void OnPlayerDisconnected(PlayerGo player)
        {
            if (_queuePlayers.Contains(player))
            {
                _queuePlayers.Remove(player);
            }
            battle?.OnPlayerDisconnected(player);
        }
        private static List<BaseItem> GetRandomThing()
        {
            var dropItem = Configurations.BattleDropList.GetRandomElementWithProbability(item => item.Probability);
            switch (Inventory.Configs.Config.GetTypeByName(dropItem.Name))
            {
                case ItemTypes.Weapon:
                    var weapon = ItemsFabric.CreateWeapon(dropItem.Name, false, true);
                    var typeAmmo = (weapon as Weapon).Config.AmmoType;
                    if (typeAmmo != ItemNames.Invalid)
                    {
                        int ammoCount = typeAmmo == ItemNames.SniperAmmo ? _rnd.Next(40, 60) : _rnd.Next(300, 450);
                        return new List<BaseItem> { weapon, ItemsFabric.CreateAmmo(typeAmmo, ammoCount, false, true) };
                    }
                    return new List<BaseItem> { weapon };
                case ItemTypes.Ammo:
                    return new List<BaseItem> { ItemsFabric.CreateAmmo(dropItem.Name, dropItem.GetCount(), false, true) };
                case ItemTypes.Clothes:
                    return new List<BaseItem> { ItemsFabric.CreateClothes(dropItem.Name, true, 1, _rnd.Next(0, 10), false, true) };
                case ItemTypes.Medicaments:
                    return new List<BaseItem> { ItemsFabric.CreateMedicaments(dropItem.Name, dropItem.GetCount(), false, true) };
                case ItemTypes.Food:
                    return new List<BaseItem> { ItemsFabric.CreateFood(dropItem.Name, dropItem.GetCount(), false, true) };
            }
            return new List<BaseItem> ();
        }
    }
}
