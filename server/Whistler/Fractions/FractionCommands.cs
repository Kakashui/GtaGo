using Whistler.Core;
using Whistler.SDK;
using System;
using System.Linq;
using GTANetworkAPI;
using Whistler.GUI;
using System.Collections.Generic;
using Whistler.Helpers;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Enums;
using Whistler.MoneySystem;
using Whistler.Fractions.PDA;
using Whistler.MP.RoyalBattle;
using Whistler.NewDonateShop;
using Whistler.GUI.Documents.Enums;
using Whistler.MoneySystem.Interface;
using Whistler.Common;
using Whistler.Entities;

namespace Whistler.Fractions
{
    class FractionCommands : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(FractionCommands));
        private static Hub hub = Hub.Default;
        public const string ON_SET_RANK = "onSetRank";

        public static Dictionary<int, DateTime> NextCarRespawn = new Dictionary<int, DateTime>()
        {
            { 1, DateTime.Now },
            { 2, DateTime.Now },
            { 3, DateTime.Now },
            { 4, DateTime.Now },
            { 5, DateTime.Now },
            { 6, DateTime.Now },
            { 7, DateTime.Now },
            { 8, DateTime.Now },
            { 9, DateTime.Now },
            { 10, DateTime.Now },
            { 11, DateTime.Now },
            { 12, DateTime.Now },
            { 13, DateTime.Now },
            { 14, DateTime.Now },
            { 15, DateTime.Now },
            { 16, DateTime.Now },
            { 17, DateTime.Now },
            //{ 18, DateTime.Now },
        };

        public static void RespawnFractionCars(int fracID)
        {

            var all_vehicles = VehicleManager.Vehicles.Where(item => item.Value.OwnerType == OwnerType.Fraction && item.Value.OwnerID == fracID);

            foreach (var vehicle in all_vehicles)
            {
                VehicleGo vehGo = VehicleManager.GetVehicleGo(vehicle.Value.ID);
                if (vehGo == null)
                {
                    vehicle.Value.RespawnVehicle();
                    continue;
                }
                vehGo.Occupants = vehGo.Occupants.Where(item => item.IsLogged()).ToList();
                if (vehGo.Occupants.Count >= 1) 
                    continue;
                vehicle.Value.RespawnVehicle();
            }
        }

        public static void playerPressCuffBut(PlayerGo player)
        {
            var fracid = player.GetCharacter().FractionID;
            if (player.GetCharacter().Cuffed)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_180", 3000);
                return;
            }              
            var target = player.GetNearestPlayer(2);
            if (target == null) return;

            if (NAPI.Player.IsPlayerInAnyVehicle(player))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_193", 3000);
                return;
            }
            if (NAPI.Player.IsPlayerInAnyVehicle(target))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_194", 3000);
                return;
            }

            if (target.IsAdmin() && player.GetCharacter().AdminLVL <= target.GetCharacter().AdminLVL) return;
            if (target.GetCharacter().UUID == 298470 && player.GetCharacter().AdminLVL < 10) return;

            if (RoyalBattleService.IsInBattle(player) || RoyalBattleService.IsInBattle(target)) return;
            if (player.GetCharacter().DemorganTime > 0) return;
            

            if (target.GetCharacter().Following != null || target.GetCharacter().Follower != null || target.GetCharacter().ArrestDate > DateTime.UtcNow)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_195", 3000);
                return;
            }

            var cuffme = ""; // message /me after cuff
            var uncuffme = ""; // message /me after uncuff

            if (Configs.GetConfigOrDefault(fracid).TypeFraction == OrgActivityType.Government || player.IsAdmin())// for  gov and admin
            {
                if (fracid == 8) return;
                if (!Manager.CanUseFunctionality(player) && Configs.GetConfigOrDefault(fracid).TypeFraction == OrgActivityType.Government && player.GetCharacter().AdminLVL == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_181", 3000);
                    return;
                }
                if (target.GetCharacter().CuffedGang)
                {
                    uncuffme = "Frac_184".Translate( player.Name);
                }
                else
                {
                    cuffme = "Frac_187".Translate( target.Name);
                    uncuffme = "Frac_190".Translate( target.Name);
                }
            }
            else // for mafia
            {
                if (target.GetCharacter().CuffedCop)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_191", 3000);
                    return;
                }
                if (!target.GetCharacter().Cuffed)
                {
                    if (target.GetCharacter().InSaveZone >= 0)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_541", 3000);
                        return;
                    }
                    if (!target.HasData("HANDS_UP"))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_548", 3000);
                        return;
                    }
                }
                var cuffs = player.GetInventory().GetItemLink(ItemNames.Cuffs);
                var count = (cuffs == null) ? 0 : cuffs.Count;
                if (!target.GetCharacter().Cuffed)
                {
                    if (count == 0)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_192", 3000);
                        return;
                    }
                    else
                        player.GetInventory().SubItemByName(ItemNames.Cuffs, 1, LogAction.Use);
                }

                cuffme = "Frac_187".Translate(target.GetCharacter().UUID);
                uncuffme = "Frac_190".Translate(target.GetCharacter().UUID);
            }
            if (!target.GetCharacter().Cuffed)
            {
                target.Cuffed(Configs.GetConfigOrDefault(fracid).TypeFraction == OrgActivityType.Government || player.IsAdmin());
                Chat.Action(player, cuffme);
            }
            else // uncuff target
            {
                target.UnCuffed();
                Chat.Action(player, uncuffme);
            }
        }

        #region every fraction commands


        public static bool SetFracRank(PlayerGo sender, int uuid, int newrank)
        {
            int senderlvl = sender.GetCharacter().FractionLVL;
            if (newrank >= senderlvl)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_203", 3000);
                return false;
            }

            var targetFraction = Manager.GetFractionByUUID(uuid);

            if (targetFraction == null || targetFraction.Id != sender.GetCharacter().FractionID)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_217", 3000);
                return false;
            }
            int playerlvl = targetFraction.Members[uuid].Rank;

            if (senderlvl < playerlvl)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_215", 3000);
                return false;
            }
            var pubsubEvent = new PubSubEvent();
            pubsubEvent.EventName = ON_SET_RANK;
            pubsubEvent.Payload["newRank"] = newrank;
            pubsubEvent.Payload["player"] = Main.PlayerNames.GetValueOrDefault(uuid);
            hub.Publish(pubsubEvent);
            return targetFraction.ChangeRank(uuid, newrank);
        }

        public static void InviteToFraction(PlayerGo sender, PlayerGo target)
        {
            if (Manager.CanUseCommand(sender, "invite"))
            {
                var senderCharacter = sender.GetCharacter();
                if (sender.Position.DistanceTo(target.Position) > 5)
                {
                    Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_207", 3000);
                    return;
                }
                if (!target.CheckInviteToFraction(senderCharacter.FractionID))
                {
                    Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_208", 3000);
                    return;
                }
                if (target.GetCharacter().LVL < 1)
                {
                    Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_209", 3000);
                    return;
                }
                if (target.GetCharacter().Warns > 0)
                {
                    Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_210", 3000);
                    return;
                }
                var fraction = Manager.GetFraction(senderCharacter.FractionID);
                if (fraction == null)
                    return;
                DialogUI.Open(target, "Frac_212".Translate(sender.Name, fraction.Configuration.Name), new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "House_58",// Да
                        Icon = "confirm",
                        Action = p =>
                        { 
                            Manager.InvitePlayerToFraction(p, fraction, 1);
                            Notify.Send(p, NotifyType.Success, NotifyPosition.BottomCenter, "Main_180".Translate(fraction.Configuration.Name), 3000);
                            Notify.Send(sender, NotifyType.Success, NotifyPosition.BottomCenter, "Main_181".Translate(p.Name), 3000);
                        }
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "House_59",// Нет
                        Icon = "cancel",
                        Action = p => 
                        { 
                        },
                    }
                });

                Notify.Send(sender, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_213".Translate( target.Name), 3000);
            }
        }

        public static bool UnInviteFromFraction(PlayerGo sender, int uuid)
        {
            if (!Manager.CanUseCommand(sender, "uninvite"))
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Core_69", 3000);
                return false;
            }
            if (sender.GetCharacter().UUID == uuid)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_214", 3000);
                return false;
            }

            int senderlvl = sender.GetCharacter().FractionLVL;

            var targetFraction = Manager.GetFractionByUUID(uuid);

            if (targetFraction == null || targetFraction.Id != sender.GetCharacter().FractionID)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_217", 3000);
                return false;
            }
            int playerlvl = targetFraction.Members[uuid].Rank;

            if (senderlvl <= playerlvl)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_215", 3000);
                return false;
            }            
            return targetFraction.DeleteMember(uuid);
        }

        #endregion

        #region cops and cityhall commands
        public static void ticketToTarget(PlayerGo player, PlayerGo target, int sum, string reason)
        {
            if (!Manager.CanUseCommand(player, "ticket")) return;
            if (sum > 7000)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_220", 3000);
                return;
            }
            if (reason.Length > 100)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_221", 3000);
                return;
            }
            if (target.GetCharacter().Money < sum && target.GetBankAccount().Balance < sum)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_222", 3000);
                return;
            }

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_224".Translate(target.Name, sum, reason), 3000);
            DialogUI.Open(target, "Frac_223".Translate(player.Name, sum, reason), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "gui_727",
                    Icon = "confirm",
                    Action = p =>
                    {
                        if (!MoneySystem.Wallet.TransferMoney(p.GetCharacter(), new List<(IMoneyOwner, int)> { (Manager.GetFraction(6), Convert.ToInt32(sum * 0.9)), (player.GetCharacter(), Convert.ToInt32(sum * 0.1)) }, "Money_Ticket") &&
                            !MoneySystem.Wallet.TransferMoney(p.GetBankAccount(), new List<(IMoneyOwner, int)> { (Manager.GetFraction(6), Convert.ToInt32(sum * 0.9)), (player.GetCharacter(), Convert.ToInt32(sum * 0.1)) }, "Money_Ticket"))
                        {
                            Notify.Send(p, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_225", 3000);
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_222", 3000);
                            return;
                        }

                        Notify.Send(p, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_226".Translate( sum, reason), 3000);
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_227".Translate( p.Name, sum, reason), 3000);
                        Chat.Action(player, "Frac_228".Translate( p.Value));
                        Chat.SendFractionMessage(7, "Frac_229".Translate(player.Name, p.Name, sum, reason), true);
                        GameLog.Ticket(player.GetCharacter().UUID, p.GetCharacter().UUID, sum, reason, player.Name, p.Name);
                    }
                },
                new DialogUI.ButtonSetting
                {
                    Name = "gui_728",
                    Icon = "cancel",
                    Action = p =>
                    {
                        Notify.Send(p, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_230".Translate( sum, reason), 3000);
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_231".Translate( p.Name, sum, reason), 3000);
                    }
                }
            });
        }
        public static void arrestTarget(PlayerGo player, PlayerGo target)
        {
            if (!Manager.CanUseCommand(player, "arrest")) return;
            if (player == target)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_232", 3000);
                return;
            }
            var targetPlayerGo = target.GetPlayerGo();
            if (!Manager.CanUseFunctionality(player))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_167", 3000);
                return;
            }
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_233", 3000);
                return;
            }
            if (!player.GetData<bool>("IS_IN_ARREST_AREA"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_234", 3000);
                return;
            }
            if (targetPlayerGo.Character.ArrestDate > DateTime.UtcNow)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_245", 3000);
                return;
            }
            if (targetPlayerGo.Character.WantedLVL == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_246", 3000);
                return;
            }
            if (!targetPlayerGo.Character.Cuffed)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_244", 3000);
                return;
            }
            target.UnFollow();
            target.UnCuffed();

            var minutes = targetPlayerGo.Character.WantedLVL.Level * (targetPlayerGo.Account.IsPrimeActive() ? Convert.ToInt32(20 * DonateService.PrimeAccount.DemorganAndArrestMultipler) : 20);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_247".Translate(target.GetCharacter().UUID, minutes), 3000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_248".Translate(player.GetCharacter().UUID, minutes), 3000);
            Chat.Action(player, "Frac_249".Translate( target.GetCharacter().UUID));
            Chat.SendFractionMessage(7, "Frac_250".Translate(player.Name, target.Name, targetPlayerGo.Character.WantedLVL.Reason), true );
            Chat.SendFractionMessage(9, "Frac_250".Translate(player.Name, target.Name, targetPlayerGo.Character.WantedLVL.Reason), true );
            targetPlayerGo.Character.ArrestDate = DateTime.UtcNow.AddMinutes(minutes);
            targetPlayerGo.Character.SetArrestTimer(target);
            PDA.PoliceArrests.NewArrest(player, target, targetPlayerGo.Character.WantedLVL.Reason);
            GameLog.Arrest(player.GetCharacter().UUID, targetPlayerGo.Character.UUID, targetPlayerGo.Character.WantedLVL.Reason, targetPlayerGo.Character.WantedLVL.Level, player.Name, target.Name);
            arrestPlayer(target);
        }

        public static void releasePlayerFromPrison(PlayerGo player, PlayerGo target)
        {
            if (!Manager.CanUseCommand(player, "rfp")) return;
            if (!Manager.CanUseFunctionality(player))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_167", 3000);
                return;
            }
            if (player == target)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_232", 3000);
                return;
            }
            if (player.Position.DistanceTo(target.Position) > 3)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_233", 3000);
                return;
            }
            if (!player.GetData<bool>("IS_IN_ARREST_AREA"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_234", 3000);
                return;
            }
            if (target.GetCharacter().ArrestDate <= DateTime.UtcNow)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_235", 3000);
                return;
            }
            PoliceArrests.ReleasePlayer(target, player, 0);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_236".Translate(target.GetCharacter().UUID), 3000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_237".Translate(player.GetCharacter().UUID), 3000);
            Chat.Action(player, "Frac_238");
        }


        static Vector3 DemorganOutPosition = new Vector3(1830.161, 2607.834, 45.5);

        public static readonly Vector3 KpzOutPosition = Police.policeCheckpoints[5];
        public static void freePlayer(PlayerGo player)
        {
            WhistlerTask.Run(() =>
            {
                try
                {
                    player.GetCharacter().ResetArrestTimer(player);
                    player.ChangePosition(DemorganOutPosition);
                    player.Dimension = 0;
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_239", 3000);
                }
                catch (Exception e)
                {
                    _logger.WriteError($"freePlayer:\n{e}");
                }
            });
        }

        public static void arrestPlayer(Player target)
        {
            target.ChangePosition(Police.policeCheckpoints[4]);
            WantedSystem.SetPlayerWantedLevel(target, null, 0, null);  
            Weapons.RemoveAll(target, true);
        }

        [RemoteEvent("playerPressFollowBut")]
        public void ClientEvent_playerPressFollow(PlayerGo player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Manager.CanUseCommand(player, "follow", false)) return;
                if (player.GetCharacter().Follower != null)
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_240".Translate(player.GetCharacter().Follower.GetCharacter().UUID), 3000);
                    Notify.Send(player.GetCharacter().Follower, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_241".Translate(player.GetCharacter().UUID), 3000);
                    player.GetCharacter().Follower?.UnFollow();
                }
                else
                {
                    var target = player.GetNearestPlayer(2);
                    if (target == null) return;
                    targetFollowPlayer(player, target);
                }
            }
            catch (Exception e) { _logger.WriteError($"PlayerPressFollow: {e.ToString()} // {e.TargetSite} // "); }
        }

        [RemoteEvent("police:spawnSpike")]
        public void ClientEvent_SpawnSpike(PlayerGo player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!NAPI.Player.IsPlayerInAnyVehicle(player) || player.Vehicle == null)
                    return;
                if (player.Vehicle.GetVehicleGo().Data.OwnerType != OwnerType.Fraction)
                    return;
                var fracVeh = player.Vehicle.GetVehicleGo().Data as FractionVehicle;
                if (!VehicleStreaming.GetEngineState(player.Vehicle))
                    return;
                if (fracVeh.SpikeIsSpawned())
                    fracVeh.DeleteSpike();
                else
                    fracVeh.SpawnSpike(player.Vehicle);
            }
            catch (Exception e) { _logger.WriteError($"ClientEvent_SpawnSpike: {e.ToString()} // {e.TargetSite} // "); }
        }


        public static void targetFollowPlayer(Player player, Player target)
        {
            if (!Manager.CanUseCommand(player, "follow")) return;
            if (!target.GetCharacter().IsAlive)
                return;
            var fracid = player.GetCharacter().FractionID;
            if (!player.IsAdmin() && Configs.GetConfigOrDefault(fracid).TypeFraction == OrgActivityType.Government) // for gov factions
            {
                if (fracid == 8)
                    return;
                if (!Manager.CanUseFunctionality(player))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_181", 3000);
                    return;
                }
            }
            if (player.IsInVehicle || target.IsInVehicle) return;
            if (player == target)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_232", 3000);
                return;
            }
            if (player.GetCharacter().Follower != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_243", 3000);
                return;
            }
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_233", 3000);
                return;
            }
            if (!target.GetCharacter().Cuffed)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_244", 3000);
                return;
            }
            if (target.GetCharacter().Following != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_251", 3000);
                return;
            }
            if (Configs.GetConfigOrDefault(fracid).TypeFraction != OrgActivityType.Government && !player.IsAdmin() && target.GetCharacter().CuffedCop)// если в наручниках, то крайму нельзя вести за собой
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_326", 3000);
                return;
                
            }
            target.FollowTo(player);
            //player.GetCharacter().Follower = target;
            //target.GetCharacter().Following = player;
            //Trigger.ClientEvent(target, "setFollow", true, player);
            Chat.Action(player, "Frac_252".Translate( target.GetCharacter().UUID));
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_253".Translate( target.GetCharacter().UUID), 3000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_254".Translate( player.GetCharacter().UUID), 3000);
        }
        public static void targetUnFollowPlayer(PlayerGo player)
        {
            if (!Manager.CanUseCommand(player, "follow")) return;
            player.LetGoFollower(true);
        }

        public static void playerInCar(PlayerGo player, PlayerGo target)
        {
            if (!Manager.CanUseCommand(player, "incar")) return;
            if (player == target)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_264", 3000);
                return;
            }
            var vehicle = VehicleManager.getNearestVehicle(player, 3);
            if (vehicle == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_265", 3000);
                return;
            }
            //if (player.VehicleSeat != VehicleConstants.DriverSeat)
            //{
            //    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_266", 3000);
            //    return;
            //}
            if (player.Position.DistanceTo(target.Position) > 5)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_233", 3000);
                return;
            }
            if (!target.GetCharacter().Cuffed)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_267", 3000);
                return;
            }

            target.UnFollow();

            var emptySlots = new List<int>
            {
                1,
                2,
                3
            };

            var players = vehicle.Occupants;
            foreach (Player p in players)
            {
                if (emptySlots.Contains(p.VehicleSeat)) 
                    emptySlots.Remove(p.VehicleSeat);
            }

            if (emptySlots.Count == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_268", 3000);
                return;
            }
            
            target.SetIntoVehicle(vehicle.Handle, emptySlots[0]);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_269".Translate( target.GetCharacter().UUID), 3000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_270".Translate( player.GetCharacter().UUID), 3000);
            Chat.Action(player, "Frac_271".Translate( target.GetCharacter().UUID));
        }

        public static void playerOutCar(PlayerGo player, PlayerGo target)
        {
            if (player == target)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_277", 3000);
                return;
            }
            if (target.IsAdmin())
                return;
            if (!Manager.CanUseCommand(player, "pull"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_326", 3000);
                return;
            }
            if (player.Position.DistanceTo(target.Position) > 5)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_276", 3000);
                return;
            }            
            if (NAPI.Player.IsPlayerInAnyVehicle(target))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_272".Translate( target.GetCharacter().UUID), 3000);
                Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_273".Translate( player.GetCharacter().UUID), 3000);
                VehicleManager.WarpPlayerOutOfVehicle(target);
                Chat.Action(player, "Frac_274".Translate( target.GetCharacter().UUID));
            }
            else Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_275", 3000);
        }

        public static void TakeGunLic(PlayerGo player, PlayerGo target)
        {
            if (!Manager.CanUseCommand(player, "takegunlic")) return;
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_233", 3000);
                return;
            }
            if (!target.TakeLic(LicenseName.Weapon) )
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_280", 3000);
                return;
            }
            else
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_281".Translate(target.GetCharacter().UUID), 3000);
                Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_282".Translate(player.GetCharacter().UUID), 3000);
                MainMenu.SendStats(target);
            }
        }
        public static void TakeDriveLic(PlayerGo player, PlayerGo target, LicenseName lic)
        {
            if (!Manager.CanUseCommand(player, "takedrivelic")) return;
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_233", 3000);
                return;
            }
            if (!target.TakeLic(lic) )
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_538".Translate(GUI.Documents.DocumentConfigs.GetLicenseWord(lic)), 3000);
                return;
            }
            else
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_539".Translate(GUI.Documents.DocumentConfigs.GetLicenseWord(lic), target.GetCharacter().UUID), 3000);
                Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_540".Translate(player.GetCharacter().UUID, GUI.Documents.DocumentConfigs.GetLicenseWord(lic)), 3000);
                MainMenu.SendStats(target);
            }
        }

        private static int _minWeaponLicensePrice = 45000;
        private static int _maxWeaponLicensePrice = 50000;
        public static void GiveGunLic(PlayerGo player, PlayerGo target, int price)
        {
            if (!Manager.CanUseCommand(player, "givegunlic")) return;
            if (player == target) return;
            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_233", 3000);
                return;
            }
            if (price < _minWeaponLicensePrice || price > _maxWeaponLicensePrice)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_283", 3000);
                return;
            }
            if (target.CheckLic(LicenseName.Weapon))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_284", 3000);
                return;
            }
            if (target.GetCharacter().Money < price)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_222", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_285".Translate( target.GetCharacter().UUID, price), 3000);

            Trigger.ClientEvent(target, "openDialog", "GUN_LIC", "Frac_286".Translate( player.GetCharacter().UUID, price));
            target.SetData("SELLER", player);
            target.SetData("GUN_PRICE", price);
        }

        public static void AcceptGunLic(PlayerGo player)
        {
            if (!player.IsLogged()) return;

            Player seller = player.GetData<Player>("SELLER");
            if (!seller.IsLogged()) return;
            int price = player.GetData<int>("GUN_PRICE");
            if (player.Position.DistanceTo(seller.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_287", 3000);
                return;
            }

            int sellerMoney = price - _minWeaponLicensePrice;
            int policeDeportament = _minWeaponLicensePrice / 4;
            int govDeportament = _minWeaponLicensePrice - policeDeportament;

            if (!MoneySystem.Wallet.TransferMoney(player.GetCharacter(), new List<(IMoneyOwner, int)> 
            { 
                (seller.GetCharacter(), sellerMoney), 
                (Manager.GetFraction(6), govDeportament), 
                (Manager.GetFraction(7), policeDeportament),  
            }, "Money_BuyGunlic"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_1", 3000);
                return;
            }

            player.GiveLic(LicenseName.Weapon);
            MainMenu.SendStats(player);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_288".Translate( seller.GetCharacter().UUID, price), 3000);
            Notify.Send(seller, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_289".Translate(player.GetCharacter().UUID), 3000);
        }

        [RemoteEvent("friskInterface:takeAllIllegal")]
        public static void OpenFriskMenu(PlayerGo player)
        {
            if (!player.HasData("frisktarget"))
                return;
            var target = player.GetData<Player>("frisktarget");
            if (target == null)
                return;
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
            target.GetInventory().RemoveItems(item => item.Type == ItemTypes.Narcotic);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Core1_55".Translate( player.GetCharacter().UUID), 3000);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core1_56".Translate( target.GetCharacter().UUID), 3000);
        }
        #endregion

        #region crimeCommands
        public static void RobberyTarget(PlayerGo player, PlayerGo target)
        {
            if (!player.IsLogged() || !target.IsLogged()) return;

            if (target.GetCharacter().AdminLVL > 0 || !target.GetCharacter().Cuffed && !target.HasData("HANDS_UP"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_295", 3000);
                return;
            }

            if (target.GetCharacter().InSaveZone >= 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_541", 3000);
                return;
            }
            if (!player.HasSharedData("IS_MASK") || !player.GetSharedData<bool>("IS_MASK"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_296", 3000);
                return;
            }

            if (target.GetCharacter().LVL < 2 || target.GetCharacter().Money <= 1000 || (target.HasData("NEXT_ROB") && DateTime.Now < target.GetData<DateTime>("NEXT_ROB")))
            {
                Chat.Action( player, "Frac_297".Translate( target.GetCharacter().UUID));
                return;
            }

            var max = (int)(target.GetCharacter().Money * 0.2);
            if (max > 50000)
                max = 50000;
            var min = 100;

            var found = Main.rnd.Next(min, max + 1);

            MoneySystem.Wallet.TransferMoney(target.GetCharacter(), player.GetCharacter(), found, 0, "Money_Robbery");
            target.SetData("NEXT_ROB", DateTime.Now.AddMinutes(60));

            Chat.Action(player, "Frac_298".Translate( target.GetCharacter().UUID, found));
        }

        #endregion

        #region EMS commands
        public static void giveMedicalLic(PlayerGo player, PlayerGo target)
        {
            if (!Manager.CanUseCommand(player, "givemedlic")) return;

            if (!target.GiveLic(LicenseName.Medical))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_307", 3000);
            }
            else
            {
                MainMenu.SendStats(target);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_308".Translate(target.Name), 3000);
                Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_309".Translate(player.Name), 3000);
            }

        }
        public static void sellMedKitToTarget(PlayerGo player, PlayerGo target, int price)
        {
            if (Manager.CanUseCommand(player, "medkit"))
            {
                if (!player.GetCharacter().OnDuty)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_167", 3000);
                    return;
                }
                var medkit = player.GetInventory().GetItemLink(ItemNames.HealthKit);
                if (medkit == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_310", 3000);
                    return;
                }
                if (price < 500 || price > 1500)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_311", 3000);
                    return;
                }
                if (player.Position.DistanceTo(target.Position) > 2)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_233", 3000);
                    return;
                }
                if (target.GetCharacter().Money < price)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_312", 3000);
                    return;
                }
                Trigger.ClientEvent(target, "openDialog", "PAY_MEDKIT", "Frac_313".Translate( player.GetCharacter().UUID, price));
                target.SetData("SELLER", player);
                target.SetData("PRICE", price);

                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_314".Translate( target.GetCharacter().UUID, price), 3000);
            }
        }

        public static void acceptEMScall(PlayerGo player, PlayerGo target)
        {
            if (Manager.CanUseCommand(player, "accept"))
            {
                if (!player.GetCharacter().OnDuty)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_315", 3000);
                    return;
                }
                if (!target.HasData("IS_CALL_EMS"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_316", 3000);
                    return;
                }
                Trigger.ClientEvent(player, "createWaypoint", target.Position.X, target.Position.Y);
                Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_317".Translate( player.GetCharacter().UUID), 3000);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_318".Translate( target.GetCharacter().UUID), 3000);
                target.ResetData("IS_CALL_EMS");
                return;
            }
        }


        public static void healTarget(PlayerGo player, PlayerGo target, int price)
        {
            if (Manager.CanUseCommand(player, "heal"))
            {
                if (player.Position.DistanceTo(target.Position) > 2)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_233", 3000);
                    return;
                }
                if (price < 50 || price > 400)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_319", 3000);
                    return;
                }
                if (NAPI.Player.IsPlayerInAnyVehicle(player) && NAPI.Player.IsPlayerInAnyVehicle(target))
                {
                    var pveh = player.Vehicle;
                    var tveh = target.Vehicle;
                    VehicleGo vehGo = pveh.GetVehicleGo();
                    if (vehGo.Data.OwnerType != OwnerType.Fraction || vehGo.Data.OwnerID != 8)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_320", 3000);
                        return;
                    }
                    if (pveh != tveh)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_103", 3000);
                        return;
                    }
                    target.SetData("SELLER", player);
                    target.SetData("PRICE", price);
                    Trigger.ClientEvent(target, "openDialog", "PAY_HEAL", "Frac_321".Translate( player.GetCharacter().UUID, price));

                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_322".Translate( target.GetCharacter().UUID, price), 3000);
                    return;
                }
                else if ((player.GetData<bool>("IN_HOSPITAL") && target.GetData<bool>("IN_HOSPITAL")) || player.Position.DistanceTo2D(Ems.HospitalPoint) < Ems.HEAL_DISTANCE_FROM_HOSPITAL)
                {
                    target.SetData("SELLER", player);
                    target.SetData("PRICE", price);
                    Trigger.ClientEvent(target, "openDialog", "PAY_HEAL", "Frac_321".Translate( player.GetCharacter().UUID, price));
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_322".Translate( target.GetCharacter().UUID, price), 3000);
                    return;
                }
                else
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_323", 3000); ;
                    return;
                }
            }
        }

        #endregion

        [Command("givemilid")]
        public static void CMD_GiveMilitaryID(PlayerGo player, int targetId)
        {
            try
            {
                if (!player.IsLogged()) return;
                var fraction = Manager.GetFraction(player);
                if (fraction == null || fraction.Id != 14) return;

                if (!fraction.IsLeaderOrSub(player)) return;

                var target = Main.GetPlayerByID(targetId);
                if (!target.IsLogged()) return;
                target.GiveLic(LicenseName.Military);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_51".Translate( target.Name), 3000);
                Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, "local_52".Translate( player.Name), 3000);
            }
            catch (Exception e) { _logger.WriteError("givemilid: " + e.ToString()); }
        }
    }
}