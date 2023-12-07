using GTANetworkAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading;
using Whistler.SDK;
using System.IO;
using Whistler.Helpers;
using Whistler.Inventory.Enums;
using Whistler.Inventory;
using Whistler.Inventory.Models;
using Whistler.Fractions;
using Whistler.Fractions.SupplyManagers;
using Whistler.Entities;

namespace Whistler.Core
{
    class Weapons : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Weapons));

        internal enum Hash
        {
            /* Handguns */
            Knife = -1716189206,
            Nightstick = 1737195953,
            Hammer = 1317494643,
            Bat = -1786099057,
            Crowbar = -2067956739,
            GolfClub = 1141786504,
            Bottle = -102323637,
            Dagger = -1834847097,
            Hatchet = -102973651,
            KnuckleDuster = -656458692,
            Machete = -581044007,
            Flashlight = -1951375401,
            SwitchBlade = -538741184,
            PoolCue = -1810795771,
            Wrench = 419712736,
            BattleAxe = -853065399,
            /* Pistols */
            Pistol = 453432689,
            CombatPistol = 1593441988,
            Pistol50 = -1716589765,
            SNSPistol = -1076751822,
            HeavyPistol = -771403250,
            VintagePistol = 137902532,
            MarksmanPistol = -598887786,
            Revolver = -1045183535,
            APPistol = 584646201,
            StunGun = 911657153,
            FlareGun = 1198879012,
            DoubleAction = -1746263880,
            PistolMk2 = -1075685676,
            SNSPistolMk2 = -2009644972,
            RevolverMk2 = -879347409,
            /* SMG */
            MicroSMG = 324215364,
            MachinePistol = -619010992,
            SMG = 736523883,
            AssaultSMG = -270015777,
            CombatPDW = 171789620,
            MG = -1660422300,
            CombatMG = 2144741730,
            Gusenberg = 1627465347,
            MiniSMG = -1121678507,
            SMGMk2 = 2024373456,
            CombatMGMk2 = -608341376,
            /* Rifles */
            AssaultRifle = -1074790547,
            CarbineRifle = -2084633992,
            AdvancedRifle = -1357824103,
            SpecialCarbine = -1063057011,
            BullpupRifle = 2132975508,
            CompactRifle = 1649403952,
            AssaultRifleMk2 = 961495388,
            CarbineRifleMk2 = -86904375,
            SpecialCarbineMk2 = -1768145561,
            BullpupRifleMk2 = -2066285827,
            /* Sniper */
            SniperRifle = 100416529,
            HeavySniper = 205991906,
            MarksmanRifle = -952879014,
            HeavySniperMk2 = 177293209,
            MarksmanRifleMk2 = 1785463520,
            /* Shotguns */
            PumpShotgun = 487013001,
            SawnOffShotgun = 2017895192,
            BullpupShotgun = -1654528753,
            AssaultShotgun = -494615257,
            Musket = -1466123874,
            HeavyShotgun = 984333226,
            DoubleBarrelShotgun = -275439685,
            SweeperShotgun = 317205821,
            PumpShotgunMk2 = 1432025498,
            /* Heavy */
            GrenadeLauncher = -1568386805,
            RPG = -1312131151,
            Minigun = 1119849093,
            Firework = 2138347493,
            Railgun = 1834241177,
            HomingLauncher = 1672152130,
            GrenadeLauncherSmoke = 1305664598,
            CompactGrenadeLauncher = 125959754,
            /* Throwables & Misc */
            Grenade = -1813897027,
            StickyBomb = 741814745,
            ProximityMine = -1420407917,
            BZGas = -1600701090,
            Molotov = 615608432,
            FireExtinguisher = 101631238,
            PetrolCan = 883325847,
            Flare = 1233104067,
            Ball = 600439132,
            Snowball = 126349499,
            SmokeGrenade = -37975472,
            PipeBomb = -1169823560,
            Parachute = 615608432,

            /* Armor */
            BodyArmor = 10000,
        }

        private static List<ItemNames> _safeWeapons = new List<ItemNames>
        {
            ItemNames.Knife,
            ItemNames.Musket,
        };

        private static int _swapDropTime = 15;

        [ServerEvent(Event.ResourceStart)]
        public void Event_ResourceStart()
        {
            try
            {
                InventoryService.OnUseItemBox += OnUseItemBox;
                InventoryEvents.ItemInteract += PickupItemBox;
                //ArmedBody.ParseConfig();
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        private static bool CheckSafeWeapon(ItemNames item)
        {
            return _safeWeapons.Contains(item);
        }
       
        public static void Event_PlayerDeath(ExtPlayer player, ExtPlayer killer, uint reason)
        {
            try
            {
                if (!player.IsLogged()) return;

                AntiCheatServices.AntiCheatService.KillPlayerHandle(player, killer, reason);

                if (player.IsAdmin()) return;
                var equip = player.GetEquip();
                if (equip.CurrentWeapon != WeaponSlots.Invalid)
                {
                    if (!equip.Weapons[equip.CurrentWeapon].Promo && !CheckSafeWeapon(equip.Weapons[equip.CurrentWeapon].Name))
                    {
                        var item = equip.RemoveItem(player, equip.CurrentWeapon, LogAction.None);
                        DropSystem.DropItem(item, player.Position, player.Dimension);
                        GameLog.ItemsLog(item.Id, $"e{equip.Id}", "0", item.Name, item.Count, item.GetItemLogData(), LogAction.Move);
                    }
                }
                if (equip.LastWeapon != WeaponSlots.Invalid && equip.Weapons.ContainsKey(equip.LastWeapon) && equip.Weapons[equip.LastWeapon] != null && (DateTime.Now - equip.LastSwap).TotalSeconds < _swapDropTime)
                {
                    if (!equip.Weapons[equip.LastWeapon].Promo && !CheckSafeWeapon(equip.Weapons[equip.LastWeapon].Name))
                    {
                        var item = equip.RemoveItem(player, equip.LastWeapon, LogAction.None);
                        DropSystem.DropItem(item, player.Position, player.Dimension);
                        GameLog.ItemsLog(item.Id, $"e{equip.Id}", "0", item.Name, item.Count, item.GetItemLogData(), LogAction.Move);
                    }
                }
            }
            catch (Exception e) { _logger.WriteError("PlayerDeath: " + e.ToString()); }
        }

        public static void OnUseItemBox(ExtPlayer player, ItemBox box)
        {
            if (box.CountStoredItem <= 0)
                return;
            BaseItem additem = null;
            switch (box.Name)
            {
                case ItemNames.WeaponBox:
                    additem = ItemsFabric.CreateWeapon(box.StoredName, false);
                    break;
                case ItemNames.AmmoBox:
                    additem = ItemsFabric.CreateAmmo(box.StoredName, box.Config.CountGetItem, false);
                    break;
                case ItemNames.ArmorBox:
                    additem = ItemsFabric.CreateClothes(box.StoredName, player.GetGender(), 0, 0, false);
                    break;
                case ItemNames.MedkitBox:
                    additem = ItemsFabric.CreateMedicaments(box.StoredName, box.Config.CountGetItem, false);
                    break;
            }
            if (additem == null)
                return;
            if (player.GetInventory().AddItem(additem))
            {
                box.CountStoredItem--;
                GameLog.Box(box.Serial, player.Character.UUID, additem.Name.ToString());
            }
        }

        public static void PickupItemBox(ExtPlayer player, int itemId, ItemTypes itemType)
        {
            if (itemType != ItemTypes.ItemBox)
                return;

            var itemFrom = DropSystem.PickupItem(player, itemId, 1);
            var box = itemFrom as ItemBox;
            player.GiveContainerToPlayer(box, box.Config.AttachId);
        }

        public static void RemoveAll(ExtPlayer player, bool ammo)
        {
            if (!player.IsLogged()) return;
            if (player.IsAdmin()) return;
            player.GetInventory().RemoveItems(item => (item.Type == ItemTypes.Weapon && !CheckSafeWeapon(item.Name) || item.Type == ItemTypes.Ammo && ammo) && !item.Promo);
            var equip = player.GetEquip();
            if (equip.Weapons[WeaponSlots.Weapon1] != null && !equip.Weapons[WeaponSlots.Weapon1].Promo && !CheckSafeWeapon(equip.Weapons[WeaponSlots.Weapon1].Name))
                equip.RemoveItem(player, WeaponSlots.Weapon1);
            if (equip.Weapons[WeaponSlots.Weapon2] != null && !equip.Weapons[WeaponSlots.Weapon2].Promo && !CheckSafeWeapon(equip.Weapons[WeaponSlots.Weapon2].Name))
                equip.RemoveItem(player, WeaponSlots.Weapon2);
            if (equip.Weapons[WeaponSlots.Weapon3] != null && !equip.Weapons[WeaponSlots.Weapon3].Promo && !CheckSafeWeapon(equip.Weapons[WeaponSlots.Weapon3].Name))
                equip.RemoveItem(player, WeaponSlots.Weapon3);
            if (equip.Weapons[WeaponSlots.Weapon4] != null && !equip.Weapons[WeaponSlots.Weapon4].Promo && !CheckSafeWeapon(equip.Weapons[WeaponSlots.Weapon4].Name))
                equip.RemoveItem(player, WeaponSlots.Weapon4);
        }

        public static void OpenFriskMenu(ExtPlayer player, ExtPlayer target)
        {
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок далеко", 3000);
                return;
            }
            if (target.IsAdmin())
                return;
            if (!Fractions.Manager.CanUseCommand(player, "takeguns")) return;
            if (!target.Character.Cuffed)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не в наручниках", 3000);
                return;
            }
            var list = target.GetInventory().Items
                .Where(item => item.Type == ItemTypes.Weapon && !CheckSafeWeapon(item.Name) || item.Type == ItemTypes.Ammo || item.Type == ItemTypes.Narcotic)
                .Select(item => new { id = (int)item.Name, count = item.Count })
                .ToList();
            var equip = target.GetEquip();
            if (equip.Weapons[WeaponSlots.Weapon1] != null && !CheckSafeWeapon(equip.Weapons[WeaponSlots.Weapon1].Name))
                list.Add(new { id = (int)equip.Weapons[WeaponSlots.Weapon1].Name, count = 1 });
            if (equip.Weapons[WeaponSlots.Weapon2] != null && !CheckSafeWeapon(equip.Weapons[WeaponSlots.Weapon2].Name))
                list.Add(new { id = (int)equip.Weapons[WeaponSlots.Weapon2].Name, count = 1 });
            if (equip.Weapons[WeaponSlots.Weapon3] != null && !CheckSafeWeapon(equip.Weapons[WeaponSlots.Weapon3].Name))
                list.Add(new { id = (int)equip.Weapons[WeaponSlots.Weapon3].Name, count = 1 });
            if (equip.Weapons[WeaponSlots.Weapon4] != null && !CheckSafeWeapon(equip.Weapons[WeaponSlots.Weapon4].Name))
                list.Add(new { id = (int)equip.Weapons[WeaponSlots.Weapon4].Name, count = 1 });
            SafeTrigger.SetData(player, "frisktarget", target);
            SafeTrigger.ClientEvent(player,"friskInterface:openmenu", JsonConvert.SerializeObject(list), target.Name, target.Character.UUID);
        }
        //private const int MusketShotsStrength = 200;
        //[RemoteEvent("weapons:musketShot")]
        //public static void HandleMusketShot(ExtPlayer player)
        //{
        //    try
        //    {
        //        if (!player.IsLogged()) return;
        //        var musket = nInventory.Find(player.GetUuid(), ItemType.Musket);
        //        if (musket == null) return;

        //        if ((string)musket.Data == "R25C6D2V0") return;

        //        var data = ((string)musket.Data).Split('_');

        //        // data[1] - прочность оружия
        //        if (data.Length == 1)
        //        {
        //            Array.Resize(ref data, data.Length + 1);
        //            data[1] = $"{MusketShotsStrength - 1}";
        //        }
        //        else
        //        {
        //            var shotsLeft = int.Parse(data[1]);
        //            data[1] = $"{--shotsLeft}";

        //            if (shotsLeft == 0)
        //            {
        //                var wHash = GetHash(ItemType.Musket.ToString());
        //                SafeTrigger.ClientEvent(player, "takeOffWeapon", (int)wHash);

        //                //nInventory.Remove(player, musket);
        //                return;
        //            }
        //        }

        //        musket.Data = $"{data[0]}_{data[1]}";
        //    }
        //    catch (Exception e) { _logger.WriteError("Unhandled exception catched on weapons:musketShot - " + e.ToString()); }
        //}
    }
    class ArmedBody : Script
    {
        static string[] WeaponKeys = { "WEAPON_OBJ_PISTOL", "WEAPON_OBJ_SMG", "WEAPON_OBJ_BACK_RIGHT", "WEAPON_OBJ_BACK_LEFT" };

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ArmedBody));

        public enum WeaponAttachmentType
        {
            RightLeg = 0,
            LeftLeg,
            RightBack,
            LeftBack
        }
        internal class WeaponAttachmentInfo
        {
            public string Model;
            public WeaponAttachmentType Type;

            public WeaponAttachmentInfo(string model, WeaponAttachmentType type)
            {
                Model = model;
                Type = type;
            }
        }

        static Dictionary<WeaponHash, WeaponAttachmentInfo> WeaponData = new Dictionary<WeaponHash, WeaponAttachmentInfo>
        {
            // pistols
            { WeaponHash.Pistol, new WeaponAttachmentInfo("w_pi_pistol", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Combatpistol, new WeaponAttachmentInfo("w_pi_combatpistol", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Pistol50, new WeaponAttachmentInfo("w_pi_pistol50", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Snspistol, new WeaponAttachmentInfo("w_pi_sns_pistol", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Heavypistol, new WeaponAttachmentInfo("w_pi_heavypistol", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Vintagepistol, new WeaponAttachmentInfo("w_pi_vintage_pistol", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Marksmanpistol, new WeaponAttachmentInfo("w_pi_singleshot", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Revolver, new WeaponAttachmentInfo("w_pi_revolver", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Appistol, new WeaponAttachmentInfo("w_pi_appistol", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Stungun, new WeaponAttachmentInfo("w_pi_stungun", WeaponAttachmentType.RightLeg) },
            { WeaponHash.Flaregun, new WeaponAttachmentInfo("w_pi_flaregun", WeaponAttachmentType.RightLeg) },

            // smgs
            { WeaponHash.Microsmg, new WeaponAttachmentInfo("w_sb_microsmg", WeaponAttachmentType.LeftLeg) },
            { WeaponHash.Machinepistol, new WeaponAttachmentInfo("w_sb_compactsmg", WeaponAttachmentType.LeftLeg) },
            { WeaponHash.Minismg, new WeaponAttachmentInfo("w_sb_minismg", WeaponAttachmentType.LeftLeg) },

            // big smgs
            { WeaponHash.Smg, new WeaponAttachmentInfo("w_sb_smg", WeaponAttachmentType.RightBack) },
            { WeaponHash.Assaultsmg, new WeaponAttachmentInfo("w_sb_assaultsmg", WeaponAttachmentType.RightBack) },
            { WeaponHash.Combatpdw, new WeaponAttachmentInfo("w_sb_pdw", WeaponAttachmentType.RightBack) },
            { WeaponHash.Gusenberg, new WeaponAttachmentInfo("w_sb_gusenberg", WeaponAttachmentType.RightBack) },

            // shotguns
            { WeaponHash.Pumpshotgun, new WeaponAttachmentInfo("w_sg_pumpshotgun", WeaponAttachmentType.LeftBack) },
            //{ WeaponHash.SawnoffShotgun, new WeaponAttachmentInfo("w_sg_sawnoff", WeaponAttachmentType.LeftBack) },
            { WeaponHash.Bullpupshotgun, new WeaponAttachmentInfo("w_sg_bullpupshotgun", WeaponAttachmentType.LeftBack) },
            { WeaponHash.Assaultshotgun, new WeaponAttachmentInfo("w_sg_assaultshotgun", WeaponAttachmentType.LeftBack) },
            { WeaponHash.Heavyshotgun, new WeaponAttachmentInfo("w_sg_heavyshotgun", WeaponAttachmentType.LeftBack) },
            { WeaponHash.Doubleaction, new WeaponAttachmentInfo("w_sg_doublebarrel", WeaponAttachmentType.LeftBack) },

            // assault rifles
            { WeaponHash.Assaultrifle, new WeaponAttachmentInfo("w_ar_assaultrifle", WeaponAttachmentType.RightBack) },
            { WeaponHash.Carbinerifle, new WeaponAttachmentInfo("w_ar_carbinerifle", WeaponAttachmentType.RightBack) },
            { WeaponHash.Advancedrifle, new WeaponAttachmentInfo("w_ar_advancedrifle", WeaponAttachmentType.RightBack) },
            { WeaponHash.Specialcarbine, new WeaponAttachmentInfo("w_ar_specialcarbine", WeaponAttachmentType.RightBack) },
            { WeaponHash.Bullpuprifle, new WeaponAttachmentInfo("w_ar_bullpuprifle", WeaponAttachmentType.RightBack) },
            { WeaponHash.Compactrifle, new WeaponAttachmentInfo("w_ar_assaultrifle_smg", WeaponAttachmentType.RightBack) },

            // sniper rifles
            { WeaponHash.Marksmanrifle, new WeaponAttachmentInfo("w_sr_marksmanrifle", WeaponAttachmentType.RightBack) },
            { WeaponHash.Sniperrifle, new WeaponAttachmentInfo("w_sr_sniperrifle", WeaponAttachmentType.RightBack) },
            { WeaponHash.Heavysniper, new WeaponAttachmentInfo("w_sr_heavysniper", WeaponAttachmentType.RightBack) },

            // lmgs
            { WeaponHash.Mg, new WeaponAttachmentInfo("w_mg_mg", WeaponAttachmentType.LeftBack) },
            { WeaponHash.Combatmg, new WeaponAttachmentInfo("w_mg_combatmg", WeaponAttachmentType.LeftBack) }
        };

        #region Methods
        public static void CreateWeaponProp(ExtPlayer player, WeaponHash weapon)
        {
            if (!WeaponData.ContainsKey(weapon)) return;
            RemoveWeaponProp(player, WeaponData[weapon].Type);

            // make sure player has the weapon
            if (Array.IndexOf(player.Weapons, weapon) == -1) return;

            string bone = "";
            Vector3 offset = new Vector3(0.0, 0.0, 0.0);
            Vector3 rotation = new Vector3(0.0, 0.0, 0.0);

            switch (WeaponData[weapon].Type)
            {
                case WeaponAttachmentType.RightLeg:
                    bone = "SKEL_R_Thigh";
                    offset = new Vector3(0.02, 0.06, 0.1);
                    rotation = new Vector3(-100.0, 0.0, 0.0);
                    break;

                case WeaponAttachmentType.LeftLeg:
                    bone = "SKEL_L_Thigh";
                    offset = new Vector3(0.08, 0.03, -0.1);
                    rotation = new Vector3(-80.77, 0.0, 0.0);
                    break;

                case WeaponAttachmentType.RightBack:
                    bone = "SKEL_Spine3";
                    offset = new Vector3(-0.1, -0.15, -0.13);
                    rotation = new Vector3(0.0, 0.0, 3.5);
                    break;

                case WeaponAttachmentType.LeftBack:
                    bone = "SKEL_Spine3";
                    offset = new Vector3(-0.1, -0.15, 0.11);
                    rotation = new Vector3(-180.0, 0.0, 0.0);
                    break;
            }

            var temp_handle = NAPI.Object.CreateObject(NAPI.Util.GetHashKey(WeaponData[weapon].Model), player.Position, player.Rotation, 255, 0);
            //temp_handle.AttachTo(player.Handle, bone, offset, rotation);

            SafeTrigger.SetData(player, WeaponKeys[(int)WeaponData[weapon].Type], temp_handle);
        }

        public static void RemoveWeaponProp(ExtPlayer player, WeaponAttachmentType type)
        {
            int type_int = (int)type;
            if (!player.HasData(WeaponKeys[type_int])) return;

            var obj = player.GetData<GTANetworkAPI.Object>(WeaponKeys[type_int]);
            obj.Delete();

            player.ResetData(WeaponKeys[type_int]);
        }

        public static void RemoveWeaponProps(ExtPlayer player)
        {
            foreach (string key in WeaponKeys)
            {
                if (!player.HasData(key)) continue;

                var obj = player.GetData<GTANetworkAPI.Object>(key);
                obj.Delete();

                player.ResetData(key);
            }
        }
        #endregion

        #region Exported Methods
        public void RemovePlayerWeapon(ExtPlayer player, WeaponHash weapon)
        {
            if (WeaponData.ContainsKey(weapon))
            {
                string key = WeaponKeys[(int)WeaponData[weapon].Type];

                if(player.HasData(key))
                {
                    GTANetworkAPI.Object obj = player.GetData<GTANetworkAPI.Object>(key);

                    if (obj.Model == NAPI.Util.GetHashKey(WeaponData[weapon].Model))
                    {
                        obj.Delete();
                        player.ResetData(key);
                    }
                }
            }
            //NAPI.Player.RemovePlayerWeapon(player, weapon);
        }

        public void RemoveAllPlayerWeapons(ExtPlayer player)
        {
            RemoveWeaponProps(player);
            NAPI.Player.RemoveAllPlayerWeapons(player);
        }
        #endregion

        #region Events
        [ServerEvent(Event.ResourceStop)]
        public void ArmedBody_Exit()
        {
            try
            {
                foreach (ExtPlayer player in NAPI.Pools.GetAllPlayers()) RemoveWeaponProps(player);
            }
            catch (Exception e) { _logger.WriteError("ResourceStop: " + e.ToString()); }
        }
        #endregion

        #region WeaponComponents


        public static Dictionary<ComponentSlots, List<WeaponComponents>> ComponentSlotConfig = new Dictionary<ComponentSlots, List<WeaponComponents>>
        {
            {
                ComponentSlots.Skin,
                new List<WeaponComponents>{
                        WeaponComponents.RevolverMk2Camo,
                        WeaponComponents.RevolverMk2Camo02,
                        WeaponComponents.RevolverMk2Camo03,
                        WeaponComponents.RevolverMk2Camo04,
                        WeaponComponents.RevolverMk2Camo05,
                        WeaponComponents.RevolverMk2Camo06,
                        WeaponComponents.RevolverMk2Camo07,
                        WeaponComponents.RevolverMk2Camo08,
                        WeaponComponents.RevolverMk2Camo09,
                        WeaponComponents.RevolverMk2Camo10,
                        WeaponComponents.RevolverMk2CamoInd01,
                        WeaponComponents.SnspistolMk2Camo,
                        WeaponComponents.SnspistolMk2Camo02,
                        WeaponComponents.SnspistolMk2Camo03,
                        WeaponComponents.SnspistolMk2Camo04,
                        WeaponComponents.SnspistolMk2Camo05,
                        WeaponComponents.SnspistolMk2Camo06,
                        WeaponComponents.SnspistolMk2Camo07,
                        WeaponComponents.SnspistolMk2Camo08,
                        WeaponComponents.SnspistolMk2Camo09,
                        WeaponComponents.SnspistolMk2Camo10,
                        WeaponComponents.SnspistolMk2CamoInd01,
                        WeaponComponents.SnspistolMk2CamoSlide,
                        WeaponComponents.SnspistolMk2Camo02Slide,
                        WeaponComponents.SnspistolMk2Camo03Slide,
                        WeaponComponents.SnspistolMk2Camo04Slide,
                        WeaponComponents.SnspistolMk2Camo05Slide,
                        WeaponComponents.SnspistolMk2Camo06Slide,
                        WeaponComponents.SnspistolMk2Camo07Slide,
                        WeaponComponents.SnspistolMk2Camo08Slide,
                        WeaponComponents.SnspistolMk2Camo09Slide,
                        WeaponComponents.SnspistolMk2Camo10Slide,
                        WeaponComponents.SnspistolMk2CamoInd01Slide,
                        WeaponComponents.PistolMk2Camo,
                        WeaponComponents.PistolMk2Camo02,
                        WeaponComponents.PistolMk2Camo03,
                        WeaponComponents.PistolMk2Camo04,
                        WeaponComponents.PistolMk2Camo05,
                        WeaponComponents.PistolMk2Camo06,
                        WeaponComponents.PistolMk2Camo07,
                        WeaponComponents.PistolMk2Camo08,
                        WeaponComponents.PistolMk2Camo09,
                        WeaponComponents.PistolMk2Camo10,
                        WeaponComponents.PistolMk2CamoInd01,
                        WeaponComponents.PistolMk2CamoSlide,
                        WeaponComponents.PistolMk2Camo02Slide,
                        WeaponComponents.PistolMk2Camo03Slide,
                        WeaponComponents.PistolMk2Camo04Slide,
                        WeaponComponents.PistolMk2Camo05Slide,
                        WeaponComponents.PistolMk2Camo06Slide,
                        WeaponComponents.PistolMk2Camo07Slide,
                        WeaponComponents.PistolMk2Camo08Slide,
                        WeaponComponents.PistolMk2Camo09Slide,
                        WeaponComponents.PistolMk2Camo10Slide,
                        WeaponComponents.PistolMk2CamoInd01Slide,
                        WeaponComponents.PumpshotgunMk2Camo,
                        WeaponComponents.PumpshotgunMk2Camo02,
                        WeaponComponents.PumpshotgunMk2Camo03,
                        WeaponComponents.PumpshotgunMk2Camo04,
                        WeaponComponents.PumpshotgunMk2Camo05,
                        WeaponComponents.PumpshotgunMk2Camo06,
                        WeaponComponents.PumpshotgunMk2Camo07,
                        WeaponComponents.PumpshotgunMk2Camo08,
                        WeaponComponents.PumpshotgunMk2Camo09,
                        WeaponComponents.PumpshotgunMk2Camo10,
                        WeaponComponents.PumpshotgunMk2CamoInd01,
                        WeaponComponents.MarksmanrifleMk2Camo,
                        WeaponComponents.MarksmanrifleMk2Camo02,
                        WeaponComponents.MarksmanrifleMk2Camo03,
                        WeaponComponents.MarksmanrifleMk2Camo04,
                        WeaponComponents.MarksmanrifleMk2Camo05,
                        WeaponComponents.MarksmanrifleMk2Camo06,
                        WeaponComponents.MarksmanrifleMk2Camo07,
                        WeaponComponents.MarksmanrifleMk2Camo08,
                        WeaponComponents.MarksmanrifleMk2Camo09,
                        WeaponComponents.MarksmanrifleMk2Camo10,
                        WeaponComponents.MarksmanrifleMk2CamoInd01,
                        WeaponComponents.HeavysniperMk2Camo,
                        WeaponComponents.HeavysniperMk2Camo02,
                        WeaponComponents.HeavysniperMk2Camo03,
                        WeaponComponents.HeavysniperMk2Camo04,
                        WeaponComponents.HeavysniperMk2Camo05,
                        WeaponComponents.HeavysniperMk2Camo06,
                        WeaponComponents.HeavysniperMk2Camo07,
                        WeaponComponents.HeavysniperMk2Camo08,
                        WeaponComponents.HeavysniperMk2Camo09,
                        WeaponComponents.HeavysniperMk2Camo10,
                        WeaponComponents.HeavysniperMk2CamoInd01,
                        WeaponComponents.BullpuprifleMk2Camo,
                        WeaponComponents.BullpuprifleMk2Camo02,
                        WeaponComponents.BullpuprifleMk2Camo03,
                        WeaponComponents.BullpuprifleMk2Camo04,
                        WeaponComponents.BullpuprifleMk2Camo05,
                        WeaponComponents.BullpuprifleMk2Camo06,
                        WeaponComponents.BullpuprifleMk2Camo07,
                        WeaponComponents.BullpuprifleMk2Camo08,
                        WeaponComponents.BullpuprifleMk2Camo09,
                        WeaponComponents.BullpuprifleMk2Camo10,
                        WeaponComponents.BullpuprifleMk2CamoInd01,
                        WeaponComponents.SpecialcarbineMk2Camo,
                        WeaponComponents.SpecialcarbineMk2Camo02,
                        WeaponComponents.SpecialcarbineMk2Camo03,
                        WeaponComponents.SpecialcarbineMk2Camo04,
                        WeaponComponents.SpecialcarbineMk2Camo05,
                        WeaponComponents.SpecialcarbineMk2Camo06,
                        WeaponComponents.SpecialcarbineMk2Camo07,
                        WeaponComponents.SpecialcarbineMk2Camo08,
                        WeaponComponents.SpecialcarbineMk2Camo09,
                        WeaponComponents.SpecialcarbineMk2Camo10,
                        WeaponComponents.SpecialcarbineMk2CamoInd01,
                        WeaponComponents.CarbinerifleMk2Camo,
                        WeaponComponents.CarbinerifleMk2Camo02,
                        WeaponComponents.CarbinerifleMk2Camo03,
                        WeaponComponents.CarbinerifleMk2Camo04,
                        WeaponComponents.CarbinerifleMk2Camo05,
                        WeaponComponents.CarbinerifleMk2Camo06,
                        WeaponComponents.CarbinerifleMk2Camo07,
                        WeaponComponents.CarbinerifleMk2Camo08,
                        WeaponComponents.CarbinerifleMk2Camo09,
                        WeaponComponents.CarbinerifleMk2Camo10,
                        WeaponComponents.CarbinerifleMk2CamoInd01,
                        WeaponComponents.AssaultrifleMk2Camo,
                        WeaponComponents.AssaultrifleMk2Camo02,
                        WeaponComponents.AssaultrifleMk2Camo03,
                        WeaponComponents.AssaultrifleMk2Camo04,
                        WeaponComponents.AssaultrifleMk2Camo05,
                        WeaponComponents.AssaultrifleMk2Camo06,
                        WeaponComponents.AssaultrifleMk2Camo07,
                        WeaponComponents.AssaultrifleMk2Camo08,
                        WeaponComponents.AssaultrifleMk2Camo09,
                        WeaponComponents.AssaultrifleMk2Camo10,
                        WeaponComponents.AssaultrifleMk2CamoInd01,
                        WeaponComponents.SmgMk2Camo,
                        WeaponComponents.SmgMk2Camo02,
                        WeaponComponents.SmgMk2Camo03,
                        WeaponComponents.SmgMk2Camo04,
                        WeaponComponents.SmgMk2Camo05,
                        WeaponComponents.SmgMk2Camo06,
                        WeaponComponents.SmgMk2Camo07,
                        WeaponComponents.SmgMk2Camo08,
                        WeaponComponents.SmgMk2Camo09,
                        WeaponComponents.SmgMk2Camo10,
                        WeaponComponents.SmgMk2CamoInd01,
                        WeaponComponents.CombatmgMk2Camo,
                        WeaponComponents.CombatmgMk2Camo02,
                        WeaponComponents.CombatmgMk2Camo03,
                        WeaponComponents.CombatmgMk2Camo04,
                        WeaponComponents.CombatmgMk2Camo05,
                        WeaponComponents.CombatmgMk2Camo06,
                        WeaponComponents.CombatmgMk2Camo07,
                        WeaponComponents.CombatmgMk2Camo08,
                        WeaponComponents.CombatmgMk2Camo09,
                        WeaponComponents.CombatmgMk2Camo10,
                        WeaponComponents.CombatmgMk2CamoInd01,
                        WeaponComponents.KnuckleVarmodPlayer,
                        WeaponComponents.MarksmanRifleVarmodLuxe,
                        WeaponComponents.RevolverVarmodBoss,
                        WeaponComponents.SMGVarmodLuxe,
                        WeaponComponents.AssaultSMGVarmodLowrider,
                        WeaponComponents.AdvancedRifleVarmodLuxe,
                        WeaponComponents.KnuckleVarmodLove,
                        WeaponComponents.SniperRifleVarmodLuxe,
                        WeaponComponents.MicroSMGVarmodLuxe,
                        WeaponComponents.AssaultRifleVarmodLuxe,
                        WeaponComponents.KnuckleVarmodDollar,
                        WeaponComponents.SwitchbladeVarmodVar1,
                        WeaponComponents.SpecialCarbineVarmodLowrider,
                        WeaponComponents.Pistol50VarmodLuxe,
                        WeaponComponents.HeavyPistolVarmodLuxe,
                        WeaponComponents.KnuckleVarmodVagos,
                        WeaponComponents.KnuckleVarmodHate,
                        WeaponComponents.SNSPistolVarmodLowrider,
                        WeaponComponents.SawnoffShotgunVarmodLuxe,
                        WeaponComponents.SwitchbladeVarmodBase,
                        WeaponComponents.CombatMGVarmodLowrider,
                        WeaponComponents.RevolverVarmodGoon,
                        WeaponComponents.KnuckleVarmodDiamond,
                        WeaponComponents.APPistolVarmodLuxe,
                        WeaponComponents.PumpShotgunVarmodLowrider,
                        WeaponComponents.BullpupRifleVarmodLow,
                        WeaponComponents.KnuckleVarmodPimp,
                        WeaponComponents.CombatPistolVarmodLowrider,
                        WeaponComponents.MGVarmodLowrider,
                        WeaponComponents.PistolVarmodLuxe,
                        WeaponComponents.CarbineRifleVarmodLuxe,
                        WeaponComponents.KnuckleVarmodKing,
                        WeaponComponents.SwitchbladeVarmodVar2,
                        WeaponComponents.KnuckleVarmodBallas,
                        WeaponComponents.KnuckleVarmodBase,
                    }
            },
            {
                ComponentSlots.Clip,
                new List<WeaponComponents>{
                    WeaponComponents.RevolverMk2Clip01,
                    WeaponComponents.RevolverMk2ClipTracer,
                    WeaponComponents.RevolverMk2ClipIncendiary,
                    WeaponComponents.RevolverMk2ClipHollowpoint,
                    WeaponComponents.RevolverMk2ClipFmj,
                    WeaponComponents.SnspistolMk2Clip01,
                    WeaponComponents.SnspistolMk2Clip02,
                    WeaponComponents.SnspistolMk2ClipTracer,
                    WeaponComponents.SnspistolMk2ClipIncendiary,
                    WeaponComponents.SnspistolMk2ClipHollowpoint,
                    WeaponComponents.SnspistolMk2ClipFmj,
                    WeaponComponents.PistolMk2Clip01,
                    WeaponComponents.PistolMk2Clip02,
                    WeaponComponents.PistolMk2ClipTracer,
                    WeaponComponents.PistolMk2ClipIncendiary,
                    WeaponComponents.PistolMk2ClipHollowpoint,
                    WeaponComponents.PistolMk2ClipFmj,
                    WeaponComponents.PumpshotgunMk2Clip01,
                    WeaponComponents.PumpshotgunMk2ClipIncendiary,
                    WeaponComponents.PumpshotgunMk2ClipArmorpiercing,
                    WeaponComponents.PumpshotgunMk2ClipHollowpoint,
                    WeaponComponents.PumpshotgunMk2ClipExplosive,
                    WeaponComponents.MarksmanrifleMk2Clip01,
                    WeaponComponents.MarksmanrifleMk2Clip02,
                    WeaponComponents.MarksmanrifleMk2ClipTracer,
                    WeaponComponents.MarksmanrifleMk2ClipIncendiary,
                    WeaponComponents.MarksmanrifleMk2ClipArmorpiercing,
                    WeaponComponents.MarksmanrifleMk2ClipFmj,
                    WeaponComponents.HeavysniperMk2Clip01,
                    WeaponComponents.HeavysniperMk2Clip02,
                    WeaponComponents.HeavysniperMk2ClipIncendiary,
                    WeaponComponents.HeavysniperMk2ClipArmorpiercing,
                    WeaponComponents.HeavysniperMk2ClipFmj,
                    WeaponComponents.HeavysniperMk2ClipExplosive,
                    WeaponComponents.BullpuprifleMk2Clip01,
                    WeaponComponents.BullpuprifleMk2Clip02,
                    WeaponComponents.BullpuprifleMk2ClipTracer,
                    WeaponComponents.BullpuprifleMk2ClipIncendiary,
                    WeaponComponents.BullpuprifleMk2ClipArmorpiercing,
                    WeaponComponents.BullpuprifleMk2ClipFmj,
                    WeaponComponents.SpecialcarbineMk2Clip01,
                    WeaponComponents.SpecialcarbineMk2Clip02,
                    WeaponComponents.SpecialcarbineMk2ClipTracer,
                    WeaponComponents.SpecialcarbineMk2ClipIncendiary,
                    WeaponComponents.SpecialcarbineMk2ClipArmorpiercing,
                    WeaponComponents.SpecialcarbineMk2ClipFmj,
                    WeaponComponents.CarbinerifleMk2Clip01,
                    WeaponComponents.CarbinerifleMk2Clip02,
                    WeaponComponents.CarbinerifleMk2ClipTracer,
                    WeaponComponents.CarbinerifleMk2ClipIncendiary,
                    WeaponComponents.CarbinerifleMk2ClipArmorpiercing,
                    WeaponComponents.CarbinerifleMk2ClipFmj,
                    WeaponComponents.AssaultrifleMk2Clip01,
                    WeaponComponents.AssaultrifleMk2Clip02,
                    WeaponComponents.AssaultrifleMk2ClipTracer,
                    WeaponComponents.AssaultrifleMk2ClipIncendiary,
                    WeaponComponents.AssaultrifleMk2ClipArmorpiercing,
                    WeaponComponents.AssaultrifleMk2ClipFmj,
                    WeaponComponents.MinismgClip01,
                    WeaponComponents.MinismgClip02,
                    WeaponComponents.SmgMk2Clip01,
                    WeaponComponents.SmgMk2Clip02,
                    WeaponComponents.SmgMk2ClipTracer,
                    WeaponComponents.SmgMk2ClipIncendiary,
                    WeaponComponents.SmgMk2ClipHollowpoint,
                    WeaponComponents.SmgMk2ClipFmj,
                    WeaponComponents.CombatmgMk2Clip01,
                    WeaponComponents.CombatmgMk2Clip02,
                    WeaponComponents.CombatmgMk2ClipTracer,
                    WeaponComponents.CombatmgMk2ClipIncendiary,
                    WeaponComponents.CombatmgMk2ClipArmorpiercing,
                    WeaponComponents.CombatmgMk2ClipFmj,
                    WeaponComponents.RailgunClip01,
                    WeaponComponents.CombatPistolClip01,
                    WeaponComponents.HeavyPistolClip01,
                    WeaponComponents.MicroSMGClip02,
                    WeaponComponents.GrenadeLauncherClip01,
                    WeaponComponents.GusenbergClip01,
                    WeaponComponents.Pistol50Clip01,
                    WeaponComponents.APPistolClip02,
                    WeaponComponents.SMGClip01,
                    WeaponComponents.DBShotgunClip01,
                    WeaponComponents.APPistolClip01,
                    WeaponComponents.HeavyShotgunClip01,
                    WeaponComponents.CombatPDWClip02,
                    WeaponComponents.VintagePistolClip02,
                    WeaponComponents.SMGClip02,
                    WeaponComponents.CombatPDWClip01,
                    WeaponComponents.VintagePistolClip01,
                    WeaponComponents.MachinePistolClip01,
                    WeaponComponents.HeavySniperClip01,
                    WeaponComponents.RPGClip01,
                    WeaponComponents.MusketClip01,
                    WeaponComponents.CompactRifleClip01,
                    WeaponComponents.CompactRifleClip02,
                    WeaponComponents.HeavyPistolClip02,
                    WeaponComponents.SpecialCarbineClip03,
                    WeaponComponents.CombatPDWClip03,
                    WeaponComponents.SMGClip03,
                    WeaponComponents.SNSPistolClip02,
                    WeaponComponents.SpecialCarbineClip02,
                    WeaponComponents.MGClip02,
                    WeaponComponents.AssaultShotgunClip02,
                    WeaponComponents.HeavyShotgunClip03,
                    WeaponComponents.AssaultSMGClip01,
                    WeaponComponents.AdvancedRifleClip02,
                    WeaponComponents.CarbineRifleClip02,
                    WeaponComponents.FlareGunClip01,
                    WeaponComponents.AssaultShotgunClip01,
                    WeaponComponents.HeavyShotgunClip02,
                    WeaponComponents.SniperRifleClip01,
                    WeaponComponents.CarbineRifleClip01,
                    WeaponComponents.MachinePistolClip03,
                    WeaponComponents.AssaultRifleClip02,
                    WeaponComponents.BullpupRifleClip02,
                    WeaponComponents.MachinePistolClip02,
                    WeaponComponents.CarbineRifleClip03,
                    WeaponComponents.AssaultSMGClip02,
                    WeaponComponents.AssaultRifleClip01,
                    WeaponComponents.BullpupRifleClip01,
                    WeaponComponents.CompactRifleClip03,
                    WeaponComponents.SpecialCarbineClip01,
                    WeaponComponents.SawnoffShotgunClip01,
                    WeaponComponents.MinigunClip01,
                    WeaponComponents.BullpupShotgunClip01,
                    WeaponComponents.MicroSMGClip01,
                    WeaponComponents.MarksmanPistolClip01,
                    WeaponComponents.MarksmanRifleClip02,
                    WeaponComponents.PumpShotgunClip01,
                    WeaponComponents.CombatPistolClip02,
                    WeaponComponents.CombatMGClip02,
                    WeaponComponents.MarksmanRifleClip01,
                    WeaponComponents.Pistol50Clip02,
                    WeaponComponents.AssaultRifleClip03,
                    WeaponComponents.CombatMGClip01,
                    WeaponComponents.FireworkClip01,
                    WeaponComponents.RevolverClip01,
                    WeaponComponents.GusenbergClip02,
                    WeaponComponents.PistolClip02,
                    WeaponComponents.MGClip01,
                    WeaponComponents.HomingLauncherClip01,
                    WeaponComponents.SNSPistolClip01,
                    WeaponComponents.AdvancedRifleClip01,
                    WeaponComponents.PistolClip01,
                }
            },
            {
                ComponentSlots.Scope,
                new List<WeaponComponents>{
                    WeaponComponents.AtPiRail02,
                    WeaponComponents.AtPiRail,
                    WeaponComponents.AtScopeLargeFixedZoomMk2,
                    WeaponComponents.AtScopeLargeMk2,
                    WeaponComponents.AtScopeNv,
                    WeaponComponents.AtScopeThermal,
                    WeaponComponents.AtScopeMacro02Mk2,
                    WeaponComponents.AtSights,
                    WeaponComponents.AtScopeMacroMk2,
                    WeaponComponents.AtScopeMediumMk2,
                    WeaponComponents.AtSightsSmg,
                    WeaponComponents.AtScopeMacro02SmgMk2,
                    WeaponComponents.AtScopeSmallSmgMk2,
                    WeaponComponents.AtScopeSmallMk2,
                    WeaponComponents.AtScopeLargeFixedZoom,
                    WeaponComponents.AtScopeSmall02,
                    WeaponComponents.AtScopeMacro02,
                    WeaponComponents.AtRailCover01,
                    WeaponComponents.AtScopeMacro,
                    WeaponComponents.AtScopeMedium,
                    WeaponComponents.AtScopeSmall,
                    WeaponComponents.AtScopeMax,
                    WeaponComponents.AtScopeLarge,
                    WeaponComponents.Invalid,
                }
            },
            {
                ComponentSlots.Flash,
                new List<WeaponComponents>{
                    WeaponComponents.AtPiFlsh03,
                    WeaponComponents.AtPiFlsh02,
                    WeaponComponents.AtArFlsh,
                    WeaponComponents.AtPiFlsh,
                    WeaponComponents.PoliceTorchFlashlight,
                    WeaponComponents.FlashlightLight,
                }
            },
            {
                ComponentSlots.Barrel,
                new List<WeaponComponents>{
                    WeaponComponents.AtMrflBarrel01,
                    WeaponComponents.AtMrflBarrel02,
                    WeaponComponents.AtSrBarrel01,
                    WeaponComponents.AtSrBarrel02,
                    WeaponComponents.AtBpBarrel01,
                    WeaponComponents.AtBpBarrel02,
                    WeaponComponents.AtScBarrel01,
                    WeaponComponents.AtScBarrel02,
                    WeaponComponents.AtCrBarrel01,
                    WeaponComponents.AtCrBarrel02,
                    WeaponComponents.AtArBarrel01,
                    WeaponComponents.AtArBarrel02,
                    WeaponComponents.AtSbBarrel01,
                    WeaponComponents.AtSbBarrel02,
                    WeaponComponents.AtMgBarrel01,
                    WeaponComponents.AtMgBarrel02
                }
            },
            {
                ComponentSlots.Grip,
                new List<WeaponComponents>{
                    WeaponComponents.AtArAfgrip02,
                    WeaponComponents.AtArAfGrip,
                }
            },
            {
                ComponentSlots.Supp,
                new List<WeaponComponents>{
                    WeaponComponents.AtPiComp03,
                    WeaponComponents.AtPiComp02,
                    WeaponComponents.AtPiComp,
                    WeaponComponents.AtSrSupp03,
                    WeaponComponents.AtMuzzle08,
                    WeaponComponents.AtMuzzle09,
                    WeaponComponents.AtArSupp02,
                    WeaponComponents.AtMuzzle01,
                    WeaponComponents.AtMuzzle02,
                    WeaponComponents.AtMuzzle03,
                    WeaponComponents.AtMuzzle04,
                    WeaponComponents.AtMuzzle05,
                    WeaponComponents.AtMuzzle06,
                    WeaponComponents.AtMuzzle07,
                    WeaponComponents.AtPiSupp,
                    WeaponComponents.AtPiSupp02,
                    WeaponComponents.AtArSupp,
                    WeaponComponents.AtSrSupp,
                }
            }
        };

        private class WeaponConfigModel
        {
            public WeaponConfigModel(uint hash)
            {
                Components = new List<ComponentModel>();
                Hash = hash;
            }
            public uint Hash { get; set; }
            public List<ComponentModel> Components { get; set; }
        }

        private class ComponentModel
        {
            public ComponentModel(WeaponComponents component)
            {
                Slot = ComponentSlots.Invalid;
                if (ComponentSlotConfig[ComponentSlots.Skin].Contains(component)) Slot = ComponentSlots.Skin;
                else if (ComponentSlotConfig[ComponentSlots.Scope].Contains(component)) Slot = ComponentSlots.Scope;
                else if (ComponentSlotConfig[ComponentSlots.Clip].Contains(component)) Slot = ComponentSlots.Clip;
                else if (ComponentSlotConfig[ComponentSlots.Supp].Contains(component)) Slot = ComponentSlots.Supp;
                else if (ComponentSlotConfig[ComponentSlots.Grip].Contains(component)) Slot = ComponentSlots.Grip;
                else if (ComponentSlotConfig[ComponentSlots.Barrel].Contains(component)) Slot = ComponentSlots.Barrel;
                else if (ComponentSlotConfig[ComponentSlots.Flash].Contains(component)) Slot = ComponentSlots.Flash;
                Hash = (uint)component;
            }
            public uint Hash { get; set; }
            public ComponentSlots Slot { get; set; }
        }
        #endregion

    }
    public enum ComponentSlots
    {
        Skin,
        Scope,
        Clip,
        Supp,
        Grip,
        Barrel,
        Flash,
        Invalid = -1
    }

    public enum WeaponComponents:uint
    {
        //wiki
        RevolverMk2Clip01 = 3122911422,
        RevolverMk2ClipTracer = 3336103030,
        RevolverMk2ClipIncendiary = 15712037,
        RevolverMk2ClipHollowpoint = 284438159,
        RevolverMk2ClipFmj = 231258687,
        AtPiComp03 = 654802123,
        RevolverMk2Camo = 3225415071,
        RevolverMk2Camo02 = 11918884,
        RevolverMk2Camo03 = 176157112,
        RevolverMk2Camo04 = 4074914441,
        RevolverMk2Camo05 = 288456487,
        RevolverMk2Camo06 = 398658626,
        RevolverMk2Camo07 = 628697006,
        RevolverMk2Camo08 = 925911836,
        RevolverMk2Camo09 = 1222307441,
        RevolverMk2Camo10 = 552442715,
        RevolverMk2CamoInd01 = 3646023783,

        SnspistolMk2Clip01 = 21392614,
        SnspistolMk2Clip02 = 3465283442,
        SnspistolMk2ClipTracer = 2418909806,
        SnspistolMk2ClipIncendiary = 3870121849,
        SnspistolMk2ClipHollowpoint = 2366665730,
        SnspistolMk2ClipFmj = 3239176998,
        AtPiFlsh03 = 1246324211,
        AtPiRail02 = 1205768792,
        AtPiComp02 = 2860680127,
        SnspistolMk2Camo = 259780317,
        SnspistolMk2Camo02 = 2321624822,
        SnspistolMk2Camo03 = 1996130345,
        SnspistolMk2Camo04 = 2839309484,
        SnspistolMk2Camo05 = 2626704212,
        SnspistolMk2Camo06 = 1308243489,
        SnspistolMk2Camo07 = 1122574335,
        SnspistolMk2Camo08 = 1420313469,
        SnspistolMk2Camo09 = 109848390,
        SnspistolMk2Camo10 = 593945703,
        SnspistolMk2CamoInd01 = 1142457062,
        SnspistolMk2CamoSlide = 3891161322,
        SnspistolMk2Camo02Slide = 691432737,
        SnspistolMk2Camo03Slide = 987648331,
        SnspistolMk2Camo04Slide = 3863286761,
        SnspistolMk2Camo05Slide = 3447384986,
        SnspistolMk2Camo06Slide = 4202375078,
        SnspistolMk2Camo07Slide = 3800418970,
        SnspistolMk2Camo08Slide = 730876697,
        SnspistolMk2Camo09Slide = 583159708,
        SnspistolMk2Camo10Slide = 2366463693,
        SnspistolMk2CamoInd01Slide = 52055783,

        PistolMk2Clip01 = 2499030370,
        PistolMk2Clip02 = 1591132456,
        PistolMk2ClipTracer = 634039983,
        PistolMk2ClipIncendiary = 733837882,
        PistolMk2ClipHollowpoint = 2248057097,
        PistolMk2ClipFmj = 1329061674,
        AtPiRail = 2396306288,
        AtPiFlsh02 = 1140676955,
        AtPiComp = 568543123,
        PistolMk2Camo = 1550611612,
        PistolMk2Camo02 = 368550800,
        PistolMk2Camo03 = 2525897947,
        PistolMk2Camo04 = 24902297,
        PistolMk2Camo05 = 4066925682,
        PistolMk2Camo06 = 3710005734,
        PistolMk2Camo07 = 3141791350,
        PistolMk2Camo08 = 1301287696,
        PistolMk2Camo09 = 1597093459,
        PistolMk2Camo10 = 1769871776,
        PistolMk2CamoInd01 = 2467084625,
        PistolMk2CamoSlide = 3036451504,
        PistolMk2Camo02Slide = 438243936,
        PistolMk2Camo03Slide = 3839888240,
        PistolMk2Camo04Slide = 740920107,
        PistolMk2Camo05Slide = 3753350949,
        PistolMk2Camo06Slide = 1809261196,
        PistolMk2Camo07Slide = 2648428428,
        PistolMk2Camo08Slide = 3004802348,
        PistolMk2Camo09Slide = 3330502162,
        PistolMk2Camo10Slide = 1135718771,
        PistolMk2CamoInd01Slide = 1253942266,

        PumpshotgunMk2Clip01 = 3449028929,
        PumpshotgunMk2ClipIncendiary = 2676628469,
        PumpshotgunMk2ClipArmorpiercing = 1315288101,
        PumpshotgunMk2ClipHollowpoint = 3914869031,
        PumpshotgunMk2ClipExplosive = 1004815965,
        PumpshotgunMk2Camo = 3820854852,
        PumpshotgunMk2Camo02 = 387223451,
        PumpshotgunMk2Camo03 = 617753366,
        PumpshotgunMk2Camo04 = 4072589040,
        PumpshotgunMk2Camo05 = 8741501,
        PumpshotgunMk2Camo06 = 3693681093,
        PumpshotgunMk2Camo07 = 3783533691,
        PumpshotgunMk2Camo08 = 3639579478,
        PumpshotgunMk2Camo09 = 4012490698,
        PumpshotgunMk2Camo10 = 1739501925,
        PumpshotgunMk2CamoInd01 = 1178671645,

        MarksmanrifleMk2Clip01 = 2497785294,
        MarksmanrifleMk2Clip02 = 3872379306,
        MarksmanrifleMk2ClipTracer = 3615105746,
        MarksmanrifleMk2ClipIncendiary = 1842849902,
        MarksmanrifleMk2ClipArmorpiercing = 4100968569,
        MarksmanrifleMk2ClipFmj = 3779763923,
        AtScopeLargeFixedZoomMk2 = 1528590652,
        AtMrflBarrel01 = 941317513,
        AtMrflBarrel02 = 1748450780,
        MarksmanrifleMk2Camo = 2425682848,
        MarksmanrifleMk2Camo02 = 1931539634,
        MarksmanrifleMk2Camo03 = 1624199183,
        MarksmanrifleMk2Camo04 = 4268133183,
        MarksmanrifleMk2Camo05 = 4084561241,
        MarksmanrifleMk2Camo06 = 423313640,
        MarksmanrifleMk2Camo07 = 276639596,
        MarksmanrifleMk2Camo08 = 3303610433,
        MarksmanrifleMk2Camo09 = 2612118995,
        MarksmanrifleMk2Camo10 = 996213771,
        MarksmanrifleMk2CamoInd01 = 3080918746,

        HeavysniperMk2Clip01 = 4196276776,
        HeavysniperMk2Clip02 = 752418717,
        HeavysniperMk2ClipIncendiary = 247526935,
        HeavysniperMk2ClipArmorpiercing = 4164277972,
        HeavysniperMk2ClipFmj = 1005144310,
        HeavysniperMk2ClipExplosive = 2313935527,
        AtScopeLargeMk2 = 2193687427,
        AtScopeNv = 3061846192,
        AtScopeThermal = 776198721,
        AtSrSupp03 = 2890063729,
        AtMuzzle08 = 1602080333,
        AtMuzzle09 = 1764221345,
        AtSrBarrel01 = 2425761975,
        AtSrBarrel02 = 277524638,
        HeavysniperMk2Camo = 4164123906,
        HeavysniperMk2Camo02 = 3317620069,
        HeavysniperMk2Camo03 = 3916506229,
        HeavysniperMk2Camo04 = 329939175,
        HeavysniperMk2Camo05 = 643374672,
        HeavysniperMk2Camo06 = 807875052,
        HeavysniperMk2Camo07 = 2893163128,
        HeavysniperMk2Camo08 = 3198471901,
        HeavysniperMk2Camo09 = 3447155842,
        HeavysniperMk2Camo10 = 2881858759,
        HeavysniperMk2CamoInd01 = 1815270123,

        BullpuprifleMk2Clip01 = 25766362,
        BullpuprifleMk2Clip02 = 4021290536,
        BullpuprifleMk2ClipTracer = 2183159977,
        BullpuprifleMk2ClipIncendiary = 2845636954,
        BullpuprifleMk2ClipArmorpiercing = 4205311469,
        BullpuprifleMk2ClipFmj = 1130501904,
        AtScopeMacro02Mk2 = 3350057221,
        AtBpBarrel01 = 1704640795,
        AtBpBarrel02 = 1005743559,
        BullpuprifleMk2Camo = 2923451831,
        BullpuprifleMk2Camo02 = 3104173419,
        BullpuprifleMk2Camo03 = 2797881576,
        BullpuprifleMk2Camo04 = 2491819116,
        BullpuprifleMk2Camo05 = 2318995410,
        BullpuprifleMk2Camo06 = 36929477,
        BullpuprifleMk2Camo07 = 4026522462,
        BullpuprifleMk2Camo08 = 3720197850,
        BullpuprifleMk2Camo09 = 3412267557,
        BullpuprifleMk2Camo10 = 2826785822,
        BullpuprifleMk2CamoInd01 = 3320426066,

        SpecialcarbineMk2Clip01 = 382112385,
        SpecialcarbineMk2Clip02 = 3726614828,
        SpecialcarbineMk2ClipTracer = 2271594122,
        SpecialcarbineMk2ClipIncendiary = 3724612230,
        SpecialcarbineMk2ClipArmorpiercing = 1362433589,
        SpecialcarbineMk2ClipFmj = 1346235024,
        AtScBarrel01 = 3879097257,
        AtScBarrel02 = 4185880635,
        SpecialcarbineMk2Camo = 3557537083,
        SpecialcarbineMk2Camo02 = 1125852043,
        SpecialcarbineMk2Camo03 = 886015732,
        SpecialcarbineMk2Camo04 = 3032680157,
        SpecialcarbineMk2Camo05 = 3999758885,
        SpecialcarbineMk2Camo06 = 3750812792,
        SpecialcarbineMk2Camo07 = 172765678,
        SpecialcarbineMk2Camo08 = 2312089847,
        SpecialcarbineMk2Camo09 = 2072122460,
        SpecialcarbineMk2Camo10 = 2308747125,
        SpecialcarbineMk2CamoInd01 = 1377355801,

        CarbinerifleMk2Clip01 = 1283078430,
        CarbinerifleMk2Clip02 = 1574296533,
        CarbinerifleMk2ClipTracer = 391640422,
        CarbinerifleMk2ClipIncendiary = 1025884839,
        CarbinerifleMk2ClipArmorpiercing = 626875735,
        CarbinerifleMk2ClipFmj = 1141059345,
        AtCrBarrel01 = 2201368575,
        AtCrBarrel02 = 2335983627,
        CarbinerifleMk2Camo = 1272803094,
        CarbinerifleMk2Camo02 = 1080719624,
        CarbinerifleMk2Camo03 = 792221348,
        CarbinerifleMk2Camo04 = 3842785869,
        CarbinerifleMk2Camo05 = 3548192559,
        CarbinerifleMk2Camo06 = 2250671235,
        CarbinerifleMk2Camo07 = 4095795318,
        CarbinerifleMk2Camo08 = 2866892280,
        CarbinerifleMk2Camo09 = 2559813981,
        CarbinerifleMk2Camo10 = 1796459838,
        CarbinerifleMk2CamoInd01 = 3663056191,

        AssaultrifleMk2Clip01 = 2249208895,
        AssaultrifleMk2Clip02 = 3509242479,
        AssaultrifleMk2ClipTracer = 4012669121,
        AssaultrifleMk2ClipIncendiary = 4218476627,
        AssaultrifleMk2ClipArmorpiercing = 2816286296,
        AssaultrifleMk2ClipFmj = 1675665560,
        AtArAfgrip02 = 2640679034,
        AtArFlsh = 2076495324,
        AtSights = 1108334355,
        AtScopeMacroMk2 = 77277509,
        AtScopeMediumMk2 = 3328927042,
        AtArSupp02 = 2805810788,
        AtMuzzle01 = 3113485012,
        AtMuzzle02 = 3362234491,
        AtMuzzle03 = 3725708239,
        AtMuzzle04 = 3968886988,
        AtMuzzle05 = 48731514,
        AtMuzzle06 = 880736428,
        AtMuzzle07 = 1303784126,
        AtArBarrel01 = 1134861606,
        AtArBarrel02 = 1447477866,
        AssaultrifleMk2Camo = 2434475183,
        AssaultrifleMk2Camo02 = 937772107,
        AssaultrifleMk2Camo03 = 1401650071,
        AssaultrifleMk2Camo04 = 628662130,
        AssaultrifleMk2Camo05 = 3309920045,
        AssaultrifleMk2Camo06 = 3482022833,
        AssaultrifleMk2Camo07 = 2847614993,
        AssaultrifleMk2Camo08 = 4234628436,
        AssaultrifleMk2Camo09 = 2088750491,
        AssaultrifleMk2Camo10 = 2781053842,
        AssaultrifleMk2CamoInd01 = 3115408816,

        MinismgClip01 = 2227745491,
        MinismgClip02 = 2474561719,

        SmgMk2Clip01 = 1277460590,
        SmgMk2Clip02 = 3112393518,
        SmgMk2ClipTracer = 2146055916,
        SmgMk2ClipIncendiary = 3650233061,
        SmgMk2ClipHollowpoint = 974903034,
        SmgMk2ClipFmj = 190476639,
        AtSightsSmg = 2681951826,
        AtScopeMacro02SmgMk2 = 3842157419,
        AtScopeSmallSmgMk2 = 1038927834,
        AtPiSupp = 3271853210,
        AtSbBarrel01 = 3641720545,
        AtSbBarrel02 = 2774849419,
        SmgMk2Camo = 3298267239,
        SmgMk2Camo02 = 940943685,
        SmgMk2Camo03 = 1263226800,
        SmgMk2Camo04 = 3966931456,
        SmgMk2Camo05 = 1224100642,
        SmgMk2Camo06 = 899228776,
        SmgMk2Camo07 = 616006309,
        SmgMk2Camo08 = 2733014785,
        SmgMk2Camo09 = 572063080,
        SmgMk2Camo10 = 1170588613,
        SmgMk2CamoInd01 = 966612367, 

        CombatmgMk2Clip01 = 1227564412,
        CombatmgMk2Clip02 = 400507625,
        CombatmgMk2ClipTracer = 4133787461,
        CombatmgMk2ClipIncendiary = 3274096058,
        CombatmgMk2ClipArmorpiercing = 696788003,
        CombatmgMk2ClipFmj = 1475288264,
        AtScopeSmallMk2 = 1060929921,
        AtMgBarrel01 = 3276730932,
        AtMgBarrel02 = 3051509595,
        CombatmgMk2Camo = 1249283253,
        CombatmgMk2Camo02 = 3437259709,
        CombatmgMk2Camo03 = 3197423398,
        CombatmgMk2Camo04 = 1980349969,
        CombatmgMk2Camo05 = 1219453777,
        CombatmgMk2Camo06 = 2441508106,
        CombatmgMk2Camo07 = 2220186280,
        CombatmgMk2Camo08 = 457967755,
        CombatmgMk2Camo09 = 235171324,
        CombatmgMk2Camo10 = 42685294,
        CombatmgMk2CamoInd01 = 3607349581,

        //rage
        RailgunClip01 = 59044840,
        CombatPistolClip01 = 119648377,
        KnuckleVarmodPlayer = 146278587,
        AtArAfGrip = 202788691,
        HeavyPistolClip01 = 222992026,
        MicroSMGClip02 = 283556395,
        GrenadeLauncherClip01 = 296639639,
        MarksmanRifleVarmodLuxe = 371102273,
        RevolverVarmodBoss = 384708672,
        AtScopeLargeFixedZoom = 471997210,
        GusenbergClip01 = 484812453,
        Pistol50Clip01 = 580369945,
        APPistolClip02 = 614078421,
        SMGClip01 = 643254679,
        SMGVarmodLuxe = 663170192,
        AssaultSMGVarmodLowrider = 663517359,
        DBShotgunClip01 = 703231006,
        APPistolClip01 = 834974250,
        HeavyShotgunClip01 = 844049759,
        CombatPDWClip02 = 860508675,
        VintagePistolClip02 = 867832552,
        SMGClip02 = 889808635,
        AtPiFlsh = 899381934,
        AdvancedRifleVarmodLuxe = 930927479,
        AtScopeSmall02 = 1006677997,
        AtScopeMacro02 = 1019656791,
        KnuckleVarmodLove = 1062111910,
        SniperRifleVarmodLuxe = 1077065191,
        CombatPDWClip01 = 1125642654,
        VintagePistolClip01 = 1168357051,
        MachinePistolClip01 = 1198425599,
        HeavySniperClip01 = 1198478068,
        MicroSMGVarmodLuxe = 1215999497,
        RPGClip01 = 1319465907,
        AssaultRifleVarmodLuxe = 1319990579,
        MusketClip01 = 1322387263,
        KnuckleVarmodDollar = 1351683121,
        CompactRifleClip01 = 1363085923,
        CompactRifleClip02 = 1509923832,
        SwitchbladeVarmodVar1 = 1530822070,
        HeavyPistolClip02 = 1694090795,
        AtPiSupp02 = 1709866683,
        SpecialCarbineClip03 = 1801039530,
        CombatPDWClip03 = 1857603803,
        SpecialCarbineVarmodLowrider = 1929467122,
        AtRailCover01 = 1967214384,
        Pistol50VarmodLuxe = 2008591151,
        SMGClip03 = 2043113590,
        HeavyPistolVarmodLuxe = 2053798779,
        KnuckleVarmodVagos = 2062808965,
        SNSPistolClip02 = 2063610803,
        SpecialCarbineClip02 = 2089537806,
        KnuckleVarmodHate = 2112683568,
        SNSPistolVarmodLowrider = 2150886575,
        MGClip02 = 2182449991,
        AtArSupp = 2205435306,
        SawnoffShotgunVarmodLuxe = 2242268665,
        AssaultShotgunClip02 = 2260565874,
        HeavyShotgunClip03 = 2294798931,
        AssaultSMGClip01 = 2366834608,
        AdvancedRifleClip02 = 2395064697,
        CarbineRifleClip02 = 2433783441,
        SwitchbladeVarmodBase = 2436343040,
        CombatMGVarmodLowrider = 2466172125,
        FlareGunClip01 = 2481569177,
        RevolverVarmodGoon = 2492708877,
        AssaultShotgunClip01 = 2498239431,
        HeavyShotgunClip02 = 2535257853,
        KnuckleVarmodDiamond = 2539772380,
        APPistolVarmodLuxe = 2608252716,
        SniperRifleClip01 = 2613461129,
        AtScopeMacro = 2637152041,
        CarbineRifleClip01 = 2680042476,
        AtScopeMedium = 2698550338,
        PumpShotgunVarmodLowrider = 2732039643,
        BullpupRifleVarmodLow = 2824322168,
        MachinePistolClip03 = 2850671348,
        AtScopeSmall = 2855028148,
        AssaultRifleClip02 = 2971750299,
        BullpupRifleClip02 = 3009973007,
        MachinePistolClip02 = 3106695545,
        CarbineRifleClip03 = 3127044405,
        AssaultSMGClip02 = 3141985303,
        AtScopeMax = 3159677559,
        AssaultRifleClip01 = 3193891350,
        BullpupRifleClip01 = 3315675008,
        PoliceTorchFlashlight = 3315797997,
        CompactRifleClip03 = 3322377230,
        KnuckleVarmodPimp = 3323197061,
        CombatPistolVarmodLowrider = 3328527730,
        SpecialCarbineClip01 = 3334989185,
        SawnoffShotgunClip01 = 3352699429,
        MinigunClip01 = 3370020614,
        BullpupShotgunClip01 = 3377353998,
        MicroSMGClip01 = 3410538224,
        MarksmanPistolClip01 = 3416146413,
        MarksmanRifleClip02 = 3439143621,
        PumpShotgunClip01 = 3513717816,
        AtScopeLarge = 3527687644,
        CombatPistolClip02 = 3598405421,
        CombatMGClip02 = 3603274966,
        MGVarmodLowrider = 3604658878,
        PistolVarmodLuxe = 3610841222,
        MarksmanRifleClip01 = 3627761985,
        CarbineRifleVarmodLuxe = 3634075224,
        Pistol50Clip02 = 3654528146,
        AssaultRifleClip03 = 3689981245,
        FlashlightLight = 3719772431,
        CombatMGClip01 = 3791631178,
        KnuckleVarmodKing = 3800804335,
        FireworkClip01 = 3840197261,
        AtSrSupp = 3859329886,
        SwitchbladeVarmodVar2 = 3885209186,
        RevolverClip01 = 3917905123,
        GusenbergClip02 = 3939025520,
        PistolClip02 = 3978713628,
        KnuckleVarmodBallas = 4007263587,
        KnuckleVarmodBase = 4081463091,
        MGClip01 = 4097109892,
        HomingLauncherClip01 = 4162006335,
        SNSPistolClip01 = 4169150169,
        AdvancedRifleClip01 = 4203716879,
        PistolClip01 = 4275109233,
        Invalid = uint.MaxValue
    }
}