using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    static class WeaponConfigs
    {
        public static Dictionary<ItemNames, WeaponConfig> Config;
        static WeaponConfigs()
        {
            Config = new Dictionary<ItemNames, WeaponConfig>();
            
            //Pistol
            Config.Add(ItemNames.Pistol, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_pistol"),
                Weight = 2500,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4},
                DisplayName = "weapon_pistol",
                AmmoType = ItemNames.PistolAmmo,
                Image = "pistol",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.Pistol,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                            new WeaponComponentModel(Components.AtPiSupp02, 500)
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.PistolClip01, 50, 12),
                             new WeaponComponentModel( Components.PistolClip02, 700, 16 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{}
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{}
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.PistolVarmodLuxe, 200 )
                        }
                    },
                },
                MaxAmmo = 12,
                ShopType = 2

            });

            Config.Add(ItemNames.CombatPistol, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_combatpistol"),
                Weight = 250,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_combatpistol",
                AmmoType = ItemNames.PistolAmmo,
                Image = "combatpiastol",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.CombatPistol,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.PistolClip01, 50, 12 ),
                             new WeaponComponentModel( Components.PistolClip02, 200, 12  )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                         new WeaponComponentModel( Components.CombatPistolVarmodLowrider, 200 )

                        }
                    }
                },
                MaxAmmo = 12

            });
            
            Config.Add(ItemNames.APPistol, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_appistol"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_appistol",
                AmmoType = ItemNames.PistolAmmo,
                Image = "appistol",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.APPistol,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.APPistolClip01, 50, 18 ),
                             new WeaponComponentModel( Components.APPistolClip02, 200, 36 )

                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.APPistolVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 18
            });

            Config.Add(ItemNames.Pistol50, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_pistol50"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.6, 0.3, 0.7),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_pistol50",
                AmmoType = ItemNames.PistolAmmo,
                Image = "pistol50",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Pistol50),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.Pistol50Clip01, 50, 9 ),
                             new WeaponComponentModel( Components.Pistol50Clip02, 200, 12 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.Pistol50VarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 9,
                ShopType = 2

            });

            Config.Add(ItemNames.Revolver, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_revolver"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.2, 0.8, 1.0),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_revolver",
                AmmoType = ItemNames.PistolAmmo,
                Image = "revolver",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Revolver),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.RevolverClip01, 50, 6 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.RevolverVarmodBoss, 200 ),
                             new WeaponComponentModel( Components.RevolverVarmodGoon, 200 )
                       }
                    }
                },
                MaxAmmo = 6,
                ShopType = 2

            });

            Config.Add(ItemNames.SNSPistol, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_sns_pistol"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_sns_pistol",
                AmmoType = ItemNames.PistolAmmo,
                Image = "snspistol",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.SNSPistol),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SNSPistolClip01, 50, 6 ),
                             new WeaponComponentModel( Components.SNSPistolClip02, 200, 12 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SNSPistolVarmodLowrider, 200 )
                        }
                    }
                },
                MaxAmmo = 6

            });

            Config.Add(ItemNames.HeavyPistol, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_heavypistol"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.7, 0.2, 0.8),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_heavypistol",
                AmmoType = ItemNames.PistolAmmo,
                Image = "heavypistol",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.HeavyPistol),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiSupp, 200 )

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.HeavyPistolClip01, 50, 18 ),
                             new WeaponComponentModel( Components.HeavyPistolClip02, 200, 36 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.HeavyPistolVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 18,
                ShopType = 2

            });

            Config.Add(ItemNames.RevolverMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_revolvermk2"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_revolvermk2",
                AmmoType = ItemNames.PistolAmmo,
                Image = "revolvermk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.RevolverMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
{
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiComp03, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.RevolverMk2Clip01, 50 ),
                             new WeaponComponentModel( Components.RevolverMk2ClipTracer, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2ClipIncendiary, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2ClipHollowpoint, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2ClipFmj, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSights, 200 ),
                             new WeaponComponentModel( Components.AtScopeMacroMk2, 200 )

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.RevolverMk2Camo, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.RevolverMk2CamoInd01, 500)
                        }
                    }
                },
                MaxAmmo = 6

            });

            Config.Add(ItemNames.SNSPistolMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_sns_pistolmk2"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_sns_pistolmk2",
                AmmoType = ItemNames.PistolAmmo,
                Image = "pistolmk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.SNSPistolMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiSupp02, 200 ),
                             new WeaponComponentModel( Components.AtPiComp02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiFlsh03, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SnspistolMk2Clip01, 50, 12 ),
                             new WeaponComponentModel( Components.SnspistolMk2Clip02, 200, 16 ),
                             new WeaponComponentModel( Components.SnspistolMk2ClipTracer, 200, 12 ),
                             new WeaponComponentModel( Components.SnspistolMk2ClipIncendiary, 200, 8 ),
                             new WeaponComponentModel( Components.SnspistolMk2ClipHollowpoint, 200, 8 ),
                             new WeaponComponentModel( Components.SnspistolMk2ClipFmj, 200, 8 )
                            
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiRail02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SnspistolMk2Camo, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2CamoInd01, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2CamoSlide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo02Slide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo03Slide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo04Slide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo05Slide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo06Slide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo07Slide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo08Slide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo09Slide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2Camo10Slide, 200 ),
                             new WeaponComponentModel( Components.SnspistolMk2CamoInd01Slide, 200 )
                        }
                    }
                },
                MaxAmmo = 6

            });

            Config.Add(ItemNames.DoubleAction, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("mk2"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_doubleaction",
                AmmoType = ItemNames.PistolAmmo,
                Image = "doubleaction",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.DoubleAction),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 6

            });

            Config.Add(ItemNames.PistolMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_pistolmk2"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_pistolmk2",
                AmmoType = ItemNames.PistolAmmo,
                Image = "pistolmk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.PistolMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiSupp02, 200 ),
                             new WeaponComponentModel( Components.AtPiComp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiFlsh02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.PistolMk2Clip01, 50 ),
                             new WeaponComponentModel( Components.PistolMk2Clip02, 200 ),
                             new WeaponComponentModel( Components.PistolMk2ClipTracer, 200 ),
                             new WeaponComponentModel( Components.PistolMk2ClipIncendiary, 200 ),
                             new WeaponComponentModel( Components.PistolMk2ClipHollowpoint, 200 ),
                             new WeaponComponentModel( Components.PistolMk2ClipFmj, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiRail, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.PistolMk2Camo, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.PistolMk2CamoInd01, 200 ),
                             new WeaponComponentModel( Components.PistolMk2CamoSlide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo02Slide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo03Slide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo04Slide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo05Slide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo06Slide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo07Slide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo08Slide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo09Slide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2Camo10Slide, 200 ),
                             new WeaponComponentModel( Components.PistolMk2CamoInd01Slide, 200 )
                        }
                    }
                },
                MaxAmmo = 12

            });

            Config.Add(ItemNames.VintagePistol, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_vintage_pistol"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_vintage_pistol",
                AmmoType = ItemNames.PistolAmmo,
                Image = "vintagepistol",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.VintagePistol,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.VintagePistolClip01, 50, 7 ),
                             new WeaponComponentModel( Components.VintagePistolClip02, 200, 14 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 7

            });
            //SMG 
            Config.Add(ItemNames.MicroSMG, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sb_microsmg"),
                Weight = 3000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(1.0, 0.3, 0.9),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_microsmg",
                AmmoType = ItemNames.SMGAmmo,
                Image = "microsmg",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.MicroSMG,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.MicroSMGClip01, 50, 16 ),
                             new WeaponComponentModel( Components.MicroSMGClip02, 200, 30 )

                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeMacro, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.MicroSMGVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 16,
                ShopType = 5

            });

            Config.Add(ItemNames.SMG, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sb_smg"),
                Weight = 3500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(1.0, 0.5, 1.0),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_smg",
                AmmoType = ItemNames.SMGAmmo,
                Image = "smg",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.SMG,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SMGClip01, 50, 30 ),
                             new WeaponComponentModel( Components.SMGClip02, 200, 60 ),
                             new WeaponComponentModel( Components.SMGClip03, 200, 100 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeMacro02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SMGVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 30,
                ShopType = 5

            });

            Config.Add(ItemNames.AssaultSMG, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sb_assaultsmg"),
                Weight = 3500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(1.0, 0.5, 0.9),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_assaultsmg",
                AmmoType = ItemNames.SMGAmmo,
                Image = "assaultsmg",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.AssaultSMG),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AssaultSMGClip01, 50, 30 ),
                             new WeaponComponentModel( Components.AssaultSMGClip02, 200, 60 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeMacro, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AssaultSMGVarmodLowrider, 200 )
                        }
                    }
                },
                MaxAmmo = 30,
                ShopType = 5

            });

            Config.Add(ItemNames.MiniSMG, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sb_minismg"),
                Weight = 3000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(1.0, 0.3, 0.7),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_minismg",
                AmmoType = ItemNames.SMGAmmo,
                Image = "minismg",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.MiniSMG),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.MinismgClip01, 50, 20 ),
                             new WeaponComponentModel( Components.MinismgClip02, 200, 30 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 20,
                ShopType = 5

            });

            Config.Add(ItemNames.SMGMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sb_smgmk2"),
                Weight = 3500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_smgmk2",
                AmmoType = ItemNames.SMGAmmo,
                Image = "smgmk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.SMGMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiSupp, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle01, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle02, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle03, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle04, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle05, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle06, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle07, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SmgMk2Clip01, 50, 30 ),
                             new WeaponComponentModel( Components.SmgMk2Clip02, 200, 60 ),
                             new WeaponComponentModel( Components.SmgMk2ClipTracer, 200, 30 ),
                             new WeaponComponentModel( Components.SmgMk2ClipIncendiary, 200, 20 ),
                             new WeaponComponentModel( Components.SmgMk2ClipHollowpoint, 200, 20 ),
                             new WeaponComponentModel( Components.SmgMk2ClipFmj, 200, 20 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSightsSmg, 200 ),
                             new WeaponComponentModel( Components.AtScopeMacro02SmgMk2, 200 ),
                             new WeaponComponentModel( Components.AtScopeSmallSmgMk2, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SmgMk2Camo, 200 ),
                             new WeaponComponentModel( Components.SmgMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.SmgMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.SmgMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.SmgMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.SmgMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.SmgMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.SmgMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.SmgMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.SmgMk2Camo10, 200 )
                        }
                    }
                },
                MaxAmmo = 30

            });

            Config.Add(ItemNames.MachinePistol, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sb_compactsmg"),
                Weight = 2500,//Вес в граммах
                Characteristics = new WeaponCharacteristics(1.0, 0.4, 0.9),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_compactsmg",
                AmmoType = ItemNames.SMGAmmo,
                Image = "machinegunpistol",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.MachinePistol),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtPiSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.MachinePistolClip01, 50, 12 ),
                             new WeaponComponentModel( Components.MachinePistolClip02, 200, 20 ),
                             new WeaponComponentModel( Components.MachinePistolClip03, 200, 30 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 12,
                ShopType = 5

            });

            Config.Add(ItemNames.CombatPDW, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sb_pdw"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.8, 0.4, 1.0),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_pdw",
                AmmoType = ItemNames.SMGAmmo,
                Image = "combatpdw",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.CombatPDW,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CombatPDWClip01, 50, 30 ),
                             new WeaponComponentModel( Components.CombatPDWClip02, 200, 60 ),
                             new WeaponComponentModel( Components.CombatPDWClip03, 200, 100 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeSmall, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 30,
                ShopType = 5

            });
            //Machine Guns
            Config.Add(ItemNames.MG, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_mg_mg"),
                Weight = 6000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_mg",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "mg",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.96),
                WeaponHash = unchecked((uint)Weapons.Hash.MG),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                         new WeaponComponentModel( Components.MGClip01, 50, 54 ),
                         new WeaponComponentModel( Components.MGClip02, 200, 100 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeSmall02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.MGVarmodLowrider, 200 )
                        }
                    }
                },
                MaxAmmo = 54

            });

            Config.Add(ItemNames.CombatMG, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_mg_combatmg"),
                Weight = 7000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.7, 0.9, 0.6),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_combatmg",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "combatmg",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = (uint)Weapons.Hash.CombatMG,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CombatMGClip01, 50, 100 ),
                             new WeaponComponentModel( Components.CombatMGClip02, 200, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeMedium, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CombatMGVarmodLowrider, 200 )
                        }
                    }
                },
                MaxAmmo = 100,
                ShopType = 6

            });

            Config.Add(ItemNames.CombatMGMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_mg_combatmgmk2"),
                Weight = 7000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_combatmgmk2",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "combatmgmk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = unchecked((uint)Weapons.Hash.CombatMGMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtMuzzle01, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle02, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle03, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle04, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle05, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle06, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle07, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CombatmgMk2Clip01, 50, 100 ),
                             new WeaponComponentModel( Components.CombatmgMk2Clip02, 200, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2ClipTracer, 200, 100 ),
                             new WeaponComponentModel( Components.CombatmgMk2ClipIncendiary, 200, 80 ),
                             new WeaponComponentModel( Components.CombatmgMk2ClipArmorpiercing, 200, 80 ),
                             new WeaponComponentModel( Components.CombatmgMk2ClipFmj, 200, 80 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSights, 200 ),
                             new WeaponComponentModel( Components.AtScopeSmallMk2, 200 ),
                             new WeaponComponentModel( Components.AtScopeMediumMk2, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfgrip02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CombatmgMk2Camo, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.CombatmgMk2CamoInd01, 200 )
                        }
                    }
                },
                MaxAmmo = 100
            });

            Config.Add(ItemNames.Gusenberg, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sb_gusenberg"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.9, 0.6, 0.9),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_gusenberg",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "gusenberg",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = (uint)Weapons.Hash.Gusenberg,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.GusenbergClip01, 50, 30 ),
                             new WeaponComponentModel( Components.GusenbergClip02, 200, 50 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 30,
                ShopType = 6

            });
            //Shotguns
            Config.Add(ItemNames.PumpShotgun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sg_pumpshotgun"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.3, 0.6, 0.2),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_pumpshotgun",
                AmmoType = ItemNames.ShotgunsAmmo,
                Image = "pumpshotgun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = (uint)Weapons.Hash.PumpShotgun,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSrSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.PumpShotgunVarmodLowrider, 200 )
                        }
                    }
                },
                MaxAmmo = 8,
                ShopType = 4

            });

            Config.Add(ItemNames.SawnOffShotgun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sg_sawnoff"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_sawnoff",
                AmmoType = ItemNames.ShotgunsAmmo,
                Image = "sawnoffshotgun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = (uint)Weapons.Hash.SawnOffShotgun,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                        new List<WeaponComponentModel>{
                            new WeaponComponentModel(Components.SawnoffShotgunVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 8

            });

            Config.Add(ItemNames.AssaultShotgun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sg_assaultshotgun"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.8, 0.6, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_assaultshotgun",
                AmmoType = ItemNames.ShotgunsAmmo,
                Image = "assaultshotgun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = unchecked((uint)Weapons.Hash.AssaultShotgun),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AssaultShotgunClip01, 50, 8 ),
                             new WeaponComponentModel( Components.AssaultShotgunClip02, 200, 32 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 8,
                ShopType = 4

            });

            Config.Add(ItemNames.Musket, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_musket"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_musket",
                AmmoType = ItemNames.MusketAmmo,
                Image = "musket",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Musket),
                MaxAmmo = 1
            });

            Config.Add(ItemNames.BullpupShotgun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sg_bullpupshotgun"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_bullpupshotgun",
                AmmoType = ItemNames.ShotgunsAmmo,
                Image = "bullpupshotgun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = unchecked((uint)Weapons.Hash.BullpupShotgun),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 14

            });

            Config.Add(ItemNames.PumpShotgunMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sg_pumpshotgunmk2"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_pumpshotgunmk2",
                AmmoType = ItemNames.ShotgunsAmmo,
                Image = "pumpshotgunmk2",
                CanUse = false,
                WeaponHash = (uint)Weapons.Hash.PumpShotgunMk2,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSrSupp03, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle08, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.PumpshotgunMk2Clip01, 50 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2ClipIncendiary, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2ClipArmorpiercing, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2ClipHollowpoint, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2ClipExplosive, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSights, 200 ),
                             new WeaponComponentModel( Components.AtScopeMacroMk2, 200 ),
                             new WeaponComponentModel( Components.AtScopeSmallMk2, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.PumpshotgunMk2CamoInd01, 200 )
                        }
                    }
                },
                MaxAmmo = 8

            });

            Config.Add(ItemNames.DoubleBarrelShotgun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sg_doublebarrel"),
                Weight = 5000,
                Characteristics = new WeaponCharacteristics(0.1, 0.9, 0.1),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_doublebarrelshotgun",
                AmmoType = ItemNames.ShotgunsAmmo,
                Image = "doublebarrelshotgun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = unchecked((uint)Weapons.Hash.DoubleBarrelShotgun),
                MaxAmmo = 2,
                ShopType = 4
            });

            Config.Add(ItemNames.SweeperShotgun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("mk2"),
                Weight = 5000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_sweepershotgun",
                AmmoType = ItemNames.ShotgunsAmmo,
                Image = "sweepershotgun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = (uint)Weapons.Hash.SweeperShotgun,
                MaxAmmo = 10
            });

            Config.Add(ItemNames.HeavyShotgun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sg_heavyshotgun"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.8, 0.8, 0.4),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_heavyshotgun",
                AmmoType = ItemNames.ShotgunsAmmo,
                Image = "heavyshotgun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = (uint)Weapons.Hash.HeavyShotgun,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.HeavyShotgunClip01, 50, 6 ),
                             new WeaponComponentModel( Components.HeavyShotgunClip02, 200, 12 ),
                             new WeaponComponentModel( Components.HeavyShotgunClip03, 200, 30 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 6,
                ShopType = 4
            });
            //Rifles
            Config.Add(ItemNames.AssaultRifle, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_assaultrifle"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.8, 0.6, 0.8),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_assaultrifle",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "assaultrifle",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.AssaultRifle),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AssaultRifleClip01, 50, 30 ),
                             new WeaponComponentModel( Components.AssaultRifleClip02, 200, 60 ),
                             new WeaponComponentModel( Components.AssaultRifleClip03, 200, 100 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeMacro, 200 )

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AssaultRifleVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 30,
                ShopType = 6
            });

            Config.Add(ItemNames.CarbineRifle, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_carbinerifle"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(1.0, 0.6, 1.0),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_carbinerifle",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "carbinerifle",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.CarbineRifle),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CarbineRifleClip01, 50, 30 ),
                             new WeaponComponentModel( Components.CarbineRifleClip02, 200, 60 ),
                             new WeaponComponentModel( Components.CarbineRifleClip03, 200, 100 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeMedium, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CarbineRifleVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 30,
                ShopType = 6
            });

            Config.Add(ItemNames.AdvancedRifle, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_advancedrifle"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(1.0, 0.6, 0.9),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_advancedrifle",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "advancedrifle",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.AdvancedRifle),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AdvancedRifleClip01, 50, 30 ),
                             new WeaponComponentModel( Components.AdvancedRifleClip02, 200, 60 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeSmall, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AdvancedRifleVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 30,
                ShopType = 6
            });

            Config.Add(ItemNames.SpecialCarbine, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_specialcarbine"),
                Weight = 5000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_specialcarbine",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "specialcarbine",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.SpecialCarbine),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SpecialCarbineClip01, 50, 30 ),
                             new WeaponComponentModel( Components.SpecialCarbineClip02, 200, 60 ),
                             new WeaponComponentModel( Components.SpecialCarbineClip03, 200, 100 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeMedium, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SpecialCarbineVarmodLowrider, 200 )
                        }
                    }
                },
                MaxAmmo = 30
            });

            Config.Add(ItemNames.BullpupRifle, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_bullpuprifle"),
                Weight = 5000,
                Characteristics = new WeaponCharacteristics(0.9, 0.6, 0.8),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_bullpuprifle",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "bullpuprifle",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.BullpupRifle,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.BullpupRifleClip01, 50, 30 ),
                             new WeaponComponentModel( Components.BullpupRifleClip02, 200, 60 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeSmall, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.BullpupRifleVarmodLow, 200 )
                        }
                    }
                },
                MaxAmmo =30,
                ShopType = 6
            });

            Config.Add(ItemNames.BullpupRifleMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_bullpupriflemk2"),
                Weight = 5000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_bullpupriflemk2",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "bullpupriflemk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.BullpupRifleMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle01, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle02, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle03, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle04, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle05, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle06, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle07, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.BullpuprifleMk2Clip01, 50 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Clip02, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2ClipTracer, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2ClipIncendiary, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2ClipArmorpiercing, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2ClipFmj, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSights, 200 ),
                             new WeaponComponentModel( Components.AtScopeMacro02Mk2, 200 ),
                             new WeaponComponentModel( Components.AtScopeSmallMk2, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfgrip02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.BullpuprifleMk2CamoInd01, 200 )
                        }
                    }
                },
                MaxAmmo = 30
            });

            Config.Add(ItemNames.SpecialCarbineMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_specialcarbinemk2"),
                Weight = 5000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_specialcarbinemk2",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "specialcarbinemk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.SpecialCarbineMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle01, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle02, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle03, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle04, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle05, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle06, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle07, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SpecialcarbineMk2Clip01, 50, 6 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Clip02, 200, 6 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2ClipTracer, 200, 6  ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2ClipIncendiary, 200, 6  ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2ClipArmorpiercing, 200, 6  ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2ClipFmj, 200, 6  )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSights, 200 ),
                             new WeaponComponentModel( Components.AtScopeMacroMk2, 200 ),
                             new WeaponComponentModel( Components.AtScopeMediumMk2, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfgrip02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.SpecialcarbineMk2CamoInd01, 200 ) 
                        }
                    }
                },
                MaxAmmo = 30
            });

            Config.Add(ItemNames.AssaultRifleMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_assaultriflemk2"),
                Weight = 5000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_assaultriflemk2",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "assaultriflemk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.AssaultRifleMk2,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle01, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle02, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle03, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle04, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle05, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle06, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle07, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AssaultrifleMk2Clip01, 50, 30 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Clip02, 200, 60 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2ClipTracer, 200, 30 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2ClipIncendiary, 200, 20 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2ClipArmorpiercing, 200, 20 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2ClipFmj, 200, 20 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSights, 200 ),
                             new WeaponComponentModel( Components.AtScopeMacroMk2, 200 ),
                             new WeaponComponentModel( Components.AtScopeMediumMk2, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfgrip02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.AssaultrifleMk2CamoInd01, 200 )
                        }
                    }
                },
                MaxAmmo = 30
            });

            Config.Add(ItemNames.CarbineRifleMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_carbineriflemk2"),
                Weight = 5000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_carbineriflemk2",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "carbineriflemk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.CarbineRifleMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle01, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle02, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle03, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle04, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle05, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle06, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle07, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CarbinerifleMk2Clip01, 50, 30 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Clip02, 200, 60 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2ClipTracer, 200, 30 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2ClipIncendiary, 200, 20 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2ClipArmorpiercing, 200, 20 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2ClipFmj, 200, 20 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSights, 200 ),
                             new WeaponComponentModel( Components.AtScopeMacroMk2, 200 ),
                             new WeaponComponentModel( Components.AtScopeMediumMk2, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfgrip02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.CarbinerifleMk2CamoInd01, 200 )
                        }
                    }
                },
                MaxAmmo = 30,
                ShopType = 6
            });

            Config.Add(ItemNames.CompactRifle, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_assaultrifle_smg"),
                Weight = 5000,
                Characteristics = new WeaponCharacteristics(0.8, 0.6, 0.7),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_compactrifle",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "compactrifle",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.CompactRifle,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.CompactRifleClip01, 50, 30 ),
                             new WeaponComponentModel( Components.CompactRifleClip02, 200, 60 ),

                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 30,
                ShopType = 6
            });
            //Sniper Rifles
            Config.Add(ItemNames.SniperRifle, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sr_sniperrifle"),
                Weight = 7000,
                Characteristics = new WeaponCharacteristics(0.3, 1.0, 1.0),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_sniperrifle",
                AmmoType = ItemNames.SniperAmmo,
                Image = "sniperrifle",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = (uint)Weapons.Hash.SniperRifle,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SniperRifleClip01, 50, 10 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeLarge, 200 ),
                             new WeaponComponentModel( Components.AtScopeMax, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.SniperRifleVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 10,
                ShopType = 3
            });

            Config.Add(ItemNames.HeavySniper, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sr_heavysniper"),
                Weight = 7000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_heavysniper",
                AmmoType = ItemNames.SniperAmmo,
                Image = "heavysniper",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.HeavySniper,
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.HeavySniperClip01, 50, 6 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeLarge, 200 ),
                             new WeaponComponentModel( Components.AtScopeMax, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{

                        }
                    }
                },
                MaxAmmo = 6,
                ShopType = 3
            });

            Config.Add(ItemNames.MarksmanRifleMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sr_marksmanriflemk2"),
                Weight = 6000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_marksmanriflemk2",
                AmmoType = ItemNames.SniperAmmo,
                Image = "marksmanriflemk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.MarksmanRifleMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle01, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle02, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle03, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle04, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle05, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle06, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle07, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.MarksmanrifleMk2Clip01, 50 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Clip02, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2ClipTracer, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2ClipIncendiary, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2ClipArmorpiercing, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2ClipFmj, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSights, 200 ),
                             new WeaponComponentModel( Components.AtScopeMediumMk2, 200 ),
                             new WeaponComponentModel( Components.AtScopeLargeFixedZoomMk2, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfgrip02, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.MarksmanrifleMk2CamoInd01, 200 )
                        }
                    }
                },
                MaxAmmo = 8
            });

            Config.Add(ItemNames.HeavySniperMk2, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sr_heavysnipermk2"),
                Weight = 7000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_heavysnipermk2",
                AmmoType = ItemNames.SniperAmmo,
                Image = "heavysnipermk2",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = unchecked((uint)Weapons.Hash.HeavySniperMk2),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtSrSupp03, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle08, 200 ),
                             new WeaponComponentModel( Components.AtMuzzle09, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.HeavysniperMk2Clip01, 50, 6 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Clip02, 200, 8 ),
                             new WeaponComponentModel( Components.HeavysniperMk2ClipIncendiary, 200, 4 ),
                             new WeaponComponentModel( Components.HeavysniperMk2ClipArmorpiercing, 200, 4 ),
                             new WeaponComponentModel( Components.HeavysniperMk2ClipFmj, 200, 4 ),
                             new WeaponComponentModel( Components.HeavysniperMk2ClipExplosive, 200, 4 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeLargeMk2, 200 ),
                             new WeaponComponentModel( Components.AtScopeMax, 200 ),
                             new WeaponComponentModel( Components.AtScopeNv, 200 ),
                             new WeaponComponentModel( Components.AtScopeThermal, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{

                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.HeavysniperMk2Camo, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Camo02, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Camo03, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Camo04, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Camo05, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Camo06, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Camo07, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Camo08, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Camo09, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2Camo10, 200 ),
                             new WeaponComponentModel( Components.HeavysniperMk2CamoInd01, 200 )
                        }
                    }
                },
                MaxAmmo = 6,
                ShopType = 3
            });

            Config.Add(ItemNames.MarksmanRifle, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_sr_marksmanrifle"),
                Weight = 6000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_marksmanrifle",
                AmmoType = ItemNames.SniperAmmo,
                Image = "marksmanrifle",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.MarksmanRifle),
                Components = new Dictionary<WeaponComponentSlots, List<WeaponComponentModel>>
                {
                    {
                        WeaponComponentSlots.Muzzle,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArSupp, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.FlashLight,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArFlsh, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Clip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.MarksmanRifleClip01, 0, 8 ),
                             new WeaponComponentModel( Components.MarksmanRifleClip02, 200, 16 )
                        }
                    },
                    {
                        WeaponComponentSlots.Scope,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtScopeLargeFixedZoom, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Grip,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.AtArAfGrip, 200 )
                        }
                    },
                    {
                        WeaponComponentSlots.Skin,
                       new List<WeaponComponentModel>{
                             new WeaponComponentModel( Components.MarksmanRifleVarmodLuxe, 200 )
                        }
                    }
                },
                MaxAmmo = 8
            });
            //Melee
            Config.Add(ItemNames.Knife, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_knife_01"),
                Weight = 1000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_knife",
                //AmmoType = ItemNames.Invalid,
                Image = "knife",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                WeaponHash = unchecked((uint)Weapons.Hash.Knife),
            });

            Config.Add(ItemNames.Nightstick, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_nightstick"),
                Weight = 1000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_nightstick",
                //AmmoType = ItemNames.Invalid,
                Image = "nightstick",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.Nightstick,
            });

            Config.Add(ItemNames.Hammer, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_hammer"),
                Weight = 1500,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_hammer",
                //AmmoType = ItemNames.Invalid,
                Image = "hammer",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.Hammer,
            });

            Config.Add(ItemNames.Bat, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_bat"),
                Weight = 1500,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_bat",
                //AmmoType = ItemNames.Invalid,
                Image = "bat",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Bat),
            });

            Config.Add(ItemNames.Crowbar, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_crowbar"),
                Weight = 1500,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_crowbar",
                //AmmoType = ItemNames.Invalid,
                Image = "crowbar",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Crowbar),
            });

            Config.Add(ItemNames.GolfClub, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_gclub"),
                Weight = 1500,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_gclub",
                //AmmoType = ItemNames.Invalid,
                Image = "gclub",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.GolfClub,
            });

            Config.Add(ItemNames.Bottle, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_bottle"),
                Weight = 1000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_bottle",
                //AmmoType = ItemNames.Invalid,
                Image = "bottle",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Bottle),
            });

            Config.Add(ItemNames.Dagger, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_dagger"),
                Weight = 1000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_dagger",
                //AmmoType = ItemNames.Invalid,
                Image = "dagger",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                WeaponHash = unchecked((uint)Weapons.Hash.Dagger),
            });

            Config.Add(ItemNames.Hatchet, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_hatchet"),
                Weight = 2000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_hatchet",
                //AmmoType = ItemNames.Invalid,
                Image = "hatchet",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Hatchet),
            });

            Config.Add(ItemNames.KnuckleDuster, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_knuckle"),
                Weight = 1000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_knuckle",
                //AmmoType = ItemNames.Invalid,
                Image = "knuckle",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.KnuckleDuster),
            });

            Config.Add(ItemNames.Machete, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("prop_ld_w_me_machette"),
                Weight = 1500,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_machette",
                //AmmoType = ItemNames.Invalid,
                Image = "machette",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Machete),
            });

            Config.Add(ItemNames.Flashlight, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_flashlight"),
                Weight = 1000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_flashlight",
                //AmmoType = ItemNames.Invalid,
                Image = "flashlight",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Flashlight),
            });

            Config.Add(ItemNames.SwitchBlade, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_switchblade"),
                Weight = 1000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_switchblade",
                //AmmoType = ItemNames.Invalid,
                Image = "switchblade",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.SwitchBlade),
            });

            Config.Add(ItemNames.PoolCue, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("prop_pool_cue"),
                Weight = 1500,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_poolcue",
                //AmmoType = ItemNames.Invalid,
                Image = "poolcue",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.PoolCue),
            });

            Config.Add(ItemNames.Wrench, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("prop_cs_wrench"),
                Weight = 1500,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_cs_wrench",
                //AmmoType = ItemNames.Invalid,
                Image = "wrench",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = (uint)Weapons.Hash.Wrench,
            });

            Config.Add(ItemNames.BattleAxe, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_me_battleaxe"),
                Weight = 2000,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_battleaxe",
                //AmmoType = ItemNames.Invalid,
                Image = "battleaxe",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.BattleAxe),
            });
            //Heavy
            Config.Add(ItemNames.GrenadeLauncher, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_lr_grenadelauncher"),
                Weight = 10000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_grenadelauncher",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "grenadelauncher",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.92),
                WeaponHash = unchecked((uint)Weapons.Hash.GrenadeLauncher),
                MaxAmmo = 10

            });

            Config.Add(ItemNames.RPG, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_lr_rpg"),
                Weight = 10000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_rpg",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "rpg",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.92),
                WeaponHash = unchecked((uint)Weapons.Hash.RPG),
                MaxAmmo = 1

            });

            Config.Add(ItemNames.Minigun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_mg_minigun"),
                Weight = 10000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_minigun",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "minigun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95),
                WeaponHash = unchecked((uint)Weapons.Hash.Minigun),
                MaxAmmo = 15000
            });

            Config.Add(ItemNames.Firework, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_lr_firework"),
                Weight = 10000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_firework",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "firework",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.85),
                WeaponHash = unchecked((uint)Weapons.Hash.Firework),
                MaxAmmo = 1
            });

            Config.Add(ItemNames.Railgun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ar_railgun"),
                Weight = 10000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_railgun",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "railgun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.92),
                WeaponHash = unchecked((uint)Weapons.Hash.Railgun),
                MaxAmmo = 1
            });

            Config.Add(ItemNames.HomingLauncher, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_lr_firework"),
                Weight = 10000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_hominglauncher",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "hominglauncher",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.HomingLauncher),
                MaxAmmo = 1
            });

            Config.Add(ItemNames.GrenadeLauncherSmoke, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_lr_grenadelauncher"),
                Weight = 10000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon1, WeaponSlots.Weapon2 },
                DisplayName = "weapon_grenadelauncher_smoke",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "grenadelaunchersmoke",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.91),
                WeaponHash = unchecked((uint)Weapons.Hash.GrenadeLauncherSmoke),
                MaxAmmo = 10
            });

            Config.Add(ItemNames.CompactGrenadeLauncher, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_flaregun"),
                Weight = 3000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_compactlauncher",
                AmmoType = ItemNames.RiflesAmmo,
                Image = "compactgrenadelauncher",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.93),
                WeaponHash = unchecked((uint)Weapons.Hash.CompactGrenadeLauncher),
                MaxAmmo = 1
            });
            // No Class ???
            Config.Add(ItemNames.StunGun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_stungun"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_stungun",
                AmmoType = ItemNames.PistolAmmo,
                Image = "stungun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                WeaponHash = unchecked((uint)Weapons.Hash.StunGun),
                MaxAmmo = 1
            });

            Config.Add(ItemNames.FlareGun, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_pi_flaregun"),
                Weight = 5000,//Вес в граммах
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "weapon_flaregun",
                AmmoType = ItemNames.PistolAmmo,
                Image = "flaregun",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.FlareGun),
                MaxAmmo = 1
            });

            Config.Add(ItemNames.Snowballs, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_ex_snowball"),
                Weight = 250,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "Снежный мяч",
                AmmoType = ItemNames.ThrowablesAmmo,
                Image = "snowball",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Snowball),
                MaxAmmo = 1
            });

            Config.Add(ItemNames.Ball, new WeaponConfig
            {
                Type = ItemTypes.Weapon,
                ModelHash = NAPI.Util.GetHashKey("w_am_baseball"),
                Weight = 500,
                Characteristics = new WeaponCharacteristics(0.5, 0.5, 0.5),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = false,
                Slots = new List<WeaponSlots> { WeaponSlots.Weapon3, WeaponSlots.Weapon4 },
                DisplayName = "Бейсбольный мяч",
                AmmoType = ItemNames.ThrowablesAmmo,
                Image = "ball",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.98),
                WeaponHash = unchecked((uint)Weapons.Hash.Ball),
                MaxAmmo = 1
            });
        }

    }
}
