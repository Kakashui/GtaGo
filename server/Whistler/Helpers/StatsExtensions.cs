using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Fractions;
using Whistler.GUI.Documents;
using Whistler.MoneySystem;

namespace Whistler.Helpers
{
    public static class StatsExtensions
    {
        public static List<string> GetStats(this Player player)
        {

            #region Stats making
            var acc = player.GetCharacter();
            string status =
                (acc.AdminLVL >= 1)
                ? "Core_247"
                : (player.GetAccount().IsPrimeActive())
                    ? $"Prime account({player.GetAccount().VipDate.ToString("dd.MM.yyyy")})"
                    : "Base account";
            var lic = player.GetLicenses();
            if (lic == "") 
                lic = "local_13";
            string work = (acc.WorkID > 0 && (acc.WorkID != Jobs.Technician.Work.WorkID && acc.WorkID != Jobs.CarThief.Work.WorkID)) 
                ? Jobs.WorkManager.JobStats[acc.WorkID - 1] 
                : "Core_248";
            string fraction = Configs.GetConfigOrDefault(acc.FractionID).Name;
            var number = acc.PhoneTemporary?.Simcard?.Number.ToString() ??  "Core_249";
            #endregion
            List<string> result = new List<string>
            {
                "local_17".Translate(acc.LVL, acc.EXP, 3 + acc.LVL * 3),
                "local_18".Translate(status, acc.Warns, acc.CreateDate.ToString("dd.MM.yyyy")),
                "local_19".Translate(number),
                "local_14".Translate(lic),
                "local_20".Translate(acc.UUID, acc.BankModel.Number),
                "local_16".Translate(work, fraction, acc.FractionLVL),
            };
            return result;
        }
        public static List<string> GetLicensesList(this Player player)
        {
            return player.GetCharacter().Licenses.Where(item => item.IsActive).Select(item => DocumentConfigs.GetLicenseWord(item.Name)).ToList();
        }
        public static string GetLicenses(this Player player)
        {
            var character = player.GetCharacter();
            var lic = "";
            foreach (var item in character.Licenses.Where(item => item.IsActive))
            {
                lic += $"{DocumentConfigs.GetLicenseWord(item.Name)} / ";
            }
            return lic;
        }
    }
}
