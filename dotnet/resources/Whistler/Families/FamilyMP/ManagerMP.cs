using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Families.FamilyMenu;
using Whistler.Families.FamilyMP.DTO;
using Whistler.Families.FamilyMP.Models;
using Whistler.Families.FamilyMP.Models.ModelsMP;
using Whistler.Families.FamilyMP.Models.MPModels;
using Whistler.Families.FamilyWars;
using Whistler.Families.Models;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Families.FamilyMP
{
    class ManagerMP : Script
    {
        private static Dictionary<int, FamilyMPModel> _familyMPs = new Dictionary<int, FamilyMPModel>();
        public static FamilyMPModel CurrentMP;
        public ManagerMP()
        {
            LoadMPFromDB();
            Timers.Start(5 * 60 * 1000, CheckStartBattles);
            CheckStartBattles();
            IslandCaptureMP.Init();
            FamilyMenuManager.FamilyLoad += LoadAllMP;
        }
        private static void LoadMPFromDB()
        {
            DataTable result = MySQL.QueryRead("SELECT * FROM `familymp` where `date` > @prop0", MySQL.ConvertTime(DateTime.Now.AddDays(-3)));
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    FamilyMPModel mp = FabricMp(row, (FamilyMPType)Convert.ToInt32(row["type"]));
                    _familyMPs.Add(mp.ID, mp);
                }
            }
        }
        private static FamilyMPModel FabricMp(DataRow row, FamilyMPType type)
        {
            switch (type)
            {
                case FamilyMPType.IslandCapture:
                    return new IslandCaptureMP(row);
                case FamilyMPType.BusinessWar:
                    return new BusinessWarMP(row);
            }
            return null;
        }
        private static void LoadAllMP(ExtPlayer player, Family family)
        {
            SafeTrigger.ClientEvent(player, "family:loadMP", JsonConvert.SerializeObject(_familyMPs.Select(item => new FamilyMPModelDTO(item.Value))));
        }
        [Command("createislandcapt")]
        public static void CreateIslandCaptMP(ExtPlayer player, int month, int day, int hour, int minute)
        {
            if (!Group.CanUseAdminCommand(player, "createislandcapt")) return;

            var time = new DateTime(DateTime.Now.Year, month, day, hour, minute, 0);
            if (DateTime.Now.AddMinutes(0) > time)
            {
                Notify.SendError(player, "Создать МП можно не позже, чем за 30 минут до начала");
                return;
            }
            if (_familyMPs.FirstOrDefault(item => time.AddHours(-1) < item.Value.Date && item.Value.Date < time.AddHours(1)).Value != null)
            {
                Notify.SendError(player, "В это время уже есть МП");
                return;
            }

            FamilyMPModel mp = new IslandCaptureMP(time, BattleLocation.TheCayoPerico);
            _familyMPs.Add(mp.ID, mp);
            FamilyMenu.FamilyMenuManager.UpdateFamilyMP(mp);
            CheckStartBattles();
            Notify.SendSuccess(player, $"Вы успешно создали войну на острове.");
            GameLog.Admin(player.Name, $"createislandcapt({MySQL.ConvertTime(time)})", "");
        }
        [Command("createbizwar")]
        public static void CreateBusinessWar(ExtPlayer player, int businessId, int month, int day, int hour, int minute)
        {
            if (!Group.CanUseAdminCommand(player, "createbizwar")) return;

            DateTime time = new DateTime(DateTime.Now.Year, month, day, hour, minute, 0);

            if (DateTime.Now.AddMinutes(30) > time)
            {
                Notify.SendError(player, "Создать МП можно не позже, чем за 30 минут до начала");
                return;
            }
            if (_familyMPs.FirstOrDefault(item => time.AddHours(-1) < item.Value.Date && item.Value.Date < time.AddHours(1)).Value != null)
            {
                Notify.SendError(player, "В это время уже есть МП");
                return;
            }
            Business biz = BusinessManager.GetBusiness(businessId);
            if (biz == null)
            {
                Notify.SendError(player, "Бизнеса с таким ID не существует");
                return;
            }

            if (biz.FamilyPatronage > 0)
            {
                Notify.SendError(player, "Бизнес не является свободным");
                return;
            }
            var location = BattleLocation.RedwoodLights;
            if (WarManager.LocationIsOccupied(location, time))
            {
                Notify.SendError(player, "В это время есть бизвар");
                return;
            }
            FamilyMPModel mp = new BusinessWarMP(time, location, businessId);
            _familyMPs.Add(mp.ID, mp);
            FamilyMenu.FamilyMenuManager.UpdateFamilyMP(mp);
            CheckStartBattles();
            Notify.SendSuccess(player, $"Вы успешно создали войну за бизнес {biz.Name}.");
            GameLog.Admin(player.Name, $"createbizwar({businessId},{MySQL.ConvertTime(time)})", "");
        }

        private static void CheckStartBattles()
        {
            foreach (var mp in _familyMPs.Values.Where(item => item.Date > DateTime.Now && item.Date < DateTime.Now.AddMinutes(30) && !item.IsFinished && !item.IsPlaying))
            {
                if (mp._timer == null)
                    mp._timer = Timers.StartOnce((int)(mp.Date - DateTime.Now).TotalMilliseconds, () => StartBattle(mp));
                if ((int)(mp.Date - DateTime.Now).TotalMinutes > 0)
                    Chat.AdminToAll($"Мероприятие \"{mp.NameMP}\" начнется через {(int)(mp.Date - DateTime.Now).TotalMinutes} минут!");
                else
                    Chat.AdminToAll($"Мероприятие \"{mp.NameMP}\" начнется через {(int)(mp.Date - DateTime.Now).TotalSeconds} секунд!");

            }
        }

        private static void StartBattle(FamilyMPModel mp)
        {
            mp._timer = null;
            if (_familyMPs.FirstOrDefault(ItemType => ItemType.Value.IsPlaying).Value != null)
                return;
            if (CurrentMP != null)
                return;
            if (mp.TryStartMP())
                CurrentMP = mp;
        }
        public static bool PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            return CurrentMP?.PlayerDeath(player, killer, weapon) ?? false;
        }
        public static void OnPlayerDisconnected(ExtPlayer player)
        {
            IslandCaptureMP.PlayerDisconnected(player);
        }
    }
}