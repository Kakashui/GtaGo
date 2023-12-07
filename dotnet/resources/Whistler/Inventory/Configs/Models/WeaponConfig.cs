using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs.Models
{
    public class WeaponConfig: BaseConfig
    {
        public List<WeaponSlots> Slots { get; set; }
        public WeaponCharacteristics Characteristics { get; set; } = new WeaponCharacteristics(0.0, 0.0, 0.0);
        public Dictionary<WeaponComponentSlots, List<WeaponComponentModel>> Components { get; set; } =
            new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                        new List<WeaponComponentModel>{ }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                        new List<WeaponComponentModel>{ }
                    },
                    {
                        WeaponComponentSlots.Clip,
                        new List<WeaponComponentModel>{ }
                    },
                    {
                        WeaponComponentSlots.Scope,
                        new List<WeaponComponentModel>{ }
                    },
                    {
                        WeaponComponentSlots.Grip,
                        new List<WeaponComponentModel>{ }
                    },
                    {
                        WeaponComponentSlots.Skin,
                        new List<WeaponComponentModel>{ }
                    }
                };
        public uint WeaponHash { get; set; }
        public ItemNames AmmoType { get; set; }
        public int MaxAmmo { get; set; } = 0;
    }
}
