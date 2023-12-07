using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.Core.Character;
using Whistler.Families;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models;
using Whistler.Possessions;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.VehicleTrading;
using Whistler.Houses.Models;
using Whistler.Entities;

namespace Whistler.Houses
{
    class GarageManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(GarageManager)); 
        public static Dictionary<int, Garage> Garages = new Dictionary<int, Garage>();
        
        public static uint DimensionID = 20000;

        public static void OnLoadGarages()
        {
            try
            {
                var result = MySQL.QueryRead("SELECT * FROM `garages`");
                if (result == null || result.Rows.Count == 0)
                {
                    _logger.WriteWarning("DB return null result.");
                    return;
                }

                House houseData;
                foreach (DataRow Row in result.Rows)
                {

                    var id = Convert.ToInt32(Row["id"]);

                    houseData = HouseManager.GetHouseByGarageId(id);
                    if (houseData == null)
                    {
                        _logger.WriteInfo($"NOT FOUND HOUSE WITH GARAGE ID = {id}");
                        MySQL.Query("DELETE FROM `garages` WHERE `id`=@prop0", id);
                        continue;
                    }
                    Vector3 position = JsonConvert.DeserializeObject<Vector3>(Row["position"].ToString());

                    if (houseData.Position.DistanceTo2D(position) > 50)
                    {
                        _logger.WriteInfo($"Garage {id} are far away from its house {houseData.ID}!");
                    }

                    var type = Convert.ToInt32(Row["type"]);
                    var rotation = JsonConvert.DeserializeObject<Vector3>(Row["rotation"].ToString());
                    var nativeType = Convert.ToInt32(Row["nativeType"]);

                    var garage = new Garage(id, type, position, rotation, nativeType);
                    garage.Dimension = (uint)(DimensionID + garage.ID);

                    Garages.Add(id, garage);
                }

                foreach (var garageType in Configs.HouseConfigs.GarageCoordModels)
                {
                    InteractShape.Create(garageType.Value.Position, 1, 2, NAPI.GlobalDimension)
                        .AddDefaultMarker()
                        .AddInteraction(ExitGarage, "interact_22");

                    InteractShape.Create(garageType.Value.OutPosition, 3, 2, NAPI.GlobalDimension)
                        .AddMarker(27, garageType.Value.OutPosition, 3, new Color(182, 211, 0, 200))
                        .AddInteraction(ExitGarageIntoVehicle, "interact_36");

                    InteractShape.Create(garageType.Value.RobberyPos, 1, 2, NAPI.GlobalDimension)
                        .AddEnterPredicate((c,p) => Jobs.HomeRobbery.HomeRobberyManager.CheckGiveContainer(p))
                        .AddInteraction(Jobs.HomeRobbery.HomeRobberyManager.GiveContainer, "interact_57");
                }

                foreach (House house in HouseManager.Houses)
                {
                    if (house == null) continue;
                    if (Garages.ContainsKey(house.GarageID)) continue;

                    _logger.WriteInfo($"{house.ID} have no real garage attached");
                }

                _logger.WriteInfo($"Loaded {Garages.Count} garages.");
            }
            catch (Exception e) { _logger.WriteError($"ResourceStart: " + e.ToString()); }
        }

        private static void GoToCoord(ExtPlayer player, Vector3 pos, Vector3 rot)
        {
            if (!player.IsInVehicle)
            {
                player.ChangePosition(pos, 5);
                return;
            }
            player.ChangePositionWithCar(pos + new Vector3(0, 0, 0.2), rot, 1000);
        }


        public static void ExitGarage(ExtPlayer player)
        {
            if (player.Character.InsideGarageID == -1) return;
            var garage = Garages[player.Character.InsideGarageID];
            garage.RemovePlayer(player);
        }

        public static void ExitGarageIntoVehicle(ExtPlayer player)
        {
            if (player.Character.InsideGarageID == -1) return;
            var garage = Garages[player.Character.InsideGarageID];
            garage.ExitGarageIntoVehicle(player);
        }

        public static void SendVehicleIntoGarage(int idkey)
        {
            if (!VehicleManager.Vehicles.ContainsKey(idkey))  return;
            SendVehicleIntoGarage(VehicleManager.Vehicles[idkey]);
        }

        public static void SendVehicleIntoGarage(VehicleBase vehData)
        {
            vehData.DestroyVehicle();
            NAPI.Task.Run(() => 
            {
                if (!(vehData is PersonalBaseVehicle personalVehData)) return;
                if (personalVehData.TradePoint > 0)
                {
                    TradeManager.SpawnTradeCar(personalVehData);
                    return;
                }
                HouseManager.GetHouse(vehData.OwnerID, vehData.OwnerType, true)?.HouseGarage?.SendVehicleIntoGarage(vehData.ID);
            }, 200);

        }

        #region Commands
        [Command("setgarage")]
        public static void CMD_SetGarage(ExtPlayer player, int ID)
        {
            if (!Group.CanUseAdminCommand(player, "ban")) return;
            if (!player.HasData("HOUSEID"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"House_4", 3000);
                return;
            }

            House house = HouseManager.Houses.FirstOrDefault(h => h.ID == player.GetData<int>("HOUSEID"));
            if (house == null) return;

            if (!Garages.ContainsKey(ID)) return;
            house.SetGarageId(ID);
        }
        [Command("creategarage")]
        public static void CMD_CreateGarage(ExtPlayer player, int houseId, int type)
        {
            if (!Group.CanUseAdminCommand(player, "creategarage")) return;
            if (!Whistler.Houses.Configs.HouseConfigs.GarageTypes.ContainsKey(type)) return;
            int id = 1;
            while (Garages.ContainsKey(id))
            {
                id++;
            }

            var house = HouseManager.Houses.FirstOrDefault(x => x.ID == houseId);
            Garage garage = new Garage(id, type, player.Position, player.Rotation, type);
            garage.Dimension = (uint)(DimensionID + garage.ID);
            Garages.Add(garage.ID, garage);
            garage.Create();
            house.SetGarageId(id);

            Chat.SendTo(player, garage.ID.ToString());
        }

        [Command("removegarage")]
        public static void CMD_RemoveGarage(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "removegarage")) return;
            if (!player.HasData("GARAGEID"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"House_4", 3000);
                return;
            }
            if (!GarageManager.Garages.ContainsKey(player.GetData<int>("GARAGEID"))) return;
            Garage garage = GarageManager.Garages[player.GetData<int>("GARAGEID")];

            garage.Destroy();
            Garages.Remove(player.GetData<int>("GARAGEID"));
            MySQL.Query("DELETE FROM `garages` WHERE `id`= @prop0", garage.ID);
        }


        #endregion
    }
}
