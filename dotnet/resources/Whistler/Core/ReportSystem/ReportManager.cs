using System;
using System.Data;
using GTANetworkAPI;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Whistler.SDK;
using System.Collections.Generic;
using Whistler.GUI;
using System.Linq;
using Newtonsoft.Json.Linq;
using Whistler.Core.ReportSystem.Models;
using Whistler.Helpers;
using Whistler.MoneySystem.Models;
using Whistler.Entities;

namespace Whistler.Core.ReportSystem
{
    class ReportManager : Script
    {
        public static Dictionary<int, Report> Reports = new Dictionary<int, Report>();
        public static Dictionary<int, MoneyTransfer> MoneyTransfers = new Dictionary<int, MoneyTransfer>();
        private static int _moneyTransfersID = 1;
        public static List<ExtPlayer> Admins = new List<ExtPlayer>();

        public ReportManager()
        {
            Main.PlayerPreDisconnect += HandlePlayerDisconnect;
            Admin.SetPlayerToAdminGroup += OnAdminLoad;
            Admin.DeletePlayerFromAdminGroup += OnAdminUnload;
        }

        public static void HandlePlayerDisconnect(ExtPlayer player)
        {
            if (Admins.Contains(player))
                Admins.Remove(player);
            int openReport = player.Character.OpenedReport;
            if (openReport != -1 && Reports.ContainsKey(openReport))
            {
                CloseReport(null, Reports[player.Character.OpenedReport]);
            }
            if (player.Character.AdminLVL >= ReportConfigs.adminLvL)
                foreach (var report in Reports.Where(item => item.Value.AdminUUID == player.Character.UUID && !item.Value.Closed))
                    SetAdminToReport(null, report.Value);
        }

        public static void OnAdminLoad(ExtPlayer player)
        {
            if (!Admins.Contains(player))
            {
                Admins.Add(player);
                if (player.Character.AdminLVL >= ReportConfigs.adminLvL)
                {
                    bool viewAllReports = Group.CanUseAdminCommand(player, "viewallreports", false);
                    player.TriggerEventWithLargeList("report:loadreports", Reports.Select(item => item.Value.GetReportDTO()), viewAllReports);
                }
                if (player.Character.AdminLVL >= ReportConfigs.adminLvLMoneyTransfer)
                {
                    player.TriggerCefEvent("transfersConfirmation/setTransfersList", JsonConvert.SerializeObject(MoneyTransfers.Values));
                }
            }
        }

        public static void OnAdminUnload(ExtPlayer player)
        {
            if (Admins.Contains(player))
                Admins.Remove(player);
        }

        public static void SendReport(ExtPlayer player, string message)
        {
            if (!player.IsLogged())
                return;
            //if (player.Character.AdminLVL > 0)
            //{
            //    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"ReportMenu_29", 3000);
            //    return;
            //}
            if (message.Length > 150)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Сообщение слишком длинное", 3000);
                return;
            }
            int currentReport = -1;
            if (player.Character.OpenedReport != -1)
            {
                if (Reports.ContainsKey(player.Character.OpenedReport))
                {
                    Report report = Reports[player.Character.OpenedReport];
                    if (!report.Closed)
                        currentReport = player.Character.OpenedReport;
                }
            }
            if (currentReport == -1 && player.HasData("NEXT_REPORT"))
            {
                DateTime nextReport = player.GetData<DateTime>("NEXT_REPORT");
                if (DateTime.Now < nextReport)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Попробуйте через несколько минут", 3000);
                    return;
                }
            }
            if (player.IsMuted)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете отправить жалобу когда у вас мут", 3000);
                return;
            }
            SafeTrigger.SetData(player, "NEXT_REPORT", DateTime.Now.AddMinutes(2));
            if (currentReport != -1)
                SendMessage(player, currentReport, message);
            else
                CreateReport(player, message);
        }

        public static void CreateReport(ExtPlayer player, string question)
        {
            Report report = new Report(player.Character, player.Value, player.Account);
            report.AddMessage(player.Character, question);
            Reports.Add(report.ID, report);
            report.Send("report:addreport", false, report.GetSerializeReportDTO());
            player.Character.OpenedReport = report.ID;
        }

        public static void SendMessage(ExtPlayer player, int id, string message, bool saveToDB = true)
        {
            if (!player.IsLogged())
                return;
            if (!Reports.ContainsKey(id))
                return;
            var character = player.Character;
            Report report = Reports[id];
            var answer = report.AddMessage(character, message, saveToDB);

            report.Send("report:addmessage", false, answer.GetSerializeReportAnswerDTO());

            if (character.UUID != report.AuthorUUID && saveToDB)
            {
                var target = Trigger.GetPlayerByUuid(report.AuthorUUID);
                if (target != null)
                {
                    SafeTrigger.ClientEvent(target, "report:player:answer", player.Value, player.Name.Replace("_", " "), message);
                    foreach (var adm in Admins)
                    {
                        Chat.AdmiAnswer(adm, target, player, message);
                    }
                    Admin.adminSMS(player, target, message);
                    // Chat.SendTo(target, message);
                }
            }
        }

        public static void SetAdminToReport(Character.Character character, Report report)
        {
            if (report.Closed)
                return;
            if (character != null)
            {
                report.AdminName = character.FullName;
                report.AdminUUID = character.UUID;
            }
            else
            {
                report.AdminName = null;
                report.AdminUUID = -1;
            }
            report.Send("report:updatetakereport", true, report.ID, report.AdminName?.Replace('_', ' '));
        }

        public static void CloseReport(Character.Character admin, Report report)
        {
            report.Closed = true;
            if (admin != null)
            {
                report.AdminName = admin.FullName;
                report.AdminUUID = admin.UUID;
            }

            report.ClosedDate = DateTime.Now;
            MySQL.Query("UPDATE `reports` SET `closedate` = @prop0, `adminuuid` = @prop1 WHERE `id` = @prop2", MySQL.ConvertTime(report.ClosedDate), report.AdminUUID, report.ID);
            report.Send("report:closereport", true, report.ID, report.AdminName?.Replace('_', ' '));
        }

        public static void SetRatingLastReportFromPlayer(ExtPlayer player, int rating)
        {
            var report = Reports.LastOrDefault(item => item.Value.AuthorUUID == player.Character.UUID && item.Value.Rating == -1).Value;
            if (report == null)
                return;
            report.Rating = rating;
            if (report.AdminUUID != -1)
            {
                var Admin = Trigger.GetPlayerByUuid(report.AdminUUID);
                if (Admin != null)
                {
                    Admin.Character.NumberOfAdminRatings++;
                    Admin.Character.AmountOfAdminRatings += rating;
                }
                else
                {
                    MySQL.Query("UPDATE `characters` SET `numberofadminratings` = `numberofadminratings` + 1, `amountofadminratings` = `amountofadminratings` + @prop0 WHERE `uuid` = @prop1;", rating, report.AdminUUID);
                }
            }
            MySQL.Query("UPDATE `reports` SET `rating` = @prop0 WHERE `id` = @prop1", report.Rating, report.ID);
            report.Send("report:sendrating", false, report.ID, report.Rating);
        }


        public static bool CreateTransfer(ulong socialClubFrom, string fromName, string fromTo, CheckingAccount from, CheckingAccount to, int amount, string comment, string reason)
        {
            if (MoneyTransfers.FirstOrDefault(item => item.Value.SocialClubFrom == socialClubFrom).Value != null) return false;
            int id = _moneyTransfersID++;
            MoneyTransfer transfer = new MoneyTransfer(id, socialClubFrom, fromName, fromTo, from, to, amount, comment, reason);
            MoneyTransfers.Add(transfer.ID, transfer);
            transfer.Send("transfersConfirmation/updateTransfersList", JsonConvert.SerializeObject(transfer));
            return true;
        }


        public static void SuccessTransfer(int id, string adminName)
        {
            if (!MoneyTransfers.ContainsKey(id))
                return;
            MoneyTransfers[id].Success(adminName);
            MoneyTransfers.Remove(id);
        }

        public static void CancelTransfer(int id, string adminName)
        {
            if (!MoneyTransfers.ContainsKey(id))
                return;
            MoneyTransfers[id].Canceled(adminName);
            MoneyTransfers.Remove(id);
        }


    }
}
