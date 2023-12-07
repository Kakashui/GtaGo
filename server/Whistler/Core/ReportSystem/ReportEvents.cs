using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core.ReportSystem.Models;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core.ReportSystem
{
    class ReportEvents : Script
    {
        //Админ взял репорт на себя
        [RemoteEvent("report:takereport")]
        public void RemoteEvent_TakeReport(PlayerGo player, int id)
        {
            if (!player.IsLogged())
                return;
            if (!ReportManager.Reports.ContainsKey(id))
                return;
            var character = player.GetCharacter();
            Report report = ReportManager.Reports[id];
            if (report.AdminName != null)
                return;
            ReportManager.SetAdminToReport(character, report);
        }

        [RemoteEvent("report:sendmessage")]
        public void RemoteEvent_SendMessage(PlayerGo player, int id, string answer)
        {
            ReportManager.SendMessage(player, id, answer);
        }

        [RemoteEvent("report:kick")]
        public void RemoteEvent_Kick(PlayerGo player, int id, string reason)
        {
            if (!ReportManager.Reports.ContainsKey(id))
                return;
            Report report = ReportManager.Reports[id];
            PlayerGo target = Main.GetPlayerByUUID(report.AuthorUUID);

            Admin.kickPlayer(player, target, reason, false);
        }

        [RemoteEvent("report:sendclosereport")]
        public void RemoteEvent_CloseReport(PlayerGo player, int ID, int rating)
        {
            var character = player.GetCharacter();
            Report report = ReportManager.Reports[ID];
            var playerAuthor = Main.GetPlayerByUUID(report.AuthorUUID);
            if (playerAuthor != null)
            {
                var characterAuthor = playerAuthor.GetCharacter();
                characterAuthor.NumberOfRatings++;
                characterAuthor.AmountOfRatings += rating;
                characterAuthor.OpenedReport = -1;
                Chat.SendTo(playerAuthor, "ReportMenu_24".Translate(player.Name, report.ID));
            }
            else
            {
                MySQL.Query("UPDATE `character` SET `numberofratings` = `numberofratings` + 1, `amountofratings` = `amountofratings` + @prop0 WHERE `uuid` = @prop1;", rating, report.AuthorUUID);
            }
            ReportManager.CloseReport(character, report);
        }

        [RemoteEvent("report:presshotkey")]
        public void RemoteEvent_PressHotKey(PlayerGo player, int id, int key)
        {
            if (!player.IsLogged())
                return;
            if (!ReportManager.Reports.ContainsKey(id))
                return;
            Report report = ReportManager.Reports[id];
            PlayerGo target = Main.GetPlayerByUUID(report.AuthorUUID);
            if (target == null)
                return;
            switch (key)
            {
                case 0:
                    Admin.teleportToPlayer(player, target);
                    player.TriggerEvent("report:closemenu");
                    break;
                case 1:
                    AdminSP.Spectate(player, target);
                    player.TriggerEvent("report:closemenu");
                    break;
                case 3:
                    if (report.StateGet > 0)
                        return;
                    target.GetStats().ForEach(item => ReportManager.SendMessage(player, id, item, false));
                    report.StateGet++;
                    break;
                default:
                    break;
            }
        }

        [RemoteEvent("report:position")]
        public void SebbdReportPosition(PlayerGo player, int id, float x, float y)
        {
            if (!player.IsLogged())
                return;
            if (!ReportManager.Reports.ContainsKey(id))
                return;
            Report report = ReportManager.Reports[id];
            PlayerGo target = Main.GetPlayerByUUID(report.AuthorUUID);
            if (target == null)
                return;
            target.TriggerEvent("createWaypoint", x, y);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Main_105".Translate(target.Name), 3000);
        }

        [RemoteEvent("report:player:send")]
        [Command("report", GreedyArg = true)]
        public static void PlayerSendReport(PlayerGo player, string message)
        {
            ReportManager.SendReport(player, message);
        }

        [RemoteEvent("report:player:close")]
        public static void PlayerCloseReport(PlayerGo player)
        {
            
            if (!player.IsLogged() || !ReportManager.Reports.ContainsKey(player.GetCharacter().OpenedReport))
                return;
            ReportManager.CloseReport(null, ReportManager.Reports[player.GetCharacter().OpenedReport]);
        }

        [RemoteEvent("report:player:raiting")]
        public static void PlayerReportRating(PlayerGo player, int rating)
        {   
            if (!player.IsLogged())
                return;
            if (rating < 1)
                rating = 1;
            if (rating > 5)
                rating = 5;
            ReportManager.SetRatingLastReportFromPlayer(player, rating);
        }

        [RemoteEvent("transfer:success")]
        public static void TransferSuccess(PlayerGo player, int id)
        {   
            if ((player.GetCharacter()?.AdminLVL ?? 0) < ReportConfigs.adminLvLMoneyTransfer) return;
            ReportManager.SuccessTransfer(id, player.Name);
        }

        [RemoteEvent("transfer:cancel")]
        public static void TransferCancel(PlayerGo player, int id)
        {
            if ((player.GetCharacter()?.AdminLVL ?? 0) < ReportConfigs.adminLvLMoneyTransfer) return;
            ReportManager.CancelTransfer(id, player.Name);

        }
    }
}