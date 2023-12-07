using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using MessagePack.Formatters;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
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
        public static int _money = 75;
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
        public static void OnPlayerConnected(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"warForEnterprice:loadPeds", JsonConvert.SerializeObject(_companies.Select(item => new { id = item.Value.ID, position = item.Value.Position, heading = item.Value.Rotation })));
        }
        public static void LoadAllWars(ExtPlayer player)
        {
            player.TriggerCefEvent("hud/warForEnterprice/setCaptureList", JsonConvert.SerializeObject(_companies.Where(item => item.Value.IsGoing).Select(item => new CompanyAttackDTO(item.Value))));
            player.TriggerCefEvent("warForEnterprice/setEnterpricesList", JsonConvert.SerializeObject(new { companies = _companies.Select(item => new CompanyDTO(item.Value)), profit = _money }));
        }
        public static void UpdateWar(Company company)
        {
            FamilyMenu.SubscribeSystem.TriggerCefEventToSubscribeAllFamily("warForEnterprice/updateEnterpricesList", JsonConvert.SerializeObject(new CompanyDTO(company)));
            Fractions.FractionSubscribeSystem.TriggerCefEventSubscribers(WarCompanyManager._fractionsInWar, "warForEnterprice/updateEnterpricesList", JsonConvert.SerializeObject(new CompanyDTO(company)));
        }

        public static void DisconnectedPlayer(ExtPlayer player)
        {
            foreach (var company in _companies)
            {
                if (company.Value.DisconnectedPlayer(player))
                    return;
            }
        }

        public static void PlayerDeath(ExtPlayer player)
        {
            foreach (var company in _companies)
            {
                if (company.Value.PlayerDeath(player))
                    return;
            }
        }

        private static void PayMoney()
        {
            if (!_companies.Any()) return;

            List<int> families = _companies.Where(item => !item.Value.IsGoing && item.Value.OwnerType == OwnerType.Family && item.Value.OwnerId > 0).Select(item => item.Value.OwnerId).ToList();
            List<int> fractions = _companies.Where(item => !item.Value.IsGoing && item.Value.OwnerType == OwnerType.Fraction && item.Value.OwnerId > 0).Select(item => item.Value.OwnerId).ToList();
            int count;
            int money;
            if (families != null && families.Any())
            {
                Family family;
                foreach (int fam in families.Distinct())
                {
                    count = families.Where(item => item == fam).Count();
                    money = _money * count;
                    if (count >= 3) money *= 2;

                    family = FamilyManager.GetFamily(fam);
                    if (family != null) family.MoneyForEnterprise += money;
                }
            }
            if (fractions != null && fractions.Any())
            {
                Fractions.Models.Fraction fraction;
                foreach (int frac in fractions.Distinct())
                {
                    count = fractions.Where(item => item == frac).Count();
                    money = _money * count;
                    if (count >= 3) money *= 2;

                    fraction = Manager.GetFraction(frac);
                    if (fraction != null) fraction.MoneyForEnterprise += money;
                }
            }
        }

        [Command("companychangeped")]
        public static void SetPed(ExtPlayer player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "companychangeped")) return;
            _companies.FirstOrDefault(item => item.Value.Key == (WarCompanies)id).Value?.ChangePosition(player);
        }
    }
}
