using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.GarbageCollector;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleRent.Configs;
using Whistler.VehicleRent.Models;
using Whistler.VehicleSystem.Models;

namespace Whistler.VehicleRent
{
    class RentManager : Script
    {
        private static Dictionary<int, RentPoint> _rentPoints = new Dictionary<int, RentPoint>();
        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {

            var result = MySQL.QueryRead("SELECT * FROM rentcarpoint");
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    var rentPoint = new RentPoint(row);
                    _rentPoints.Add(rentPoint.ID, rentPoint);
                }
            }
        }

        [ServerEvent(Event.PlayerConnected)]
        public static void OnPlayerConnected(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"vehicleRent:loadPeds", JsonConvert.SerializeObject(_rentPoints.Select(item => new { id = item.Value.ID, position = item.Value.PositionPed, heading = item.Value.RotationPed })));
        }

        [RemoteEvent("vehicleRent:tryRent")]
        public static void TryRentVehicle(ExtPlayer player, int pointId, string model, int category, int payment)
        {
            if (!player.IsLogged())
                return;
            if (!_rentPoints.ContainsKey(pointId))
                return;
            _rentPoints[pointId].TryRentVehicle(player, model, category, payment);
        }

        //[Command("setrentpos")]
        public static void SetRentVehiclePosition(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            if (!player.IsInVehicle)
            {
                Notify.SendError(player, "vehicleRent_3");
                return;
            }
            SafeTrigger.SetData(player, "VEHICLERENT::POSITION", player.Vehicle.Position);
            SafeTrigger.SetData(player, "VEHICLERENT::ROTATION", player.Vehicle.Rotation.Z);
            Core.Chat.SendTo(player, "vehicleRent_16");
        }
        //[Command("createrentpoint")]
        public static void CreateRentPoint(ExtPlayer player)
        {
            if (!player.HasData("VEHICLERENT::POSITION") || !player.HasData("VEHICLERENT::ROTATION"))
            {
                Notify.SendError(player, "vehicleRent_2");
                return;
            }
            var posCar = player.GetData<Vector3>("VEHICLERENT::POSITION");
            var rotCar = player.GetData<float>("VEHICLERENT::ROTATION");
            player.ResetData("VEHICLERENT::POSITION");
            player.ResetData("VEHICLERENT::ROTATION");

            var rentPoint = new RentPoint(player.Position, player.Rotation.Z, posCar, rotCar);
            _rentPoints.Add(rentPoint.ID, rentPoint);
        }

        //[Command("changerentpedpos")]
        public static void ChangeRentPedPosition(ExtPlayer player, int pointId)
        {
            if (!player.IsLogged())
                return;
            if (!_rentPoints.ContainsKey(pointId))
                return;
            _rentPoints[pointId].ChangePositionPed(player);
        }

        //[Command("changerentcarpos")]
        public static void ChangeRentCarPosition(ExtPlayer player, int pointId)
        {
            if (!player.IsLogged())
                return;
            if (!player.IsInVehicle)
            {
                Notify.SendError(player, "vehicleRent_3");
                return;
            }
            if (!_rentPoints.ContainsKey(pointId))
                return;
            _rentPoints[pointId].ChangePositionCar(player.Vehicle as ExtVehicle);
            Core.Chat.SendTo(player, "vehicleRent_16");
        }
        //[Command("addrentcategory")]
        public static void AddRentVehicleCategory(ExtPlayer player, int pointId, int category)
        {
            if (!player.IsLogged())
                return;
            if (!_rentPoints.ContainsKey(pointId))
                return;
            if (!Enum.IsDefined(typeof(RentSection), category))
            {
                Core.Chat.SendTo(player, "vehicleRent_5".Translate(JsonConvert.SerializeObject(Enum.GetValues(typeof(RentSection)).Cast<RentSection>().Select(item => item.ToString()).ToList())));
                return;
            }
            _rentPoints[pointId].AddCategory(category);
        }
       // [Command("delrentcategory")]
        public static void DeleteRentVehicleCategory(ExtPlayer player, int pointId, int category)
        {
            if (!player.IsLogged())
                return;
            if (!_rentPoints.ContainsKey(pointId))
                return;
            _rentPoints[pointId].DeleteCategory(category);
        }
        //[Command("deleterent")]
        public static void DeleteRentVehicle(ExtPlayer player, int pointId)
        {
            if (!player.IsLogged())
                return;
            if (!_rentPoints.ContainsKey(pointId))
                return;
            _rentPoints[pointId].Delete();
            _rentPoints.Remove(pointId);
        }
    }
}
