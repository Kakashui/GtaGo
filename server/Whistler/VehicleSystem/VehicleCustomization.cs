using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Helpers;
using Whistler.VehicleSystem;

namespace Whistler.VehicleSystem
{
    class VehicleCustomization
    {
        /// <summary>
        /// Применение кастомизации к машине
        /// </summary>
        /// <param name="vehicle"></param>
        public static void ApplyVehCustomization(Vehicle vehicle)
        {
            foreach (ModTypes key in Enum.GetValues(typeof(ModTypes)))
            {
                ApplyMod(vehicle, key);
            }
            ApplyHandlingVehCustomization(vehicle);
            ApplyColor(vehicle, true);
            ApplyColor(vehicle, false);
            ApplyNeon(vehicle);
            ApplyTyreColor(vehicle);
            ApplyPowerAndTorque(vehicle);
        }
        /// <summary>
        /// Применение кастомизации к машине
        /// </summary>
        /// <param name="vehicle"></param>
        public static void ApplyHandlingVehCustomization(Vehicle vehicle)
        {
            foreach (HandlingKeys key in Enum.GetValues(typeof(HandlingKeys)))
            {
                ApplyHandlingMod(vehicle, key);
            }
        }

        /// <summary>
        /// установка и применение модификации
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="modType"></param>
        /// <param name="mod"></param>
        public static void SetMod(Vehicle vehicle, ModTypes modType, int mod)
        {
            vehicle.GetVehicleGo().Data.VehCustomization.AddComponent(modType, mod);
            vehicle.GetVehicleGo().Data.Save();
            ApplyMod(vehicle, modType);
        }

        /// <summary>
        /// Применение модификации
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="modType"></param>
        /// <param name="mod"></param>
        private static void ApplyMod(Vehicle vehicle, ModTypes modType)
        {
            int mod = vehicle.GetVehicleGo().Data.VehCustomization.GetComponent(modType);
            switch (modType)
            {
                case ModTypes.Xenon:
                    if (mod > -1)
                        vehicle.SetSharedData("hlcolor", mod);
                    else if (vehicle.HasSharedData("hlcolor"))
                        vehicle.ResetSharedData("hlcolor");
                    vehicle.SetMod((int)modType, mod);
                    break;
                case ModTypes.WheelsColor:
                    if (mod > -1)
                        vehicle.SetSharedData("wheelcolor", mod);
                    else if(vehicle.HasSharedData("wheelcolor"))
                        vehicle.ResetSharedData("wheelcolor");
                    break;
                case ModTypes.PearlColor:
                    if (mod > -1)
                        vehicle.SetSharedData("pearlColorCar", mod);
                    else if (vehicle.HasSharedData("pearlColorCar"))
                        vehicle.ResetSharedData("pearlColorCar");
                    break;
                case ModTypes.FrontWheels:
                    vehicle.SetMod((int)ModTypes.WheelsType, vehicle.GetVehicleGo().Data.VehCustomization.GetComponent(ModTypes.WheelsType));
                    vehicle.SetMod((int)modType, mod);
                    break;
                case ModTypes.Turbo:
                    vehicle.SetMod((int)modType, mod == -1 ? -1 : 0);
                    ApplyPowerAndTorque(vehicle);
                    break;
                case ModTypes.Engine:
                    vehicle.SetMod((int)modType, mod);
                    ApplyPowerAndTorque(vehicle);
                    break;
                case ModTypes.LiveryTwo:
                    if (mod < 0)
                        vehicle.RemoveMod((int)modType);
                    else
                        vehicle.SetMod((int)modType, mod);
                    break;
                default:
                    vehicle.SetMod((int)modType, mod);
                    break;
            }
        }

        /// <summary>
        /// установка и применение handling параметра
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetHandlingMod(Vehicle vehicle, HandlingKeys key, object value)
        {
            vehicle.GetVehicleGo().Data.VehCustomization.SetHandling(key, value);
            vehicle.GetVehicleGo().Data.Save();
            ApplyHandlingMod(vehicle, key);
        }

        /// <summary>
        /// Применение handling параметра
        /// </summary>
        /// <param name="vehicle">авто</param>
        /// <param name="key">ключ</param>
        private static void ApplyHandlingMod(Vehicle vehicle, HandlingKeys key)
        {
            object value = vehicle.GetVehicleGo().Data.VehCustomization.GetHandling(key);
            if (value != null)
                vehicle.SetSharedData($"veh:handl:{(int)key}", $"{value}");
            else if (vehicle.HasSharedData($"veh:handl:{(int)key}"))
                vehicle.ResetSharedData($"veh:handl:{(int)key}");
        }

        /// <summary>
        /// Установка и применение цвета
        /// </summary>
        /// <param name="vehicle">Авто</param>
        /// <param name="color">Цвет</param>
        /// <param name="type">Тип покраски</param>
        /// <param name="primColor">true - primary color, false - second color</param>
        public static void SetColor(Vehicle vehicle, Color color, int type, bool primColor)
        {
            if (primColor)
            {
                vehicle.GetVehicleGo().Data.VehCustomization.PrimColor = color;
                vehicle.GetVehicleGo().Data.VehCustomization.PaintTypePrim = type;
            }
            else
            {
                vehicle.GetVehicleGo().Data.VehCustomization.SecColor = color;
                vehicle.GetVehicleGo().Data.VehCustomization.PaintTypeSec = type;
            }
            vehicle.GetVehicleGo().Data.Save();
            ApplyColor(vehicle, primColor);
        }

        /// <summary>
        /// Применение цвета
        /// </summary>
        /// <param name="vehicle">Авто</param>
        /// <param name="primColor">true - primary color, false - second color</param>
        private static void ApplyColor(Vehicle vehicle, bool primColor)
        {
            if (primColor)
            {
                vehicle.CustomPrimaryColor = vehicle.GetVehicleGo().Data.VehCustomization.PrimColor;
                vehicle.SetSharedData("paintTypeCarPrim", vehicle.GetVehicleGo().Data.VehCustomization.PaintTypePrim);
            }
            else
            {
                vehicle.CustomSecondaryColor = vehicle.GetVehicleGo().Data.VehCustomization.SecColor;
                vehicle.SetSharedData("paintTypeCarSec", vehicle.GetVehicleGo().Data.VehCustomization.PaintTypeSec);
            }
        }

        /// <summary>
        /// Установка и применение цвета
        /// </summary>
        /// <param name="vehicle">Авто</param>
        /// <param name="colors">Цвет</param>
        public static void SetNeon(Vehicle vehicle, List<Color> colors)
        {
            vehicle.GetVehicleGo().Data.VehCustomization.NeonColors = colors;
            vehicle.GetVehicleGo().Data.Save();
            ApplyNeon(vehicle);
        }

        /// <summary>
        /// Установка и применение неона
        /// </summary>
        /// <param name="vehicle">Авто</param>
        /// <param name="color">Цвет</param>
        public static void AddNeon(Vehicle vehicle, Color color)
        {
            if (vehicle.GetVehicleGo().Data.VehCustomization.NeonColors == null)
                return;
            vehicle.GetVehicleGo().Data.VehCustomization.NeonColors.Add(color);
            vehicle.GetVehicleGo().Data.Save();
            ApplyNeon(vehicle);
        }

        /// <summary>
        /// Применение неона
        /// </summary>
        /// <param name="vehicle">Авто</param>
        public static void ApplyNeon(Vehicle vehicle)
        {
            if (vehicle.GetVehicleGo().Data.VehCustomization.NeonColors == null)
                return;
            List<Color> colors = vehicle.GetVehicleGo().Data.VehCustomization.NeonColors.Where(item => item.Alpha > 0).ToList();
            if (colors.Count > 0) { 
               // Console.WriteLine($"{JsonConvert.SerializeObject(colors.Select(c => new List<int> { c.Red, c.Green, c.Blue }).ToList())}");
                vehicle.SetSharedData("veh:flashingneon", colors.Select(c => new List<int> { c.Red, c.Green, c.Blue }).ToList());
            }
            else if(vehicle.HasSharedData("veh:flashingneon"))
                vehicle.ResetSharedData("veh:flashingneon");
        }

        /// <summary>
        /// Установка и применение цвета дыма из под колес
        /// </summary>
        /// <param name="vehicle">Авто</param>
        /// <param name="color">Цвет</param>
        public static void SetTyreColor(Vehicle vehicle, Color color)
        {
            vehicle.GetVehicleGo().Data.VehCustomization.TyreSmokeColor = color;
            vehicle.GetVehicleGo().Data.Save();
            ApplyTyreColor(vehicle);
        }

        /// <summary>
        /// Применение цвета дыма из под колес
        /// </summary>
        /// <param name="vehicle">Авто</param>
        private static void ApplyTyreColor(Vehicle vehicle)
        {
            var clr = vehicle.GetVehicleGo().Data.VehCustomization.TyreSmokeColor;
            if (clr.Alpha > 0)
                vehicle.SetSharedData("tyrecolor", new List<int> { clr.Red, clr.Green, clr.Blue });
            else if(vehicle.HasSharedData("tyrecolor"))
                vehicle.ResetSharedData("tyrecolor");
        }

        public static void SetPowerTorque(Vehicle vehicle, float power = -1000, float torque = -1000)
        {
            if (power != -1000)
                vehicle.GetVehicleGo().Data.EnginePower = power;
            if (torque != -1000)
                vehicle.GetVehicleGo().Data.EngineTorque = torque;
            vehicle.GetVehicleGo().Data.Save();
            ApplyPowerAndTorque(vehicle);
        }

        /// <summary>
        /// Применение power и torque
        /// </summary>
        /// <param name="vehicle">Авто</param>
        private static void ApplyPowerAndTorque(Vehicle vehicle)
        {
            float power = vehicle.GetVehicleGo().Data.EnginePower;
            float torque = vehicle.GetVehicleGo().Data.EngineTorque;

            var turbo = vehicle.GetVehicleGo().Data.VehCustomization.GetComponent(ModTypes.Turbo);
            var engine = vehicle.GetVehicleGo().Data.VehCustomization.GetComponent(ModTypes.Engine);
            power += (turbo < 0 ? 0 : (turbo * 10 + 3)) + (engine + 1) * 3;

            vehicle.SetSharedData("ENGINEPOWER", power);
            vehicle.SetSharedData("ENGINETORQUE", torque);
        }

    }
}
