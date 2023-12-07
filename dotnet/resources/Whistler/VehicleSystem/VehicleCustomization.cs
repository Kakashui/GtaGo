using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.VehicleSystem
{
    class VehicleCustomization
    {
        /// <summary>
        /// Применение кастомизации к машине
        /// </summary>
        /// <param name="vehicle"></param>
        public static void ApplyVehCustomization(ExtVehicle vehicle)
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
        public static void ApplyHandlingVehCustomization(ExtVehicle vehicle)
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
        public static void SetMod(ExtVehicle vehicle, ModTypes modType, int mod)
        {
            if (vehicle == null) return;

            vehicle.Data.VehCustomization.AddComponent(modType, mod);
            vehicle.Data.Save();
            ApplyMod(vehicle, modType);
        }

        /// <summary>
        /// Применение модификации
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="modType"></param>
        /// <param name="mod"></param>
        private static void ApplyMod(ExtVehicle vehicle, ModTypes modType)
        {
            if (vehicle == null) return;

            int mod = vehicle.Data.VehCustomization.GetComponent(modType);
            switch (modType)
            {
                case ModTypes.Xenon:
                    if (mod > -1)
                        SafeTrigger.SetSharedData(vehicle, "hlcolor", mod);
                    else if (vehicle.HasSharedData("hlcolor"))
                        vehicle.ResetSharedData("hlcolor");
                    vehicle.SetMod((int)modType, mod);
                    break;
                case ModTypes.WheelsColor:
                    if (mod > -1)
                        SafeTrigger.SetSharedData(vehicle, "wheelcolor", mod);
                    else if(vehicle.HasSharedData("wheelcolor"))
                        vehicle.ResetSharedData("wheelcolor");
                    break;
                case ModTypes.PearlColor:
                    if (mod > -1)
                        SafeTrigger.SetSharedData(vehicle, "pearlColorCar", mod);
                    else if (vehicle.HasSharedData("pearlColorCar"))
                        vehicle.ResetSharedData("pearlColorCar");
                    break;
                case ModTypes.FrontWheels:
                    vehicle.SetMod((int)ModTypes.WheelsType, vehicle.Data.VehCustomization.GetComponent(ModTypes.WheelsType));
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
        public static void SetHandlingMod(ExtVehicle vehicle, HandlingKeys key, object value)
        {
            if (vehicle == null) return;

            vehicle.Data.VehCustomization.SetHandling(key, value);
            vehicle.Data.Save();
            ApplyHandlingMod(vehicle, key);
        }

        /// <summary>
        /// Применение handling параметра
        /// </summary>
        /// <param name="vehicle">авто</param>
        /// <param name="key">ключ</param>
        private static void ApplyHandlingMod(ExtVehicle vehicle, HandlingKeys key)
        {
            object value = vehicle.Data.VehCustomization.GetHandling(key);
            if (value != null)
                SafeTrigger.SetSharedData(vehicle, $"veh:handl:{(int)key}", $"{value}");
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
        public static void SetColor(ExtVehicle vehicle, Color color, int type, bool primColor)
        {
            if (vehicle == null) return;

            if (primColor)
            {
                vehicle.Data.VehCustomization.PrimColor = color;
                vehicle.Data.VehCustomization.PaintTypePrim = type;
            }
            else
            {
                vehicle.Data.VehCustomization.SecColor = color;
                vehicle.Data.VehCustomization.PaintTypeSec = type;
            }
            vehicle.Data.Save();
            ApplyColor(vehicle, primColor);
        }

        /// <summary>
        /// Применение цвета
        /// </summary>
        /// <param name="vehicle">Авто</param>
        /// <param name="primColor">true - primary color, false - second color</param>
        private static void ApplyColor(ExtVehicle vehicle, bool primColor)
        {
            if (vehicle == null) return;

            if (primColor)
            {
                vehicle.CustomPrimaryColor = vehicle.Data.VehCustomization.PrimColor;
                SafeTrigger.SetSharedData(vehicle, "paintTypeCarPrim", vehicle.Data.VehCustomization.PaintTypePrim);
            }
            else
            {
                vehicle.CustomSecondaryColor = vehicle.Data.VehCustomization.SecColor;
                SafeTrigger.SetSharedData(vehicle, "paintTypeCarSec", vehicle.Data.VehCustomization.PaintTypeSec);
            }
        }

        /// <summary>
        /// Установка и применение цвета
        /// </summary>
        /// <param name="vehicle">Авто</param>
        /// <param name="colors">Цвет</param>
        public static void SetNeon(ExtVehicle vehicle, List<Color> colors)
        {
            if (vehicle == null) return;

            vehicle.Data.VehCustomization.NeonColors = colors;
            vehicle.Data.Save();
            ApplyNeon(vehicle);
        }

        /// <summary>
        /// Установка и применение неона
        /// </summary>
        /// <param name="vehicle">Авто</param>
        /// <param name="color">Цвет</param>
        public static void AddNeon(ExtVehicle vehicle, Color color)
        {
            if (vehicle == null) return;

            if (vehicle.Data.VehCustomization.NeonColors == null)
                return;
            vehicle.Data.VehCustomization.NeonColors.Add(color);
            vehicle.Data.Save();
            ApplyNeon(vehicle);
        }

        /// <summary>
        /// Применение неона
        /// </summary>
        /// <param name="vehicle">Авто</param>
        public static void ApplyNeon(ExtVehicle vehicle)
        {
            if (vehicle == null) return;

            if (vehicle.Data.VehCustomization.NeonColors == null)
                return;
            List<Color> colors = vehicle.Data.VehCustomization.NeonColors.Where(item => item.Alpha > 0).ToList();
            if (colors.Count > 0) { 
               // Console.WriteLine($"{JsonConvert.SerializeObject(colors.Select(c => new List<int> { c.Red, c.Green, c.Blue }).ToList())}");
                SafeTrigger.SetSharedData(vehicle, "veh:flashingneon", colors.Select(c => new List<int> { c.Red, c.Green, c.Blue }).ToList());
            }
            else if(vehicle.HasSharedData("veh:flashingneon"))
                vehicle.ResetSharedData("veh:flashingneon");
        }

        /// <summary>
        /// Установка и применение цвета дыма из под колес
        /// </summary>
        /// <param name="vehicle">Авто</param>
        /// <param name="color">Цвет</param>
        public static void SetTyreColor(ExtVehicle vehicle, Color color)
        {
            if (vehicle == null) return;

            vehicle.Data.VehCustomization.TyreSmokeColor = color;
            vehicle.Data.Save();
            ApplyTyreColor(vehicle);
        }

        /// <summary>
        /// Применение цвета дыма из под колес
        /// </summary>
        /// <param name="vehicle">Авто</param>
        private static void ApplyTyreColor(ExtVehicle vehicle)
        {
            var clr = vehicle.Data.VehCustomization.TyreSmokeColor;
            if (clr.Alpha > 0)
                SafeTrigger.SetSharedData(vehicle, "tyrecolor", new List<int> { clr.Red, clr.Green, clr.Blue });
            else if(vehicle.HasSharedData("tyrecolor"))
                vehicle.ResetSharedData("tyrecolor");
        }

        public static void SetPowerTorque(ExtVehicle vehicle, float power = -1000, float torque = -1000)
        {
            if (vehicle == null) return;

            if (power != -1000)
                vehicle.Data.EnginePower = power;
            if (torque != -1000)
                vehicle.Data.EngineTorque = torque;
            vehicle.Data.Save();
            ApplyPowerAndTorque(vehicle);
        }

        /// <summary>
        /// Применение power и torque
        /// </summary>
        /// <param name="vehicle">Авто</param>
        private static void ApplyPowerAndTorque(ExtVehicle vehicle)
        {
            if (vehicle == null) return;

            float power = vehicle.Data.EnginePower;
            float torque = vehicle.Data.EngineTorque;

            var turbo = vehicle.Data.VehCustomization.GetComponent(ModTypes.Turbo);
            var engine = vehicle.Data.VehCustomization.GetComponent(ModTypes.Engine);
            power += (turbo < 0 ? 0 : (turbo * 10 + 3)) + (engine + 1) * 3;

            SafeTrigger.SetSharedData(vehicle, "ENGINEPOWER", power);
            SafeTrigger.SetSharedData(vehicle, "ENGINETORQUE", torque);
        }

    }
}
