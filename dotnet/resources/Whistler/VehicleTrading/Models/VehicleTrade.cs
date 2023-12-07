using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.MoneySystem;
using Whistler.MoneySystem.Interface;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.VehicleTrading.Models
{
    class VehicleTrade
    {
        public int Id { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public int CurrVehicle { get; private set; }
        public int Price { get; private set; }

        public InteractShape Shape { get; private set; }

        private static int _comissionPercent = 5; 
        public VehicleTrade(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            Position = JsonConvert.DeserializeObject<Vector3>(row["position"].ToString());
            Rotation = JsonConvert.DeserializeObject<Vector3>(row["rotation"].ToString());
            CurrVehicle = Convert.ToInt32(row["currentveh"]);
            Price = Convert.ToInt32(row["price"]);

            if (VehicleManager.GetVehicleBaseByUUID(CurrVehicle) == null)
            {
                CurrVehicle = -1;
                Price = 0;
            }

            CreateInteract();
        }

        public VehicleTrade(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
            CurrVehicle = -1;
            Price = 0;
            var dataQuery = MySQL.QueryRead("INSERT INTO `vehicletrading`(`position`, `rotation`, `currentveh`, `price`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3); SELECT @@identity;",
                JsonConvert.SerializeObject(Position), JsonConvert.SerializeObject(Rotation), CurrVehicle, Price);
            var id = Convert.ToInt32(dataQuery.Rows[0][0]);
            Id = id;
            CreateInteract();
        }
        private void CreateInteract()
        {
            if (Shape != null)
                Shape.Destroy();
            Shape = InteractShape.Create(Position, 1.5F, 2, 0)
                .AddMarker(36, Position + new Vector3(0, 0, 0.3), 1.5F, InteractShape.DefaultMarkerColor);
            UpdateInteract();
        }

        private void UpdateInteract()
        {
            if (Shape == null)
                CreateInteract();
            if (CurrVehicle <= 0)
                Shape.AddInteraction(OpenSellMenu, "veh:trader:1".Translate(Id))
                    .DeleteInteraction(Key.VK_I)
                    .AddMarker(36, Position + new Vector3(0, 0, 0.3), 1.5F, InteractShape.DefaultMarkerColor);
            else
                Shape.AddInteraction(OpenBuyMenu, "veh:trader:2", Key.VK_I)
                    .DeleteInteraction(Key.VK_E)
                    .DeleteMarker();
        }

        private void OpenSellMenu(ExtPlayer player)
        {
            if (CurrVehicle > 0)
            {
                Notify.SendError(player, "veh:trader:menu:1");
                return;
            }
            if (!player.IsInVehicle)
            {
                Notify.SendError(player, "veh:trader:menu:2");
                return;
            }
            ExtVehicle extVehicle = player.Vehicle as ExtVehicle;
            if (extVehicle.Data.OwnerType != OwnerType.Personal || !extVehicle.Data.CanAccessVehicle(player, AccessType.SellDollars))
            {
                Notify.SendError(player, "veh:trader:menu:3");
                return;
            }
            SafeTrigger.ClientEvent(player,"vehTrade::openSellMenu", Id, extVehicle.Config.DisplayName);
        }

        public void OpenBuyMenu(ExtPlayer player)
        {
            if (CurrVehicle <= 0)
            {
                Notify.SendError(player, "veh:trader:menu:4");
                return;
            }
            var vehData = VehicleManager.GetVehicleBaseByUUID(CurrVehicle);
            if (vehData == null || !(vehData is PersonalBaseVehicle))
                return;
            var vehDataPersonal = vehData as PersonalBaseVehicle;
            var state = new CarPassDTO(vehDataPersonal);
            player.TriggerCefEvent("sellCar/setCarState", JsonConvert.SerializeObject(state));
            SafeTrigger.ClientEvent(player,"vehTrade::openBuyMenu", Id, Price, vehDataPersonal.Number);
        }

        public bool BuyVehicle(ExtPlayer player)
        {
            var vehData = VehicleManager.GetVehicleBaseByUUID(CurrVehicle);
            if (vehData == null || !(vehData is PersonalBaseVehicle))
                return false;
            var sellerUUID = vehData.OwnerID;
            if (sellerUUID == player.Character.UUID)
                return false;
            if ((vehData as PersonalBaseVehicle).Pledged)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "vehTrade:isPledged", 3000);
                return false;
            }
            int comission = Price * _comissionPercent / 100;
            if (!Wallet.TransferMoney(player.Character.BankModel, new List<(IMoneyOwner, int)>
                {
                    (BankManager.GetAccountByUUID(sellerUUID), Price - comission),
                    (Manager.GetFraction(6), comission),
                }, "Money_BuyCarTrade".Translate(CurrVehicle)))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_1", 3000);
                return false;
            }
            vehData.OwnerID = player.Character.UUID;
            vehData.Save();
            GUI.MainMenu.SendProperty(player);
            var seller = Trigger.GetPlayerByUuid(sellerUUID);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Main_178".Translate(vehData.ModelName, vehData.Number, Price, Main.PlayerNames.GetValueOrDefault(sellerUUID, "Unknown")), 3000);
            if (seller.IsLogged())
            {
                GUI.MainMenu.SendProperty(seller);
                Notify.Send(seller, NotifyType.Success, NotifyPosition.BottomCenter, "Main_179".Translate(player.Name, vehData.ModelName, vehData.Number, Price), 3000);
            }
            UnSetVehicle();
            return true;
        }

        public bool SetVehicle(PersonalBaseVehicle vehicle, int price)
        {
            if (CurrVehicle > 0)
                return false;
            if (vehicle.TradePoint > 0)
                return false;
            if (vehicle.Pledged)
                return false;
            vehicle.SetTradePoint(Id);
            CurrVehicle = vehicle.ID;
            Price = price;
            MySQL.Query("UPDATE vehicletrading SET `currentveh` = @prop0, `price` = @prop1 WHERE `id` = @prop2", CurrVehicle, Price, Id);
            GarageManager.SendVehicleIntoGarage(vehicle.ID); 
            UpdateInteract();
            return true;
        }

        public bool UnSetVehicle()
        {
            var vehicle = VehicleManager.GetVehicleBaseByUUID(CurrVehicle);
            if (vehicle == null || !(vehicle is PersonalBaseVehicle))
                return false;
            if ((vehicle as PersonalBaseVehicle).TradePoint != Id)
                return false;
            (vehicle as PersonalBaseVehicle).SetTradePoint(-1);
            CurrVehicle = -1;
            Price = 0;
            MySQL.Query("UPDATE vehicletrading SET `currentveh` = @prop0, `price` = @prop1 WHERE `id` = @prop2", CurrVehicle, Price, Id);
            GarageManager.SendVehicleIntoGarage(vehicle.ID);
            UpdateInteract();
            return true;
        }

        public void SpawnVehicle(PersonalBaseVehicle vehicle)
        {
            vehicle.Spawn(Position + new Vector3(0, 0, 1), Rotation, 0);
        }

        public void Destroy()
        {
            Shape.Destroy();
            MySQL.Query("DELETE FROM `vehicletrading` WHERE id = @prop0", Id);
        }
    }
}
