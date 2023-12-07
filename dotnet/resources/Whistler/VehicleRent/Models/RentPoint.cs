using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.GarbageCollector;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleRent.Configs;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.VehicleRent.Models
{
    class RentPoint
    {
        public int ID { get; set; }
        public Vector3 PositionPed { get; set; }
        public double RotationPed { get; set; }
        public Vector3 PositionCar { get; set; }
        public double RotationCar { get; set; }
        public List<int> Categories { get; set; }

        private InteractShape Shape;
        private Blip Blip;
        private static Random _rnd = new Random();
        public RentPoint(DataRow row)
        {
            ID = Convert.ToInt32(row["id"].ToString());
            PositionPed = JsonConvert.DeserializeObject<Vector3>(row["positionped"].ToString());
            RotationPed = Convert.ToDouble(row["rotationped"].ToString());
            PositionCar = JsonConvert.DeserializeObject<Vector3>(row["positioncar"].ToString());
            RotationCar = Convert.ToDouble(row["rotationcar"].ToString());
            Categories = JsonConvert.DeserializeObject<List<int>>(row["categories"].ToString());
            CreateInteract();
        }
        public RentPoint(Vector3 positionPed, double rotationPed, Vector3 positionCar, double rotationCar)
        {
            PositionPed = positionPed;
            RotationPed = rotationPed;
            PositionCar = positionCar;
            RotationCar = rotationCar;
            Categories = new List<int>();
            var responce = MySQL.QueryRead("INSERT INTO `rentcarpoint`(`positionped`, `rotationped`, `positioncar`, `rotationcar`, `categories`) VALUES(@prop0, @prop1, @prop2, @prop3, @prop4); SELECT @@identity;", JsonConvert.SerializeObject(PositionPed), RotationPed, JsonConvert.SerializeObject(PositionCar), RotationCar, JsonConvert.SerializeObject(Categories));
            ID = Convert.ToInt32(responce.Rows[0][0]);

            UpdatePed();
            CreateInteract();
        }
        private void CreateInteract()
        {
            Shape?.Destroy();
            Blip?.Delete();
            Blip = NAPI.Blip.CreateBlip(810, PositionPed, 1, 5, "Аренда транспорта", 255, 0, true, 0, 0);
            Shape = InteractShape.Create(PositionPed, 2, 2)
                .AddInteraction(ActionShape, "Чтобы арендовать транспорт");
        }

        private void ActionShape(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            SafeTrigger.ClientEvent(player,"vehicleRent:openMenu", ID, JsonConvert.SerializeObject(Categories));
        }

        public void TryRentVehicle(ExtPlayer player, string model, int category, int payment)
        {
            if (!Enum.IsDefined(typeof(RentSection), category))
                return;
            var conf = RentVehicleConfig.SectConfigs.GetValueOrDefault((RentSection)category);
            if (conf == null) return;
            var rentModel = conf.Cars.FirstOrDefault(item => item.Model == model);
            if (rentModel == null) return;
            var currVeh = player.GetTempVehicle(VehicleAccess.Rent);
            if (currVeh != null)
            {
                DialogUI.Open(player, "vehicleRent_1", new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "House_58",// Да
                        Icon = "confirm",
                        Action = p =>
                        {
                            player.RemoveTempVehicle(VehicleAccess.Rent)?.CustomDelete();
                            CreateRentVehicle(p, conf, rentModel, payment);
                        }
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "House_59",// Нет
                        Icon = "cancel"
                    }
                });
            }
            else
                CreateRentVehicle(player, conf, rentModel, payment);
        }

        public void CreateRentVehicle(ExtPlayer player, CategoryModel categoryModel, RentModel model, int payment)
        {
            if (!MoneySystem.Wallet.MoneySub(player.GetMoneyPayment((MoneySystem.PaymentsType)payment), model.Price, $"Аренда машины ({model.Model})"))
            {
                Notify.SendError(player, "vehicleRent_4");
                return;
            }
            var veh = VehicleManager.CreateTemporaryVehicle(model.Model, PositionCar, new Vector3(0, 0, RotationCar), VehicleManager.GenerateNumber(false), VehicleAccess.Rent, player);
            if (veh == null || !veh.Exists) return;

            player.AddTempVehicle(veh, VehicleAccess.Rent);
            //player.CustomSetIntoVehicle(veh, VehicleConstants.DriverSeat);
            SafeTrigger.SetSharedData(veh, "HOLDERNAME", veh.Data.GetHolderName());
            veh.Dimension = 0;
            var color = categoryModel.Colors.GetRandomElement();
            VehicleCustomization.SetColor(veh, color, 1, true);
            VehicleCustomization.SetColor(veh, color, 1, false);
            VehicleStreaming.SetEngineState(veh, false);
            GarbageManager.Add(veh, RentVehicleConfig.RespawnTime);
        }

        public void Delete()
        {
            MySQL.Query("DELETE FROM `rentcarpoint` WHERE `id` = @prop0", ID);
            Destroy();
        }

        public void Destroy()
        {
            Shape?.Destroy();
            Blip?.Delete();
        
            NAPI.ClientEvent.TriggerClientEventForAll("vehicleRent:deletePed", ID);
        }
        public void ChangePositionPed(ExtPlayer player)
        {
            PositionPed = player.Position;
            RotationPed = player.Rotation.Z;
            CreateInteract();
            MySQL.Query("UPDATE `rentcarpoint` SET `positionped` = @prop0, `rotationped` = @prop1 WHERE `id` = @prop2", JsonConvert.SerializeObject(PositionPed), RotationPed, ID);
            UpdatePed();
        }
        public void ChangePositionCar(ExtVehicle vehicle)
        {
            PositionCar = vehicle.Position;
            RotationCar = vehicle.Rotation.Z;
            MySQL.Query("UPDATE `rentcarpoint` SET `positioncar` = @prop0, `rotationcar` = @prop1 WHERE `id` = @prop2", JsonConvert.SerializeObject(PositionCar), RotationCar, ID);
        }

        public void AddCategory(int category)
        {
            if (Categories.Contains(category))
                return;
            Categories.Add(category);
            MySQL.Query("UPDATE `rentcarpoint` SET `categories` = @prop0 WHERE `id` = @prop1", JsonConvert.SerializeObject(Categories), ID);
        }
        public void DeleteCategory(int category)
        {
            if (!Categories.Contains(category))
                return;
            Categories.Remove(category);
            MySQL.Query("UPDATE `rentcarpoint` SET `categories` = @prop0 WHERE `id` = @prop1", JsonConvert.SerializeObject(Categories), ID);
        }

        private void UpdatePed()
        {
            NAPI.ClientEvent.TriggerClientEventForAll("vehicleRent:updatePeds", JsonConvert.SerializeObject(new { id = ID, position = PositionPed, heading = RotationPed }));
        }
    }
}
