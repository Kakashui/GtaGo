using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Families;
using Whistler.Families.FamilyWars;
using Whistler.Helpers;
using Whistler.MP.OrgBattle.Models;
using Whistler.SDK;
using Whistler.VehicleSystem;

namespace Whistler.MP.OrgBattle.BattleModels
{
    class OrgBattleWithPoints : BattleBase
    {
        private const int _intervalTimeGivenPoints = 5000;
        private const int _pointForPlayer = 5;
        private const int _pointForKillPlayer = 2;

        private List<ExtPlayer> AllPlayers;

        public Dictionary<int, StatsModel> OrgStats;
        public Dictionary<ExtPlayer, int> Players;
        private string _intervalGivenPoints = null;

        private DateTime StartDate = DateTime.Now.AddDays(999);
        private readonly uint _dimension;
        private int _maxPlayers;
        /// <summary>
        /// time length in seconds
        /// </summary>
        private int _timeLength = 20*60;
        private int _timeWaitPlayers = 5*60;
        private OrganizationType _orgType;
        private OrgActivityType _orgActiveType;

        private OrgBattleStatus _battleStatus
        {
            get
            {
                if (DateTime.Now < StartDate)
                    return OrgBattleStatus.WaitStart;
                else if (DateTime.Now < StartDate.AddSeconds(_timeWaitPlayers))
                    return OrgBattleStatus.GoingAndOpenForEnter;
                else if (DateTime.Now < StartDate.AddSeconds(_timeLength))
                    return OrgBattleStatus.GoingAndClosedForEnter;
                else return OrgBattleStatus.Canceled;
            }
        }
        public BattleLocation Location { get; set; }
        public List<KillerModel> KillLog { get; set; }
        public OrgBattleWithPoints(int maxPlayers, int timeLength, int timeWaitPlayers, OrganizationType orgType, BattleLocation location, OrgActivityType orgActiveType)
        {
            _maxPlayers = maxPlayers;
            _timeLength = timeLength;
            _timeWaitPlayers = timeWaitPlayers;
            _orgType = orgType;
            _orgActiveType = orgActiveType;
            Location = location;
            _dimension = Core.Dimensions.RequestPrivateDimension();
        }
        public KillerModel AddKill(int uuid, string name, int familyId)
        {
            var killer = KillLog.FirstOrDefault(item => item.UUID == uuid);
            if (killer != null)
                killer.AddKill();
            else
            {
                killer = new KillerModel(name, uuid, 1, familyId);
                KillLog.Add(killer);
            }
            return killer;
        }
        public void PlayerEnterZone(ExtPlayer player, BattleLocation location)
        {
            if (location != Location)
                return;
            if (_battleStatus == OrgBattleStatus.GoingAndOpenForEnter)
            {
                if (RegisterPlayer(player))
                    EnterToZone(player);
            }
        }

        public void PlayerExitZone(ExtPlayer player, BattleLocation location)
        {
            if (location != Location)
                return;
            if (Players.ContainsKey(player))
                Players.Remove(player);
            else
                return;
            ExitFromZone(player);
        }

        internal override bool PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            if (!Players.ContainsKey(player))
                return false;
            int killerOrg = 0;
            if (killer.IsLogged() && Players.ContainsKey(killer))
            {
                killerOrg = Players[killer];
                SendKillStatAllPlayers(AddKill(killer.Character.UUID, killer.Name, Players[killer]));
            }

            if (Players.ContainsKey(player))
            {
                if (Players[player] > 0 && OrgStats.ContainsKey(killerOrg))
                {
                    OrgStats[killerOrg].Points += _pointForKillPlayer;
                    OrgStats[killerOrg].Kills++;
                }
                Players.Remove(player);
            }
            UnloadGameData(player);
            NAPI.Task.Run(() =>
            {
                player.Health = 0;
            }, 5000);
            return true;
        }

        internal override void PlayerDisconnected(ExtPlayer player)
        {
            if (Players.ContainsKey(player))
                Players.Remove(player);
        }

        private bool RegisterPlayer(ExtPlayer player)
        {
            if (player.Dimension != 0)
                return false;
            var org = player.GetOrganization(_orgType);
            if (org != null && org.OrgActiveType == _orgActiveType && Players.Where(item => item.Value == org.Id).Count() <= _maxPlayers && !Players.ContainsKey(player) && !AllPlayers.Contains(player))
            {
                Players.Add(player, org.Id);
                AllPlayers.Add(player);
                if (!OrgStats.ContainsKey(org.Id))
                    OrgStats.Add(org.Id, new StatsModel(org));
                return true;
            }
            else
                return false;
        }

        protected override void StartBattle()
        {
            StartDate = DateTime.Now;
            Players = new Dictionary<ExtPlayer, int>();
            OrgStats = new Dictionary<int, StatsModel>();
            AllPlayers = new List<ExtPlayer>();
            var players = Main.GetExtPlayersListByPredicate((player) => player.Character.IsAlive && player.Character.WarZone == Location);
            foreach (var player in players)
            {
                if (RegisterPlayer(player))
                    EnterToZone(player);
            }
            WarManager.SubscribeToBattleEvent(Location, PlayerEnterZone, PlayerExitZone);
            if (_intervalGivenPoints != null)
            {
                Timers.Stop(_intervalGivenPoints);
                _intervalGivenPoints = null;
            }
            _intervalGivenPoints = Timers.Start(_intervalTimeGivenPoints, AdditionalPoints);
        }
        private void EndBattle()
        {
            if (_battleStatus != OrgBattleStatus.Canceled)
                return;
            if (_intervalGivenPoints != null)
            {
                Timers.Stop(_intervalGivenPoints);
                _intervalGivenPoints = null;
            }
            int maxPoint = OrgStats.Count > 0 ? OrgStats.Max(item => item.Value.Points) : 0;
            var familyWin = OrgStats.FirstOrDefault(item => item.Value.Points == maxPoint).Value;

            WarManager.UnsubscribeToBattleEvent(Location, PlayerEnterZone, PlayerExitZone);
            foreach (var player in Players.Keys)
            {
                ExitFromZone(player);
            }
            BattleEnd((familyWin == null || familyWin.Points <= 0) ? 0 : familyWin.FamilyId);
        }


        private int GetPoints(int familyId)
        {
            return Players.Where(item => item.Value == familyId).Count() * _pointForPlayer;
        }
        private void AdditionalPoints()
        {
            foreach (var familyStats in OrgStats.Values)
            {
                familyStats.Points += GetPoints(familyStats.FamilyId);
            }
            foreach (var player in Players.Keys)
            {
                UpdatePoints(player);
            }
            EndBattle();
        }
        private void UpdatePoints(ExtPlayer player)
        {
            player.TriggerCefEvent("war/setTotalList", JsonConvert.SerializeObject(OrgStats.Values));
        }
        private void SendKillStatAllPlayers(KillerModel killerModel)
        {
            foreach (var player in Players.Keys)
            {
                player.TriggerCefEvent("war/updateTeamList", JsonConvert.SerializeObject(killerModel));
            }
        }

        private void EnterToZone(ExtPlayer player)
        {
            if (player.IsInVehicle)
                VehicleManager.WarpPlayerOutOfVehicle(player);
            SafeTrigger.UpdateDimension(player,  _dimension);
            LoadGameData(player);
        }

        private void ExitFromZone(ExtPlayer player)
        {
            if (player.IsInVehicle)
                VehicleManager.WarpPlayerOutOfVehicle(player);
            SafeTrigger.UpdateDimension(player,  0);
            UnloadGameData(player);
        }
        private void LoadGameData(ExtPlayer player)
        {
            UpdatePoints(player);
            player.TriggerCefEvent("war/setTeamList", JsonConvert.SerializeObject(KillLog));
            SafeTrigger.ClientEvent(player,"familyMp:showWarKey", true, WarManager._warPointPositions.GetValueOrDefault(Location)?.Position, WarManager._warPointPositions.GetValueOrDefault(Location)?.Range, Players.ContainsKey(player), (StartDate.AddSeconds(_timeLength) - DateTime.Now).TotalSeconds);
            player.TriggerCefEvent("war/setTittle", $"{(int)_orgType}");
        }

        private void UnloadGameData(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"familyMp:showWarKey", false, null, null, false, 0);
        }
    }
}
