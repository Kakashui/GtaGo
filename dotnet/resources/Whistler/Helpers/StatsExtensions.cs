using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.GUI.Documents;
using Whistler.MoneySystem;

namespace Whistler.Helpers
{
    public static class StatsExtensions
    {
        public static List<string> GetStats(this ExtPlayer player)
        {

            #region Stats making
            var acc = player.Character;
            string status =
                (acc.AdminLVL >= 1)
                ? "Администратор"
                : (player.Character.IsPrimeActive())
                    ? $"Prime account({player.Character.VipDate.ToString("dd.MM.yyyy")})"
                    : "Base account";
            var lic = player.GetLicenses();
            if (lic == "") 
                lic = "Нету";
            string work = (acc.WorkID > 0 && (acc.WorkID != Jobs.Technician.Work.WorkID && acc.WorkID != Jobs.CarThief.Work.WorkID)) 
                ? Jobs.WorkManager.JobStats[acc.WorkID - 1] 
                : "Безработный";
            string fraction = Configs.GetConfigOrDefault(acc.FractionID).Name;
            var number = acc.PhoneTemporary?.Phone?.SimCard?.Number.ToString() ??  "Сим-карта отсутствует";
            #endregion
            List<string> result = new List<string>
            {
                $"Level: {acc.LVL} | EXP: {acc.EXP}/{3 + acc.LVL * 3}",
                $"Status: {status} | Warns: {acc.Warns} | CreateDate: {acc.CreateDate.ToString("dd.MM.yyyy")}",
                $"SIM Number: {number}",
                $"Licenses: {lic}",
                $"Passport: {acc.UUID} | Bank number: {acc.BankModel.Number}",
                $"Work: {work} | Fraction: {fraction} | FractionRank: {acc.FractionLVL}",
            };
            return result;
        }
        public static List<string> GetLicensesList(this ExtPlayer player)
        {
            return player.Character.Licenses.Where(item => item.IsActive).Select(item => DocumentConfigs.GetLicenseWord(item.Name)).ToList();
        }
        public static string GetLicenses(this ExtPlayer player)
        {
            Core.Character.Character character = player.Character;
            string lic = "";
            foreach (var item in character.Licenses.Where(item => item.IsActive))
            {
                lic += $"{DocumentConfigs.GetLicenseWord(item.Name)} / ";
            }
            return lic;
        }
    }
}
