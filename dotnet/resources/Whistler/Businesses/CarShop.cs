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
using Whistler.Entities;
using Whistler.VehicleSystem.Models;
using Whistler.Inventory.Configs;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        private static Vector3 RoomPlayerPosition = new Vector3(-3414.16, -587.5141, 454.5) + new Vector3(5060, 110, -345);

        [ServerEvent(Event.PlayerEnterVehicleAttempt)]
        public void OnPlayerEnterVehicleAttemptHandler(ExtPlayer player, ExtVehicle vehicle, sbyte seatid)
        {
            if (vehicle == null) return;
            if (!player.IsPlayerHaveContainer()) return;

            player.StopAnimation();
            player.SendAlert("Вы не можете сесть в транспорт с ящиком в руках.", 5000);
        }

        public static void OpenCarromMenu(ExtPlayer player, int bizID)
        {
            if (NAPI.Player.IsPlayerInAnyVehicle(player))
                return;
            List<dynamic> list = new List<dynamic>();
            Business biz = BusinessManager.BizList[bizID];
            foreach (var p in biz.Products)
            {
                VehicleSystem.Models.VehConfig config = VehicleConfiguration.GetConfig(p.Name);
                list.Add(new
                {
                    price = biz.GetPriceByProduct(p).CurrentPrice,
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
            player.Character.BusinessInsideId = bizID;
            player.Character.ExteriorPos = biz.EnterPoint.DistanceTo(player.Position) > 50 ? biz.EnterPoint : player.Position;
            player.ChangePosition(RoomPlayerPosition);
            SafeTrigger.UpdateDimension(player,  Dimensions.RequestPrivateDimension());
            SafeTrigger.ClientEvent(player,"carshop:open", JsonConvert.SerializeObject(list), biz.TypeModel.TypeName);
        }

        [RemoteEvent("testDriveAuto")]
        public static void RemoteEvent_testDriveAuto(ExtPlayer player, string vName, int r, int g, int b)
        {
            try
            {
                if (player.Character.BusinessInsideId < 1) return;
                Business biz = BizList[player.Character.BusinessInsideId];
                Vector3 testDrivePos = biz.UnloadPoint;

                var playerDimension = player.Dimension;

                var vehicle = VehicleManager.CreateTemporaryVehicle((VehicleHash)NAPI.Util.GetHashKey(vName), testDrivePos, new Vector3(), "TESTDRIVE", VehicleAccess.TestDrive, player, playerDimension);
                player.AddTempVehicle(vehicle, VehicleAccess.TestDrive);
                vehicle.CustomPrimaryColor = new Color(r, g, b);
                vehicle.CustomSecondaryColor = new Color(r, g, b);
                player.ChangePosition(testDrivePos);

                NAPI.Task.Run(() =>
                {
                    player.CustomSetIntoVehicle(vehicle, VehicleConstants.DriverSeatClientSideBroken);
                }, 500);

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
                        if (!(client is ExtPlayer extPlayer)) return;
                        if (extPlayer.HasData("TESTDRIVE_WAS_EXIT_FROM_SHAPE"))
                            EndTestDriveAuto(extPlayer);
                    }
                    catch { }
                };
                colshape.OnEntityExitColShape += (shape, client) =>
                {
                    try
                    {
                        if (!(client is ExtPlayer extPlayer)) return;

                        SafeTrigger.SetData(extPlayer, "TESTDRIVE_WAS_EXIT_FROM_SHAPE", true);
                    }
                    catch { }
                };

                SafeTrigger.SetData(player, "TESTDRIVE_COLSHAPE", colshape);
                SafeTrigger.SetData(player, "TESTDRIVE_MARKER", marker);
                SafeTrigger.SetData(player, "TESTDRIVE_TEXTLABEL", textlabel);
            }
            catch (Exception e) { _logger.WriteError("testDriveAuto: " + e.ToString()); }
        }

        [RemoteEvent("endTestDriveAuto")]
        public static void EndTestDriveAuto(ExtPlayer player)
        {
            try
            {
                if (!player.TempVehicleIsExist(VehicleAccess.TestDrive)) return;

                DeleteTestDriveEntities(player);

                player.ResetData("TESTDRIVE_WAS_EXIT_FROM_SHAPE");

                player.ChangePosition(RoomPlayerPosition);
                SafeTrigger.ClientEvent(player,"endTestDrive");
            }
            catch (Exception e) { _logger.WriteError("endTestDriveAuto: " + e.ToString()); }

        }

        public static void Event_PlayerDeath(ExtPlayer player)
        {
            try
            {
                if (!player.TempVehicleIsExist(VehicleAccess.TestDrive)) return;

                DeleteTestDriveEntities(player);

                player.ResetData("TESTDRIVE_WAS_EXIT_FROM_SHAPE");
                SafeTrigger.ClientEvent(player,"endTestDrive", true);
            }
            catch (Exception e) { _logger.WriteError("ServerEvent_PlayerDisconnected: " + e.ToString()); }
        }

        public static void TestDrive_PlayerDisconnected(ExtPlayer player)
        {
            try
            {
                DeleteTestDriveEntities(player);
            }
            catch (Exception e) { _logger.WriteError("ServerEvent_PlayerDisconnected: " + e.ToString()); }
        }

        private static void DeleteTestDriveEntities(ExtPlayer player)
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
        public static void RemoteEvent_carroomBuy(ExtPlayer player, string vName, int r, int g, int b, bool cashPay, bool forFamily)
        {
            try
            {
                var character = player.Character;
                if (character == null)
                    return;
                Business biz = GetBusiness(character.BusinessInsideId);
                if (biz == null) return;
                character.BusinessInsideId = -1;
                player.ChangePosition(character.ExteriorPos);
                character.ExteriorPos = null;
                SafeTrigger.UpdateDimension(player,  0);

                if (!CheckBuyPersonalVehicle(player, forFamily)) return;

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
                            VehConfig config = VehicleConfiguration.GetConfig(vName);
                            var vehData = VehicleManager.Create(owner, vName, color, color, config.MaxFuel, price, typeOwner: typeOwner);
                            GarageManager.SendVehicleIntoGarage(vehData);

                            MainMenu.SendProperty(player);

                            Notify.Alert(player, $"Вы купили {vName}, номер {vehData.Number}");
                            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Очень скоро его доставят в ваш гараж", 5000);
                            return cnt;
                        }),
                    $"Покупка авто {vName}",
                    null);
            }
            catch (Exception e) { _logger.WriteError("CarroomBuy: " + e.ToString()); }
        }

        private static bool CheckBuyPersonalVehicle(ExtPlayer player, bool forFamily)
        {
            if (forFamily)
            {
                if (player.Character.FamilyID > 0)
                {
                    if (HouseManager.GetHouse(player.Character.FamilyID, OwnerType.Family) == null)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вашей семьи нет дома", 3000);
                        return false;
                    }
                    return true; 
                }
                else
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не состоите в семье", 3000);
                    return false;
                }
            }
            else
            {
                if (VehicleManager.getAllHolderVehicles(player.Character.UUID, OwnerType.Personal).Count == 0)
                    return true;
                var house = HouseManager.GetHouse(player, true);
                if (house == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У Вас нет личного дома", 3000);
                    return false;
                }
                if (house.GarageID == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас нет гаража", 3000);
                    return false;
                }
                if (VehicleManager.getAllHolderVehicles(player.Character.UUID, OwnerType.Personal).Count >= house.HouseGarage.GarageConfig.MaxCars)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас уже есть максимальное количество машин", 3000);
                    return false;
                }
                return true;
            }
        }

        [RemoteEvent("carshop:exitMenu")]
        public static void RemoteEvent_carroomCancel(ExtPlayer player)
        {
            try
            {
                player.ChangePosition(player.Character.ExteriorPos);
                player.Character.ExteriorPos = null;
                SafeTrigger.UpdateDimension(player,  0);
                player.Character.BusinessInsideId = -1;
            }
            catch (Exception e) { _logger.WriteError("carroomCancel: " + e.ToString()); }
        }
    }
}







