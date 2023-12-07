using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.GUI;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.Houses;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.Businesses.Models;
using Whistler.Common;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        //private static Vector3 RoomPlayerPosition = new Vector3(-1499.8, -3018.9, -79.2);
        private static Vector3 RoomPlayerPosition = new Vector3(-3414.16, -587.5141, 454.5) + new Vector3(5060, 110, -345);

        public static void OpenCarromMenu(Player player, int bizID)
        {
            if (NAPI.Player.IsPlayerInAnyVehicle(player))
                return;
            List<dynamic> list = new List<dynamic>();
            Business biz = BusinessManager.BizList[bizID];
            foreach (var p in biz.Products)
            {
                var config = VehicleConfiguration.GetConfig(p.Name);
                list.Add(new
                {
                    price = biz.GetPriceByProductName(p.Name).CurrentPrice,
                    model = p.Name,
                    name = config.DisplayName ?? p.Name,
                    selled = p.Lefts == 0,
                    fuel = config.MaxFuel,
                    trunc = config.MaxWeight/1000,
                    fuelConsumption = config.FuelConsumption
                });
            }
            if (list.Count == 0)
            {
                return;
            }
            player.GetCharacter().BusinessInsideId = bizID;
            player.GetCharacter().ExteriorPos = biz.EnterPoint.DistanceTo(player.Position) > 50 ? biz.EnterPoint : player.Position;
            player.ChangePosition(RoomPlayerPosition);
            player.Dimension = Dimensions.RequestPrivateDimension();
            player.TriggerEvent("carshop:open", JsonConvert.SerializeObject(list), biz.Type);
        }

        [RemoteEvent("testDriveAuto")]
        public static void RemoteEvent_testDriveAuto(Player player, string vName, int r, int g, int b)
        {
            try
            {
                if (player.GetCharacter().BusinessInsideId < 1) return;
                Business biz = BizList[player.GetCharacter().BusinessInsideId];
                Vector3 testDrivePos = biz.UnloadPoint;

                var playerDimension = player.Dimension;

                var vehicle = VehicleManager.CreateTemporaryVehicle((VehicleHash)NAPI.Util.GetHashKey(vName), testDrivePos, new Vector3(), "TESTDRIVE", VehicleAccess.TestDrive, player, playerDimension);
                player.AddTempVehicle(vehicle, VehicleAccess.TestDrive);
                vehicle.CustomPrimaryColor = new Color(r, g, b);
                vehicle.CustomSecondaryColor = new Color(r, g, b);

                player.CustomSetIntoVehicle(vehicle, VehicleConstants.DriverSeatClientSideBroken);
                //WhistlerTask.Run(() =>
                //{
                //}, 3000);

                VehicleStreaming.SetEngineState(vehicle, true);
                VehicleStreaming.SetLockStatus(vehicle, true);
                VehicleStreaming.SetVehicleFuel(vehicle, 1000);


                var colshape = NAPI.ColShape.CreateCylinderColShape(testDrivePos - new Vector3(0, 0, 2), 3, 3, playerDimension);
                var marker = NAPI.Marker.CreateMarker(MarkerType.VerticalCylinder, testDrivePos - new Vector3(0, 0, 3), new Vector3(), new Vector3(), 3, new Color(255, 255, 255, 180), false, playerDimension);
                var textlabel = NAPI.TextLabel.CreateTextLabel("End testdrive", testDrivePos, 20F, 1F, 2, new Color(255, 255, 255), true, playerDimension);
                player.CreateClientBlip(1337, 1, "END TEST DRIVE", testDrivePos, 2, 59, playerDimension);

                colshape.OnEntityEnterColShape += (shape, client) =>
                {
                    try
                    {
                        if (client.HasData("TESTDRIVE_WAS_EXIT_FROM_SHAPE"))
                            EndTestDriveAuto(client);
                    }
                    catch { }
                };
                colshape.OnEntityExitColShape += (shape, client) =>
                {
                    try
                    {
                        client.SetData("TESTDRIVE_WAS_EXIT_FROM_SHAPE", true);
                    }
                    catch { }
                };

                player.SetData("TESTDRIVE_COLSHAPE", colshape);
                player.SetData("TESTDRIVE_MARKER", marker);
                player.SetData("TESTDRIVE_TEXTLABEL", textlabel);
            }
            catch (Exception e) { _logger.WriteError("testDriveAuto: " + e.ToString()); }
        }

        [RemoteEvent("endTestDriveAuto")]
        public static void EndTestDriveAuto(Player player)
        {
            try
            {
                if (!player.TempVehicleIsExist(VehicleAccess.TestDrive)) return;

                DeleteTestDriveEntities(player);

                player.ResetData("TESTDRIVE_WAS_EXIT_FROM_SHAPE");

                player.ChangePosition(RoomPlayerPosition);
                player.TriggerEvent("endTestDrive");
            }
            catch (Exception e) { _logger.WriteError("endTestDriveAuto: " + e.ToString()); }

        }

        public static void Event_PlayerDeath(Player player)
        {
            try
            {
                if (!player.TempVehicleIsExist(VehicleAccess.TestDrive)) return;

                DeleteTestDriveEntities(player);

                player.ResetData("TESTDRIVE_WAS_EXIT_FROM_SHAPE");
                player.TriggerEvent("endTestDrive", true);
            }
            catch (Exception e) { _logger.WriteError("ServerEvent_PlayerDisconnected: " + e.ToString()); }
        }

        public static void TestDrive_PlayerDisconnected(Player player)
        {
            try
            {
                DeleteTestDriveEntities(player);
            }
            catch (Exception e) { _logger.WriteError("ServerEvent_PlayerDisconnected: " + e.ToString()); }
        }

        private static void DeleteTestDriveEntities(Player player)
        {
            if (!player.TempVehicleIsExist(VehicleAccess.TestDrive)) return;

            player.RemoveTempVehicle(VehicleAccess.TestDrive)?.CustomDelete();

            ColShape shape = player.GetData<ColShape>("TESTDRIVE_COLSHAPE");
            shape.Delete();

            Marker marker = player.GetData<Marker>("TESTDRIVE_MARKER");
            marker.Delete();

            TextLabel label = player.GetData<TextLabel>("TESTDRIVE_TEXTLABEL");
            label.Delete();

            player.DeleteClientBlip(1337);
        }

        [RemoteEvent("carshop:buyvehicle")]
        public static void RemoteEvent_carroomBuy(Player player, string vName, int r, int g, int b, bool cashPay, bool forFamily)
        {
            try
            {
                var character = player.GetCharacter();
                if (character == null)
                    return;
                Business biz = GetBusiness(character.BusinessInsideId);
                if (biz == null) return;
                character.BusinessInsideId = -1;
                player.ChangePosition(character.ExteriorPos);
                character.ExteriorPos = null;
                player.Dimension = 0;

                if (!CheckBuyPersonalVehicle(player, forFamily))
                    return;

                var prod = biz.Products.FirstOrDefault(p => p.Name == vName);
                if (prod == null)
                    return;

                BusinessManager.TakeProd(
                    player,
                    biz,
                    player.GetMoneyPayment(cashPay ? PaymentsType.Cash : PaymentsType.Card),
                    new BuyModel(
                        vName,
                        1,
                        true,
                        (cnt) =>
                        {
                            Color color = new Color(r, g, b);
                            int owner = forFamily ? character.FamilyID : character.UUID;
                            OwnerType typeOwner = forFamily ? OwnerType.Family : OwnerType.Personal;
                            var price = biz.GetPriceByProductName(vName)?.CurrentPrice ?? 0;
                            var vehData = VehicleManager.Create(owner, vName, color, color, price: price, typeOwner: typeOwner);
                            GarageManager.SendVehicleIntoGarage(vehData);

                            MainMenu.SendProperty(player);

                            Notify.Alert(player, "Biz_93".Translate(vName, vehData.Number));
                            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Biz_96", 5000);
                            return cnt;
                        }),
                    "Money_BuyCar".Translate(vName),
                    null);
            }
            catch (Exception e) { _logger.WriteError("CarroomBuy: " + e.ToString()); }
        }

        private static bool CheckBuyPersonalVehicle(Player player, bool forFamily)
        {
            if (forFamily)
            {
                if (player.GetCharacter().FamilyID > 0)
                {
                    if (HouseManager.GetHouse(player.GetCharacter().FamilyID, OwnerType.Family) == null)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "carshop_5", 3000);
                        return false;
                    }
                    return true; 
                }
                else
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "carshop_1", 3000);
                    return false;
                }
            }
            else
            {
                if (VehicleManager.getAllHolderVehicles(player.GetCharacter().UUID, OwnerType.Personal).Count == 0)
                    return true;
                var house = HouseManager.GetHouse(player, true);
                if (house == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_2", 3000);
                    return false;
                }
                if (house.GarageID == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_3", 3000);
                    return false;
                }
                if (VehicleManager.getAllHolderVehicles(player.GetCharacter().UUID, OwnerType.Personal).Count >= house.HouseGarage.GarageConfig.MaxCars)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_4", 3000);
                    return false;
                }
                return true;
            }
        }

        [RemoteEvent("carshop:exitMenu")]
        public static void RemoteEvent_carroomCancel(Player player)
        {
            try
            {
                player.ChangePosition(player.GetCharacter().ExteriorPos);
                player.GetCharacter().ExteriorPos = null;
                player.Dimension = 0;
                player.GetCharacter().BusinessInsideId = -1;
            }
            catch (Exception e) { _logger.WriteError("carroomCancel: " + e.ToString()); }
        }
    }
}







