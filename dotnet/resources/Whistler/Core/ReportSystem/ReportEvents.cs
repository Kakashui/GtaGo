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
        public void RemoteEvent_TakeReport(ExtPlayer player, int id)
        {
            if (!player.IsLogged())
                return;
            if (!ReportManager.Reports.ContainsKey(id))
                return;
            var character = player.Character;
            Report report = ReportManager.Reports[id];
            if (report.AdminName != null)
                return;
            ReportManager.SetAdminToReport(character, report);
        }

        [RemoteEvent("report:sendmessage")]
        public void RemoteEvent_SendMessage(ExtPlayer player, int id, string answer)
        {
            ReportManager.SendMessage(player, id, answer);
        }

        [RemoteEvent("report:kick")]
        public void RemoteEvent_Kick(ExtPlayer player, int id, string reason)
        {
            if (!ReportManager.Reports.ContainsKey(id))
                return;
            Report report = ReportManager.Reports[id];
            ExtPlayer target = Trigger.GetPlayerByUuid(report.AuthorUUID);

            Admin.kickPlayer(player, target, reason, false);
        }

        [RemoteEvent("report:sendclosereport")]
        public void RemoteEvent_CloseReport(ExtPlayer player, int ID, int rating)
        {
            var character = player.Character;
            Report report = ReportManager.Reports[ID];
            var playerAuthor = Trigger.GetPlayerByUuid(report.AuthorUUID);
            if (playerAuthor != null)
            {
                var characterAuthor = playerAuthor.Character;
                characterAuthor.NumberOfRatings++;
                characterAuthor.AmountOfRatings += rating;
                characterAuthor.OpenedReport = -1;
                Chat.SendTo(playerAuthor, $"{player.Name} завершил ваше обращение #{report.ID}. Вы можете оценить работу администратора по 5-бальной шкале в меню репортов");
            }
            else
            {
                MySQL.Query("UPDATE `character` SET `numberofratings` = `numberofratings` + 1, `amountofratings` = `amountofratings` + @prop0 WHERE `uuid` = @prop1;", rating, report.AuthorUUID);
            }
            ReportManager.CloseReport(character, report);
        }

        [RemoteEvent("report:presshotkey")]
        public void RemoteEvent_PressHotKey(ExtPlayer player, int id, int key)
        {
            if (!player.IsLogged())
                return;
            if (!ReportManager.Reports.ContainsKey(id))
                return;
            Report report = ReportManager.Reports[id];
            ExtPlayer target = Trigger.GetPlayerByUuid(report.AuthorUUID);
            if (target == null)
                return;
            switch (key)
            {
                case 0:
                    Admin.teleportToPlayer(player, target);
                    SafeTrigger.ClientEvent(player,"report:closemenu");
                    break;
                case 1:
                    AdminSP.Spectate(player, target);
                    SafeTrigger.ClientEvent(player,"report:closemenu");
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
        public void SebbdReportPosition(ExtPlayer player, int id, float x, float y)
        {
            if (!player.IsLogged())
                return;
            if (!ReportManager.Reports.ContainsKey(id))
                return;
            Report report = ReportManager.Reports[id];
            ExtPlayer target = Trigger.GetPlayerByUuid(report.AuthorUUID);
            if (target == null)
                return;
            SafeTrigger.ClientEvent(target, "createWaypoint", x, y);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы передали {target.Name} данные о своём маршруте!", 3000);
        }

        [RemoteEvent("report:player:send")]
        [Command("report", GreedyArg = true)]
        public static void PlayerSendReport(ExtPlayer player, string message)
        {
            ReportManager.SendReport(player, message);
        }

        [RemoteEvent("report:player:close")]
        public static void PlayerCloseReport(ExtPlayer player)
        {
            
            if (!player.IsLogged() || !ReportManager.Reports.ContainsKey(player.Character.OpenedReport))
                return;
            ReportManager.CloseReport(null, ReportManager.Reports[player.Character.OpenedReport]);
        }

        [RemoteEvent("report:player:raiting")]
        public static void PlayerReportRating(ExtPlayer player, int rating)
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
        public static void TransferSuccess(ExtPlayer player, int id)
        {   
            if ((player.Character?.AdminLVL ?? 0) < ReportConfigs.adminLvLMoneyTransfer) return;
            ReportManager.SuccessTransfer(id, player.Name);
        }

        [RemoteEvent("transfer:cancel")]
        public static void TransferCancel(ExtPlayer player, int id)
        {
            if ((player.Character?.AdminLVL ?? 0) < ReportConfigs.adminLvLMoneyTransfer) return;
            ReportManager.CancelTransfer(id, player.Name);

        }
    }
}