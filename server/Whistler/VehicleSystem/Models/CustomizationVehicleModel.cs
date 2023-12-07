﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem;

namespace Whistler.VehicleSystem.Models
{
    public class CustomizationVehicleModel
    {
        public Dictionary<HandlingKeys, object> HandlingTuning { get; set; }
        public Dictionary<ModTypes, int> Components { get; set; }
        public Color PrimColor { get; set; }
        public Color SecColor { get; set; }
        public Color NeonColor { get; set; }
        public List<Color> NeonColors { get; set; } = new List<Color>();
        public Color TyreSmokeColor { get; set; }
        public int PaintTypePrim { get; set; }
        public int PaintTypeSec { get; set; }
        public CustomizationVehicleModel()
        {
            HandlingTuning = new Dictionary<HandlingKeys, object>();
            Components = new Dictionary<ModTypes, int>();

            PrimColor = new Color(0, 0, 0, 0);
            SecColor = new Color(0, 0, 0, 0);
            NeonColor = new Color(0, 0, 0, 0);
            NeonColors = new List<Color>();
            TyreSmokeColor = new Color(0, 0, 0, 0);
            PaintTypePrim = 0;
            PaintTypeSec = 0;
        }
        public CustomizationVehicleModel(CustomizationVehicleModel copy, bool handlingCopy)
        {
            HandlingTuning = new Dictionary<HandlingKeys, object>();
            if (handlingCopy)
                foreach (var comp in copy.HandlingTuning)
                {
                    HandlingTuning.Add(comp.Key, comp.Value);
                }
            Components = new Dictionary<ModTypes, int>();
            foreach (var comp in copy.Components)
            {
                Components.Add(comp.Key, comp.Value);
            }
            PrimColor = new Color(copy.PrimColor.Red, copy.PrimColor.Green, copy.PrimColor.Blue, copy.PrimColor.Alpha);
            SecColor = new Color(copy.SecColor.Red, copy.SecColor.Green, copy.SecColor.Blue, copy.SecColor.Alpha);
            NeonColor = new Color(0, 0, 0, 0);
            NeonColors = new List<Color>();
            foreach (var comp in copy.NeonColors)
            {
                NeonColors.Add(new Color(comp.Red, comp.Green, comp.Blue, comp.Alpha));
            }
            TyreSmokeColor = new Color(copy.TyreSmokeColor.Red, copy.TyreSmokeColor.Green, copy.TyreSmokeColor.Blue, copy.TyreSmokeColor.Alpha);
            PaintTypePrim = copy.PaintTypePrim;
            PaintTypeSec = copy.PaintTypeSec;
        }

        public int GetComponent(ModTypes type)
        {
            if (Components.ContainsKey(type))
                return Components[type];
            else return -1;
        }

        public void AddComponent(ModTypes type, int mod)
        {
            if (Components.ContainsKey(type))
            {
                if (mod < 0)
                    Components.Remove(type);
                else
                    Components[type] = mod;
            }
            else if (mod > -1)
                Components.Add(type, mod);
        }

        public object GetHandling(HandlingKeys key)
        {
            if (HandlingTuning.ContainsKey(key))
                return HandlingTuning[key];
            return null;
        }

        public void SetHandling(HandlingKeys key, object value)
        {
            if (HandlingTuning.ContainsKey(key))
            {
                if (value == null)
                    HandlingTuning.Remove(key);
                else
                    HandlingTuning[key] = value;
            }
            else if (value != null)
                HandlingTuning.Add(key, value);
        }

        public Color GetNeonColor(int index)
        {
            if (NeonColors.Count > index)
                return NeonColors[index];
            else
                return new Color(0, 0, 0, 0);
        }

        public void SetNeonColor(Color color, int index)
        {
            if (NeonColors.Count > index)
                NeonColors[index] = color;
            else
            {
                while (NeonColors.Count < index)
                    NeonColors.Add(new Color(0, 0, 0, 0));
                NeonColors.Add(color);
            }
        }
    }
}