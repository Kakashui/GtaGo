using GTANetworkAPI;
using Whistler.Core.Character;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Whistler.Inventory.Models;
using System.IO;
using Whistler.Core.nAccount;
using Whistler.Infrastructure.DataAccess;
using Whistler.Families.Models;
using Whistler.Families;
using Whistler.GUI.Tips;
using Whistler.Core;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.GUI.Documents.Enums;
using Whistler.SDK;
using Whistler.Core.CustomSync.Attachments;
using Whistler.VehicleSystem;
using Whistler.Fractions;
using Whistler.Phone;
using Whistler.Entities;
using Whistler.Customization.Models;
using Whistler.Fractions.Models;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.MoneySystem.Models;
using Whistler.MoneySystem;
using Whistler.MoneySystem.Interface;
using Whistler.Common.Interfaces;
using Whistler.Common;

namespace Whistler.Helpers
{
    public static class ClientExtensions
    {
        public static void TriggerEventSafe(this Player player, string eventName, params object[] args)
        {
            Trigger.ClientEvent(player, eventName, args);
        }

        public static void TriggerEventWithLargeList(this Player player, string eventName, IEnumerable<object> objects, params object[] args)
        {
            const int deliversPerEvent = 40;

            var eventsShouldBeSended = objects.Count() / deliversPerEvent;
            for (int i = 0; i <= eventsShouldBeSended; i++)
            {
                player.TriggerEventSafe(eventName, JsonConvert.SerializeObject(objects.Skip(i * deliversPerEvent).Take(deliversPerEvent).ToList()), args);
            }
        }

        public static void TriggerEventWithLargeList(this Player player, int deliversPerEvent, string eventName, IEnumerable<object> objects, params object[] args)
        {
            var eventsShouldBeSended = objects.Count() / deliversPerEvent;
            for (int i = 0; i <= eventsShouldBeSended; i++)
            {
                player.TriggerEventSafe(eventName, JsonConvert.SerializeObject(objects.Skip(i * deliversPerEvent).Take(deliversPerEvent).ToList()), args);
            }
        }

        public static void TriggerCefAction(this Player player, string storeAction, object data)
        {
            Trigger.ClientEvent(player, "gui:dispatch", storeAction, data);
        }

        public static void TriggerCefEvent(this Player player, string storeFunction, object data)
        {
            Trigger.ClientEvent(player, "gui:setData", storeFunction, data);
        }

        public static void TriggerCefEventWithLargeList(this Player player, int deliversPerEvent, string storeFunction, IEnumerable<object> objects)
        {
            var eventsShouldBeSended = objects.Count() / deliversPerEvent;
            for (int i = 0; i <= eventsShouldBeSended; i++)
            {
                Trigger.ClientEvent(player, "gui:setData", storeFunction, JsonConvert.SerializeObject(objects.Skip(i * deliversPerEvent).Take(deliversPerEvent).ToList()));
            }
        }
        public static void OpenDialog(this Player player, string key, string question)
        {
            Trigger.ClientEvent(player, "openDialog", key, question);
        }

        public static void OpenInput(this Player player, string text, string inputMask, int inputCountSymbol, string key)
        {
            Trigger.ClientEvent(player, "openInput", text, inputMask, inputCountSymbol, key);
        }

        #region Player markers, blips and waypoints
        public static void CreateClientCheckpoint(this Player player, int uid, int type, Vector3 position, float scale, uint dimension, Color color, Vector3 direction = null)
        {
            player.TriggerEvent("createCheckpoint", uid, type, position, scale, dimension, color.Red, color.Green, color.Blue, direction);
        }
        public static void CreateClientMarker(this Player player, int uid, int type, Vector3 position, float scale, uint dimension, Color color, Vector3 rotation)
        {
            player.TriggerEvent("createMarker", uid, type, position, scale, dimension, color.Red, color.Green, color.Blue, rotation);
        }

        public static void DeleteClientMarker(this Player player, int uid)
        {
            player.TriggerEvent("deleteCheckpoint", uid);
        }

        public static void CreateClientBlip(this Player player, int uid, int sprite, string name, Vector3 position, float scale, int color, uint dimension)
        {
            player.TriggerEvent("createBlip", uid, sprite, name, position, scale, color, dimension);
        }

        public static void DeleteClientBlip(this Player player, int uid)
        {
            player.TriggerEvent("deleteBlip", uid);
        }
        public static void CreateWaypoint(this Player player, Vector3 position)
        {
            player.TriggerEvent("createWaypoint", position.X, position.Y);
        }
        #endregion

        public static PhoneTemporaryData GetPhone(this Player player) => player.GetCharacter().PhoneTemporary;

        public static bool IsLogged(this Player player)
        {
            return (player as PlayerGo)?.Logged() ?? false;
        }
        public static bool IsLogged(this PlayerGo player)
        {
            return player?.Logged() ?? false;
        }


        public static Character GetCharacter(this Player player)
        {
            return (player as PlayerGo)?.Character;
        }

        internal static CheckingAccount GetBankAccount(this Player player)
        {
            return (player as PlayerGo)?.Character.BankModel;
        }

        internal static IMoneyOwner GetMoneyPayment(this Player player, PaymentsType payments, IMoneyOwner defaultValue = null)
        {
            switch (payments)
            {
                case PaymentsType.Cash:
                    return player.GetCharacter();
                case PaymentsType.Card:
                    return player.GetBankAccount();
            }
            return defaultValue;
        }

        public static PlayerGo GetPlayerGo(this Player player)
        {
            return player as PlayerGo;
        }
        public static bool GetGender(this Player player) {
            var custom = player.GetCustomization();
            return custom == null ? Main.PlayerSlotsInfo[player.GetCharacter().UUID].Gender : custom.Gender;
        }

        public static CustomizationModel GetCustomization(this Player player) => player.GetCharacter()?.Customization;

        public static Account GetAccount(this Player player) => player.GetPlayerGo()?.Account;

        public static bool IsAdmin(this Player player) => player.IsLogged() && player.GetCharacter()?.AdminLVL > 0;

        //public static T GetData<T>(this Player player, string key) => (T)player.GetData(key);


        internal static Family GetFamily(this Player player) => FamilyManager.GetFamily(player.GetCharacter().FamilyID);
        internal static Family GetFamily(this PlayerGo player) => FamilyManager.GetFamily(player.Character.FamilyID);
        internal static Fraction GetFraction(this Player player) => Manager.GetFraction(player.GetCharacter()?.FractionID ?? 0);
        internal static IOrganization GetOrganization(this Player player, OrganizationType type)
        {
            switch (type)
            {
                case OrganizationType.Family:
                    return player.GetFamily();
                case OrganizationType.Fraction:
                    return player.GetFraction();
                default:
                    return null;
            }
        }
        internal static IOrganization GetOrganization(this PlayerGo player, OrganizationType type)
        {
            switch (type)
            {
                case OrganizationType.Family:
                    return FamilyManager.GetFamily(player.Character.FamilyID);
                case OrganizationType.Fraction:
                    return Manager.GetFraction(player.Character.FractionID);
                default:
                    return null;
            }
        }
        internal static Business GetBusiness(this Player player) => BusinessManager.GetBusinessByOwner(player);
        public static List<Player> GetPlayersInRange(this Player player, float range, bool includeMySelf = false)
        {
            var players = NAPI.Pools.GetAllPlayers();

            return players.Where(
                p => includeMySelf ? 
                    p.IsLogged() && player.Dimension == p.Dimension && p.Position.DistanceTo(player.Position) < range :
                    p != player && p.IsLogged() && player.Dimension == p.Dimension && p.Position.DistanceTo(player.Position) < range).ToList();
        }
        public static PlayerGo GetNearestPlayer(this PlayerGo player, int radius)
        {
            List<Player> players = player.GetPlayersInRange(radius);
            if (players.Count == 0) return null;
            PlayerGo nearestPlayer = players[0] as PlayerGo;
            if(players.Count > 1)
            {
                for (int i = 1; i < players.Count; i++)
                {
                    var p = players[i];
                    if (player.Position.DistanceTo(p.Position) < player.Position.DistanceTo(nearestPlayer.Position)) 
                        nearestPlayer = p as PlayerGo;
                }
            }           

            return nearestPlayer;
        }
        public static void SendExpUpdate(this Player player)
        {
            player.TriggerEvent("exp:upd", player.GetCharacter().EXP, player.GetCharacter().LVL);
        }
        
        public static void SendTip(this Player player, string tip)
        {
            Tip.SendTip(player, tip);
        }

        /// <summary>
        /// player move
        /// </summary>
        /// <param name="player">player</param>
        /// <param name="position">position (null - if tp on client)</param>
        /// <param name="second">time to stop anticheat (+5s)</param>
        public static void ChangePosition(this Player player, Vector3 position, int second = 0)
        {
            if (position != null)
            {
                if (player.Position.DistanceTo(position) > 5)
                    player.SetData("lastTeleport", System.DateTime.Now.AddSeconds(second + 5));
                player.TriggerEvent("teleport:newPos", position);
                //player.Position = position;
            }
            else 
                player.SetData("lastTeleport", System.DateTime.Now.AddSeconds(second + 5));
        }

        public static void ChangePositionWithCar(this Player player, Vector3 position, Vector3 rotation, int second = 0)
        {
            var vehicle = player.Vehicle;
            if (vehicle == null)
                return;
            var players = vehicle.Occupants.ToList();
            foreach (var pl in players)
            {
                (pl as Player)?.ChangePosition(null);
            }
            Trigger.ClientEvent(player, "player:teleportInCar", position, 1000);
            if (rotation != null)
                vehicle.Rotation = rotation;
        }

        public static void SendTODemorgan(this Player player)
        {
            player.ChangePosition(Admin.DemorganPosition + new Vector3(0, 0, 1.5));
            player.TriggerEvent("admin:toDemorgan", true);
        }

        public static void CreateTemporaryInventory(this Player player, int maxWeight, int size)
        {
            if (!player.IsLogged()) return;
            InventoryModel tempInventory = new InventoryModel(maxWeight, size, InventoryTypes.Personal, true);
            player.GetCharacter().TempInventory = tempInventory;
            player.GetInventory().Subscribe(player);
            player.SyncInventoryId();

        }
        public static void DeleteTemporaryInventory(this PlayerGo player)
        {
            if (!player.IsLogged()) return;
            player.GetCharacter().TempInventory = null;
            player.GetInventory().Subscribe(player);
            player.SyncInventoryId();
        }
        public static void CreateTemporaryEquip(this PlayerGo player)
        {
            if (!player.IsLogged()) return;
            Equip tempEquip = new Equip(true);
            player.GetCharacter().TempEquip = tempEquip;
            player.GetEquip().Subscribe(player);
            player.GetEquip().Update(true);
        }
        public static void DeleteTemporaryEquip(this PlayerGo player)
        {
            if (!player.IsLogged()) return;
            player.GetCharacter().TempEquip = null;
            player.GetEquip().Subscribe(player);
            player.GetEquip().Update(true);
        }

        #region Licenses
        public static bool CheckLic(this Player player, LicenseName license)
        {
            if (!player.IsLogged()) return false;
            var lic = player.GetCharacter().Licenses.FirstOrDefault(item => item.Name == license && item.DateEnd > System.DateTime.Now);
            return lic != null;
        }

        public static bool GiveLic(this Player player, IEnumerable<LicenseName> licenses)
        {
            if (!player.IsLogged()) return false;
            bool res = false;
            foreach (var lic in licenses)
            {
                var currentLic = player.GetCharacter().Licenses.FirstOrDefault(item => item.Name == lic);
                if (currentLic == null)
                {
                    player.GetCharacter().Licenses.Add(new GUI.Documents.Models.License(lic));
                    res = true;
                }
                else if (currentLic.DateEnd < System.DateTime.Now)
                {
                    currentLic.ToExtend();
                    res = true;
                }
            }
            return res;
        }
        public static bool GiveLic(this Player player, LicenseName license, int days = 0)
        {
            if (!player.IsLogged()) return false;
            var currentLic = player.GetCharacter().Licenses.FirstOrDefault(item => item.Name == license);
            if (currentLic == null)
            {
                player.GetCharacter().Licenses.Add(new GUI.Documents.Models.License(license));
                return true;
            }
            else if (currentLic.DateEnd < System.DateTime.Now)
            {
                currentLic.ToExtend(days);
                return true;
            }
            return false;
        }
        public static bool TakeLic(this Player player, List<LicenseName> licenses)
        {
            if (!player.IsLogged()) return false;
            bool res = false;
            foreach (var lic in licenses)
            {
                var currentLic = player.GetCharacter().Licenses.FirstOrDefault(item => item.Name == lic);
                if (currentLic != null)
                {
                    res = res || currentLic.DateEnd > System.DateTime.Now;
                    player.GetCharacter().Licenses.Remove(currentLic);
                }
            }
            return res;
        }
        public static bool TakeLic(this Player player, LicenseName license)
        {
            if (!player.IsLogged()) return false;
            bool res = false;
            var currentLic = player.GetCharacter().Licenses.FirstOrDefault(item => item.Name == license);
            if (currentLic != null)
            {
                res = currentLic.DateEnd > System.DateTime.Now;
                player.GetCharacter().Licenses.Remove(currentLic);
            }
            return res;
        }
        #endregion

        #region Cuff & Follow
        public static void FollowTo(this Player player, Player target)
        {
            if (!player.IsLogged() || !target.IsLogged()) return;
            target.GetCharacter().Follower = player;
            player.GetCharacter().Following = target;
            Trigger.ClientEvent(player, "setFollow", true, target);
        }

        public static void UnFollow(this Player player)
        {
            if (!player.IsLogged()) return;
            Player following = player.GetCharacter().Following;
            player.GetCharacter().Following = null;
            Trigger.ClientEvent(player, "setFollow", false);
            if (following.IsLogged())
                following.GetCharacter().Follower = null;
        }

        public static void LetGoFollower(this Player player, bool notify = false)
        {
            if (!player.IsLogged()) return;
            Player follower = player.GetCharacter().Follower;
            player.GetCharacter().Follower = null;
            if (follower.IsLogged())
            {
                follower.GetCharacter().Following = null;
                Trigger.ClientEvent(follower, "setFollow", false);
                if (notify)
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_256".Translate(follower.GetCharacter().UUID), 3000);
                    Notify.Send(follower, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_241".Translate(player.GetCharacter().UUID), 3000);
                }
            }
        }
        public static void Cuffed(this PlayerGo player, bool byCop)
        {
            if (!player.IsLogged()) return;
            player.GetCharacter().Cuffed = true;
            player.GetCharacter().CuffedCop = byCop;
            player.GetCharacter().CuffedGang = !byCop;
            Trigger.ClientEvent(player, "blockMove", true);
            Trigger.ClientEvent(player, "CUFFED", true);
            Main.OnAntiAnim(player);
            NAPI.Player.PlayPlayerAnimation(player, 49, "mp_arresting", "idle");
            AttachmentSync.AddAttachment(player, AttachId.Cuffs);
        }

        public static void UnCuffed(this PlayerGo player)
        {
            Trigger.ClientEvent(player, "CUFFED", false);
            player.GetCharacter().Cuffed = false;
            player.GetCharacter().CuffedCop = false;
            player.GetCharacter().CuffedGang = false;
            NAPI.Player.StopPlayerAnimation(player);
            AttachmentSync.RemoveAttachment(player, AttachId.Cuffs);
            Trigger.ClientEvent(player, "blockMove", false);
            Main.OffAntiAnim(player);
        }
        #endregion

        public static Vehicle GetTempVehicle(this Player player, VehicleAccess vehicleType)
        {
            return player?.GetCharacter()?.TempVehicles.GetValueOrDefault(vehicleType);
        }

        public static bool TempVehicleIsExist(this Player player, VehicleAccess vehicleType)
        {
            return player?.GetTempVehicle(vehicleType) != null;
        }

        public static bool AddTempVehicle(this Player player, Vehicle vehicle, VehicleAccess vehicleType)
        {
            var character = player.GetCharacter();
            if (character == null) return false;
            if (character.TempVehicles.ContainsKey(vehicleType)) return false;
            character.TempVehicles.Add(vehicleType, vehicle);
            return true;
        }

        public static Vehicle RemoveTempVehicle(this Player player, VehicleAccess vehicleType)
        {
            var character = player.GetCharacter();
            if (character == null) return null;
            if (!character.TempVehicles.ContainsKey(vehicleType)) return null;
            var vehicle = character.TempVehicles[vehicleType];
            character.TempVehicles.Remove(vehicleType);
            return vehicle;
        }

        public static void CustomSetIntoVehicle(this Player player, Vehicle vehicle, int seatId)
        {
            player.Dimension = vehicle.Dimension;
            player.TriggerEvent("teleport:toVehicle", vehicle, vehicle.Position, seatId);
        }

        internal static bool CheckInviteToFamily(this Player player, Family family)
        {
            if (!player.IsLogged())
                return false;
            if (player.GetFamily() != null)
                return false;
            if (Manager.isHaveFraction(player))
            {
                if (family == null)
                    return false;
                switch (family.OrgActiveType)
                {
                    case OrgActivityType.Crime:
                        return false;
                }
            }
            return true;
        }
        public static bool CheckInviteToFraction(this Player player, int fractionId)
        {
            if (!player.IsLogged())
                return false;
            if (Manager.isHaveFraction(player))
                return false;
            var family = player.GetFamily();
            if (family != null)
            {
                switch (family.OrgActiveType)
                {
                    case OrgActivityType.Crime:
                        return false;
                }
            }
            return true;
        }
    }
}
