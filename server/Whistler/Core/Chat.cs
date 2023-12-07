using GTANetworkAPI;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Whistler.Fractions;
using Whistler.Families;
using Whistler.Helpers;
using Whistler.Families.Models;
using Whistler.Entities;
using Whistler.Common;

enum ChatType
{
    Normal,
    Cry,
    OOC,
    AdminChat,
    AdminResponse,
    Fraction,
    Family,
    Dep,
    AdminAction,
    AdminAnswer,
    Me,
    Do,
    Try,
    Gov,
    Global,
    Megafone,
    Advert,
    AdminWarning,
    SendTo
}

namespace Whistler.Core
{
    class Chat : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Chat));
        private static Random rnd = new Random();

        public static void SendToAdmins(ushort minLVL, string message)
        {
            Main.ForEachAllPlayer((p) =>
            {
                if (p.Character.AdminLVL >= minLVL)
                {
                    p.TriggerEvent("chat:api:action", ChatType.AdminWarning, message);
                }
            });
        }

        public static void AdminSMS (Player player, string message)
        {
            player.TriggerEvent("chat:api:action", ChatType.AdminWarning, message);
        }

        public static void SendFractionMessage(int fracid, string message, bool inChat)
        {
            var all_players = NAPI.Pools.GetAllPlayers();
            if (inChat)
            {
                foreach (var p in all_players)
                {
                    if (p == null) continue;
                    if (!p.IsLogged()) continue;

                    if (p.GetCharacter().FractionID == fracid)
                        p.TriggerEvent("chat:api:action", ChatType.Fraction, message, 0, 1);
                }
            }
            else
            {
                foreach (var p in all_players)
                {
                    if (p == null) continue;
                    if (!p.IsLogged()) continue;

                    if (p.GetCharacter().FractionID == fracid)
                        Notify.Send(p, NotifyType.Warning, NotifyPosition.BottomCenter, message, 4000);
                }
            }
        }
        public static void SendFractionMessage(int fracid, Func<Player, string> message, bool inChat)
        {
            var all_players = NAPI.Pools.GetAllPlayers();
            if (inChat)
            {
                foreach (var p in all_players)
                {
                    if (p == null) continue;
                    if (!p.IsLogged()) continue;

                    if (p.GetCharacter().FractionID == fracid)
                        p.TriggerEvent("chat:api:action", ChatType.Fraction, message(p), 0, 1);
                }
            }
            else
            {
                foreach (var p in all_players)
                {
                    if (p == null) continue;
                    if (!p.IsLogged()) continue;

                    if (p.GetCharacter().FractionID == fracid)
                        Notify.Send(p, NotifyType.Warning, NotifyPosition.BottomCenter, message(p), 4000);
                }
            }
        }

        public static void SendAllFamilyMessage(string message, bool inChat, params object[] args)
        {
            foreach (var famId in FamilyManager.GetFamiliesKeys())
            {
                SendFamilyMessage(famId, message, inChat, args);
            }
        }
        public static void SendFamilyMessage(int famId, string message, bool inChat, params object[] args)
        {

            Family family = FamilyManager.GetFamily(famId);

            if (family == null)
                return;
            
            var all_players = family.OnlineMembers.Values.ToArray();
            if (!inChat)
            {
                foreach (var p in all_players)
                {
                    if (!p.IsLogged()) continue;
                    Notify.Send(p, NotifyType.Info, NotifyPosition.BottomCenter, message.Translate(args), 4000);
                }
            }
            //else
            //{
            //    foreach (var p in all_players)
            //    {
            //        if (p == null) continue;
            //        if (!p.IsLogged()) continue;

            //        if (p.GetCharacter().FamilyID == famId)
            //            p.TriggerEvent("chat:api:action", ChatType.Fraction, message.Translate(args), 0);
            //    }
            //}
        }

        public static void Advert(Player redactor, string message, string author, int sim)
        {
            NAPI.ClientEvent.TriggerClientEventForAll("chat:api:advert", ChatType.Advert, redactor.Value, message, author, sim);
            GameLog.Chat(redactor.GetCharacter().UUID, (int)ChatType.Advert, $"{author}: {message}");
        }

        [Command("chat", GreedyArg = true)] // normal
        public static void Push(Player player, string msg)
        {
            if (player.GetCharacter().UnmuteDate > DateTime.Now)
            {
                var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_295".Translate( time), 3000);
                return;
            }
            foreach (Player p in player.GetPlayersInRange(10, true))
                p.TriggerEvent("chat:api:action", ChatType.Normal, msg, player.Value);

            GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Normal, msg);
        }

        [Command("s", GreedyArg = true)]
        public static void Cry(Player player, string msg)
        {
            if (player.GetCharacter().UnmuteDate > DateTime.Now)
            {
                var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_295".Translate( time), 3000);
                return;
            }
            foreach (Player p in player.GetPlayersInRange(30, true))
                p.TriggerEvent("chat:api:action", ChatType.Cry, msg, player.Value);
            GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Cry, msg);
        }

        [Command("b", GreedyArg = true)] // ooc
        public static void CMD_OOCChat(Player player, string msg)
        {
            if (player.GetCharacter().UnmuteDate > DateTime.Now)
            {
                var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_295".Translate( time), 3000);
                return;
            }
            foreach (Player p in player.GetPlayersInRange(10, true))
                p.TriggerEvent("chat:api:action", ChatType.OOC, msg, player.Value);
            GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.OOC, msg);
        }

        [Command("a", GreedyArg = true)] // admin chat
        public static void CMD_AdminChat(Player player, string message)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "a")) return;
                Main.ForEachAllPlayer((playerGo) =>
                {
                    if (playerGo.Character.AdminLVL >= 1)
                        playerGo.TriggerEvent("chat:api:action", ChatType.AdminChat, message, player.Value);
                });
                GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.AdminChat, message);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        public static void AdmiResponse(Player target, Player admin, string response)
        {
            target.TriggerEvent("chat:api:action", ChatType.AdminResponse, response, admin.Value);
            GameLog.Chat(admin.GetCharacter().UUID, (int)ChatType.AdminResponse, response);
        }

        [Command("f", GreedyArg = true)]
        public static void CMD_FracChat(Player player, string msg)
        {
            FractionMessage(player, msg);
        }

        private static void FractionMessage(Player player, string msg, bool noneRp = false)
        {
            try
            {
                if (!player.IsLogged()) return;
                var fraction = Manager.GetFraction(player);
                if (fraction == null)
                    return;

                if (player.GetCharacter().UnmuteDate > DateTime.Now)
                {
                    var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_325".Translate( time), 3000);
                    return;
                }
                if (noneRp)
                    msg = $"(( {msg} ))";
                foreach (var p in fraction.OnlineMembers.Values)
                {
                    if (p == null || !p.IsLogged()) continue;
                    p.TriggerEvent("chat:api:action", ChatType.Fraction, msg, player.Value, 0);
                }
                GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Fraction, (noneRp ? "ooc:" : "") + msg);
            }
            catch (Exception e) { _logger.WriteError($"FractionChat:\n {e.ToString()}"); }
        }

        [Command("fb", GreedyArg = true)]
        public static void CMD_fracChatOOC(Player player, string msg)
        {
            FractionMessage(player, msg, true);
        }

        [Command("fam", GreedyArg = true)]
        public static void CMD_FamilyRadio(Player player, string message)
        {
            try
            {
                if (player.GetCharacter().UnmuteDate > DateTime.Now)
                {
                    var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_295".Translate( time), 3000);
                    return;
                }
                Family family = player.GetFamily();

                if (family == null)
                {
                    Chat.SendTo(player, "chat_1");
                    return;
                }
                Trigger.ClientEventToPlayers(family.OnlineMembers.Values.ToArray(), "chat:api:action", ChatType.Family, message, player.Value);
                GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Family, message);

            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_FamilyRadio\": " + e.ToString());
            }
        }

        [Command("dep", GreedyArg = true)]
        public static void CMD_govFracChat(Player player, string msg)
        {
            if (!Manager.GovIds.ContainsKey(player.GetCharacter().FractionID)) return;
            if (!Manager.CanUseCommand(player, "dep")) return;
            if (player.GetCharacter().UnmuteDate > DateTime.Now)
            {
                var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_325".Translate( time), 3000);
                return;
            }
            int Fraction = player.GetCharacter().FractionID;
            foreach (var p in NAPI.Pools.GetAllPlayers())
            {
                if (p == null) continue;
                if (!p.IsLogged()) continue;
                if (Manager.GovIds.ContainsKey(p.GetCharacter().FractionID))
                    p.TriggerEvent("chat:api:action", ChatType.Dep, msg, player.Value, Fraction);
            }
            GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Dep, msg);
        }

        public static void AdminToAll(string message)
        {
            NAPI.ClientEvent.TriggerClientEventForAll("chat:api:action", ChatType.AdminAction, message);
        }

        public static void AdmiAnswer(Player to, Player target, Player admin, string response)
        {
            to.TriggerEvent("chat:api:action", ChatType.AdminAnswer, response, admin.Value, target.Value);
        }

        [Command("me", GreedyArg = true)]
        public static void Me(Player player, string msg)
        {
            if (player.GetCharacter().UnmuteDate > DateTime.Now)
            {
                var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_295".Translate( time), 3000);
                return;
            }
            foreach (var p in player.GetPlayersInRange(10, true))
                p.TriggerEvent("chat:api:action", ChatType.Me, msg, player.Value);
            GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Me, msg);
        }

        [Command("do", GreedyArg = true)]
        public static void Do(Player player, string msg)
        {
            if (player.GetCharacter().UnmuteDate > DateTime.Now)
            {
                var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_295".Translate( time), 3000);
                return;
            }
            foreach (var p in player.GetPlayersInRange(10, true))
                p.TriggerEvent("chat:api:action", ChatType.Do, msg, player.Value);
            GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Do, msg);
        }

        [Command("try", GreedyArg = true)]
        public static void Try(Player player, string msg)
        {
            if (player.GetCharacter().UnmuteDate > DateTime.Now)
            {
                var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_295".Translate( time), 3000);
                return;
            }
            try
            {
                //Random
                int result = new Random().Next(0, 2);
                foreach (Player p in player.GetPlayersInRange(10, true))
                    p.TriggerEvent("chat:api:action", ChatType.Try, $"{msg} {result > 0}", player.Value);
                GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Try, $"{result > 0}: {msg}");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("gov", GreedyArg = true)]
        public static void CMD_gov(Player player, string msg)
        {
            try
            {
                if (!Manager.CanUseCommand(player, "gov")) return;
                if (player.GetCharacter().UnmuteDate > DateTime.Now)
                {
                    var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_295".Translate( time), 3000);
                    return;
                }
                NAPI.ClientEvent.TriggerClientEventForAll("chat:api:action", ChatType.Gov, msg, player.Value, player.GetCharacter().FractionID);
                GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Gov, msg);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("news", GreedyArg = true)]
        public static void CMD_News(Player player, string msg)
        {
            try
            {
                if (!Manager.CanUseCommand(player, "news")) return;
                if (player.GetCharacter().UnmuteDate > DateTime.Now)
                {
                    var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_295".Translate( time), 3000);
                    return;
                }
                NAPI.ClientEvent.TriggerClientEventForAll("chat:api:action", ChatType.Gov, msg, player.Value, player.GetCharacter().FractionID);
                GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Gov, msg);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("global", GreedyArg = true)]
        public static void CMD_adminGlobalChat(Player player, string message)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "global")) return;
                NAPI.ClientEvent.TriggerClientEventForAll("chat:api:action", ChatType.Global, message, player.Value);
                GameLog.Admin($"{player.Name}", $"global({message})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("m", GreedyArg = true)]
        public static void Megafone(Player player, string msg)
        {
            if (player.GetCharacter().UnmuteDate > DateTime.Now)
            {
                var time = (player.GetCharacter().UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_296".Translate( time), 3000);
                return;
            }
            if ((player.GetCharacter().FractionID != 7 && player.GetCharacter().FractionID != 9) || !NAPI.Player.IsPlayerInAnyVehicle(player)) return;
            var vehGo = player.Vehicle.GetVehicleGo();
            if (vehGo.Data.OwnerType != OwnerType.Fraction) return;
            if (vehGo.Data.OwnerID != 7 && vehGo.Data.OwnerID != 9) return;
            foreach (var P in player.GetPlayersInRange(120, true))
                P.TriggerEvent("chat:api:action", ChatType.Megafone, msg, player.Value);
            GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.Megafone, msg);
        }

        public static void SendTo(Player player, string msg)
        {
            player.TriggerEvent("chat:api:action", ChatType.SendTo, msg);
        }

        public static void Action(Player player, string msg)
        {
            if (!player.GetGender())
                msg = "Fem_" + msg;
            foreach (var p in player.GetPlayersInRange(10, true))
                p.TriggerEvent("chat:api:action", ChatType.Me, msg, player.Value);
        }


        [Command("clearchat")] // normal
        public static void ClearChat(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "global")) return;
            NAPI.ClientEvent.TriggerClientEventForAll("chat:api:clear");
        }

    }
}
