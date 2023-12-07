using GTANetworkAPI;
using System;
using Whistler.GUI;
using Whistler.Houses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Whistler.SDK;
using Whistler.Families;
using Whistler.Helpers;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.MoneySystem;
using Whistler.Families.Models;
using Whistler.Inventory.Models;
using Whistler.Entities;

namespace Whistler.Core
{
    class Selecting : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Selecting));

        public static Action<PlayerGo, GTANetworkAPI.Object> ObjectInteract;

        [RemoteEvent("objectInteracted")]
        public static void objectSelected(PlayerGo player, GTANetworkAPI.Object entity)
        {
            try
            {
                if (entity == null || player == null || !player.IsLogged()) return;
                if (player.GetCharacter().DemorganTime > 0)
                    return;
                ObjectInteract?.Invoke(player, entity);
            }
            catch (Exception e) { _logger.WriteError($"oSelected/: {e.ToString()}\n{e.StackTrace}"); }
        }

        [Command("checkbone")]
        public static void Command_CheckBone(PlayerGo player, int vehid, string bone)
        {
            var vehicle = VehicleManager.GetVehicleByRemoteId(vehid);
            if (vehicle != null)
                player.TriggerEvent("checkBone", vehicle, bone);
        }

        public static void playerTransferMoney(PlayerGo player, string arg)
        {
            try
            {
                Convert.ToInt32(arg);
            }
            catch
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_161", 3000);
                return;
            }
            var amount = Convert.ToInt32(arg);
            if (amount < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_161", 3000);
                return;
            }

            if (amount > 100000)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_41_1", 3000);
                return;
            }

            PlayerGo target = player.GetData<PlayerGo>("SELECTEDPLAYER");
            if (!target.IsLogged() || player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Jobs_48", 3000);
                return;
            }
            if (amount > player.GetCharacter().Money)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_1", 3000);
                return;
            }
            if (player.HasData("NEXT_TRANSFERM") && DateTime.Now < player.GetData<DateTime>("NEXT_TRANSFERM") && player.GetCharacter().AdminLVL == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_41", 3000);
                return;
            }
            player.SetData("NEXT_TRANSFERM", DateTime.Now.AddMinutes(3));
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_42".Translate(player.GetCharacter().UUID, amount), 3000);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core1_43".Translate(target.GetCharacter().UUID, amount), 3000);

            Wallet.TransferMoney(player.GetCharacter(), target.GetCharacter(), amount, 0, "Money_TransferHand");
            Chat.Action(player, "Core1_44".Translate(amount, target.GetCharacter().UUID));
        }

        public static void PlayerTakeGuns(PlayerGo player, PlayerGo target)
        {
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_52", 3000);
                return;
            }
            if (!Fractions.Manager.CanUseCommand(player, "takeguns")) return;
            if (!target.GetCharacter().Cuffed)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_244", 3000);                
                return;
            }
            Weapons.RemoveAll(target, true);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Core1_52".Translate( player.GetCharacter().UUID), 3000);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core1_53".Translate( target.GetCharacter().UUID), 3000);
            return;
        }
        public static void PlayerTakeIlleagal(PlayerGo player, PlayerGo target)
        {
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_52", 3000);
                return;
            }
            if (target.GetInventory().RemoveItems(item => item.Type == ItemTypes.Narcotic))
            {
                Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Core1_55".Translate( player.GetCharacter().UUID), 3000);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core1_56".Translate( target.GetCharacter().UUID), 3000);
            }
            else
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_54", 3000);
        }

        public static void playerHandshakeTarget(PlayerGo player, PlayerGo target)
        {
            if (!isPlayerCanHandshake(player, target))
            {
                return;
            }
            target.SetData("HANDSHAKER", player);
            target.OpenDialog("HANDSHAKE", "Core1_57".Translate( player.GetCharacter().UUID));
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_58".Translate( target.GetCharacter().UUID), 3000);
        }

        private static bool isPlayerCanHandshake(PlayerGo player, PlayerGo target)
        {
            var isInDeath = false;
            if (player.HasSharedData("InDeath"))
            {
                isInDeath = player.GetSharedData<bool>("InDeath");
            }
            if (player.GetCharacter().Cuffed || isInDeath)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_59", 3000);
                return false;
            }

            isInDeath = false;
            if (target.HasSharedData("InDeath"))
            {
                isInDeath = target.GetSharedData<bool>("InDeath");
            }
            if (target.GetCharacter().Cuffed || isInDeath)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_59", 3000);
                return false;
            }
            return true;
        }

        public static void hanshakeTarget(PlayerGo player)
        {
            if (!player.HasData("HANDSHAKER")) return;
            PlayerGo target = player.GetData<PlayerGo>("HANDSHAKER");
            if (!target.IsLogged())
                return;
            if (!isPlayerCanHandshake(player, target))
            {
                return;
            }

            player.PlayAnimation("mp_ped_interaction", "handshake_guy_a", 39);
            target.PlayAnimation("mp_ped_interaction", "handshake_guy_a", 39);
           
            Main.OnAntiAnim(player);
            Main.OnAntiAnim(target);

            WhistlerTask.Run(() => 
            {
                Main.OffAntiAnim(player); 
                Main.OffAntiAnim(target); 
                player.StopAnimation(); 
                target.StopAnimation();
            }, 4500);

            if (player.GetCharacter().AddFriend(target.Name))
                Trigger.ClientEvent(player, "addFriendToList", target.Name);

            if (target.GetCharacter().AddFriend(player.Name))
                Trigger.ClientEvent(target, "addFriendToList", player.Name);
        }

        public static void OpenMoneyTransferMenu(PlayerGo player, PlayerGo target)
        {
            if (player.GetCharacter().LVL < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Перевод денег доступен после 1 уровня", 3000);
                return;
            }
            player.OpenInput("Core1_28", "Core1_29", 4, "player_givemoney");
        }

        public static void TakeMask(PlayerGo player, PlayerGo target)
        {
            if (!target.GetCharacter().Cuffed || (target.IsAdmin() && player.GetCharacter().AdminLVL <= target.GetCharacter().AdminLVL))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_244", 3000);
                return;
            }
            var mask = (ClothesBase)target.GetEquip().RemoveItem(target, ClothesSlots.Mask, LogAction.Move);
            if (mask.Drawable > 499 && mask.Drawable < 507) return;

            if (mask.Promo)
                target.GetInventory().AddItem(mask, log: LogAction.Move);
            Chat.Action(player, "Core_352".Translate(target.GetCharacter().UUID));
        }

        public static void Unarrest(PlayerGo player, PlayerGo target)
        {
            if (target.GetCharacter().Following == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_19", 3000);
                return;
            }
            if (player.GetCharacter().Follower != target)
            {
                if (!target.GetCharacter().Following.IsLogged() || target.GetCharacter().Following.Position.DistanceTo(target.Position) > 100)
                {
                    target.UnFollow();
                    return;
                }
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_20", 3000);
                return;
            }
            player.LetGoFollower(true);
        }

        public static void MakePenalty(PlayerGo player, PlayerGo target)
        {
            player.SetData("TICKETTARGET", target);
            player.OpenInput("Core1_32", "Core1_33", 4, "player_ticketsum");
        }
        public static void OfferSellMedKit(PlayerGo player, PlayerGo target)
        {
            player.SetData("SELECTEDPLAYER", target);
            player.OpenInput("Core1_23", "Core1_18", 4, "player_medkit");
        }
        public static void OfferHealTarget(PlayerGo player, PlayerGo target)
        {
            player.SetData("SELECTEDPLAYER", target);
            player.OpenInput("Core1_24", "Core1_18", 4, "player_heal");
        }

        public static void InvitePlayerToFamily(PlayerGo player, PlayerGo target)
        {
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!target.CheckInviteToFamily(family))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Fam_44", 3000);
                return;
            }
            if (!FamilyManager.CanAccessToMemberManagement(player))
                return;
            DialogUI.Open(target, "Fam_30".Translate(player.GetCharacter().UUID, family.Name), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "House_58",// Да
                    Icon = "confirm",
                    Action = p => FamilyManager.InvitePlayerToFamily(p, family)
                },

                new DialogUI.ButtonSetting
                {
                    Name = "House_59",// Нет
                    Icon = "cancel",
                    Action = p => {}
                }
            });
        }
        public static void GiveGunLicense(PlayerGo player, PlayerGo target)
        {
            player.SetData("SELECTEDPLAYER", target);
            player.OpenInput("Core1_87", "Core1_18", 5, "player_givegunlic");
        }
        public static void ToPrison(PlayerGo player, PlayerGo target)
        {
            if (Fractions.PrisonFib.CanUsePrisonFib(player) && target.HasData("putprison"))
            {
                int time = target.GetData<int>("putprison");
                Fractions.PrisonFib.ToPrison(player, target, time);
            }
            else
                Chat.SendTo(player, "Frac_504");
        }

        public static void SellHouse(PlayerGo player, int suggestedAmount)
        {
            if (!player.HasData("SELECTEDPLAYER")) return;
            
            var target = player.GetData<PlayerGo>("SELECTEDPLAYER");
            
            DialogUI.Open(player, "cef_86_3".Translate(suggestedAmount), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "House_58",// Да
                    Icon = "confirm",
                    Action = p => HouseManager.OfferHouseSell(player, target, suggestedAmount)
                },

                new DialogUI.ButtonSetting
                {
                    Name = "House_59",// Нет
                    Icon = "cancel",
                    Action = p => {}
                }
            });
        }
        
        public static void OpenCarStock(PlayerGo player, Vehicle vehicle)
        {
            if (player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_11", 3000);
                return;
            }
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (vehicle.Class == 13 || vehicle.Class == 8)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_12", 3000);
                return;
            }


            if (!vehGo.Data.CanAccessVehicle(player, AccessType.Inventory))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_14", 3000);
                return;
            }
            InventoryService.OpenStock(player, vehGo.Data.InventoryId, StockTypes.VehicleTrunk);
        }
    }
}
