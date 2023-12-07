using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Core.CustomSync;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.MP.RoyalBattle.Configs;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.MP.RoyalBattle.Models
{
    class RoyalBattleModel
    {
        private static Random _rnd = new Random();
        private ZoneModel currentZone { get; set; }
        private ZoneModel nextZone { get; set; }
        private ZoneModel finalZone { get; set; }
        private Dictionary<ExtPlayer, BattlePlayer> _players { get; set; }
        private StageModel _currentStageModel 
        {
            get
            {
                if (Configurations.ListRadiuses.ContainsKey(_currentStage))
                    return Configurations.ListRadiuses[_currentStage];
                return Configurations.ListRadiuses[0];
            }
        }
        private int _currentStage { get; set; }
        private string updateTimer { get; set; }
        private uint _dimension { get; set; }
        public RoyalBattleModel(List<ExtPlayer> players)
        {
            _currentStage = 0;
            currentZone = new ZoneModel(Configurations.StartZone);
            finalZone = new ZoneModel(Configurations.FinalPoints.GetRandomElement(), 20);
            //NAPI.Marker.CreateMarker(1, finalZone.Center, new Vector3(), new Vector3(), finalZone.Range * 2, new Color(100, 255, 10));
            nextZone = GetNextZone(currentZone);

            _dimension = 11333377;
            _players = players.Select(p => new BattlePlayer(p)).ToDictionary(item => item._player);
        }

        public void CreateRange()
        {
            updateTimer = Timers.StartOnce(_currentStageModel.Time * 1000, UpdateRange);
            SafeTrigger.ClientEventToPlayers(_players.Keys.ToArray(), "royalBattle:createZone", JsonConvert.SerializeObject(currentZone), JsonConvert.SerializeObject(nextZone), _currentStageModel.TimeForConstriction, _currentStageModel.Time, false);

            //SafeTrigger.ClientEventToPlayers(Main.Players.Where(p => p.Value.DemorganTime > 0).Select(item => item.Key).ToArray(), "royalBattle:createZone", JsonConvert.SerializeObject(currentZone), JsonConvert.SerializeObject(nextZone), _currentStageModel.TimeForConstriction, _currentStageModel.Time, true);
        }

        public void UpdateRange()
        {
            currentZone = nextZone;
            nextZone = GetNextZone(currentZone);
            SetUpdateRangeTimer();
        }
        public bool IsInBattle(ExtPlayer player)
        {
            return _players.ContainsKey(player);
        }

        public List<PlayerRatingDTO> GetPlayers()
        {
            return _players.Select(item => new PlayerRatingDTO(item.Value._player.Name, item.Value.Kills, -1)).ToList();
        }

        private ZoneModel GetNextZone(ZoneModel currentZone)
        {
            _currentStage++;
            ZoneModel zone = new ZoneModel(currentZone);
            zone.Range = _currentStageModel.Range;
            if (zone.Range < finalZone.Range)
                zone.Range = 0;
            else if (zone.Range == finalZone.Range)
                zone = finalZone;
            else
            {
                zone.Center = GetRandomPointInCrossingTwoCircle(zone.Center, (currentZone.Range - zone.Range), finalZone.Center, (zone.Range - finalZone.Range)); //zone.Center.GetRandomPointInRange((currentZone.Range - zone.Range) / 2);
            }
            return zone;
        }
        

        private static Vector3 GetRandomPointInCrossingTwoCircle(Vector3 center1, double radius1, Vector3 center2, double radius2)
        {
            Vector3 result = center1;

            int iter = 0;
            double d = center1.DistanceTo2D(center2);
            do
            {
                if (d > radius1 + radius2)
                    break;
                result = center1.GetRandomPointInRange(radius1);
                iter++;
                if (iter > 1000)
                {
                    double a = (radius1 * radius1 - radius2 * radius2 + d * d) / (2 * d);
                    double resX = center1.X + a * (center2.X - center1.X) / d;
                    double resY = center1.Y + a * (center2.Y - center1.Y) / d;
                    result = new Vector3(resX, resY, center1.Z);
                    break;
                }
            }
            while (Math.Pow((result.X - center2.X), 2) + Math.Pow((result.Y - center2.Y), 2) > Math.Pow(radius2, 2));
            return result;
        }

        private void SetUpdateRangeTimer()
        {
            if (updateTimer != null)
            {
                Timers.Stop(updateTimer);
                updateTimer = null;
            }
            if (nextZone.Range > 0)
                updateTimer = Timers.StartOnce(_currentStageModel.Time * 1000, UpdateRange);
            SafeTrigger.ClientEventToPlayers(_players.Keys.ToArray(), "royalBattle:updateZone", JsonConvert.SerializeObject(nextZone), _currentStageModel.TimeForConstriction, _currentStageModel.Time, false);

            //if (_dimension == 1337)
            //    SafeTrigger.ClientEventToPlayers(Main.Players.Where(p => p.Value.DemorganTime > 0).Select(item => item.Key).ToArray(), "royalBattle:updateZone", JsonConvert.SerializeObject(nextZone), _currentStageModel.TimeForConstriction, _currentStageModel.Time, true);
        }

        private void PlayerStartBattle(ExtPlayer player)
        {
            player.UnCuffed();
            var point = Configurations.JumpPoints.GetRandomElement();
            var pos = point.Center.GetRandomPointInRange(point.Range);
            if (player.Character.IsAlive)
            {
                player.StopAnimGo();
                player.ChangePosition(pos);
                player.Health = 100;
            }
            else
                Ems.RevivePlayer(player, pos, 100);
            SafeTrigger.UpdateDimension(player,  _dimension);

            player.CreateTemporaryInventory(40000, 40);
            player.CreateTemporaryEquip();

            bool gender = player.GetGender();
            var cloth = ItemsFabric.CreateCostume(ItemNames.StandartCostume, Configurations.CostumesList[gender].GetRandomElement(), ClothesOwn.BattleRoyal, gender, false, true);
            BaseItem oldItem = null;
            player.GetEquip().EquipItem(player, cloth, ClothesSlots.Costume, ref oldItem, LogAction.None);
            var mask = ItemsFabric.CreateClothes(ItemNames.Mask, gender, _rnd.Next(28, 120), 0, false, true);
            player.GetEquip().EquipItem(player, mask, ClothesSlots.Mask, ref oldItem, LogAction.None);

            SafeTrigger.ClientEvent(player,"royalBattle:startBattle", _players.Count);
        }        

        private void PlayerEndBattle(ExtPlayer player, int rating, bool isDeath = true)
        {
            if (!player.IsLogged())
                return;
            SafeTrigger.ClientEvent(player,"royalBattle:endBattle", Configurations.TimeToSendToStartPosition);
            RoyalBattleService.AddBattleRating(player.Character.UUID, rating, true);
            ClearTempInventory(player);

            NAPI.Task.Run(() =>
            {
                if (isDeath)
                    Ems.RevivePlayer(player, Configurations.ExitPosition, 100);
                else
                    player.ChangePosition(Configurations.ExitPosition);
                SafeTrigger.UpdateDimension(player,  0);
            }, Configurations.TimeToSendToStartPosition);
        }

        public void StartRoyalBattle()
        {
            _players.Keys.ToList().ForEach(player => PlayerStartBattle(player));
            CreateRange();
            SpawnBattleData();
        }

        public bool PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            bool res = false;
            if (_players.ContainsKey(player))
            {
                res = true;
                PlayerEndBattle(player, _players[player].Kills);
                _players.Remove(player);
                SafeTrigger.ClientEventToPlayers(_players.Keys.ToArray(), "royalBattle:updateCountPlayers", _players.Count);
                if (killer.IsLogged() && _players.ContainsKey(killer))
                {
                    _players[killer].Kills++;
                    SafeTrigger.ClientEvent(killer, "royalBattle:updateKills", _players[killer].Kills);
                }
            }
            if (_dimension == 1337)
                if (player.Character.DemorganTime > 0)
                    SafeTrigger.ClientEvent(player,"royalBattle:endDemorganBattle");
            CheckEndBattle();
            return res;
        }
        public void OnPlayerDisconnected(ExtPlayer player)
        {
            if (!_players.ContainsKey(player)) return;

            PlayerEndBattle(player, _players[player].Kills, false);
            _players.Remove(player);
            SafeTrigger.ClientEventToPlayers(_players.Keys.ToArray(), "royalBattle:updateCountPlayers", _players.Count);
            CheckEndBattle();
        }

        private void CheckEndBattle()
        {
            if (_players.Count > 1) return;

            KeyValuePair<ExtPlayer, BattlePlayer> winner = _players.FirstOrDefault();
            if (winner.Key != null)
            {
                PlayerEndBattle(winner.Key, winner.Value.Kills, false);
                winner.Key.CreatePlayerAction(PersonalEvents.PlayerActions.WinRoyalBattle, 1);
            }
            RoyalBattleService.EndBattle(winner.Key);
        }

        private void SpawnBattleData()
        {
            Configurations.WeaponPoints.ForEach(points =>
                points.GetElementsWithRandomProbability(0.35).ForEach(point => RoyalBattleService.DropThingOnPoint(point, _dimension))
                );
            Configurations.VehiclePoints.ForEach(points =>
                points.GetElementsWithRandomProbability(0.15).ForEach(point =>
                {
                    var vehModel = Configurations.VehicleList.GetRandomElement();
                    ExtVehicle vehicle = VehicleManager.CreateTemporaryVehicle(NAPI.Util.GetHashKey(vehModel), point.Position + new Vector3(0, 0, 0.15), point.Rotation, "BATTLE", VehicleAccess.RoyalBattle, dimension: _dimension);
                    VehicleStreaming.SetVehicleFuel(vehicle, 30);
                    VehicleStreaming.SetLockStatus(vehicle, false);
                })
            );
        }

        public void Destroy()
        {
            if (updateTimer != null)
            {
                Timers.Stop(updateTimer);
                updateTimer = null;
            }
            DropSystem.ClearItemsInDimension(_dimension);
            Trigger.GetAllVehicles().ForEach(item =>
            {
                if (item.Data.OwnerType == OwnerType.Temporary && (item.Data as VehicleSystem.Models.VehiclesData.TemporaryVehicle).Access == VehicleAccess.RoyalBattle)
                    item.CustomDelete();
            });
        }


        private void ClearTempInventory(ExtPlayer player)
        {
            var tempInventory = player.GetInventory();
            tempInventory.RemoveItems(item => true, player: player);
            var tempEquip = player.GetEquip();
            tempEquip.RemoveClothes(item => true, player: player);
            tempEquip.RemoveWeapons(item => true, player: player);

            player.DeleteTemporaryInventory();
            player.DeleteTemporaryEquip();
        }
    }
}
