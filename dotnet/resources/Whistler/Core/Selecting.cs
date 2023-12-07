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

        public static Action<ExtPlayer, GTANetworkAPI.Object> ObjectInteract;

        [RemoteEvent("objectInteracted")]
        public static void objectSelected(ExtPlayer player, GTANetworkAPI.Object entity)
        {
            try
            {
                if (entity == null || player == null || !player.IsLogged()) return;
                if (player.Character.DemorganTime > 0)
                    return;
                ObjectInteract?.Invoke(player, entity);
            }
            catch (Exception e) { _logger.WriteError($"oSelected/: {e.ToString()}\n{e.StackTrace}"); }
        }

        [Command("checkbone")]
        public static void Command_CheckBone(ExtPlayer player, int vehid, string bone)
        {
            var vehicle = SafeTrigger.GetVehicleById(vehid);
            if (vehicle != null)
                SafeTrigger.ClientEvent(player,"checkBone", vehicle, bone);
        }

        public static void playerTransferMoney(ExtPlayer player, string arg)
        {
            try
            {
                Convert.ToInt32(arg);
            }
            catch
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Введены неправилные данные", 3000);
                return;
            }
            var amount = Convert.ToInt32(arg);
            if (amount < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Введены неправилные данные", 3000);
                return;
            }

            if (amount > 100000)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Максимальная сумма для передачи 100.000$", 3000);
                return;
            }

            ExtPlayer target = player.GetData<ExtPlayer>("SELECTEDPLAYER");
            if (!target.IsLogged() || player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок слишком далеко от Вас", 3000);
                return;
            }
            if (amount > player.Character.Money)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно денежных средств", 3000);
                return;
            }
            if (player.HasData("NEXT_TRANSFERM") && DateTime.Now < player.GetData<DateTime>("NEXT_TRANSFERM") && player.Character.AdminLVL == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "С момента последней передачи денег прошло мало времени", 3000);
                return;
            }
            SafeTrigger.SetData(player, "NEXT_TRANSFERM", DateTime.Now.AddMinutes(3));
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"{target.isFriend(player)} передал Вам {amount}$", 3000);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы передали {amount}$ {player.isFriend(target)}", 3000);

            Wallet.TransferMoney(player.Character, target.Character, amount, 0, "Передача денег");
            Chat.Action(player, $"передал ${amount} гражданину {player.isFriend(target)}");
        }

        public static void PlayerTakeGuns(ExtPlayer player, ExtPlayer target)
        {
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок далеко", 3000);
                return;
            }
            if (!Fractions.Manager.CanUseCommand(player, "takeguns")) return;
            if (!target.Character.Cuffed)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не в наручниках", 3000);                
                return;
            }
            Weapons.RemoveAll(target, true);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, $"{target.isFriend(player)} изъял у Вас всё оружие", 3000);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы изъяли всё оружие у {player.isFriend(target)}", 3000);
            return;
        }
        public static void PlayerTakeIlleagal(ExtPlayer player, ExtPlayer target)
        {
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок далеко", 3000);
                return;
            }
            if (target.GetInventory().RemoveItems(item => item.Type == ItemTypes.Narcotic))
            {
                Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, $"{target.isFriend(player)} изъял у Вас запрещённые предметы", 3000);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы изъяили у {player.isFriend(target)} запрещённые предметы", 3000);
            }
            else
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не имеет ничего запрещённого", 3000);
        }

        public static void playerHandshakeTarget(ExtPlayer player, ExtPlayer target)
        {
            if (!isPlayerCanHandshake(player, target))
            {
                return;
            }
            SafeTrigger.SetData(target, "HANDSHAKER", player);
            target.OpenDialog("HANDSHAKE", $"{target.isFriend(player)} хочет пожать вам руку.");
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы предложили {player.isFriend(target)} пожать руку", 3000);
        }

        private static bool isPlayerCanHandshake(ExtPlayer player, ExtPlayer target)
        {
            var isInDeath = false;
            if (player.HasSharedData("InDeath"))
            {
                isInDeath = player.GetSharedData<bool>("InDeath");
            }
            if (player.Character.Cuffed || isInDeath)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Невозможно пожать ему руку в данный момент", 3000);
                return false;
            }

            isInDeath = false;
            if (target.HasSharedData("InDeath"))
            {
                isInDeath = target.GetSharedData<bool>("InDeath");
            }
            if (target.Character.Cuffed || isInDeath)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Невозможно пожать ему руку в данный момент", 3000);
                return false;
            }
            if (target.Position.DistanceTo(player.Position) >= 5)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Невозможно пожать ему руку в данный момент", 3000);
                return false;
            }
            return true;
        }

        public static void hanshakeTarget(ExtPlayer player)
        {
            if (!player.HasData("HANDSHAKER")) return;
            ExtPlayer target = player.GetData<ExtPlayer>("HANDSHAKER");
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

            NAPI.Task.Run(() => 
            {
                Main.OffAntiAnim(player); 
                Main.OffAntiAnim(target); 
                player.StopAnimation(); 
                target.StopAnimation();
            }, 4500);

            if (player.Character.AddFriend(target.Name))
                SafeTrigger.ClientEvent(player, "addFriendToList", target.Name);

            if (target.Character.AddFriend(player.Name))
                SafeTrigger.ClientEvent(target, "addFriendToList", player.Name);
        }

        public static void OpenMoneyTransferMenu(ExtPlayer player, ExtPlayer target)
        {
            if (player.Character.LVL < 3)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Перевод денег доступен после 3 уровня", 3000);
                return;
            }
            player.OpenInput("Передать деньги", "Сумма", 4, "player_givemoney");
        }

        public static void TakeMask(ExtPlayer player, ExtPlayer target)
        {
            if (!target.Character.Cuffed || (target.IsAdmin() && player.Character.AdminLVL <= target.Character.AdminLVL))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не в наручниках", 3000);
                return;
            }
            var mask = (ClothesBase)target.GetEquip().RemoveItem(target, ClothesSlots.Mask, LogAction.Move);
            if (mask.Drawable > 499 && mask.Drawable < 507) return;

            if (mask.Promo)
                target.GetInventory().AddItem(mask, log: LogAction.Move);
            Chat.Action(player, $"сорвал маску с гражданина {player.isFriend(target)}");
        }

        public static void Unarrest(ExtPlayer player, ExtPlayer target)
        {
            if (target.Character.Following == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этого игрока никто не тащит", 3000);
                return;
            }
            if (player.Character.Follower != target)
            {
                if (!target.Character.Following.IsLogged() || target.Character.Following.Position.DistanceTo(target.Position) > 100)
                {
                    target.UnFollow();
                    return;
                }
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этого игрока тащит кто-то другой", 3000);
                return;
            }
            player.LetGoFollower(true);
        }

        public static void MakePenalty(ExtPlayer player, ExtPlayer target)
        {
            SafeTrigger.SetData(player, "TICKETTARGET", target);
            player.OpenInput("Выписать штраф (сумма)", "Сумма от 0 до 7000$", 4, "player_ticketsum");
        }
        public static void OfferSellMedKit(ExtPlayer player, ExtPlayer target)
        {
            SafeTrigger.SetData(player, "SELECTEDPLAYER", target);
            player.OpenInput("Продать аптечку", "Цена $$$", 4, "player_medkit");
        }
        public static void OfferHealTarget(ExtPlayer player, ExtPlayer target)
        {
            SafeTrigger.SetData(player, "SELECTEDPLAYER", target);
            player.OpenInput("Предложить лечение", "Цена $$$", 4, "player_heal");
        }

        public static void InvitePlayerToFamily(ExtPlayer player, ExtPlayer target)
        {
            Family family = player.GetFamily();
            if (family == null) return;

            if (!target.CheckInviteToFamily(family))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок уже состоит во фракции или семье", 3000);
                return;
            }
            if (!FamilyManager.CanAccessToMemberManagement(player)) return;

            DialogUI.Open(target, $"Гражданин {target.isFriend(player)} предложил Вам вступить в организацию {family.Name}. Принять приглашение?", new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "Да",// Да
                    Icon = "confirm",
                    Action = (p) => 
                    {
                        if (family == null) return;

                        int familyRank = family.Ranks.Count > 1 ? family.Ranks.Last().Key : 1;
                        FamilyManager.InvitePlayerToFamily(p, family, familyRank);
                    }
                },

                new DialogUI.ButtonSetting
                {
                    Name = "Нет",// Нет
                    Icon = "cancel",
                    Action = p => {}
                }
            });
        }
        public static void GiveGunLicense(ExtPlayer player, ExtPlayer target)
        {
            SafeTrigger.SetData(player, "SELECTEDPLAYER", target);
            player.OpenInput("Продать лицензию на оружие ($45000-$50000)", "Цена $$$", 5, "player_givegunlic");
        }
        public static void ToPrison(ExtPlayer player, ExtPlayer target)
        {
            if (Fractions.PrisonFib.CanUsePrisonFib(player) && target != null)
            {
                var minutes = target.Character.WantedLVL.Level * (target.Character.IsPrimeActive() ? 5 : 10);
                //int time = target.GetData<int>("putprison");
                Fractions.PrisonFib.ToPrison(player, target, minutes);
            }
            else
                Chat.SendTo(player, "Вы не можете посадить человека в тюрьму!");
        }

        public static void SellHouse(ExtPlayer player, int suggestedAmount)
        {
            if (!player.HasData("SELECTEDPLAYER")) return;
            
            var target = player.GetData<ExtPlayer>("SELECTEDPLAYER");
            DialogUI.Open(player, $"Вы действительно хотите продать дом игроку {target.Name} ({target.Character.UUID}) за {suggestedAmount}$?", new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "Да",// Да
                    Icon = "confirm",
                    Action = p => HouseManager.OfferHouseSell(player, target, suggestedAmount)
                },

                new DialogUI.ButtonSetting
                {
                    Name = "Нет",// Нет
                    Icon = "cancel",
                    Action = p => {}
                }
            });
        }

        public static void SellFamilyHouse(ExtPlayer player, int suggestedAmount)
        {
            if (!player.HasData("SELECTEDPLAYER")) return;

            ExtPlayer target = player.GetData<ExtPlayer>("SELECTEDPLAYER");
            DialogUI.Open(player, $"Вы действительно хотите продать семейный дом игроку {target.Name} ({target.Character.UUID}) за {suggestedAmount}$?", new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "Да",// Да
                    Icon = "confirm",
                    Action = p => HouseManager.OfferFamilyHouseSell(player, target, suggestedAmount)
                },

                new DialogUI.ButtonSetting
                {
                    Name = "Нет",// Нет
                    Icon = "cancel",
                    Action = p => {}
                }
            });
        }

        public static void OpenCarStock(ExtPlayer player, ExtVehicle vehicle)
        {
            if (player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете открыть инвентарь, находясь в машине", 3000);
                return;
            }
            if (vehicle.Class == 13 || vehicle.Class == 8)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У этой машины нет багажника", 3000);
                return;
            }


            if (!vehicle.Data.CanAccessVehicle(player, AccessType.Inventory))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Багажник закрыт", 3000);
                return;
            }
            InventoryService.OpenStock(player, vehicle.Data.InventoryId, StockTypes.VehicleTrunk);
        }
    }
}
