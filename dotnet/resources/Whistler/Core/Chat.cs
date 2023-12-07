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
using Whistler.VehicleSystem.Models;
using Whistler.Domain.Phone.Messenger;

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
                    SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.AdminWarning, message);
                }
            });
        }

        public static void AdminSMS (ExtPlayer player, string message)
        {
            SafeTrigger.ClientEvent(player,"chat:api:action", ChatType.AdminWarning, message);
        }

        public static void SendFractionMessage(int fracid, string message, bool inChat)
        {
            var all_players = SafeTrigger.GetAllPlayers();
            if (inChat)
            {
                foreach (var p in all_players)
                {
                    if (p == null) continue;
                    if (!p.IsLogged()) continue;

                    if (p.Character.FractionID == fracid)
                        SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Fraction, message, 0, 1);
                }
            }
            else
            {
                foreach (var p in all_players)
                {
                    if (p == null) continue;
                    if (!p.IsLogged()) continue;

                    if (p.Character.FractionID == fracid)
                        Notify.Send(p, NotifyType.Info, NotifyPosition.BottomCenter, message, 4000);
                }
            }
        }
        public static void SendFractionMessage(int fracid, Func<ExtPlayer, string> message, bool inChat)
        {
            var all_players = NAPI.Pools.GetAllPlayers().Cast<ExtPlayer>().ToList();
            if (inChat)
            {
                foreach (var p in all_players)
                {
                    if (p == null) continue;
                    if (!p.IsLogged()) continue;

                    if (p.Character.FractionID == fracid)
                        SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Fraction, message(p), 0, 1);
                }
            }
            else
            {
                foreach (var p in all_players)
                {
                    if (p == null) continue;
                    if (!p.IsLogged()) continue;

                    if (p.Character.FractionID == fracid)
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

            //        if (p.Character.FamilyID == famId)
            //            SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Fraction, message.Translate(args), 0);
            //    }
            //}
        }

        public static void Advert(ExtPlayer redactor, string message, string author, int sim)
        {
            NAPI.ClientEvent.TriggerClientEventForAll("chat:api:advert", ChatType.Advert, redactor.Value, message, author, sim);
            GameLog.Chat(redactor.Character.UUID, (int)ChatType.Advert, $"{author}: {message}");
        }

        [Command("chat", GreedyArg = true)] // normal
        public static void Push(ExtPlayer player, string msg)
        {
            if (player.Character.UnmuteDate > DateTime.Now)
            {
                var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                return;
            }
            foreach (ExtPlayer p in player.GetPlayersInRange(10, true))
                SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Normal, msg, player.Value);

            GameLog.Chat(player.Character.UUID, (int)ChatType.Normal, msg);
        }

        [Command("s", GreedyArg = true)]
        public static void Cry(ExtPlayer player, string msg)
        {
            if (player.Character.UnmuteDate > DateTime.Now)
            {
                var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                return;
            }
            foreach (ExtPlayer p in player.GetPlayersInRange(30, true))
                SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Cry, msg, player.Value);
            GameLog.Chat(player.Character.UUID, (int)ChatType.Cry, msg);
        }

        [Command("b", GreedyArg = true)] // ooc
        public static void CMD_OOCChat(ExtPlayer player, string msg)
        {
            if (player.Character.UnmuteDate > DateTime.Now)
            {
                var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                return;
            }
            foreach (ExtPlayer p in player.GetPlayersInRange(10, true))
                SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.OOC, msg, player.Value);
            GameLog.Chat(player.Character.UUID, (int)ChatType.OOC, msg);
        }

        [Command("a", GreedyArg = true)] // admin chat
        public static void CMD_AdminChat(ExtPlayer player, string message)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "a")) return;
                Main.ForEachAllPlayer((ExtPlayer) =>
                {
                    if (ExtPlayer.Character.AdminLVL >= 1)
                        SafeTrigger.ClientEvent(ExtPlayer, "chat:api:action", ChatType.AdminChat, message, player.Value);
                });
                GameLog.Chat(player.Character.UUID, (int)ChatType.AdminChat, message);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        public static void AdmiResponse(ExtPlayer target, ExtPlayer admin, string response)
        {
            SafeTrigger.ClientEvent(target, "chat:api:action", ChatType.AdminResponse, response, admin.Value);
            GameLog.Chat(admin.Character.UUID, (int)ChatType.AdminResponse, response);
        }

        [Command("f", GreedyArg = true)]
        public static void CMD_FracChat(ExtPlayer player, string msg)
        {
            FractionMessage(player, msg);
        }

        private static void FractionMessage(ExtPlayer player, string msg, bool noneRp = false)
        {
            try
            {
                if (!player.IsLogged()) return;
                var fraction = Manager.GetFraction(player);
                if (fraction == null)
                    return;

                if (player.Character.UnmuteDate > DateTime.Now)
                {
                    var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы замучены еще на {time} минут", 3000);
                    return;
                }
                if (noneRp) msg = $"(( {msg} ))";
                SafeTrigger.ClientEventToPlayers(fraction.OnlineMembers.Values.ToArray(), "chat:api:action", ChatType.Fraction, msg, player.Value);
                GameLog.Chat(player.Character.UUID, (int)ChatType.Fraction, (noneRp ? "ooc:" : "") + msg);
            }
            catch (Exception e) { _logger.WriteError($"FractionChat:\n {e.ToString()}"); }
        }

        [Command("fb", GreedyArg = true)]
        public static void CMD_fracChatOOC(ExtPlayer player, string msg)
        {
            FractionMessage(player, msg, true);
        }

        [Command("fam", GreedyArg = true)]
        public static void CMD_FamilyRadio(ExtPlayer player, string message)
        {
            try
            {
                if (player.Character.UnmuteDate > DateTime.Now)
                {
                    var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                    return;
                }
                Family family = player.GetFamily();

                if (family == null)
                {
                    Chat.SendTo(player, "Вы должны состоять в семье, чтобы использовать эту команду");
                    return;
                }
                SafeTrigger.ClientEventToPlayers(family.OnlineMembers.Values.ToArray(), "chat:api:action", ChatType.Family, message, player.Value);
                GameLog.Chat(player.Character.UUID, (int)ChatType.Family, message);

            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_FamilyRadio\": " + e.ToString());
            }
        }

        [Command("dep", GreedyArg = true)]
        public static void CMD_govFracChat(ExtPlayer player, string msg)
        {
            if (!Manager.GovIds.ContainsKey(player.Character.FractionID)) return;
            if (!Manager.CanUseCommand(player, "dep")) return;
            if (player.Character.UnmuteDate > DateTime.Now)
            {
                var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы замучены еще на {time} минут", 3000);
                return;
            }
            int Fraction = player.Character.FractionID;
            var players = NAPI.Pools.GetAllPlayers().Cast<ExtPlayer>().ToList();
            foreach (var p in players)
            {
                if (p == null) continue;
                if (!p.IsLogged()) continue;
                if (Manager.GovIds.ContainsKey(p.Character.FractionID))
                    SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Dep, msg, player.Value, Fraction);
            }
            GameLog.Chat(player.Character.UUID, (int)ChatType.Dep, msg);
        }

        public static void AdminToAll(string message)
        {
            SafeTrigger.ClientEventForAll("chat:api:action", ChatType.AdminAction, message);
        }

        public static void AdmiAnswer(ExtPlayer to, ExtPlayer target, ExtPlayer admin, string response)
        {
            SafeTrigger.ClientEvent(to, "chat:api:action", ChatType.AdminAnswer, response, admin.Value, target.Value);
        }

        [Command("me", GreedyArg = true)]
        public static void Me(ExtPlayer player, string msg)
        {
            if (player.Character.UnmuteDate > DateTime.Now)
            {
                var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                return;
            }
            foreach (var p in player.GetPlayersInRange(10, true))
                SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Me, msg, player.Value);
            GameLog.Chat(player.Character.UUID, (int)ChatType.Me, msg);
        }

        [Command("do", GreedyArg = true)]
        public static void Do(ExtPlayer player, string msg)
        {
            if (player.Character.UnmuteDate > DateTime.Now)
            {
                var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                return;
            }
            foreach (var p in player.GetPlayersInRange(10, true))
                SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Do, msg, player.Value);
            GameLog.Chat(player.Character.UUID, (int)ChatType.Do, msg);
        }

        [Command("try", GreedyArg = true)]
        public static void Try(ExtPlayer player, string msg)
        {
            if (player.Character.UnmuteDate > DateTime.Now)
            {
                var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                return;
            }
            try
            {
                //Random
                int result = new Random().Next(0, 2);
                foreach (ExtPlayer p in player.GetPlayersInRange(10, true))
                    SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Try, $"{msg} {result > 0}", player.Value);
                GameLog.Chat(player.Character.UUID, (int)ChatType.Try, $"{result > 0}: {msg}");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("gov", GreedyArg = true)]
        public static void CMD_gov(ExtPlayer player, string msg)
        {
            try
            {
                if (!Manager.CanUseCommand(player, "gov")) return;
                if (player.Character.UnmuteDate > DateTime.Now)
                {
                    var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                    return;
                }
                NAPI.ClientEvent.TriggerClientEventForAll("chat:api:action", ChatType.Gov, msg, player.Value, player.Character.FractionID);
                GameLog.Chat(player.Character.UUID, (int)ChatType.Gov, msg);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("news", GreedyArg = true)]
        public static void CMD_News(ExtPlayer player, string msg)
        {
            try
            {
                if (!Manager.CanUseCommand(player, "news")) return;
                if (player.Character.UnmuteDate > DateTime.Now)
                {
                    var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                    return;
                }
                NAPI.ClientEvent.TriggerClientEventForAll("chat:api:action", ChatType.Gov, msg, player.Value, player.Character.FractionID);
                GameLog.Chat(player.Character.UUID, (int)ChatType.Gov, msg);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("global", GreedyArg = true)]
        public static void CMD_adminGlobalChat(ExtPlayer player, string message)
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
        public static void Megafone(ExtPlayer player, string msg)
        {
            if (player.Character.UnmuteDate > DateTime.Now)
            {
                var time = (player.Character.UnmuteDate - DateTime.Now).Minutes;
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас мут на {time} минут", 3000);
                return;
            }
            if ((player.Character.FractionID != 7 && player.Character.FractionID != 9) || !NAPI.Player.IsPlayerInAnyVehicle(player)) return;
            ExtVehicle extVehicle = player.Vehicle as ExtVehicle;
            if (extVehicle.Data.OwnerType != OwnerType.Fraction) return;
            if (extVehicle.Data.OwnerID != 7 && extVehicle.Data.OwnerID != 9) return;
            foreach (var p in player.GetPlayersInRange(120, true))
                SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Megafone, msg, player.Value);
            GameLog.Chat(player.Character.UUID, (int)ChatType.Megafone, msg);
        }

        public static void SendTo(ExtPlayer player, string msg)
        {
            SafeTrigger.ClientEvent(player,"chat:api:action", ChatType.SendTo, msg);
        }

        public static void Action(ExtPlayer player, string msg)
        {
            if (!player.GetGender())
                msg = "Fem_" + msg;
            foreach (var p in player.GetPlayersInRange(10, true))
                SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Me, msg, player.Value);
        }


        [Command("clearchat")] // normal
        public static void ClearChat(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "global")) return;
            NAPI.ClientEvent.TriggerClientEventForAll("chat:api:clear");
        }

    }
}
