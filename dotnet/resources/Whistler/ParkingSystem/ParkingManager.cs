using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Whistler.Core;
using Whistler.SDK;
using System.Linq;
using Whistler.Houses;
using Whistler.Fractions;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.GUI;
using Whistler.MoneySystem;
using Whistler.Common;
using Whistler.Entities;

namespace Whistler.ParkingSystem
{
    class ParkingManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ParkingManager));
        private static List<Parking> ParkingList;
        public static int ParkingTariff = 500;
        private static List<ParkingConfig> PointParking = new List<ParkingConfig>() {
            new ParkingConfig(new Vector3(638.3456, 206.6573, 96.48418), new List<PosAndRot>
            {
                new PosAndRot(new Vector3(659.7241, 196.5725, 94.94661), new Vector3(-1.224387, -3.749594, 68.66577)),
                new PosAndRot(new Vector3(658.4304, 192.8275, 94.94559), new Vector3(-1.435506, -3.772766, 70.29144)),
                new PosAndRot(new Vector3(657.2913, 188.653, 94.91451), new Vector3(-1.276545, -3.570715, 69.53354)),
                new PosAndRot(new Vector3(656.134, 184.8595, 94.88713), new Vector3(-1.126065, -3.059856, 71.40076)),
                new PosAndRot(new Vector3(654.3495, 181.1973, 94.93661), new Vector3(-0.8676305, -3.973384, 71.93799)),
                new PosAndRot(new Vector3(652.8878, 177.2425, 94.94452), new Vector3(-1.458927, -4.099485, 69.41113)),
                new PosAndRot(new Vector3(651.4514, 173.1249, 94.93993), new Vector3(-1.456512, -4.063287, 69.95685)),
                new PosAndRot(new Vector3(650.0035, 169.4619, 94.95058), new Vector3(-1.497472, -4.181016, 69.3663)),
                new PosAndRot(new Vector3(628.5827, 196.2237, 97.10545), new Vector3(-2.112899, -5.273224, 248.9619)),
                new PosAndRot(new Vector3(627.2545, 192.2676, 97.09882), new Vector3(-1.365157, -5.607278, 250.6249)),
                new PosAndRot(new Vector3(625.9432, 188.401, 97.14838), new Vector3(-1.674954, -6.53116, 248.3566)),
                new PosAndRot(new Vector3(633.5689, 184.0598, 96.37316), new Vector3(-2.355544, -5.538383, 68.82355)),
                new PosAndRot(new Vector3(643.6538, 180.4303, 95.67887), new Vector3(-0.5668221, -4.468388, 249.7692)),
            }),
            new ParkingConfig(new Vector3(-1216.897, -644.9938, 24.7813), new List<PosAndRot>
            {
                new PosAndRot(new Vector3(-1204.619, -654.4253, 25.78879), new Vector3(-0.3960331, 0.4743967, 130.7585)),
                new PosAndRot(new Vector3(-1198.065, -661.9796, 25.78821), new Vector3(-0.391218, 0.4765008, 129.3817)),
                new PosAndRot(new Vector3(-1191.636, -669.6641, 25.78811), new Vector3(-0.4033053, 0.4538181, 131.6093)),
                new PosAndRot(new Vector3(-1185.151, -677.4843, 25.78817), new Vector3(-0.3779067, 0.4909597, 127.6158)),
                new PosAndRot(new Vector3(-1218.875, -686.2852, 25.78815), new Vector3(0.400572, -0.457658, 311.1985)),
                new PosAndRot(new Vector3(-1225.671, -678.7367, 25.78844), new Vector3(0.3790034, -0.4803745, 309.1901)),
                new PosAndRot(new Vector3(-1232.062, -671.0748, 25.78962), new Vector3(0.3980379, -0.4614737, 310.5448)),
                new PosAndRot(new Vector3(-1238.723, -663.4174, 25.78794), new Vector3(0.4016941, -0.4657642, 310.7727)),
                new PosAndRot(new Vector3(-1245.418, -655.775, 25.78815), new Vector3(0.3898723, -0.4683923, 309.765)),
                new PosAndRot(new Vector3(-1251.71, -648.191, 25.78847), new Vector3(0.3902369, -0.4722621, 310.7345)),
            }),
            new ParkingConfig(new Vector3(-17.30688, 6303.709, 30.25466), new List<PosAndRot>
            {
                new PosAndRot(new Vector3(-19.66507, 6321.953, 31.11614), new Vector3(-0.5190203, -0.2637529, 211.3044)),
                new PosAndRot(new Vector3(-16.99489, 6324.633, 31.1191), new Vector3(-0.4659165, -0.2709568, 212.1564)),
                new PosAndRot(new Vector3(-14.15051, 6327.283, 31.11747), new Vector3(-0.4675295, -0.2790313, 211.537)),
                new PosAndRot(new Vector3(-11.38475, 6330.06, 31.11754), new Vector3(-0.4708399, -0.2889853, 212.7542)),
                new PosAndRot(new Vector3(-8.568148, 6332.789, 31.11812), new Vector3(-0.4818792, -0.2787994, 211.5686)),
                new PosAndRot(new Vector3(-5.796357, 6335.531, 31.11892), new Vector3(-0.4665925, -0.2891496, 212.4235)),
                new PosAndRot(new Vector3(-2.92857, 6338.265, 31.11708), new Vector3(-0.4765728, -0.2733096, 211.2244)),
                new PosAndRot(new Vector3(-0.1031849, 6340.966, 31.11618), new Vector3(-0.464718, -0.3567886, 212.8599)),
            }),
        };


        [ServerEvent(Event.ResourceStart)]
        public static void ResourceStart()
        {
            try
            {
                ParkingList = new List<Parking>();
                for (int i = 0; i < PointParking.Count; i++)
                    ParkingList.Add(new Parking(i, PointParking[i]));
            }
            catch (Exception e)
            {
                _logger.WriteError($"ParkingManager | ResourceStart" + e.ToString());
            }
        }

        public static void interactPressed(ExtPlayer player, int id)
        {
            try
            {
                if (!player.IsLogged()) return;

                Parking par = ParkingList.FirstOrDefault(c => c.ParkingId == id);
                if (par == null) return;

                if (HouseManager.GetHouse(player, true) != null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Парковка доступна только для игроков у которых нет собственного дома.", 3000);
                    return;
                }

                List<int> allVehicles = VehicleManager.getAllHolderVehicles(player.Character.UUID, OwnerType.Personal);
                if (!allVehicles.Any())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас нет машин на парковке.", 3000);
                    return;
                }

                List<DialogUI.ButtonSetting> buttons = new List<DialogUI.ButtonSetting>();
                VehicleSystem.Models.VehiclesData.VehicleBase vehicleData;
                DialogUI.ButtonSetting button;
                foreach (int vehicle in allVehicles)
                {
                    if (vehicle < 0) continue;
                    if (!VehicleManager.Vehicles.ContainsKey(vehicle)) continue;
                    vehicleData = VehicleManager.Vehicles[vehicle];

                    button = new DialogUI.ButtonSetting
                    {
                        Name = $"{vehicleData.ModelName} ({vehicleData.Number})",
                        Icon = "confirm",
                        Action = p =>
                        {
                            GetVehicleFromParking(p, par, vehicle);
                        }
                    };
                    buttons.Add(button);
                }
                buttons.Add(new DialogUI.ButtonSetting
                {
                    Name = "Отмена",
                    Icon = "cancel",
                    Action = p => { }
                });
                if (buttons.Count <= 1)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас нет машин на парковке, готовых к использованию.", 3000);
                    return;
                }

                DialogUI.Open(player, $"Выберите машину, которую хотите забрать с парковки (${ParkingTariff})", buttons);

            }
            catch (Exception e)
            {
                _logger.WriteError($"ParkingManager | interactPressed" + e.ToString());
            }
        }

        private static void GetVehicleFromParking(ExtPlayer player, Parking parking, int vehicleId)
        {
            if (!Wallet.TransferMoney(player.Character, Manager.GetFraction(6), ParkingTariff, 0, "Оплата парковки"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Park_5", 3000);
                return;
            }
            DeleteParkingVehicle(player);

            VehicleSystem.Models.VehiclesData.VehicleBase vehicle = VehicleManager.Vehicles[vehicleId];
            PosAndRot position = parking.Positions.Dequeue();
            parking.Positions.Enqueue(position);
            vehicle.Spawn(position.Position + new Vector3(0, 0, 0.3), position.Rotation, 0);
            player.Character.ParkingSpawnCar = vehicleId;
        }

        public static void DeleteParkingVehicle(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (player.Character.ParkingSpawnCar > 0)
            {
                SafeTrigger.GetVehicleByDataId(player.Character.ParkingSpawnCar)?.CustomDelete();
                player.Character.ParkingSpawnCar = -1;
            }
        }
    }

    class Parking
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Parking));
        private Vector3 ParkingPos;
        public Queue<PosAndRot> Positions;
        public int ParkingId;
        internal Parking(int id, ParkingConfig coords)
        {
            try
            {
                ParkingId = id;
                ParkingPos = coords.BlipPosition;
                Positions = new Queue<PosAndRot>(coords.Position);
                NAPI.Blip.CreateBlip(267, ParkingPos, 1, 38, Main.StringToU16("Парковка"), 255, 0, true, 0, NAPI.GlobalDimension);
                CreateColShape(ParkingId, ParkingPos);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Parking | Parking()" + e.ToString());
            }
        }

        private void CreateColShape(int id, Vector3 Pos)
        {
            InteractShape.Create(Pos, 2f, 3)
                .AddInteraction((player) =>
                {
                    ParkingManager.interactPressed(player, id);
                });
        }
    }
}
