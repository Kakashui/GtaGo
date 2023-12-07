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
using Whistler.VehicleSystem.Models;

namespace Whistler.Helpers
{
    public static class ClientExtensions
    {
        private const int deliversPerEvent = 40;
        public static void TriggerEventSafe(this ExtPlayer player, string eventName, params object[] args)
        {
            SafeTrigger.ClientEvent(player, eventName, args);
        }

        public static void TriggerEventWithLargeList(this ExtPlayer player, string eventName, IEnumerable<object> objects, params object[] args)
        {
            var eventsShouldBeSended = objects.Count() / deliversPerEvent;
            for (int i = 0; i <= eventsShouldBeSended; i++)
            {
                player.TriggerEventSafe(eventName, JsonConvert.SerializeObject(objects.Skip(i * deliversPerEvent).Take(deliversPerEvent).ToList()), args);
            }
        }

        public static void TriggerEventWithLargeList(this ExtPlayer player, int deliversPerEvent, string eventName, IEnumerable<object> objects, params object[] args)
        {
            var eventsShouldBeSended = objects.Count() / deliversPerEvent;
            for (int i = 0; i <= eventsShouldBeSended; i++)
            {
                player.TriggerEventSafe(eventName, JsonConvert.SerializeObject(objects.Skip(i * deliversPerEvent).Take(deliversPerEvent).ToList()), args);
            }
        }

        public static void TriggerCefAction(this ExtPlayer player, string storeAction, object data)
        {
            SafeTrigger.ClientEvent(player, "gui:dispatch", storeAction, data);
        }

        public static void TriggerCefEvent(this ExtPlayer player, string storeFunction, object data)
        {
            SafeTrigger.ClientEvent(player, "gui:setData", storeFunction, data);
        }

        public static void TriggerCefEventWithLargeList(this ExtPlayer player, int deliversPerEvent, string storeFunction, IEnumerable<object> objects)
        {
            var eventsShouldBeSended = objects.Count() / deliversPerEvent;
            for (int i = 0; i <= eventsShouldBeSended; i++)
            {
                SafeTrigger.ClientEvent(player, "gui:setData", storeFunction, JsonConvert.SerializeObject(objects.Skip(i * deliversPerEvent).Take(deliversPerEvent).ToList()));
            }
        }
        public static void OpenDialog(this ExtPlayer player, string key, string question)
        {
            SafeTrigger.ClientEvent(player, "openDialog", key, question);
        }

        public static void OpenInput(this ExtPlayer player, string text, string inputMask, int inputCountSymbol, string key)
        {
            SafeTrigger.ClientEvent(player, "openInput", text, inputMask, inputCountSymbol, key);
        }

        #region Player markers, blips and waypoints
        public static void CreateClientCheckpoint(this ExtPlayer player, int uid, int type, Vector3 position, float scale, uint dimension, Color color, Vector3 direction = null)
        {
            SafeTrigger.ClientEvent(player,"createCheckpoint", uid, type, position, scale, dimension, color.Red, color.Green, color.Blue, direction);
        }
        public static void CreateClientMarker(this ExtPlayer player, int uid, int type, Vector3 position, float scale, uint dimension, Color color, Vector3 rotation)
        {
            SafeTrigger.ClientEvent(player,"createMarker", uid, type, position, scale, dimension, color.Red, color.Green, color.Blue, rotation);
        }

        public static void DeleteClientMarker(this ExtPlayer player, int uid)
        {
            SafeTrigger.ClientEvent(player,"deleteCheckpoint", uid);
        }

        public static void CreateClientBlip(this ExtPlayer player, int uid, int sprite, string name, Vector3 position, float scale, int color, uint dimension)
        {
            SafeTrigger.ClientEvent(player,"createBlip", uid, sprite, name, position, scale, color, dimension);
        }

        public static void DeleteClientBlip(this ExtPlayer player, int uid)
        {
            SafeTrigger.ClientEvent(player,"deleteBlip", uid);
        }
        public static void CreateWaypoint(this ExtPlayer player, Vector3 position)
        {
            SafeTrigger.ClientEvent(player,"createWaypoint", position.X, position.Y);
        }
        #endregion

        public static PhoneTemporaryData GetPhone(this ExtPlayer player) => player.Character.PhoneTemporary;

        public static bool IsLogged(this ExtPlayer player)
        {
            return player?.Logged() ?? false;
        }

        internal static IMoneyOwner GetMoneyPayment(this ExtPlayer player, PaymentsType payments, IMoneyOwner defaultValue = null)
        {
            return payments switch
            {
                PaymentsType.Cash => player.Character,
                PaymentsType.Card => player.Character.BankModel,
                _ => defaultValue,
            };
        }

        public static bool GetGender(this ExtPlayer player) 
        {
            var custom = player.Character.Customization;
            return custom == null ? Main.PlayerSlotsInfo[player.Character.UUID].Gender : custom.Gender;
        }

        public static bool IsAdmin(this ExtPlayer player) => player.IsLogged() && player.Character?.AdminLVL > 0;
        internal static Family GetFamily(this ExtPlayer player) => FamilyManager.GetFamily(player);
        internal static Fraction GetFraction(this ExtPlayer player) => Manager.GetFraction(player);

        internal static IOrganization GetOrganization(this ExtPlayer player, OrganizationType type)
        {
            switch (type)
            {
                case OrganizationType.Family:
                    return FamilyManager.GetFamily(player);
                case OrganizationType.Fraction:
                    return Manager.GetFraction(player);
                default:
                    return null;
            }
        }
        internal static Business GetBusiness(this ExtPlayer player) => BusinessManager.GetBusinessByOwner(player);
        public static List<ExtPlayer> GetPlayersInRange(this ExtPlayer player, float range, bool includeMySelf = false)
        {
            List<ExtPlayer> players = Trigger.GetAllPlayers();

            return players.Where(
                p => includeMySelf ? 
                    p.IsLogged() && player.Session?.Dimension == p.Dimension && p.Position.DistanceTo(player.Position) < range :
                    p != player && p.IsLogged() && player.Session?.Dimension == p.Dimension && p.Position.DistanceTo(player.Position) < range).ToList();
        }
        public static ExtPlayer GetNearestPlayer(this ExtPlayer player, int radius)
        {
            List<ExtPlayer> players = player.GetPlayersInRange(radius);
            if (players.Count == 0) return null;
            ExtPlayer nearestPlayer = players[0];
            if(players.Count > 1)
            {
                for (int i = 1; i < players.Count; i++)
                {
                    var p = players[i];
                    if (player.Position.DistanceTo(p.Position) < player.Position.DistanceTo(nearestPlayer.Position)) 
                        nearestPlayer = p;
                }
            }           

            return nearestPlayer;
        }
        public static void SendExpUpdate(this ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"exp:upd", player.Character.EXP, player.Character.LVL);
        }
        
        public static void SendTip(this ExtPlayer player, string tip)
        {
            Tip.SendTip(player, tip);
        }

        /// <summary>
        /// player move
        /// </summary>
        /// <param name="player">player</param>
        /// <param name="position">position (null - if tp on client)</param>
        /// <param name="second">time to stop anticheat (+5s)</param>
        public static void ChangePosition(this ExtPlayer player, Vector3 position, int second = 0)
        {
            if (position != null)
            {
                if (player.Position.DistanceTo(position) > 5)
                    SafeTrigger.SetData(player, "lastTeleport", System.DateTime.Now.AddSeconds(second + 5));
                SafeTrigger.ClientEvent(player, "teleport:newPos", position);
                player.Session.Position = position;
                //SafeTrigger.UpdatePosition(player,  position);
            }
            else SafeTrigger.SetData(player, "lastTeleport", System.DateTime.Now.AddSeconds(second + 5));
        }

        public static void ChangePositionWithCar(this ExtPlayer player, Vector3 position, Vector3 rotation, int second = 0)
        {
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null)
                return;

            if (vehicle.AllOccupants.Any())
            {
                List<ExtPlayer> players = vehicle.AllOccupants.Values.ToList();
                foreach (ExtPlayer pl in players)
                {
                    if (pl == null) continue;

                    pl.ChangePosition(null);
                }
            }
            SafeTrigger.ClientEvent(player, "player:teleportInCar", position, 1000);
            if (rotation != null) vehicle.Rotation = rotation;
        }

        public static void SendTODemorgan(this ExtPlayer player)
        {
            player.ChangePosition(Admin.DemorganPosition + new Vector3(0, 0, 1.5));
            SafeTrigger.ClientEvent(player,"admin:toDemorgan", true);
        }

        public static void CreateTemporaryInventory(this ExtPlayer player, int maxWeight, int size)
        {
            if (!player.IsLogged()) return;
            InventoryModel tempInventory = new InventoryModel(maxWeight, size, InventoryTypes.Personal, true);
            player.Character.TempInventory = tempInventory;
            player.GetInventory().Subscribe(player);
            player.SyncInventoryId();

        }
        public static void DeleteTemporaryInventory(this ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            player.Character.TempInventory = null;
            player.GetInventory().Subscribe(player);
            player.SyncInventoryId();
        }
        public static void CreateTemporaryEquip(this ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            Equip tempEquip = new Equip(true);
            player.Character.TempEquip = tempEquip;
            player.GetEquip().Subscribe(player);
            player.GetEquip().Update(true);
        }
        public static void DeleteTemporaryEquip(this ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            player.Character.TempEquip = null;
            player.GetEquip().Subscribe(player);
            player.GetEquip().Update(true);
        }

        #region Licenses
        public static bool CheckLic(this ExtPlayer player, LicenseName license)
        {
            if (!player.IsLogged()) return false;
            var lic = player.Character.Licenses.FirstOrDefault(item => item.Name == license && item.DateEnd > System.DateTime.Now);
            return lic != null;
        }

        public static bool GiveLic(this ExtPlayer player, IEnumerable<LicenseName> licenses)
        {
            if (!player.IsLogged()) return false;
            bool res = false;
            foreach (var lic in licenses)
            {
                var currentLic = player.Character.Licenses.FirstOrDefault(item => item.Name == lic);
                if (currentLic == null)
                {
                    player.Character.Licenses.Add(new GUI.Documents.Models.License(lic));
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
        public static bool GiveLic(this ExtPlayer player, LicenseName license, int days = 0)
        {
            if (!player.IsLogged()) return false;

            GUI.Documents.Models.License currentLic = player.Character.Licenses.FirstOrDefault(item => item.Name == license);
            if (currentLic == null)
            {
                player.Character.Licenses.Add(new GUI.Documents.Models.License(license));
                return true;
            }
            else if (currentLic.DateEnd < System.DateTime.Now)
            {
                currentLic.ToExtend(days);
                return true;
            }
            return false;
        }

        public static bool TakeLic(this ExtPlayer player, IEnumerable<LicenseName> licenses)
        {
            if (!player.IsLogged()) return false;
            bool res = false;
            foreach (var lic in licenses)
            {
                var currentLic = player.Character.Licenses.FirstOrDefault(item => item.Name == lic);
                if (currentLic == null)
                {
                    res = res || currentLic.DateEnd > System.DateTime.Now;
                    player.Character.Licenses.Remove(currentLic);
                }
            }
            return res;
        }
        public static bool TakeLic(this ExtPlayer player, List<LicenseName> licenses)
        {
            if (!player.IsLogged()) return false;
            bool res = false;
            foreach (var lic in licenses)
            {
                var currentLic = player.Character.Licenses.FirstOrDefault(item => item.Name == lic);
                if (currentLic != null)
                {
                    res = res || currentLic.DateEnd > System.DateTime.Now;
                    player.Character.Licenses.Remove(currentLic);
                }
            }
            return res;
        }
        public static bool TakeLic(this ExtPlayer player, LicenseName license)
        {
            if (!player.IsLogged()) return false;
            bool res = false;
            var currentLic = player.Character.Licenses.FirstOrDefault(item => item.Name == license);
            if (currentLic != null)
            {
                res = currentLic.DateEnd > System.DateTime.Now;
                player.Character.Licenses.Remove(currentLic);
            }
            return res;
        }
        #endregion

        #region Cuff & Follow
        public static void FollowTo(this ExtPlayer player, ExtPlayer target)
        {
            if (!player.IsLogged() || !target.IsLogged()) return;
            target.Character.Follower = player;
            player.Character.Following = target;
            SafeTrigger.ClientEvent(player, "setFollow", true, target);
            
        }

        [RemoteEvent("Server_ChangeFollowCoordCorrect")]
        public static void ChangeFollowCoordCorrect(ExtPlayer player)
        {
            if (player.Character.Follower != null)
            {
                ExtPlayer target = player.Character.Follower;
                FollowTo(player, target);
            }
        }

        [RemoteEvent("Server_StopFollowPlayer")]
        public static void StopFollowPlayer(ExtPlayer player)
        {
            if (player.Character.Follower != null)
            {
                ExtPlayer target = player.Character.Follower;
                UnFollow(target);
            }
        }

        public static void UnFollow(this ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            ExtPlayer following = player.Character.Following;
            player.Character.Following = null;
            SafeTrigger.ClientEvent(player, "setFollow", false);
            if (following.IsLogged())
                following.Character.Follower = null;
        }

        public static void LetGoFollower(this ExtPlayer player, bool notify = false)
        {
            if (!player.IsLogged()) return;
            // Trigger.GetPlayerByUuid(id)
            ExtPlayer follower = player.Character.Follower;
            ExtPlayer followerGo = Trigger.GetPlayerByUuid(follower.Character.UUID);
            ExtPlayer ExtPlayer = Trigger.GetPlayerByUuid(player.Character.UUID);
            player.Character.Follower = null;
            if (follower.IsLogged())
            {
                follower.Character.Following = null;
                SafeTrigger.ClientEvent(follower, "setFollow", false);
                if (notify)
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы отпустили {ExtPlayer.isFriend(followerGo)}", 3000);
                    Notify.Send(follower, NotifyType.Warning, NotifyPosition.BottomCenter, $"Гражданин {followerGo.isFriend(ExtPlayer)} отпустил Вас", 3000);
                }
            }
        }
        public static void Cuffed(this ExtPlayer player, bool byCop)
        {
            if (!player.IsLogged()) return;
            player.Character.Cuffed = true;
            player.Character.CuffedCop = byCop;
            player.Character.CuffedGang = !byCop;
            SafeTrigger.ClientEvent(player, "blockMove", true);
            SafeTrigger.ClientEvent(player, "CUFFED", true);
            // Main.OnAntiAnim(player);
            NAPI.Player.PlayPlayerAnimation(player, 49, "mp_arresting", "idle");
            AttachmentSync.AddAttachment(player, AttachId.Cuffs);
        }

        public static void UnCuffed(this ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player, "CUFFED", false);
            player.Character.Cuffed = false;
            player.Character.CuffedCop = false;
            player.Character.CuffedGang = false;
            NAPI.Player.StopPlayerAnimation(player);
            AttachmentSync.RemoveAttachment(player, AttachId.Cuffs);
            SafeTrigger.ClientEvent(player, "blockMove", false);
            Main.OffAntiAnim(player);
        }
        #endregion

        public static ExtVehicle GetTempVehicle(this ExtPlayer player, VehicleAccess vehicleType)
        {
            return player?.Character?.TempVehicles.GetValueOrDefault(vehicleType);
        }

        public static bool TempVehicleIsExist(this ExtPlayer player, VehicleAccess vehicleType)
        {
            return player?.GetTempVehicle(vehicleType) != null;
        }

        public static bool AddTempVehicle(this ExtPlayer player, ExtVehicle vehicle, VehicleAccess vehicleType)
        {
            var character = player.Character;
            if (character == null) return false;
            if (character.TempVehicles.ContainsKey(vehicleType)) return false;
            character.TempVehicles.Add(vehicleType, vehicle);
            return true;
        }

        public static ExtVehicle RemoveTempVehicle(this ExtPlayer player, VehicleAccess vehicleType)
        {
            var character = player.Character;
            if (character == null) return null;
            if (!character.TempVehicles.ContainsKey(vehicleType)) return null;
            var vehicle = character.TempVehicles[vehicleType];
            character.TempVehicles.Remove(vehicleType);
            return vehicle;
        }

        public static void CustomSetIntoVehicle(this ExtPlayer player, ExtVehicle vehicle, int seatId)
        {
            SafeTrigger.UpdateDimension(player, vehicle.Dimension);
            SafeTrigger.ClientEvent(player,"teleport:toVehicle", vehicle, vehicle.Position, seatId);
        }

        internal static bool CheckInviteToFamily(this ExtPlayer player, Family family)
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
        public static bool CheckInviteToFraction(this ExtPlayer player, int fractionId)
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
