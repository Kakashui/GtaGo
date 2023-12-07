using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Families.FamilyMenu;
using Whistler.Families.Models;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Families.FamilyWars
{
    class WarManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(WarManager));
        private static Dictionary<int, BattleModel> _battles = new Dictionary<int, BattleModel>();
        public static Dictionary<BattleLocation, BattleZoneModel> _warPointPositions = new Dictionary<BattleLocation, BattleZoneModel>()
        {
            { BattleLocation.ElBurroHeights, new BattleZoneModel(new Vector3(1714.411, -1646.583, 110.5078), 150, true, 543, 40, 0) },
            { BattleLocation.ElysianIsland, new BattleZoneModel(new Vector3(525.6157, -3163.575, 2.183115), 150, false, 543, 40, 0) },
            { BattleLocation.GrandSenoraDesert, new BattleZoneModel(new Vector3(2367, 3074, 48), 150, false, 543, 40, 0) },
            { BattleLocation.TheCayoPerico, new BattleZoneModel(new Vector3(4991, -4886, 0), 1500, false, 543, 40, 0) },
            { BattleLocation.StabCity, new BattleZoneModel(new Vector3(61.87453, 3703.324, 39.75499), 150, true, 543, 40, uint.MaxValue) },
            { BattleLocation.Galilee, new BattleZoneModel(new Vector3(1349.495, 4361.81, 44.21515), 150, true, 543, 40, 0) },
            { BattleLocation.RedwoodLights, new BattleZoneModel(new Vector3(1020, 2364, 51.10004), 300, true, 543, 4, uint.MaxValue) },
            //{ BattleLocation.WeedFarmBattle, new BattleZoneModel(new Vector3(62.81924, 3693.778, 39.75498), 300, true, 543, 4) },
        };
        private static Dictionary<BattleLocation, BusinessWar> _warPoints = new Dictionary<BattleLocation, BusinessWar>();

        //Интервал проверки битв
        private static int _checkInterval = 1;

        private static int minHour = 13; //13
        private static int maxHour = 23; //23

        //Время на подготовку к битве, сек
        internal static int TimeToCapt = 600; //600
        //Длительность битвы, сек
        internal static int BizwarLength = 300; //300
        //минимальное время перез назначением битвы, мин
        private static int _createBattleTimeOut = 60; //60

        //Время на одну битву (чтоб не наслаивались друг на друга на одной территории / у одной семьи), сек
        private static int _battleInterval = 60 * 60;


        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                foreach (var point in _warPointPositions)
                {
                    _warPoints.Add(point.Key, new BusinessWar(point.Key, point.Value));
                }
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        public static void LoadBattles()
        {
            var result = MySQL.QueryRead("SELECT * FROM `familybattles` WHERE `time` > @prop0 OR `status` < @prop1", MySQL.ConvertTime(DateTime.Now.AddDays(-3)), (int)BattleStatus.Canceled);

            if (result == null)
            {
                _logger.WriteWarning("DB `familybattles` return null result");
            }
            else
            {
                foreach (DataRow Row in result.Rows)
                {
                    BattleModel battle = new BattleModel(Row);
                    _battles.Add(battle.Id, battle);
                }
            }
            CheckBattles();
            Timers.Start(_checkInterval * 60 * 1000, CheckBattles);
        }
        public static List<BattleModel> GetFamilyBattles(int family)
        {
            return _battles.Values.Where(item => item.FamilyAttack == family || item.FamilyDef == family).ToList();
        }
        public static BattleModel GetBattle(int battleId)
        {
            if (!_battles.ContainsKey(battleId))
                return null;
            return _battles[battleId];
        }
        private static void CheckBattles()
        {
            foreach (var battle in _battles)
            {
                CheckBattle(battle.Value);
            }
        }
        private static void CheckBattle(BattleModel battle)
        {
            var timeToBattle = (int)(battle.Date - DateTime.Now).TotalSeconds;
            if (timeToBattle >= TimeToCapt && timeToBattle <= 2.5 * TimeToCapt && battle.Timer == null && battle.Status == BattleStatus.Confirm)
            {
                battle.Timer = Timers.StartOnce((timeToBattle - TimeToCapt) * 1000, () => 
                { 
                    StartBusinessWar(battle); 
                });
            }
            else if (timeToBattle < TimeToCapt && battle.Timer == null && battle.Status < BattleStatus.Fighting)
            {
                if (battle.Status == BattleStatus.WaitConfirm)
                    battle.ConfirmBattle(false);
                else
                    battle.NotCarriedOut();
            }
        }


        public static int GetDiffRating(Family family, int eloTeamTwo, bool teamOneWinner)
        {
            if (family == null)
                return 0;
            double point = 1.0 / (1.0 + Math.Pow(10.0, (eloTeamTwo - family.EloRating) / 400.0));
            double diff = GetCoeffElo(family.EloRating, family.CountBattles) * ((teamOneWinner ? 1 : 0) - point);
            return (int)diff;
        }

        private static int GetCoeffElo(int elo, int countBattle)
        {
            if (elo >= 2400)
                return 10;
            if (countBattle <= 30)
                return 40;
            return 20;
        }

        public static bool StartBusinessWar(BattleModel battleModel)
        {
            Business biz = BusinessManager.BizList.FirstOrDefault(item => item.Key == battleModel.BizId).Value;
            if (biz == null)
            {
                return false;
            }
            if (biz.FamilyPatronage != battleModel.FamilyDef)
            {
                return false;
            }

            Family family = FamilyManager.GetFamily(battleModel.FamilyAttack);
            if (family == null)
            {
                return false;
            }
            Family familyDefend = FamilyManager.GetFamily(biz.FamilyPatronage);
            if (familyDefend == null)
            {
                return false;
            }
            if (!_warPoints.ContainsKey(battleModel.Location))
                return false;
            return _warPoints[battleModel.Location].CMD_startBizwar(family, familyDefend, battleModel);
        }

        public static CreateBattleResult CreateBattle(int bizId, int familyDef, int familyAttack, BattleLocation location, int weapon, DateTime date, string comment)
        {
            /*
            NAPI.Util.ConsoleOutput($"{date} - date");
            NAPI.Util.ConsoleOutput($"{DateTime.Now} - dateTimeNow");
            NAPI.Util.ConsoleOutput($"{(date - DateTime.Now).TotalMinutes} - totalmin");
            NAPI.Util.ConsoleOutput($"{_createBattleTimeOut} - _createBattleTimeOut");
            */

            if ((date - DateTime.Now).TotalMinutes < _createBattleTimeOut)
                return CreateBattleResult.TooEarly;
            if (date.Hour < minHour || date.Hour >= maxHour)
                return CreateBattleResult.BadTime;
            if (familyDef == familyAttack)
                return CreateBattleResult.ItIsYourBusiness;
            foreach (var battle in _battles.Values.Where(item => item.Status <= BattleStatus.Fighting))
            {
                if (battle.BizId == bizId)
                    return CreateBattleResult.BusinessIsAttaked;
                if ( Math.Abs((battle.Date - date).TotalSeconds) < _battleInterval)
                {
                    if (battle.Location == location)
                        return CreateBattleResult.LocationIsOccupied;
                    if (battle.FamilyAttack == familyAttack || battle.FamilyDef == familyAttack)
                        return CreateBattleResult.AttackersIsBusy;
                    if (battle.FamilyAttack == familyDef || battle.FamilyDef == familyDef)
                        return CreateBattleResult.DefendersIsBusy;
                }
            }
            var battleModel = new BattleModel(bizId, familyDef, familyAttack, location, weapon, date, comment);
            _battles.Add(battleModel.Id, battleModel);
            FamilyMenuManager.UpdateBattleInfo(battleModel);
            return CreateBattleResult.Success;
        }

        public static bool LocationIsOccupied(BattleLocation location, DateTime date)
        {
            return _battles.Values.FirstOrDefault(battle => battle.Location == location && Math.Abs((battle.Date - date).TotalSeconds) < _battleInterval) != null;
        }
        public static bool Event_PlayerDeath(ExtPlayer player, ExtPlayer Killer, uint weapon)
        {
            try
            {
                bool result = false;
                foreach (var warPoint in _warPoints)
                {
                    if (warPoint.Value.Event_PlayerDeath(player, Killer, weapon))
                        result = true;
                }
                return result;
            }
            catch (Exception e) 
            { 
                _logger.WriteError("PlayerDeath: " + e.ToString());
                return false;
            }
        }

        public static void SubscribeToBattleEvent(BattleLocation location, Action<ExtPlayer, BattleLocation> actionEnter, Action<ExtPlayer, BattleLocation> actionExit )
        {
            _warPoints[location].PlayerEnterBattleLocation += actionEnter;
            _warPoints[location].PlayerExitBattleLocation += actionExit;
            if (_warPoints[location].WarBlip != null)
                _warPoints[location].WarBlip.Color = 1;
        }

        public static void UnsubscribeToBattleEvent(BattleLocation location, Action<ExtPlayer, BattleLocation> actionEnter, Action<ExtPlayer, BattleLocation> actionExit )
        {
            _warPoints[location].PlayerEnterBattleLocation -= actionEnter;
            _warPoints[location].PlayerExitBattleLocation -= actionExit;
            if (_warPoints[location].WarBlip != null)
                _warPoints[location].WarBlip.Color = _warPointPositions.GetValueOrDefault(location)?.BlipColor ?? 40;
        }
    }
}
