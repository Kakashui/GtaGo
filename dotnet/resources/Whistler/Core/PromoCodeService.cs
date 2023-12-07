using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using GTANetworkMethods;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Whistler.Core.Models;
using Whistler.Core.Models.Rewards;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Inventory.Models;
using Whistler.NewDonateShop;
using Whistler.NewDonateShop.Models;
using Whistler.SDK;

namespace Whistler.Core
{
    internal class PromoCodesService : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PromoCodesService));
        /*
        public static readonly Dictionary<int, RewardBase> PromoRewards = new Dictionary<int, RewardBase>
        {
            // [10] = new GoCoinsReward(5000),
            // [20] = new BonusPointReward(5000),
            // [30] = new GoCoinsReward(10000),
            // [40] = new RespectReward(1000),
            // [50] = new VehicleReward(new List<ItemModel> { DonateService.Items[12585], DonateService.Items[12581], DonateService.Items[12551] }),
            // [60] = new GoCoinsReward(15000),
            // [70] = new EmptyReward("Уникальная одежда", "ClothesReward"),
            // [80] = new RespectReward(5000),
            // [90] = new GoCoinsReward(20000),
            // [100] = new VehicleReward(new List<ItemModel> { DonateService.Items[12585], DonateService.Items[12581], DonateService.Items[12551] }),
            // [500] = new EmptyReward("Бизнес", "MoneyReward"),
            // [1000] = new EmptyReward("Криптовалютная биржа", "MoneyReward"),
        };
        */

        public static readonly Dictionary<int, RewardBase> PromoRewards = new Dictionary<int, RewardBase>
        {
            [5] = new MoneyReward(25000),
            [10] = new MoneyReward(75000),
            [20] = new VehicleReward("sabregt2"),
            [30] = new PrimeReward(30),
            [50] = new EmptyReward("Эксклюзивная одежда на выбор"),
            [80] = new EmptyReward("Возможность бесплатного создания семьи"),
            [100] = new VehicleReward("amgc63coupe"),
            [1000] = new EmptyReward("Дом с открытым интерьером")
        };

        private static Dictionary<string, PromoCode> _promos = new Dictionary<string, PromoCode>();
        private static Dictionary<string, ReferalCode> _referals = new Dictionary<string, ReferalCode>();

        private static readonly char[] _allowedSymbols = "abcdefghijklmnpqrstuvwxyz123456789".ToCharArray();
        public const int MoneyForReferal = 50000;
        private const int CoinsForPromo = 0;
        public PromoCodesService()
        {
            if (Directory.Exists("interfaces/gui/src/configs/optionsMenu"))
            {
                using (var w = new StreamWriter("interfaces/gui/src/configs/optionsMenu/referalRewards.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(PromoRewards)}");
                }
            };
        }
        
        public static void Initialize()
        {
            DataTable result = MySQL.QueryRead("SELECT * FROM promos");
            if (result == null || result.Rows.Count == 0) return;

            string name;
            int level;
            int money;
            int donate;
            int usages;
            int activated;
            int ownerUuid;
            foreach (DataRow row in result.Rows)
            {
                name = row["name"].ToString();
                name = name.ToLower();
                if (_promos.ContainsKey(name)) continue;

                level = Convert.ToInt32(row["level"]);
                money = Convert.ToInt32(row["money"]);
                donate = Convert.ToInt32(row["mcoins"]);
                usages = Convert.ToInt32(row["usages"]);
                activated = Convert.ToInt32(row["activated"]);
                ownerUuid = Convert.ToInt32(row["owneruuid"]);

                _promos.Add(name, new PromoCode(level, name, ownerUuid, activated, usages, money, donate));
            }
        }

        public static void AddReferalCode(string promocode, int uuid, int promocodeLevel,  int activatedCount, int usedCount)
        {
            if (_referals.ContainsKey(promocode)) return;

            _referals.Add(promocode, new ReferalCode(promocodeLevel, promocode, uuid, activatedCount, usedCount, MoneyForReferal));
        }

        public static void GiveReward(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (player.Account.PromoReceived) return;

            string promocode = player.Account.PromoUsed;
            if (string.IsNullOrEmpty(promocode)) return;

            RewardData rewardData = _promos.ContainsKey(promocode) ? _promos[promocode].Reward : _referals.ContainsKey(promocode) ? _referals[promocode].Reward : null;
            if (rewardData == null) return;

            if (rewardData.Money > 0) MoneySystem.Wallet.MoneyAdd(player.Character, rewardData.Money, $"Промокод {promocode}");
            if (rewardData.Donate > 0) player.Account.AddMCoins(rewardData.Donate);

            if (_promos.ContainsKey(promocode)) 
            {
                PromoCode data = _promos[promocode];

                data.ActivatedCount++;
                data.Changed = true;
                if (data.Owner != null && data.Owner.IsLogged()) 
                {
                    data.Owner.Character.UpdateReferal(data.Owner, false);
                    GiveOwnerReward(data.Owner, promocode, data.Level, data.ActivatedCount);
                }
            }
            else 
            {
                ReferalCode data = _referals[promocode];

                data.ActivatedCount++;
                data.Changed = true;
                if (data.Owner != null && data.Owner.IsLogged()) 
                {
                    data.Owner.Character.PromocodeActivatedCount++;
                    data.Owner.Character.UpdateReferal(data.Owner, false);
                    GiveOwnerReward(data.Owner, promocode, data.Level, data.ActivatedCount);
                }
            }

            player.Account.PromoReceived = true;
            MySQL.Query($"UPDATE accounts SET promoreceived = 1 WHERE login = @prop0", player.Account.Login);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы получили награду за промокод {promocode}!", 3000);
        }

        private static void GiveOwnerReward(ExtPlayer owner, string promocode, int currentLevel, int currentActivates)
        {
            if (currentLevel < 0) return;
            if (PromoRewards == null || !PromoRewards.Any()) return;
            if (PromoRewards.Count <= currentLevel) return;

            int rewardLevel = PromoRewards.Keys.ElementAt(currentLevel);
            if (!PromoRewards.ContainsKey(rewardLevel)) return;
            if (currentActivates < rewardLevel) return;

            RewardBase rewardData = PromoRewards[rewardLevel];
            if (!rewardData.GiveReward(owner)) return;
            if (_promos.ContainsKey(promocode))
            {
                _promos[promocode].Level++;
                _promos[promocode].Changed = true;
            }
            else
            {
                _referals[promocode].Level++;
                _referals[promocode].Changed = true;
            }
            _logger.WriteDebug($"Successfully gave {owner.Name} an referal system owner reward");
        }

        public static void RegisterUserPromo(ExtPlayer player, string promoName)
        {
            if (string.IsNullOrEmpty(promoName)) return;

            promoName = promoName.ToLower();
            if (_referals.ContainsKey(promoName))
            {
                ReferalCode referalCode = _referals[promoName];
                if (player.Account.Characters.Contains(referalCode.OwnerUuid))
                {
                    Notify.SendError(player, "Вы не можете активировать свой реферальный код!");
                    return;
                }
                referalCode.UsedCount++;
                if (referalCode.Owner != null && referalCode.Owner.IsLogged()) referalCode.Owner.Character.PromocodeUsedCount++;
                referalCode.Changed = true;
                ActivatePromoCode(player, promoName);
                return;
            }
            else if (_promos.ContainsKey(promoName))
            {
                PromoCode promocode = _promos[promoName];
                if (player.Account.Characters.Contains(promocode.OwnerUuid))
                {
                    Notify.SendError(player, "Вы не можете активировать свой промокод!");
                    return;
                }
                promocode.UsedCount++;
                promocode.Changed = true;
                ActivatePromoCode(player, promoName);
                return;
            }

            Notify.SendError(player, "К сожалению, такого промокода не существует.");
        }

        public static void Subscribe(ExtPlayer player)
        {
            PromoCode promocode = GetPromocodeDataByOwnerUuid(player.Character.UUID);
            if (promocode != null) 
            {
                promocode.Owner = player;
                GiveOwnerReward(player, promocode.Name, promocode.Level, promocode.ActivatedCount);
            }

            if (string.IsNullOrEmpty(player.Character.Promocode)) return;

            ReferalCode referalcode = GetRefferalcodeDataByOwnerUuid(player.Character.UUID);
            if (referalcode == null) return;

            referalcode.Owner = player;
            GiveOwnerReward(player, referalcode.Name, referalcode.Level, referalcode.ActivatedCount);
        }

        public static void UnSubscribe(ExtPlayer player)
        {
            PromoCode promocode = GetPromocodeDataByOwner(player);
            if (promocode != null) promocode.Owner = null;

            if (string.IsNullOrEmpty(player.Character.Promocode)) return;

            ReferalCode referalcode = GetRefferalcodeDataByOwner(player);
            if (referalcode == null) return;

            referalcode.Owner = null;
        }

        public static void Save()
        {
            if (_promos.Any())
            {
                foreach (PromoCode promocode in _promos.Values)
                {
                    if (!promocode.Changed) continue;

                    promocode.Changed = false;
                    MySQL.Query("UPDATE `promos` SET level = @prop1, activated = @prop2, usages = @prop3 WHERE name = @prop0", promocode.Name, promocode.Level, promocode.ActivatedCount, promocode.UsedCount);
                }
            }

            if (_referals.Any())
            {
                foreach (ReferalCode referalcode in _referals.Values)
                {
                    if (!referalcode.Changed) continue;

                    referalcode.Changed = false;
                    MySQL.Query("UPDATE `characters` SET promocodeLevel = @prop1, promocodeActivated = @prop2, promocodeUsed = @prop3 WHERE promocode = @prop0", referalcode.Name, referalcode.Level, referalcode.ActivatedCount, referalcode.UsedCount);
                }
            }
        }

        [Command("newpromo")]
        public static void CreateNewPromo(ExtPlayer player, string name, int owner, int money, int mcoins = 0)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "newpromo")) return;
                if (string.IsNullOrEmpty(name) || money < 0 || mcoins < 0)
                {
                    Notify.SendAlert(player, "Пожалуйста, введите корректные данные.");
                    return;
                }

                name = name.ToLower();
                if (!CheckPromocodeName(name))
                {
                    Notify.SendAlert(player, "Такой промокод уже существует.");
                    return;
                }

                if (owner != -1 && !Main.UUIDs.Contains(owner))
                {
                    Notify.SendAlert(player, "Для создания промокода нужно ввести существующий UUID владельца.");
                    return;
                }
                PromoCode promocode = new PromoCode(0, name, owner, 0, 0, money, mcoins);
                MySQL.Query("INSERT INTO `promos`(`name`, `money`, `mcoins`, `owneruuid`) VALUES (@prop0, @prop1, @prop2, @prop3)", name, money, mcoins, owner);
                if (owner != -1)
                {
                    ExtPlayer target = Trigger.GetPlayerByUuid(owner);
                    if (target != null)
                    {
                        promocode.Owner = target;
                        Notify.SendSuccess(target, $"Для Вас был создан промокод {name} !");
                    }
                }
                _promos.Add(name, promocode);

                Notify.SendSuccess(player, $"Вы успешно создали промокод {name} !");
            }
            catch (Exception ex)
            {
                _logger.WriteError("promo_exception: " + ex);
                Notify.SendError(player, "Не создался промокод");
            }
        }

        [Command("deletepromo")]
        public static void DeletePromo(ExtPlayer player, string promoName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "newpromo")) return;
                if (string.IsNullOrEmpty(promoName))
                {
                    Notify.SendAlert(player, "Пожалуйста, введите корректные данные.");
                    return;
                }

                if (!_promos.Any())
                {
                    Notify.SendAlert(player, "В данный момент на сервере не зарегистрировано ни одного промокода.");
                    return;
                }

                promoName = promoName.ToLower();
                if (!_promos.ContainsKey(promoName))
                {
                    Notify.SendAlert(player, "Промокод не найден.");
                    return;
                }

                if (_promos[promoName].Owner != null) Notify.SendAlert(_promos[promoName].Owner, $"Промокод {promoName} был удалён администрацией.");
                _promos.Remove(promoName);
                MySQL.Query("DELETE FROM `promos` WHERE `name`=@prop0", promoName);
                Notify.SendSuccess(player, $"Вы успешно удалили промокод {promoName} !");
            }
            catch (Exception ex)
            {
                _logger.WriteError("promo_exception: " + ex);
                Notify.SendError(player, "Не создался промокод");
            }
        }

        [Command("checkpromo")]
        public static void CheckPromo(ExtPlayer player, string promoName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkpromo")) return;
                
                if (string.IsNullOrEmpty(promoName))
                {
                    Notify.SendAlert(player, "Пожалуйста, введите корректные данные.");
                    return;
                }
                if (!_promos.Any())
                {
                    Notify.SendAlert(player, "В данный момент на сервере не зарегистрировано ни одного промокода.");
                    return;
                }

                promoName = promoName.ToLower();
                if (!_promos.ContainsKey(promoName))
                {
                    Notify.SendAlert(player, "Промокод не найден.");
                    return;
                }

                PromoCode promocode = _promos[promoName];
                player.SendChatMessage($"Промокод {promoName} принадлежит {promocode.OwnerUuid}, промокод использовали {promocode.UsedCount} раз.");
                if (promocode.Owner != null) player.SendChatMessage("Владелец сейчас в сети.");
            }
            catch (Exception ex)
            {
                _logger.WriteError("promo_exception: " + ex);
                Notify.SendError(player, "Невозможно найти промокод");
            }
        }

        [Command("checkreferal")]
        public static void CheckReferal(ExtPlayer player, string referalcode)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkpromo")) return;

                if (string.IsNullOrEmpty(referalcode))
                {
                    Notify.SendAlert(player, "Пожалуйста, введите корректные данные.");
                    return;
                }
                if (!_referals.Any())
                {
                    Notify.SendAlert(player, "В данный момент на сервере не зарегистрировано ни одного реферального кода.");
                    return;
                }

                referalcode = referalcode.ToLower();
                if (!_referals.ContainsKey(referalcode))
                {
                    Notify.SendAlert(player, "Реферальный код не найден.");
                    return;
                }

                ReferalCode referal = _referals[referalcode];
                player.SendChatMessage($"Реферальный код {referalcode} принадлежит {referal.OwnerUuid}, промокод использовали {referal.UsedCount} раз.");
                if (referal.Owner != null) player.SendChatMessage("Владелец сейчас в сети.");
            }
            catch (Exception ex)
            {
                _logger.WriteError("promo_exception: " + ex);
                Notify.SendError(player, "Невозможно найти реферальный код");
            }
        }

        [RemoteEvent("promocode:setup")]
        public static void UsePromo(ExtPlayer player, string promoName)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!string.IsNullOrEmpty(player.Account.PromoUsed))
                {
                    Notify.SendAlert(player, "Вы уже вводили промокод на своём аккаунте.");
                    return;
                }
                RegisterUserPromo(player, promoName);
            }
            catch (Exception ex)
            {
                _logger.WriteError("execption whe use promocode " + ex);
            }
        }

        private static void ActivatePromoCode(ExtPlayer player, string promoName)
        {
            player.Account.PromoUsed = promoName;
            player.TriggerCefEvent("optionsMenu/setCanUsePromo", false);
            MySQL.QuerySync("UPDATE accounts SET promoused = @prop1 WHERE idkey = @prop0", player.Account.Id, promoName);
            Notify.SendSuccess(player, $"Вы успешно применили промокод {promoName}!");
            if (player.Character != null && player.Character.LVL < 5) Notify.SendSuccess(player, $"Награды будут выданы при получении 5 уровня.");
        }

        public static string GenerateReferalCode()
        {
            string promocode;
            Random random = new Random();
            do
            {
                promocode = "";
                for (int i = 0; i < 8; i++) promocode += _allowedSymbols[random.Next(_allowedSymbols.Length)];
            }
            while (!CheckPromocodeName(promocode));
            return promocode;
        }

        private static bool CheckPromocodeName(string promocode)
        {
            if (string.IsNullOrEmpty(promocode)) return false;

            promocode = promocode.ToLower();
            if (_promos.Any() && _promos.ContainsKey(promocode)) return false;
            if (_referals.Any() && _referals.ContainsKey(promocode)) return false;
            return true;
        }

        public static PromoCode GetPromocodeDataByOwner(ExtPlayer owner)
        {
            if (_promos == null || !_promos.Any()) return null;

            return _promos.Values.FirstOrDefault(p => p.Owner == owner);
        }

        public static PromoCode GetPromocodeDataByOwnerUuid(int ownerUuid)
        {
            if (_promos == null || !_promos.Any()) return null;

            return _promos.Values.FirstOrDefault(p => p.OwnerUuid == ownerUuid);
        }

        public static ReferalCode GetRefferalcodeDataByOwner(ExtPlayer owner)
        {
            if (_referals == null || !_referals.Any()) return null;

            return _referals.Values.FirstOrDefault(p => p.Owner == owner);
        }

        public static ReferalCode GetRefferalcodeDataByOwnerUuid(int ownerUuid)
        {
            if (_referals == null || !_referals.Any()) return null;

            return _referals.Values.FirstOrDefault(p => p.OwnerUuid == ownerUuid);
        }
    }
}