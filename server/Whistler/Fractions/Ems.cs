using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using Whistler.GUI;
using System.Linq;
using Newtonsoft.Json;
using Whistler.Jobs.Transporteur;
using Whistler.VehicleSystem;
using Whistler.Core.CustomSync;
using Whistler.VehicleSystem.Models;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Enums;
using Whistler.MoneySystem;
using Whistler.MP.Arena.Battles;
using Whistler.MP.Arena.Helpers;
using Whistler.MP.Arena.Racing;
using Whistler.GUI.Lifts;
using Whistler.MP.RoyalBattle;
using ServerGo.Casino.Business;
using Whistler.Phone.Taxi.Job;
using Whistler.Enviroment;
using Whistler.Customization.Models;
using Whistler.Customization;
using Whistler.Customization.Enums;
using Whistler.Families.FamilyMP;
using Whistler.Families.WarForCompany;
using Whistler.MoneySystem.Interface;
using Whistler.Common;
using Whistler.MP.OrgBattle;
using Whistler.Enviroment.Models;
using Whistler.Entities;

namespace Whistler.Fractions
{
    class Ems : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Ems));
        public static int HumanMedkitsLefts = 100;
        public static Vector3 HospitalPoint = new Vector3(331.5396, -581.3446, 42.16403);
        public const int HEAL_DISTANCE_FROM_HOSPITAL = 40;
        private const int MINUTES_FOR_DEATH = 3;
        private const int MINUTES_AWAIT_MEDICS = 10;
        private const int _moneyForRevive = 1500;
        public const int HealByBotPrice = 500;
        private static Vector3 PlasticCabinet = new Vector3(328.7, -571.3, 43.1);


        private static List<Vector3> _spawnPointsAfterDeath = new List<Vector3>
        {
            new Vector3(318.158, -584.0826, 82.61895),
            new Vector3(323.0276, -571.48, 82.61895),
            new Vector3(314.9012, -567.1143, 82.61892),
            new Vector3(309.3766, -582.1899, 82.61892)
        };

        private static List<Vector3> _hallElevators = new List<Vector3> { new Vector3(309.5298, -576.9191, 42.16231), new Vector3(330.3914, -584.8033, 42.16227) };
        private static List<Vector3> _levelElevatorPoints = new List<Vector3> {new Vector3(329.9874, -580.7507, 81.6521), new Vector3(303.6479, -571.2131, 81.6521) };
        private static int _levelCount = 6;
        private static List<uint> _spawnLevels = new List<uint> {};

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                for (int i = 0; i < _levelCount; i++)
                {
                    _spawnLevels.Add(Dimensions.RequestPrivateDimension($"Ems Level {i + 1}"));
                }

                InteractShape.Create(emsCheckpoints[2], 1, 2)
                    .AddDefaultMarker()
                    .AddInteraction(ChangeDutyStatus, "interact_3");

                //InteractShape.Create(emsCheckpoints[4], 1, 2)
                //    .AddDefaultMarker()
                //    .AddInteraction(OpenTattooDeleteMenu, "interact_7");

                var liftLevel = Lift.Create()
                    .AddFloor("lift:1", new Vector3(307.4487, -576.4088, 42.2), new Vector3(0, 0, 60.61023), marker: false, input: false);
                _levelElevatorPoints.ForEach(p =>
                {
                    liftLevel.AddFloor("elevatorInput", p, dimension: NAPI.GlobalDimension, exit: false);
                });


                var lift = Lift.Create(CheckAccessElevator)
                    .AddFloor("lift:2", new Vector3(360.1283, -585.4325, 27.82049), new Vector3(0, 0, 294.7093))
                    .AddFloor("lift:3", new Vector3(319.4968, -559.87, 27.74343), new Vector3(0, 0, 30))
                    .AddFloor("lift:4", new Vector3(307.4487, -576.4088, 42.2), new Vector3(0, 0, 91), input: false)
                    .AddFloor("lift:5", new Vector3(339.1128, -583.9385, 73.16557), new Vector3(0, 0, 250.0563));
                _hallElevators.ForEach(p =>
                {
                    lift.AddFloor("elevatorInput", p, exit: false);
                });
                int level = 1;
                _spawnLevels.ForEach(dim =>
                {
                    lift.AddFloor("lift:6".Translate(++level), new Vector3(305.5769, -571.6871, 81.61897), new Vector3(0, 0, 250), dimension: dim, input: false);
                });;
            }
            catch (Exception e) {_logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        private static bool CheckAccessElevator(Player player)
        {
            if (player.GetCharacter().FractionID != 8 && !player.IsAdmin())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_108", 3000);
                return false;
            }
            return true;
        }

        public static List<Vector3> emsCheckpoints = new List<Vector3>()
        {
            new Vector3(354.0953, -599.1022, 43.2),  // spawn after death        0
            new Vector3(306.974, -601.4298, 42.3),  // open hospital stock      1
            new Vector3(320.2743, -591.908, 42.3),   // duty change              2
            new Vector3(312.363, -592.8296, 42.28398),  // start heal course        3
            new Vector3(319.1934, -567.0559, 42.3), // tattoo delete         4
        };

        [RemoteEvent("callEms")]
        public static void callEms(Player player)
        {
            if (player.HasData("IS_CALLEMS")) return;
            //if (player.HasData("canDeath") && DateTime.Now > player.GetData<DateTime>("canDeath")) return;
            if (Manager.countOfFractionMembers(8) == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_84", 3000);
                return;
            }

            if (player.GetCharacter().InsideHouseID != -1 || player.GetCharacter().InsideGarageID != -1) return;

            if (player.HasData("IS_DYING") && player.GetData<bool>("IS_DYING"))
            {
                player.TriggerEvent("dthscrtimer", MINUTES_AWAIT_MEDICS);
            }

            if (player.HasData("CALLEMS_BLIP"))
                WhistlerTask.Run(() => { NAPI.Entity.DeleteEntity(player.GetData<Blip>("CALLEMS_BLIP")); });

            var Blip = NAPI.Blip.CreateBlip(0, player.Position, 1, 70, $"Call from player ({player.GetCharacter().UUID})", 0, 0, true, 0, NAPI.GlobalDimension);
            NAPI.Blip.SetBlipTransparency(Blip, 0);
            foreach (var p in NAPI.Pools.GetAllPlayers())
            {
                if (!p.IsLogged() || p.GetCharacter().FractionID != 8) continue;
                Trigger.ClientEvent(p, "changeBlipAlpha", Blip, 255);
            }
            player.SetData("CALLEMS_BLIP", Blip);

            var colshape = NAPI.ColShape.CreateCylinderColShape(player.Position, 70, 4, 0);
            colshape.OnEntityExitColShape += (s, e) =>
            {
                if (e == player)
                {
                    try
                    {
                        if (Blip != null) Blip.Delete();
                        e.ResetData("CALLEMS_BLIP");

                        WhistlerTask.Run(() =>
                        {
                            colshape.Delete();
                        }, 20);
                        e.ResetData("CALLEMS_COL");
                        e.ResetData("IS_CALLEMS");
                    }
                    catch (Exception ex) {_logger.WriteError("EnterEmsCall: " + ex.Message); }
                }
            };
            player.SetData("CALLEMS_COL", colshape);

            player.SetData("IS_CALLEMS", true);
            Chat.SendFractionMessage(8, "Frac_86".Translate(player.GetCharacter().UUID), false );
            Chat.SendFractionMessage(8, "Frac_87".Translate(player.GetCharacter().UUID), true);
        }
        
        public static void HealPlayerByPed(Player player)
        {
            if (player.Health > 95)
            {
                Notify.SendError(player, "med:heal:ped:1");
                return;
            }
            if (Wallet.TransferMoney(player.GetCharacter(), Manager.GetFraction(8), HealByBotPrice, 0, "Money_Healed"))
            {
                player.Health = 100;
                Notify.SendSuccess(player, "med:heal:ped:2");
            }
            else
            {
                Notify.SendError(player, "med:heal:ped:3");
            }
        }

        [Command("ems")]
        public static void CMD_emsAccept(Player player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                Ems.acceptCall(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) {_logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        public static void acceptCall(Player player, Player target)
        {
            try
            {
                if (!Manager.CanUseCommand(player, "ems")) return;
                if (!target.HasData("IS_CALLEMS"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_88", 3000);
                    return;
                }
                Blip blip = target.GetData<Blip>("CALLEMS_BLIP");

                Trigger.ClientEvent(player, "changeBlipColor", blip, 38);
                Trigger.ClientEvent(player, "createWaypoint", blip.Position.X, blip.Position.Y);

                ColShape colshape = target.GetData<ColShape>("CALLEMS_COL");
                colshape.OnEntityEnterColShape += (s, e) =>
                {
                    if (e == player)
                    {
                        try
                        {
                            NAPI.Entity.DeleteEntity(target.GetData<Blip>("CALLEMS_BLIP"));
                            target.ResetData("CALLEMS_BLIP");
                            WhistlerTask.Run(() =>
                            {
                                colshape.Delete();
                            }, 20);
                        }
                        catch (Exception ex) {_logger.WriteError("EnterEmsCall: " + ex.Message); }
                    }
                };

                Chat.SendFractionMessage(8, "Frac_89".Translate(player.Name.Replace('_', ' '), target.GetCharacter().UUID), false );
                Chat.SendFractionMessage(8, "Frac_90".Translate(player.Name.Replace('_', ' '), target.GetCharacter().UUID), true );
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_91".Translate( player.GetCharacter().UUID), 3000);
            }
            catch (Exception e) {_logger.WriteError($"acceptCall/: {e.ToString()}"); }
        }

        private static void ResetCallEms(Player player)
        {
            try
            {
                if (player.HasData("CALLEMS_BLIP"))
                {
                    NAPI.Entity.DeleteEntity(player.GetData<Blip>("CALLEMS_BLIP"));
                    Chat.SendFractionMessage(8, "Frac_92".Translate(player.Name.Replace('_', ' ')), false);
                    player.ResetData("CALLEMS_BLIP");
                }
                if (player.HasData("CALLEMS_COL"))
                {
                    NAPI.ColShape.DeleteColShape(player.GetData<ColShape>("CALLEMS_COL"));
                    player.ResetData("CALLEMS_BLIP");
                }
                player.ResetData("IS_CALLEMS");
            }
            catch (Exception e) {_logger.WriteError("ResetCallEms: " + e.ToString()); }
        }

        public static void RevivePlayer(PlayerGo player, Vector3 pos, int health = 20)
        {
            if (!player.IsLogged())
                return;
            Main.OffAntiAnim(player);
            player.ResetData("canDeath");
            player.ResetData("IS_DYING");
            player.SetSharedData("InDeath", false);
            player.GetCharacter().IsAlive = true;
            player.ChangePosition(pos);
            NAPI.Player.SpawnPlayer(player, pos);
            player.Health = health;
            player.TriggerEvent("dthscrclose");

            WhistlerTask.Run(() =>
            {
                player.StopAnimGo();
            }, 500);
        }


        public static void onPlayerDisconnectedhandler(Player player, DisconnectionType type, string reason)
        {
            ResetCallEms(player);
        }

        [Command("makejesus")]
        public static void Command_ResurrectPlayer(Player admin, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(admin, "makejesus")) return;

                var player = Main.GetPlayerByID(id);
                if (player == null)
                {
                    Notify.Send(admin, NotifyType.Error, NotifyPosition.BottomCenter, "Core_1", 3000);
                    return;
                }
                ResetCallEms(player);
                RevivePlayer(player, player.Position - new Vector3(0,0,.3), 100);

                Notify.Send(admin, NotifyType.Success, NotifyPosition.BottomCenter, "local_140".Translate( player.Name), 3000);
            }
            catch (Exception e) {_logger.WriteError("EXCEPTION AT \"makejesus\":\n" + e.ToString()); }
        }

        private static readonly List<(string dict, string name)> DeathAnimations = new List<(string dict, string name)>
        {
            (dict: "dead", name: "dead_a"),
            (dict: "dead", name: "dead_b"),
            (dict: "dead", name: "dead_c"),
            (dict: "dead", name: "dead_d"),
            (dict: "dead", name: "dead_e"),
            (dict: "dead", name: "dead_f"),
            (dict: "dead", name: "dead_g"),
            (dict: "dead", name: "dead_h")
        };

        public static Vector3 GetRandomSpawnPointAfterDeath()
        {
            return _spawnPointsAfterDeath.GetRandomElement();
        }

        [ServerEvent(Event.PlayerDeath)]
        public void onPlayerDeathHandler( PlayerGo player, PlayerGo entityKiller, uint weapon)
        {
            try
            {
                if (!player.IsLogged()) return;

                if (player.HasSharedData("env:data:action:sit") && player.GetSharedData<SitDTO>("env:data:action:sit") != null)
                {
                    EnvActionService.FreeSitPlace(player);
                }
                CancelHandler.HandleCancelOrder(player);

                ServerGo.Casino.Business.Casino casino = CasinoManager.FindFirstCasino();
                if (casino != null) casino.OnPlayerLeftGame(player);

                if (ArenaBattleHelper.IsPlayerInAnyBattle(player))
                {
                    BattleManager.OnPlayerDead(player, entityKiller, weapon);
                    return;
                }

                if (GameEventsHelper.IsPlayerInAnyRace(player))
                {
                    RacingManager.OnPlayerDeadOnRace(player);
                    return;
                }

                #region Check Transporteur Worker
                if (Work.Workers.FirstOrDefault(e => e.Player == player) is Pilot pilot)
                {
                    player.GetCharacter().WorkID = 0;
                    pilot.Dispose();
                }
                #endregion

                Weapons.Event_PlayerDeath(player, entityKiller, weapon);
                Houses.HouseManager.Event_OnPlayerDeath(player, entityKiller, weapon);
                BusinessManager.Event_PlayerDeath(player);

                VehicleManager.WarpPlayerOutOfVehicle(player);
                player.GetCharacter().IsAlive = false;

                if(player.HasData("AdminSkin")) {
                    player.ResetData("AdminSkin");
                    player.GetCustomization().Apply(player);
                }

                uint dimension = 0;
                if (!player.HasData("IS_DYING") && player.GetCharacter().DemorganTime == 0 && player.GetCharacter().ArrestDate <= DateTime.UtcNow)
                {
                    bool callmedics = true;
                    WarCompanyManager.PlayerDeath(player);
                    if (GangsCapture.Event_PlayerDeath(player, entityKiller, weapon))
                    {
                        player.TriggerEvent("dthscr", -1);
                        callmedics = false;
                    }
                    if (Families.FamilyWars.WarManager.Event_PlayerDeath(player, entityKiller, weapon))
                    {
                        player.TriggerEvent("dthscr", -1);
                        callmedics = false;
                    }
                    if (RoyalBattleService.PlayerDeath(player, entityKiller, weapon))
                    {
                        player.TriggerEvent("dthscr", -1);
                        callmedics = false;
                    }
                    if (ManagerMP.PlayerDeath(player, entityKiller, weapon))
                    {
                        player.TriggerEvent("dthscr", -1);
                        callmedics = false;
                    }
                    if (OrgBattleManager.PlayerDeath(player, entityKiller, weapon))
                    {
                        player.TriggerEvent("dthscr", -1);
                        callmedics = false;
                    }
                    if (callmedics)
                    {
                        var medics = Manager.GetFraction(8).OnlineMembers.Count;
                        player.TriggerEvent("dthscr", medics);
                    }
                    player.SetData("canDeath", DateTime.Now.AddMinutes(MINUTES_FOR_DEATH)); 
                    player.SetData("IS_DYING", true);
                    player.SetSharedData("InDeath", true);
                    player.TriggerEvent("voice.mute");
                    //if (CasinoManager.Casinos.Any())
                    //{
                    //    CasinoManager.FindFirstCasino().OnPlayerLeftGame(player);
                    //    CasinoManager.FindFirstCasino().OnPlayerDisconnected(player);
                    //}
                    WhistlerTask.Run(() =>
                    {
                        player.ChangePosition(null);
                        NAPI.Player.SpawnPlayer(player, player.Position);
                        player.Health = 100;
                        //var animation = DeathAnimations.GetRandomElement();
                        //player.PlayAnimGo(animation.dict, animation.name, (AnimFlag)39);
                    }, 500);
                }
                else
                {
                    WhistlerTask.Run(() => {
                        if (!player.IsLogged()) return;

                        var spawnPos = new Vector3();

                        if (player.GetCharacter().DemorganTime != 0)
                        {
                            spawnPos = Admin.DemorganPosition + new Vector3(0, 0, 1.12);
                            player.TriggerEvent("admin:toDemorgan", true);
                            dimension = 1337;
                        }
                        else if (player.GetCharacter().ArrestDate > DateTime.UtcNow)
                            spawnPos = Police.policeCheckpoints[4];
                        else if (player.GetCharacter().CourtTime != 0)
                            spawnPos = Fractions.PrisonFib.randomPrisonpointFib();
                        else if (player.GetCharacter().FractionID == 14)
                            spawnPos = new Vector3(-1832.552, 3119.957, 32.81062);                        
                        else
                        {
                            player.SetData("IN_HOSPITAL", true);

                            //TODO: temp spawn position after death
                            //spawnPos = new Vector3(301.38596, -594.08057, 43.12971);
                            //dimension = 0;

                            spawnPos = GetRandomSpawnPointAfterDeath();
                            dimension = _spawnLevels.GetRandomElement();
                        }

                        player.UnCuffed();
                        ResetCallEms(player);
                        RevivePlayer(player, spawnPos);
                        player.Dimension = dimension;
                    }, 4000);   
                }
            }
            catch (Exception e) {_logger.WriteError("PlayerDeath: " + e.ToString()); }
        }

        [RemoteEvent("dieEms")]
        public static void DeathConfirm(Player player)
        {
            if (player.HasData("canDeath") && DateTime.Now < player.GetData<DateTime>("canDeath")) return;
            if (player.HasData("IS_CALLEMS"))
            {
                DateTime time = player.GetData<DateTime>("canDeath");
                if (DateTime.Now < time.AddMinutes(MINUTES_AWAIT_MEDICS)) return;
            }
            player.SetData("IS_DYING", true);
            player.Health = 0;
        }

        public static void payMedkit(Player player)
        {
            int price = player.GetData<int>("PRICE");
            if (!Wallet.TryChange(player.GetCharacter(), -price))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_1", 3000);
                return;
            }
            Player seller = player.GetData<Player>("SELLER");
            if (player.Position.DistanceTo(seller.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_97", 3000);
                return;
            }

            var medkit = seller.GetInventory().GetItemLink(ItemNames.HealthKit);
            if (medkit == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_98", 3000);
                return;
            }

            var item = ItemsFabric.CreateMedicaments(ItemNames.HealthKit, 1, false);
            if (!player.GetInventory().AddItem(item))
            {
                return;
            }
            seller.GetInventory().SubItemByName(ItemNames.HealthKit, 1, LogAction.Use);
            Wallet.TransferMoney(player.GetCharacter(), new List<(IMoneyOwner, int)> 
            { 
                (seller.GetCharacter(), Convert.ToInt32(price * 0.15)),
                (Manager.GetFraction(6), Convert.ToInt32(price * 0.85)),
            }, "Money_PayMedkit");

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_99", 3000);
            Notify.Send(seller, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_100".Translate( player.GetCharacter().UUID), 3000);
        }

        
        public static void PlayerHealTarget(PlayerGo player, PlayerGo target)
        {
            try
            {
                if (player.Position.DistanceTo(target.Position) > 2)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_52", 3000);
                    return;
                }
                var inventory = player.GetInventory();
                if (inventory == null)
                    return;
                var item = inventory.SubItemByName(ItemNames.HealthKit, 1, LogAction.Use);
                if (item == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_129", 3000);
                    return;
                }

                if (target.HasData("IS_DYING"))
                {
                    player.PlayAnimation("amb@medic@standing@tendtodead@idle_a", "idle_a", 39);
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_45".Translate( target.GetCharacter().UUID), 3000);
                    Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_46".Translate( player.GetCharacter().UUID), 3000);
                    Chat.Action(player, "Core1_90");
                    WhistlerTask.Run(() =>
                    {
                        if (player.GetCharacter().FractionID == 8)
                        {
                            if (!target.HasData("NEXT_DEATH_MONEY") || DateTime.Now > target.GetData<DateTime>("NEXT_DEATH_MONEY"))
                            {
                                Wallet.MoneyAdd(player.GetCharacter(), _moneyForRevive, "Money_Revieve".Translate(target.GetCharacter().UUID));
                                target.SetData("NEXT_DEATH_MONEY", DateTime.Now.AddMinutes(15));
                            }
                        }

                        ResetCallEms(target);
                        RevivePlayer(target, target.Position + new Vector3(0, 0, .5), 50);
                        player.StopAnimation();
                        player.ChangePosition(player.Position + new Vector3(0, 0, .5));
                        player.CreatePlayerAction(PersonalEvents.PlayerActions.HealPlayer, 1);

                        Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_48".Translate( player.GetCharacter().UUID), 3000);
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core1_49".Translate( target.GetCharacter().UUID), 3000);
                    }, 10000);
                }
                else
                {
                    Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_50".Translate( player.GetCharacter().UUID), 3000);
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core1_51".Translate( target.GetCharacter().UUID), 3000);
                    target.Health = 100;
                }
                return;
            }
            catch (Exception e) {_logger.WriteError("playerHealTarget: " + e.ToString()); }
        }

        public static void payHeal(Player player)
        {
            int price = player.GetData<int>("PRICE");
            if (!Wallet.TryChange(player.GetCharacter(), -price))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_1", 3000);
                return;
            }
            Player seller = player.GetData<Player>("SELLER");
            if (player.Position.DistanceTo(seller.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_101", 3000);
                return;
            }
            if (NAPI.Player.IsPlayerInAnyVehicle(seller) && NAPI.Player.IsPlayerInAnyVehicle(player))
            {
                var pveh = seller.Vehicle;
                var tveh = player.Vehicle;
                VehicleGo vehGo = pveh.GetVehicleGo();
                if (vehGo.Data.OwnerType != OwnerType.Fraction || vehGo.Data.OwnerID != 8) //TODO: change check id holder
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_320", 3000);
                    return;
                }
                if (pveh != tveh)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_103", 3000);
                    return;
                }
                Notify.Send(seller, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_104".Translate(player.GetCharacter().UUID), 3000);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_105".Translate( seller.GetCharacter().UUID), 3000);
                Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                NAPI.Player.SetPlayerHealth(player, 100);
                Wallet.TransferMoney(player.GetCharacter(), seller.GetCharacter(), price, 0, "Money_PayHeal");
                return;
            }
            else if (seller.GetData<bool>("IN_HOSPITAL") && player.GetData<bool>("IN_HOSPITAL") || player.Position.DistanceTo2D(Ems.HospitalPoint) < Ems.HEAL_DISTANCE_FROM_HOSPITAL)
            {
                Notify.Send(seller, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_106".Translate(player.GetCharacter().UUID), 3000);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_105".Translate( seller.GetCharacter().UUID), 3000);
                NAPI.Player.SetPlayerHealth(player, 100);
                Wallet.TransferMoney(player.GetCharacter(), seller.GetCharacter(), price, 0, "Money_PayHeal");
                Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                return;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_107", 3000);
                return;
            }
        }

        private static void ChangeDutyStatus(Player player)
        {
            if (player.GetCharacter().FractionID == 8)
                SkinManager.OpenSkinMenu(player);
            else 
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_108", 3000);
        }

        //private static List<string> TattooZonesNames = new List<string>() { "tattoo:zone:1", "tattoo:zone:2", "tattoo:zone:3", "tattoo:zone:4", "tattoo:zone:5", "tattoo:zone:6" };

        //[Command("cleartattoo")]
        //private static void callback_tattoodelete(Player player, int targetId, int zoneId)
        //{
        //    if (Group.CanUseCmd(player, "cleartattoo"))
        //        return;
        //    var target = Main.GetPlayerByID(targetId);
        //    if (!target.IsLogged())
        //        return;
        //    if (!Enum.IsDefined(typeof(TattooZones), zoneId))
        //        return;
        //    var zone = (TattooZones)zoneId;
        //    var tattoos = target.GetCustomization().Tattoos;
        //    if (tattoos[Convert.ToInt32(zone)].Count == 0)
        //    {
        //        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_140".Translate(), 3000);
        //        return;
        //    };

        //    foreach (var tattoo in tattoos[Convert.ToInt32(zone)])
        //    {
        //        var decoration = new Decoration();
        //        decoration.Collection = NAPI.Util.GetHashKey(tattoo.Dictionary);
        //        decoration.Overlay = NAPI.Util.GetHashKey(tattoo.Hash);
        //        target.RemoveDecoration(decoration);
        //    }
        //    tattoos[Convert.ToInt32(zone)] = new List<TattooModel>();
        //    target.SetSharedData("TATTOOS", JsonConvert.SerializeObject(tattoos));
        //    CustomizationService.UpdateTattoos(player.GetCustomization());
        //    Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_142".Translate() + TattooZonesNames[Convert.ToInt32(zone)], 3000);
        //}
    }
}
