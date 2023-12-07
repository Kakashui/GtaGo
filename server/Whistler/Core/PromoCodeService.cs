using System;
using System.Collections.Generic;
using System.IO;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop;
using Whistler.NewDonateShop.Models;
using Whistler.PersonalEvents.Models.Rewards;
using Whistler.SDK;

namespace Whistler.Core
{
    internal class PromoCodesService : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PromoCodesService));
        private static Dictionary<int, RewardBase> PromoRewards = new Dictionary<int, RewardBase>
        {
            [10] = new GoCoinsReward(5000),
            [20] = new BonusPointReward(5000),
            [30] = new GoCoinsReward(10000),
            [40] = new RespectReward(1000),
            //[50] = new VehicleReward(new List<ItemModel> { DonateService.Items[12585], DonateService.Items[12581], DonateService.Items[12551] }),
            [60] = new GoCoinsReward(15000),
            [70] = new EmptyReward("Reward_UniqClothes", "ClothesReward"),
            [80] = new RespectReward(5000),
            [90] = new GoCoinsReward(20000),
            //[100] = new VehicleReward(new List<ItemModel> { DonateService.Items[12585], DonateService.Items[12581], DonateService.Items[12551] }),
            [500] = new EmptyReward("Reward_Business", "MoneyReward"),
            [1000] = new EmptyReward("Reward_CryptRialto", "MoneyReward"),

        };

        private const int MoneyForPromo = 25000;
        private const int CoinsForPromo = 100;
        public PromoCodesService()
        {
            if (Directory.Exists("interfaces"))
            {
                using (var w = new StreamWriter("interfaces/gui/src/configs/optionsMenu/referalRewards.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(PromoRewards)}");
                }
            };
        }
        
        [Command("newpromo", "enter /newpromo [name, money, gocoins]")]
        public static void CreateNewPromo(Player player, string name, int money, int gocoins)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "newpromo") && player.GetCharacter().AdminLVL != -2) return;

                if (!CheckPromocodeName(name))
                {
                    Notify.SendAlert(player, "promo:1");
                    return;
                }

                MySQL.Query("INSERT INTO `promos`(`name`, `money`, `gocoins`) VALUES (@prop0, @prop1, @prop2)", name.ToLower(), money, gocoins);
                Notify.SendSuccess(player, "promo:2");
            }
            catch (Exception ex)
            {
                _logger.WriteError("promo_exception: " + ex);
                Notify.SendError(player, "promo:3");
            }
        }
        
        [Command("checkpromo")]
        public static void CheckPromo(Player player, string promoName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkpromo")) return;
                
                var response = MySQL.QueryRead("SELECT * FROM promos WHERE name = @prop0", promoName.ToLower());
                if (response == null || response.Rows.Count == 0)
                {
                    Notify.SendAlert(player, "promo:4");
                    return;
                }

                var row = response.Rows[0];

                var usages = Convert.ToInt32(row["usages"]);
                player.SendChatMessage("promo:5".Translate(promoName, usages));
            }
            catch (Exception ex)
            {
                _logger.WriteError("promo_exception: " + ex);
                Notify.SendError(player, "promo:6");
            }
        }
        [RemoteEvent("usepromoopt")]
        public static void UsePromo(PlayerGo player, string promoName)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!string.IsNullOrEmpty(player.Account.PromoUsed))
                {
                    Notify.SendAlert(player, "promo:7");
                    return;
                }

                var response = MySQL.QueryRead("SELECT * FROM promos WHERE name = @prop0", promoName.ToLower());
                if (response != null && response.Rows.Count > 0)
                {
                    var money = Convert.ToInt32(response.Rows[0]["money"]);
                    var gocoins = Convert.ToInt32(response.Rows[0]["gocoins"]);
                    MySQL.Query("UPDATE promos SET usages = usages + 1 WHERE name = @prop0", promoName.ToLower());
                    ActivatePromoCode(player, promoName, money, gocoins);
                    return;
                }


                response = MySQL.QueryRead("SELECT * FROM characters WHERE mypromocode = @prop0", promoName.ToLower());
                if (response != null && response.Rows.Count > 0)
                {
                    int uuid = Convert.ToInt32(response.Rows[0]["uuid"]);
                    if (player.Account.Characters.Contains(uuid))
                    {
                        Notify.SendError(player, "promo:10");
                        return;
                    }
                    int countUsed = Convert.ToInt32(response.Rows[0]["countUseMyPromocode"]) + 1;
                    GiveRewardPromoOwner(uuid, countUsed);
                    ActivatePromoCode(player, promoName, MoneyForPromo, CoinsForPromo);
                    return;
                }

                Notify.SendError(player, "promo:8");

            }
            catch (Exception ex)
            {
                _logger.WriteError("execption whe use promocode " + ex);
            }
        }

        private static void ActivatePromoCode(PlayerGo player, string promoName, int money, int coins)
        {
            player.Account.PromoUsed = promoName.ToLower();
            MySQL.QuerySync("UPDATE accounts SET promoused = @prop1 WHERE idkey = @prop0", player.Account.Id, promoName.ToLower());
            MoneySystem.Wallet.MoneyAdd(player.Character, money, "Money_Promo".Translate(promoName));
            player.AddGoCoins(coins);
            DonateLog.OperationLog(player, coins, $"promo({promoName})");
            Notify.SendSuccess(player, "promo:9".Translate(promoName));
        }


        private static void GiveRewardPromoOwner(int uuid, int countUsed)
        {
            MySQL.QuerySync("UPDATE characters SET countUseMyPromocode = @prop1 WHERE uuid = @prop0", uuid, countUsed);
            var promoOwner = Main.GetPlayerByUUID(uuid);
            if (!promoOwner.IsLogged())
            {
                var characterOwner = Main.GetCharacterByUUID(uuid);
                if (characterOwner != null)
                    characterOwner.CountUseMyPromocode = countUsed;
            }
            else
            {
                promoOwner.Character.CountUseMyPromocode = countUsed;
                promoOwner.Character.UpdateReferal(promoOwner, false);
            }
            PromoRewards.GetValueOrDefault(countUsed)?.GiveReward(uuid, $"PromoUsed({countUsed})");
        }
        public static string GeteratePromoCode()
        {
            string promocode;
            Random random = new Random();
            do
            {
                promocode = "";
                for (int i = 0; i < 8; i++)
                    promocode += random.Next(0, 4) == 0 ? (char)random.Next(0x0030, 0x003A) : (char)random.Next(0x0061, 0x007B);
            }
            while (!CheckPromocodeName(promocode));
            return promocode;
        }

        private static bool CheckPromocodeName(string promocode)
        {
            promocode = promocode.ToLower();
            var response = MySQL.QueryRead("SELECT * FROM promos WHERE name = @prop0", promocode);
            if (response != null && response.Rows != null && response.Rows.Count > 0)
                return false;
            response = MySQL.QueryRead("SELECT * FROM characters WHERE mypromocode = @prop0", promocode);
            if (response != null && response.Rows != null && response.Rows.Count > 0)
                return false;
            return true;
        }
    }
}