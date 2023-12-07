using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Core;
using Whistler.Families.FamilyMenu;
using Whistler.Families.Models;
using Whistler.Families.WarForCompany.DTO;
using Whistler.Families.WarForCompany.Models;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.SDK;

namespace Whistler.Families.WarForCompany
{
    class WarCompanyManager : Script
    {
        private static Dictionary<int, Company> _companies = new Dictionary<int, Company>();
        public static readonly List<int> _fractionsInWar = new List<int> { 1, 2, 3, 4, 5, 16 };
        public static int _money = 100; //100$ в минуту за одну точку
        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {

            var result = MySQL.QueryRead("SELECT * FROM familycompanies");
            if (result == null || result.Rows.Count == 0)
            {
                CreateCompanies();
            }
            else
            {
                foreach (DataRow row in result.Rows)
                {
                    var company = new Company(row);
                    _companies.Add(company.ID, company);
                }
            }
            WarConfigs.Parse();
            Timers.Start(60 * 1000, PayMoney);
            FamilyMenuManager.FamilyLoad += (p,f) => LoadAllWars(p);
        }
        private static void CreateCompanies()
        {

            var company = new Company(new Vector3(2667.692, 2852.832, 39.0831), WarCompanies.DavisQuartz);
            _companies.Add(company.ID, company);
            company = new Company(new Vector3(5262.373, -5434.454, 64.59714), WarCompanies.TheCayoPerico);
            _companies.Add(company.ID, company);
            company = new Company(new Vector3(1721.863, -1657.297, 111.5422), WarCompanies.ElBurroHeights);
            _companies.Add(company.ID, company);
            company = new Company(new Vector3(-565.8944, 5326.192, 72.59285), WarCompanies.Sawmill);
            _companies.Add(company.ID, company);
            company = new Company(new Vector3(1382.5845, -2080.6091, 51.998558), WarCompanies.MurrietaHeights);
            _companies.Add(company.ID, company);
            company = new Company(new Vector3(2873.189, 4422.417, 47.76363), WarCompanies.UnionGrainSupply);
            _companies.Add(company.ID, company);
        }

        [ServerEvent(Event.PlayerConnected)]
        public static void OnPlayerConnected(Player player)
        {
            player.TriggerEvent("warForEnterprice:loadPeds", JsonConvert.SerializeObject(_companies.Select(item => new { id = item.Value.ID, position = item.Value.Position, heading = item.Value.Rotation })));
        }
        public static void LoadAllWars(Player player)
        {
            player.TriggerCefEvent("hud/warForEnterprice/setCaptureList", JsonConvert.SerializeObject(_companies.Where(item => item.Value.IsGoing).Select(item => new CompanyAttackDTO(item.Value))));
            player.TriggerCefEvent("warForEnterprice/setEnterpricesList", JsonConvert.SerializeObject(new { companies = _companies.Select(item => new CompanyDTO(item.Value)), profit = _money }));
        }
        public static void UpdateWar(Company company)
        {
            FamilyMenu.SubscribeSystem.TriggerCefEventToSubscribeAllFamily("warForEnterprice/updateEnterpricesList", JsonConvert.SerializeObject(new CompanyDTO(company)));
            Fractions.FractionSubscribeSystem.TriggerCefEventSubscribers(WarCompanyManager._fractionsInWar, "warForEnterprice/updateEnterpricesList", JsonConvert.SerializeObject(new CompanyDTO(company)));
        }

        public static void DisconnectedPlayer(Player player)
        {
            foreach (var company in _companies)
            {
                if (company.Value.DisconnectedPlayer(player))
                    return;
            }
        }

        public static void PlayerDeath(Player player)
        {
            foreach (var company in _companies)
            {
                if (company.Value.PlayerDeath(player))
                    return;
            }
        }

        private static void PayMoney()
        {
            var families = _companies.Where(item => !item.Value.IsGoing && item.Value.OwnerType == OwnerType.Family && item.Value.OwnerId > 0).Select(item => item.Value.OwnerId).ToList();
            var fractions = _companies.Where(item => !item.Value.IsGoing && item.Value.OwnerType == OwnerType.Fraction && item.Value.OwnerId > 0).Select(item => item.Value.OwnerId).ToList();
            foreach (var fam in families.Distinct())
            {
                int count = families.Where(item => item == fam).Count();
                int money = _money * count;
                if (count >= 3)
                    money *= 2;
                FamilyManager.GetFamily(fam).MoneyForEnterprise += money;
            }
            foreach (var frac in fractions.Distinct())
            {
                int count = fractions.Where(item => item == frac).Count();
                int money = _money * count;
                if (count >= 3)
                    money *= 2;
                Manager.GetFraction(frac).MoneyForEnterprise += money;
            }
        }

        [Command("companychangeped")]
        public static void SetPed(Player player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "companychangeped")) return;
            _companies.FirstOrDefault(item => item.Value.Key == (WarCompanies)id).Value?.ChangePosition(player);
        }
    }
}
