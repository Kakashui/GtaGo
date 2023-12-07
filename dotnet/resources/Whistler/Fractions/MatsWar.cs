using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.Inventory.Enums;
using Whistler.VehicleSystem.Models;

namespace Whistler.Fractions
{

    class MatsWar : Script
    {
        public static bool isWar = false;
        private static Vector3 warPosition = new Vector3(2832.379, 1528.562, 23.44748);
        private static Blip warblip;
        private static string startWarTimer = null;

        private static string _spawnCarTimer = null;
        private static int _numberCar = 0;
        private static int _maxCars = 6;
        private static int _cntArmInOneCar = 50;
        private static int _spawnCarInterval = 300;
        private static int _waitStartWarTime = 600;
        private static List<int> _occupiedPointCarResp = new List<int>();
        private static List<Vector3> _carPosition = new List<Vector3>()
        {
            new Vector3(2836.028, 1459.517, 23.44012),
            new Vector3(2776.070, 1601.539, 23.38066),
            new Vector3(2781.568, 1409.127, 23.31832),
            new Vector3(2862.105, 1469.837, 23.43554),
            new Vector3(2865.661, 1514.513, 23.44753),
            new Vector3(2880.079, 1560.370, 23.44749),
            new Vector3(2702.694, 1517.958, 23.40023),
            new Vector3(2676.853, 1430.898, 23.38078),
            new Vector3(2707.046, 1391.668, 23.40992),
            new Vector3(2677.055, 1405.516, 23.49475),
            new Vector3(2677.101, 1611.574, 23.38043),
            new Vector3(2783.282, 1654.597, 23.37751),
            new Vector3(2680.655, 1550.915, 23.46132),
            new Vector3(2763.117, 1453.339, 23.40718),
            new Vector3(2701.380, 1470.666, 23.39209),
            new Vector3(2815.512, 1569.294, 23.44578),
        };
        private static List<Vector3> _carRotation = new List<Vector3>()
        {
            new Vector3(0, 0, 4.515463),
            new Vector3(0, 0, 264.7142),
            new Vector3(0, 0, 77.79422),
            new Vector3(0, 0, 74.17953),
            new Vector3(0, 0, 70.82795),
            new Vector3(0, 0, 90.55449),
            new Vector3(0, 0, 86.85670),
            new Vector3(0, 0, 273.1433),
            new Vector3(0, 0, 178.4408),
            new Vector3(0, 0, 264.3368),
            new Vector3(0, 0, 264.0617),
            new Vector3(0, 0, 176.0137),
            new Vector3(0, 0, 260.8004),
            new Vector3(0, 0, 166.3103),
            new Vector3(0, 0, 88.52740),
            new Vector3(0, 0, 340.1337),
        };


        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MatsWar));

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                warblip = NAPI.Blip.CreateBlip(615, warPosition, 1, 40, Main.StringToU16("Война за материалы"), 255, 0, true, 0, 0);
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        public static void startMatWarTimer()
        {
            Chat.SendFractionMessage(1, "Frac_389", false);
            Chat.SendFractionMessage(2, "Frac_389", false);
            Chat.SendFractionMessage(3, "Frac_389", false);
            Chat.SendFractionMessage(4, "Frac_389", false);
            Chat.SendFractionMessage(5, "Frac_389", false);
            Chat.SendAllFamilyMessage("Frac_389", false);
            startWarTimer = Timers.StartOnce(_waitStartWarTime * 1000, () => startWar());
        }

        public static void startWar()
        {
            NAPI.Task.Run(() =>
            {
                if (isWar) return;
                isWar = true;
                _numberCar = 0;
                _occupiedPointCarResp = new List<int>();
                Chat.SendFractionMessage(1, "Frac_390", false);
                Chat.SendFractionMessage(2, "Frac_390", false);
                Chat.SendFractionMessage(3, "Frac_390", false);
                Chat.SendFractionMessage(4, "Frac_390", false);
                Chat.SendFractionMessage(5, "Frac_390", false);
                Chat.SendAllFamilyMessage("Frac_390", false);
                warblip.Color = 49;
                if (_spawnCarTimer != null)
                {
                    Timers.Stop(_spawnCarTimer);
                    _spawnCarTimer = null;
                }
                _spawnCarTimer = Timers.Start(_spawnCarInterval * 1000, () => SpawnMatCar());
                //Main.StopT(startWarTimer, "timer_11");
            });
        }

        public static void endWar()
        {
            try
            {
                NAPI.Task.Run(() =>
                {
                    //NAPI.Entity.DeleteEntity(warMarker);
                    isWar = false;
                    Chat.SendFractionMessage(1, "Frac_391", false);
                    Chat.SendFractionMessage(2, "Frac_391", false);
                    Chat.SendFractionMessage(3, "Frac_391", false);
                    Chat.SendFractionMessage(4, "Frac_391", false);
                    Chat.SendFractionMessage(5, "Frac_391", false);
                    Chat.SendAllFamilyMessage("Frac_391", false);
                    warblip.Color = 40;
                    if (_spawnCarTimer != null)
                    {
                        Timers.Stop(_spawnCarTimer);
                        _spawnCarTimer = null;
                    }
                });
            }
            catch (Exception e) { _logger.WriteError($"EndMatsWar: " + e.ToString()); }
        }

        public static void SpawnMatCar()
        {
            NAPI.Task.Run(() =>
            {
                _numberCar++;
                if (_numberCar <= _maxCars)
                {
                    int point = GetRandomPosition();
                    ExtVehicle vehicle = VehicleManager.CreateTemporaryVehicle("landstalker2", _carPosition[point], _carRotation[point], $"MATWAR{_numberCar}", VehicleAccess.MatWars);
                    if (vehicle == null || !vehicle.Exists) return;

                    vehicle.SetMod(55, 1);
                    vehicle.SetMod(53, 1);
                    var vehInv = InventoryService.GetById(vehicle.Data.InventoryId);
                    vehInv.ChangeMaxWeight(200000);
                    var item = ItemsFabric.CreateItemBox(ItemNames.ArmorBox, ItemNames.BodyArmor, _cntArmInOneCar, false);
                    vehInv.AddItem(item);
                    //var data = new nItem(ItemType.Material);
                    //data.Count = _conMatInOneCar; //TODO: добавиь загрузку ящиков в авто
                    GarbageCollector.GarbageManager.Add(vehicle, (90 - _numberCar * 5) * 60);
                    Chat.SendFractionMessage(1, "Frac_523", false);
                    Chat.SendFractionMessage(2, "Frac_523", false);
                    Chat.SendFractionMessage(3, "Frac_523", false);
                    Chat.SendFractionMessage(4, "Frac_523", false);
                    Chat.SendFractionMessage(5, "Frac_523", false);
                    Chat.SendAllFamilyMessage("Frac_523", false);
                }
                if (_numberCar >= _maxCars)
                {
                    endWar();
                }
            });
        }

        private static int GetRandomPosition()
        {
            Random random = new Random();
            int key;
            do
            {
                key = random.Next(0, _carPosition.Count);
            }
            while (_occupiedPointCarResp.Contains(key));
            _occupiedPointCarResp.Add(key);
            return key;
        }

        public static void DestroyCar(ExtVehicle vehicle)
        {
            GarbageCollector.GarbageManager.Remove(vehicle);
            vehicle.CustomDelete();
        }
    }
}
