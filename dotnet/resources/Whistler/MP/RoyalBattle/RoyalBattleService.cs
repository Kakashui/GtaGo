using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private static List<ExtPlayer> _queuePlayers = new List<ExtPlayer>();

        private static RoyalBattleModel battle;
        private static DateTime _battleDelay = DateTime.Now;
        private static string Timer = null;
        private static DateTime TimerEndDate = DateTime.Now;
        private const int RegisterTimeMs = 300000;

        public static void RegisterForBattle(ExtPlayer player)
        {
            if (battle != null) return;
            if (_queuePlayers.Contains(player)) return;
            if (DateTime.Now < _battleDelay)
            {
                TimeSpan difference = _battleDelay - DateTime.Now;
                Notify.SendError(player, $"Регистрация станет доступна через {Convert.ToInt32(difference.TotalMinutes)} минут и {difference.Seconds} секунд.");
                return;
            }
            if (_queuePlayers.Count >= Configurations.MaxPlayerInOneBattle)
            {
                Notify.SendError(player, $"На мероприятие зарегистрировано уже максимальное количество игроков ({Configurations.MaxPlayerInOneBattle}).");
                return;
            }
            if (!MoneySystem.Wallet.MoneySub(player.Character, Configurations.MoneyForRegister, "Регистрация на 'Охота за головами'"))
            {
                Notify.SendError(player, $"Регистрация на 'Охота за головами' стоит {Configurations.MoneyForRegister}$, у Вас не хватает денег.");
                return;
            }

            SetTimer();
            _queuePlayers.Add(player);
            player.TriggerCefEvent("battlegroundReg/setDate", Convert.ToInt32((TimerEndDate - DateTime.Now).TotalSeconds));
            UpdatePlayersRegisterCEF();
            Notify.SendSuccess(player, "Вы успешно зарегистрировались на 'Охота за головами'.");
        }

        private static void SetTimer()
        {
            if (Timer != null) return;

            TimerEndDate = DateTime.Now.AddMilliseconds(RegisterTimeMs);
            Chat.AdminToAll($"Регистрация на Охота за Головами началась. Успей принять участие и выиграй приз!");
            Timer = Timers.StartOnce(RegisterTimeMs, () =>
            {
                CreateBattle(false);
            });
        }

        private static void UpdatePlayersRegisterCEF()
        {
            if (!_queuePlayers.Any()) return;

            RegistrationDTO registerCefData = new RegistrationDTO(true, _queuePlayers.Count);
            string json = JsonConvert.SerializeObject(registerCefData);
            foreach (ExtPlayer target in _queuePlayers)
            {
                if (target == null) continue;

                target.TriggerCefEvent("battlegroundReg/setData", json);
            }
        }

        public static void CreateBattle(bool force)
        {
            Timers.Stop(Timer);
            Timer = null;
            Chat.AdminToAll($"Регистрация на Охота за Головами закончена.");
            if (!_queuePlayers.Any()) return;
            if (!force && _queuePlayers.Count < Configurations.MinPlayerInOneBattle)
            {
                RegistrationDTO registerCefData = new RegistrationDTO(false, 0);
                string json = JsonConvert.SerializeObject(registerCefData);
                foreach (ExtPlayer player in _queuePlayers)
                {
                    MoneySystem.Wallet.MoneyAdd(player.Character, Configurations.MoneyForRegister, "Возврат за регистрацию на 'Охота за головами'");
                    Notify.SendError(player, $"Для начала игры необходимо {Configurations.MinPlayerInOneBattle} человек. Зарегистрируйтесь еще раз.");
                    player.TriggerCefEvent("battlegroundReg/setData", json);
                    player.TriggerCefEvent("battlegroundReg/setDate", -1);
                }
                _queuePlayers = new List<ExtPlayer>();
                return;
            }

            battle = new RoyalBattleModel(new List<ExtPlayer> (_queuePlayers));
            _queuePlayers = new List<ExtPlayer>();
            battle.StartRoyalBattle();
        }

        public static void StopBattle(ExtPlayer player, int minutes)
        {
            if (minutes < 1) minutes = 1;
            _battleDelay = DateTime.Now.AddMinutes(minutes);
            _queuePlayers = new List<ExtPlayer>();
            DestroyBattle();
            Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, $"Регистрация на мероприятие закрыта на {minutes} минут.", 3000);
        }

        public static void EndBattle(ExtPlayer winner)
        {
            NAPI.Task.Run(() =>
            {
                if (winner == null || !winner.IsLogged())
                {
                    Chat.AdminToAll($"Охота за головами была закончена.");
                    return;
                }

                MoneySystem.Wallet.MoneyAdd(winner.Character, Configurations.MoneyForWinner, "Победа в 'Охота за головами'");
                Notify.Alert(winner, "Поздравляем! Вы выиграли 'Охота за головами'!");
                Chat.AdminToAll($"{winner.Name} выиграл в Охоте за головами! Поздравляем!");

            }, Configurations.TimeToSendToStartPosition + 1500);

            DestroyBattle();
        }

        private static void DestroyBattle()
        {
            if (battle == null) return;

            battle.Destroy();
            battle = null;
        }

        public static bool IsInBattle(ExtPlayer player)
        {
            if (battle == null) return false;
            return battle.IsInBattle(player);
        }
        public static void InterractOpenBattleMenu(ExtPlayer player)
        {
            if (battle != null)
            {
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Мероприятие уже началось. Попробуйте прийти позже.", 3000);
                return;
            }

            if (DateTime.Now < _battleDelay)
            {
                TimeSpan difference = _battleDelay - DateTime.Now;
                Notify.SendError(player, $"Регистрация станет доступна через {Convert.ToInt32(difference.TotalMinutes)} минут и {difference.Seconds} секунд.");
                return;
            }

            int secondsUntilStart = TimerEndDate > DateTime.Now ? Convert.ToInt32((TimerEndDate - DateTime.Now).TotalSeconds) : -1;
            SafeTrigger.ClientEvent(player,"royalBattle:openMenuEnterBattle", _queuePlayers.Contains(player), _queuePlayers.Count, secondsUntilStart);
        }
        public static void InterractOpenBattleStatsMenu(ExtPlayer player)
        {
            List<PlayerRatingDTO> currentStats = battle?.GetPlayers().Sorted(false) ?? new List<PlayerRatingDTO>();
            SafeTrigger.ClientEvent(player,"royalBattle:openBattleStats", JsonConvert.SerializeObject(new { currentMatch = currentStats, allTime = GetAllTimeStats() }));
        }
        public static void SearchPlayerInStats(ExtPlayer player, string name)
        {
            SafeTrigger.ClientEvent(player,"royalBattle:sendSearchBattleStats", JsonConvert.SerializeObject(GetAllTimeStats(name)));
        }
        public static List<PlayerRatingDTO> GetAllTimeStats(string name = null)
        {
            return _battleRating.Values.Where(item => name == null || item.nickname.ToLower().Contains(name.ToLower())).ToList().Sorted(false).Where(item => item.place <= 50).ToList();
        }
        public static void OnExitRegisterZone(ExtPlayer player)
        {
            if (!_queuePlayers.Contains(player)) return;

            _queuePlayers.Remove(player);
            UpdatePlayersRegisterCEF();
            Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Вы покинули зону регистрации и были исключены из заявки на охоту за головами", 3000);
            MoneySystem.Wallet.MoneyAdd(player.Character, Configurations.MoneyForRegister, "Возврат за регистрацию на 'Охота за головами'");
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

        public static bool PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            return battle?.PlayerDeath(player, killer, weapon) ?? false;
        }
        public static void OnPlayerDisconnected(ExtPlayer player)
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
