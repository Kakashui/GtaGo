using GTANetworkAPI;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using Whistler.Fishing.Models;
using Whistler.Fishing.Extensions;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Fishing
{
    class FishingManager
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(FishingManager));
        private Dictionary<int, FishingSpot> _spotList;
        private Dictionary<int, GTANetworkAPI.Marker> _markers;
        private static int _rodChance = 50;
        private static List<FishingSpot> _seaSpots = new List<FishingSpot>
        {
            new FishingSpot(new Vector3(-1808.428, -1924.931, 1.130133), new Vector3(-6.834919, -1.904598, 191.8044), false),
            new FishingSpot(new Vector3(-2154.182, -1475.645, 0.566226), new Vector3(1.284342, -0.8108431, 41.73535), false),
            new FishingSpot(new Vector3(-2591.448, -662.799, 1.596276), new Vector3(0.9785811, 0.1610055, 328.2831), false),
            new FishingSpot(new Vector3(-2262.901, -3062.563, 1.062013), new Vector3(-2.567086, 5.433365, 183.8428), false),
            new FishingSpot(new Vector3(-3480.988, 2627.243, 1.175579), new Vector3(-1.069484, 4.356974, 15.24551), false),
            new FishingSpot(new Vector3(-2528.712, 4818.244, 1.493386), new Vector3(1.899142, 2.749632, 343.1891), false),
            new FishingSpot(new Vector3(-535.1511, 6817.493, 0.4809905), new Vector3(1.853396, -1.567439, 309.757), false),
            new FishingSpot(new Vector3(1233.514, 7114.726, 1.72679), new Vector3(4.599545, -2.819381, 261.6898), false),
            new FishingSpot(new Vector3(3796.75, 5271.145, 1.423738), new Vector3(-3.60454, 2.691448, 189.3977), false),
            new FishingSpot(new Vector3(4152.704, 4015.797, 1.337925), new Vector3(-0.7456471, -2.334875, 168.476), false),
            new FishingSpot(new Vector3(3768.095, 2161.172, 1.10874), new Vector3(-0.4352216, -1.023708, 203.0609), false),
            new FishingSpot(new Vector3(3096.185, -952.1195, 1.692702), new Vector3(-0.1684851, 5.222973, 180.4982), false)
        };
        private static List<Vector3> _spotBlips = new List<Vector3>
        {
            new Vector3(-1843.08, -1256.018, 7.463037),
            //new Vector3(-3428.327, 975.9633, 7.226685),
            //new Vector3(-269.3914, 6646.348, 6.31192)
        };
        private static DateTime _lastSeaSpotsUpdate;
        internal void DoAction(ExtPlayer client)
        {
            try
            {
                FishingActions action = client.Session.FishingAction;
                switch (action)
                {
                    case FishingActions.FishSpot:
                        int id = client.Session.FishingSpotId;
                        if (id < 0)
                        {
                            SafeTrigger.ClientEvent(client, "notify", 4, 9, "Здесь нельзя ловить рыбу", 3000);
                            return;
                        }
                        if (client.IsInVehicle)
                        {
                            Notify.Send(client, NotifyType.Warning, NotifyPosition.BottomCenter, "Нельзя ловить рыбу находясь в транспорте", 3000);
                            return;
                        }

                        GUI.Documents.Models.License license = client.Character.Licenses.FirstOrDefault(item => item.Name == GUI.Documents.Enums.LicenseName.Fishing);
                        if (license == null)
                        {
                            Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Вам нужно приобрести лицензию рыболова, чтобы ловить рыбу", 5000);
                            return;
                        }

                        Inventory.Models.InventoryModel inventory = client.GetInventory();
                        if (inventory == null || inventory.Items == null) return;

                        Inventory.Models.Rod rod = inventory.Items.GetActiveRod();
                        if (rod == null)
                        {
                            Notify.Send(client, NotifyType.Warning, NotifyPosition.BottomCenter, "Вы не активировали удочку", 3000);
                            return;
                        }

                        Inventory.Models.BaseItem bait = inventory.Items.GetBait();
                        if (bait == null)
                        {
                            Notify.Send(client, NotifyType.Warning, NotifyPosition.BottomCenter, "У вас закончилась наживка для рыбы", 3000);
                            return;
                        }

                        var cage = inventory.Items.GetFreeCage();
                        if (cage == null)
                        {
                            Notify.Send(client, NotifyType.Warning, NotifyPosition.BottomCenter, "У вас нет ведра для рыбы", 3000);
                            return;
                        };

                        FisherData fisher = client.Session.FisherData;
                        if (fisher.MiniGame.Active)
                        {
                            fisher.MiniGame.Stop();
                            Notify.Send(client, NotifyType.Info, NotifyPosition.BottomCenter, "Вы закончили рыбачить", 3000);
                        }
                        else
                        {
                            inventory.SubItemByName(ItemNames.FishingBait, 1, LogAction.Use);
                            Notify.Send(client, NotifyType.Info, NotifyPosition.BottomCenter, "Вы начали рыбачить. Использована 1 наживка", 3000);
                            fisher.MiniGame.Start(_spotList[id], fisher, rod);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e) { _logger.WriteError("DoAction: " + e.ToString()); }
        }

        internal void DropFish(ExtPlayer client, int id, int count)
        {
            try
            {
                var inventory = client.GetInventory();
                var cage = inventory.Items.GetFreeCage();
                if (cage == null)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Ведро не найдено", 3000);
                    return;
                }
                if (!cage.Fishings.ContainsKey(id))
                    return;
                cage.Fishings[id] -= count;
                if (cage.Fishings[id] < 1)
                    cage.Fishings.Remove(id);
                inventory.UpdateItemData(cage.Index);
                SafeTrigger.ClientEvent(client, Const.CLIENT_EVENT_UPDATE_CAGE, cage.Fishings);
            }
            catch (Exception e) { _logger.WriteError("DropFish: " + e.ToString()); }
        }

        internal void EndMiniGame(ExtPlayer client, bool result)
        {
            try
            {
                var inventory = client.GetInventory();
                Inventory.Models.Rod rod = inventory.Items.GetActiveRod();
                FisherData fisher = client.Session.FisherData;
                fisher.MiniGame.Stop();
                if (rod == null)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас нет удочки в руках", 3000);
                    return;
                }
                if (result)
                {
                    var cage = inventory.Items.GetFreeCage();
                    if (cage == null)
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет ведра для рыбы", 3000);
                    else
                    {
                        //if (rod.CountUsing < 100 && FishingAPI.Random.Next(0, 100) < _rodChance) rod.CountUsing--;
                        var data = cage.Fishings;
                        if (cage.Fishings.ContainsKey(fisher.MiniGame.Fish))
                            cage.Fishings[fisher.MiniGame.Fish]++;
                        else
                            cage.Fishings.Add(fisher.MiniGame.Fish, 1);
                        inventory.UpdateItemData(cage.Index);
                        if (fisher.AddExp())
                            Notify.Send(client, NotifyType.Info, NotifyPosition.BottomCenter, $"Поздравляем! Вы только что вытащили: {FishingAPI.FishShops.GetFishName(fisher.MiniGame.Fish)} и ваш уровень повысился до {fisher.Lvl}", 3000);
                        else
                            Notify.Send(client, NotifyType.Info, NotifyPosition.BottomCenter, $"Поздравляем! Вы только что вытащили: {FishingAPI.FishShops.GetFishName(fisher.MiniGame.Fish)}", 3000);
                        client.CreatePlayerAction(PersonalEvents.PlayerActions.CathAFish, 1);
                    }
                }
                else
                {
                    //if (rod.CountUsing < 100) rod.CountUsing--;
                    Notify.Send(client, NotifyType.Info, NotifyPosition.BottomCenter, "Вы упустили рыбу", 3000);
                }

                if (rod.CountUsing < 1)
                {
                    inventory.SubItem(rod.Index);
                    Notify.Send(client, NotifyType.Info, NotifyPosition.BottomCenter, "Кажется ваша удочка сломалась и больше не пригодна для ловли рыбы", 3000);
                }
            }
            catch (Exception e) { _logger.WriteError("EndMiniGame: " + e.ToString()); }
        }

        internal void LoadConfig()
        {
            try
            {

                _spotList = new Dictionary<int, FishingSpot>();
                _markers = new Dictionary<int, GTANetworkAPI.Marker>();
                try
                {
                    var query = $"CREATE TABLE IF NOT EXISTS `fisher_data`(" +
                    $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                    $"`socialname` VARCHAR(45) NOT NULL," +
                    $"`lvl` int(11) NOT NULL," +
                    $"`exp` int(11) NOT NULL," +
                    $"`license` int(1) NOT NULL," +
                    $"`map_expires` DATETIME NOT NULL," +
                    $"PRIMARY KEY(`id`)" +
                    $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
                    MySQL.Query(query);

                    query = $"CREATE TABLE IF NOT EXISTS `fishing_spots`(" +
                    $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                    $"`position` TEXT NOT NULL," +
                    $"`rotation` TEXT NOT NULL," +
                    $"PRIMARY KEY(`id`)" +
                    $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";

                    MySQL.Query(query);
                    query = "SELECT * FROM `fishing_spots`";
                    var resp = MySQL.QueryRead(query);
                    if (resp != null)
                    {
                        foreach (DataRow row in resp.Rows)
                        {
                            var pos = JsonConvert.DeserializeObject<Vector3>((string)row["position"]);
                            var rot = JsonConvert.DeserializeObject<Vector3>((string)row["rotation"]);
                            var colsh = NAPI.ColShape.CreatCircleColShape(pos.X, pos.Y, 3);
                            _spotList.Add(colsh.Value, new FishingSpot((int)row["id"], pos, rot));

                            colsh.OnEntityEnterColShape += Colsh_OnEntityEnterColShape;
                            colsh.OnEntityExitColShape += Colsh_OnEntityExitColShape;
                        }
                        foreach (var spot in _spotBlips)
                        {
                            NAPI.Blip.CreateBlip(Const.BLIP_SPOT_SPRITE, spot, 1, Const.BLIP_SPOT_COLOR, "Место для рыбалки", 255, 0, true);
                        }
                    }
                    else NAPI.Util.ConsoleOutput($"no fihisng spots");
                }
                catch (Exception ex)
                {
                    NAPI.Util.ConsoleOutput($"error fihisng load: {ex.ToString()}");
                }
            }
            catch (Exception e) { _logger.WriteError("LoadConfig: " + e.ToString()); }
        }

        public void UpdateSeaSpots()
        {
            try
            {
                _seaSpots.OrderBy(x => FishingAPI.Random.Next()).Take(3).ToList().ForEach(i => CreateSeaSpot(i));
                _lastSeaSpotsUpdate = DateTime.Now;
            }
            catch (Exception e) { _logger.WriteError("UpdateSeaSpots: " + e.ToString()); }
        }

        private void DeleteSeaSpot(FishingSpot s)
        {
            try
            {
                var colsh = NAPI.Pools.GetAllColShapes().FirstOrDefault(b => b.Value == s.Id);
                s.InSea = false;
                if (colsh != null)
                {
                    _spotList.Remove(colsh.Value);
                    colsh.Delete();
                }
            }
            catch (Exception e) { _logger.WriteError("DeleteSeaSpot: " + e.ToString()); }
        }

        private void CreateSeaSpot(FishingSpot spot)
        {
            try
            {
                var colshape = NAPI.ColShape.CreatCircleColShape(spot.Position.X, spot.Position.Y, Const.SPOT_RADIUS_IN_SEA);
                var blip = NAPI.Blip.CreateBlip(Const.BLIP_SPOT_SPRITE, spot.Position, 2, Const.BLIP_SPOT_COLOR, "Место для рыбалки", 255, 0, true);
                spot.Id = colshape.Value;
                spot.InSea = true;
                _spotList.Add(colshape.Value, spot);
                colshape.OnEntityEnterColShape += Colsh_OnEntityEnterColShape;
                colshape.OnEntityExitColShape += Colsh_OnEntityExitColShape;
            }
            catch (Exception e) { _logger.WriteError("CreateSeaSpot: " + e.ToString()); }
        }

        #region SDK
        public void DeleteFishingSpot(ExtPlayer client)
        {
            try
            {
                var id = client.Session.FishingSpotId;
                if (id < 0) return;
                if (!_spotList.ContainsKey(id)) return;
                var spot = _spotList[id];
                if (_markers.ContainsKey(id))
                {
                    _markers[id].Delete();
                    _markers.Remove(id);
                }
                try
                {
                    var query = $"DELETE FROM `fishing_spots` WHERE `id`={spot.Id};";
                    MySQL.Query(query);
                    var colsh = NAPI.Pools.GetAllColShapes().FirstOrDefault(c => c.Value == id);
                    colsh.Delete();
                    _spotList.Remove(id);
                }
                catch (Exception ex)
                {
                    NAPI.Util.ConsoleOutput("add fish spot error: " + ex.ToString());
                }
            }
            catch (Exception e) { _logger.WriteError("DeleteFishingSpot: " + e.ToString()); }
        }

        public void AddFishingSpot(ExtPlayer client)
        {
            try
            {
                var colsh = NAPI.ColShape.CreatCircleColShape(client.Position.X, client.Position.Y, 2);
                colsh.OnEntityEnterColShape += Colsh_OnEntityEnterColShape;
                colsh.OnEntityExitColShape += Colsh_OnEntityExitColShape;
                var pos = client.Position;
                pos.Z--;
                try
                {
                    MySQL.Query("INSERT INTO `fishing_spots` (`position`, `rotation`) VALUES(@prop0, @prop1);", JsonConvert.SerializeObject(pos), JsonConvert.SerializeObject(client.Rotation));
                    int id = (int)MySQL.QueryRead("SELECT MAX(`id`) FROM `fishing_spots`;").Rows[0][0];
                    _spotList.Add(colsh.Value, new FishingSpot(id, client.Position, client.Rotation));

                    var marker = NAPI.Marker.CreateMarker(1, pos, new Vector3(), new Vector3(), 4, new Color(255, 255, 255));
                    _markers.Add(marker.Value, marker);
                }
                catch (Exception ex)
                {
                    NAPI.Util.ConsoleOutput("add fish spot error: " + ex.ToString());
                }
            }
            catch (Exception e) { _logger.WriteError("AddFishingSpot: " + e.ToString()); }
        }
        #endregion SDK

        public void Colsh_OnEntityExitColShape(GTANetworkAPI.ColShape colShape, Player client)
        {
            try
            {

                if (!(client is ExtPlayer player)) return;

                player.Session.FishingSpotId = -1;
                player.Session.FishingAction = FishingActions.NoAction;
                SafeTrigger.ClientEvent(player, Const.CLIENT_EVENT_SHOW_ACTION, false);
            }
            catch (Exception e) { _logger.WriteError("Colsh_OnEntityExitColShape: " + e.ToString()); }
        }

        public void Colsh_OnEntityEnterColShape(GTANetworkAPI.ColShape colShape, Player client)
        {
            try
            {

                if (!(client is ExtPlayer player)) return;

                player.Session.FishingSpotId = colShape.Value;
                player.Session.FishingAction = FishingActions.FishSpot;
                SafeTrigger.ClientEvent(player, Const.CLIENT_EVENT_SHOW_ACTION, true);
            }
            catch (Exception e) { _logger.WriteError("Colsh_OnEntityEnterColShape: " + e.ToString()); }
        }

    }
}
