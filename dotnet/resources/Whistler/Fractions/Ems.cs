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
        private const int MINUTES_AWAIT_MEDICS_ADD = 0;
        private const int _moneyForRevive = 1500;
        public const int HealByBotPrice = 500;
        public const int HealPetByBotPrice = 500;
        public const int ToyPetByBotPrice = 100;
        public const int RenamePetByBotPrice = 50000;
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
                    lift.AddFloor($"{++level} этаж", new Vector3(305.5769, -571.6871, 81.61897), new Vector3(0, 0, 250), dimension: dim, input: false);
                });;
            }
            catch (Exception e) {_logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        private static bool CheckAccessElevator(ExtPlayer player)
        {
            if (player.Character.FractionID != 8 && !player.IsAdmin())
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
        public static void callEms(ExtPlayer player)
        {
            if (player.HasData("IS_CALLEMS")) return;
            //if (player.HasData("canDeath") && DateTime.Now > player.GetData<DateTime>("canDeath")) return;
            if (Manager.countOfFractionMembers(8) == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_84", 3000);
                return;
            }

            if (player.Character.InsideHouseID != -1 || player.Character.InsideGarageID != -1) return;

            if (player.HasData("IS_DYING") && player.GetData<bool>("IS_DYING"))
            {
                SafeTrigger.ClientEvent(player,"dthscrtimer", MINUTES_AWAIT_MEDICS_ADD);
            }

            if (player.HasData("CALLEMS_BLIP"))
                NAPI.Task.Run(() => { NAPI.Entity.DeleteEntity(player.GetData<Blip>("CALLEMS_BLIP")); });

            var Blip = NAPI.Blip.CreateBlip(0, player.Position, 1, 70, $"Нуждается в помощи ({player.Character.UUID})", 0, 0, true, 0, NAPI.GlobalDimension);
            NAPI.Blip.SetBlipTransparency(Blip, 0);
            foreach (var p in Trigger.GetAllPlayers())
            {
                if (!p.IsLogged() || p.Character.FractionID != 8) continue;
                SafeTrigger.ClientEvent(p, "changeBlipAlpha", Blip, 255);
            }
            SafeTrigger.SetData(player, "CALLEMS_BLIP", Blip);

            var colshape = NAPI.ColShape.CreateCylinderColShape(player.Position, 70, 4, 0);
            colshape.OnEntityExitColShape += (s, e) =>
            {
                if (e == player)
                {
                    try
                    {
                        if (Blip != null) Blip.Delete();
                        e.ResetData("CALLEMS_BLIP");

                        NAPI.Task.Run(() =>
                        {
                            colshape.Delete();
                        }, 20);
                        e.ResetData("CALLEMS_COL");
                        e.ResetData("IS_CALLEMS");
                    }
                    catch (Exception ex) {_logger.WriteError("EnterEmsCall: " + ex.Message); }
                }
            };
            SafeTrigger.SetData(player, "CALLEMS_COL", colshape);

            SafeTrigger.SetData(player, "IS_CALLEMS", true);
            int uuid = player.Character.UUID;
            Chat.SendFractionMessage(8, $"Поступил вызов от {uuid}", false );
            Chat.SendFractionMessage(8, $"Поступил вызов от {uuid}. Введите /ems {uuid}, чтобы принять вызов.", true);
        }
        
        public static void HealPlayerByPed(ExtPlayer player)
        {
            if (player.Health > 95)
            {
                Notify.SendError(player, "med:heal:ped:1");
                return;
            }
            if (Wallet.TransferMoney(player.Character, Manager.GetFraction(8), HealByBotPrice, 0, "Оплата за лечение (NPC)"))
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
        public static void CMD_emsAccept(ExtPlayer player, int id)
        {
            try
            {
                if (Trigger.GetPlayerByUuid(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                Ems.acceptCall(player, Trigger.GetPlayerByUuid(id));
            }
            catch (Exception e) {_logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        public static void acceptCall(ExtPlayer player, ExtPlayer target)
        {
            try
            {
                if (!Manager.CanUseCommand(player, "ems")) return;
                if (!target.HasData("IS_CALLEMS"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_88", 3000);
                    return;
                }
                target.ResetData("IS_CALLEMS");
                Blip blip = target.GetData<Blip>("CALLEMS_BLIP");
                if (blip != null)
                {
                    SafeTrigger.ClientEvent(player, "createWaypoint", blip.Position.X, blip.Position.Y);
                    blip.Delete();
                    target.ResetData("CALLEMS_BLIP");
                }
                else
                {
                    SafeTrigger.ClientEvent(player, "createWaypoint", target.Position.X, target.Position.Y);
                    target.ResetData("CALLEMS_BLIP");
                }

                Chat.SendFractionMessage(8, "Frac_89".Translate(player.Name.Replace('_', ' '), target.Character.UUID), false );
                Chat.SendFractionMessage(8, "Frac_90".Translate(player.Name.Replace('_', ' '), target.Character.UUID), true );
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_91".Translate( player.Character.UUID), 3000);
            }
            catch (Exception e) {_logger.WriteError($"acceptCall/: {e.ToString()}"); }
        }

        private static void ResetCallEms(ExtPlayer player)
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

        public static void RevivePlayer(ExtPlayer player, Vector3 pos, int health = 20)
        {
            if (!player.IsLogged())
                return;
            Main.OffAntiAnim(player);
            player.ResetData("canDeath");
            player.ResetData("IS_DYING");
            SafeTrigger.SetSharedData(player, "InDeath", false);
            player.Character.IsAlive = true;
            player.ChangePosition(pos);
            NAPI.Player.SpawnPlayer(player, pos);
            player.Health = health;
            SafeTrigger.ClientEvent(player,"dthscrclose");

            NAPI.Task.Run(() =>
            {
                player.StopAnimGo();
            }, 500);
        }


        public static void onPlayerDisconnectedhandler(ExtPlayer player, DisconnectionType type, string reason)
        {
            ResetCallEms(player);
        }

        [Command("makejesus")]
        public static void Command_ResurrectPlayer(ExtPlayer admin, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(admin, "makejesus")) return;

                var player = Trigger.GetPlayerByUuid(id);
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
        public void onPlayerDeathHandler(ExtPlayer player, ExtPlayer entityKiller, uint weapon)
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
                Pilot worker = Work.Workers.FirstOrDefault(e => e.Player == player);
                if (worker != null)
                {
                    player.Character.WorkID = 0;
                    MainMenu.SendStats(player);
                    worker.Dispose();
                }
                #endregion

                Weapons.Event_PlayerDeath(player, entityKiller, weapon);
                Houses.HouseManager.Event_OnPlayerDeath(player, entityKiller, weapon);
                BusinessManager.Event_PlayerDeath(player);

                VehicleManager.WarpPlayerOutOfVehicle(player);
                player.Character.IsAlive = false;

                if(player.HasData("AdminSkin")) {
                    player.ResetData("AdminSkin");
                    player.Character.Customization.Apply(player);
                }

                uint dimension = 0;
                if (!player.HasData("IS_DYING") && player.Character.DemorganTime == 0 && player.Character.ArrestDate <= DateTime.Now)
                {
                    bool callmedics = true;
                    WarCompanyManager.PlayerDeath(player);
                    if (GangsCapture.Event_PlayerDeath(player, entityKiller, weapon))
                    {
                        SafeTrigger.ClientEvent(player,"dthscr", -1);
                        callmedics = false;
                    }
                    if (Families.FamilyWars.WarManager.Event_PlayerDeath(player, entityKiller, weapon))
                    {
                        SafeTrigger.ClientEvent(player,"dthscr", -1);
                        callmedics = false;
                    }
                    if (RoyalBattleService.PlayerDeath(player, entityKiller, weapon))
                    {
                        SafeTrigger.ClientEvent(player,"dthscr", -1);
                        callmedics = false;
                    }
                    if (ManagerMP.PlayerDeath(player, entityKiller, weapon))
                    {
                        SafeTrigger.ClientEvent(player,"dthscr", -1);
                        callmedics = false;
                    }
                    if (OrgBattleManager.PlayerDeath(player, entityKiller, weapon))
                    {
                        SafeTrigger.ClientEvent(player,"dthscr", -1);
                        callmedics = false;
                    }
                    if (callmedics)
                    {
                        int medics = 0;
                        Models.Fraction fraction = Manager.GetFraction(8);
                        if (fraction != null) medics = fraction.OnlineMembers.Count;
                        SafeTrigger.ClientEvent(player,"dthscr", medics);
                    }
                    SafeTrigger.SetData(player, "canDeath", DateTime.Now.AddMinutes(MINUTES_FOR_DEATH)); 
                    SafeTrigger.SetData(player, "IS_DYING", true);
                    SafeTrigger.SetSharedData(player, "InDeath", true);
                    SafeTrigger.ClientEvent(player,"voice.mute");
                    //if (CasinoManager.Casinos.Any())
                    //{
                    //    CasinoManager.FindFirstCasino().OnPlayerLeftGame(player);
                    //    CasinoManager.FindFirstCasino().OnPlayerDisconnected(player);
                    //}
                    NAPI.Task.Run(() =>
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
                    NAPI.Task.Run(() => 
                    {
                        if (!player.IsLogged()) return;

                        var spawnPos = new Vector3();

                        if (player.Character.DemorganTime != 0)
                        {
                            spawnPos = Admin.DemorganPosition + new Vector3(0, 0, 1.12);
                            SafeTrigger.ClientEvent(player,"admin:toDemorgan", true);
                            dimension = 1337;
                        }
                        else if (player.Character.ArrestDate > DateTime.Now)
                            spawnPos = Police.policeCheckpoints[4];
                        else if (player.Character.CourtTime != 0){
                            spawnPos = Fractions.PrisonFib.randomPrisonpointFib();
                            SafeTrigger.ClientEvent(player, "Client_CheckIsInJail");
                            //Client_CheckIsInJail
                        }     
                        else if (player.Character.FractionID == 14)
                            spawnPos = new Vector3(-1832.552, 3119.957, 32.81062);                        
                        else
                        {
                            SafeTrigger.SetData(player, "IN_HOSPITAL", true);

                            //TODO: temp spawn position after death
                            //spawnPos = new Vector3(301.38596, -594.08057, 43.12971);
                            //dimension = 0;

                            spawnPos = GetRandomSpawnPointAfterDeath();
                            dimension = _spawnLevels.GetRandomElement();
                        }

                        player.UnCuffed();
                        ResetCallEms(player);
                        RevivePlayer(player, spawnPos, player.Character.IsPrimeActive() ? 100 : 20);
                        SafeTrigger.UpdateDimension(player,  dimension);
                    }, 4000);   
                }
            }
            catch (Exception e) {_logger.WriteError("PlayerDeath: " + e.ToString()); }
        }

        [RemoteEvent("dieEms")]
        public static void DeathConfirm(ExtPlayer player)
        {
            if (player.HasData("canDeath") && DateTime.Now < player.GetData<DateTime>("canDeath")) return;
            if (player.HasData("IS_CALLEMS"))
            {
                DateTime time = player.GetData<DateTime>("canDeath");
                if (DateTime.Now < time.AddMinutes(MINUTES_AWAIT_MEDICS_ADD)) return;
            }
            SafeTrigger.SetData(player, "IS_DYING", true);
            player.Health = 0;
        }

        public static void payMedkit(ExtPlayer player)
        {
            int price = player.GetData<int>("PRICE");
            if (!Wallet.TryChange(player.Character, -price))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_1", 3000);
                return;
            }
            ExtPlayer seller = player.GetData<ExtPlayer>("SELLER");
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
            Wallet.TransferMoney(player.Character, new List<(IMoneyOwner, int)> 
            { 
                (seller.Character, Convert.ToInt32(price * 0.15)),
                (Manager.GetFraction(6), Convert.ToInt32(price * 0.85)),
            }, "Money_PayMedkit");

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_99", 3000);
            Notify.Send(seller, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_100".Translate( player.Character.UUID), 3000);
        }

        
        public static void PlayerHealTarget(ExtPlayer player, ExtPlayer target)
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
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_45".Translate( target.Character.UUID), 3000);
                    Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_46".Translate( player.Character.UUID), 3000);
                    Chat.Action(player, "Core1_90");
                    NAPI.Task.Run(() =>
                    {
                        if (player.Character.FractionID == 8)
                        {
                            if (!target.HasData("NEXT_DEATH_MONEY") || DateTime.Now > target.GetData<DateTime>("NEXT_DEATH_MONEY"))
                            {
                                Wallet.MoneyAdd(player.Character, _moneyForRevive, $"Лечение игрока ({target.Character.UUID})");
                                SafeTrigger.SetData(target, "NEXT_DEATH_MONEY", DateTime.Now.AddMinutes(15));
                            }
                        }

                        ResetCallEms(target);
                        RevivePlayer(target, target.Position + new Vector3(0, 0, .5), 50);
                        player.StopAnimation();
                        player.ChangePosition(player.Position + new Vector3(0, 0, .5));
                        player.CreatePlayerAction(PersonalEvents.PlayerActions.HealPlayer, 1);

                        Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_48".Translate( player.Character.UUID), 3000);
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core1_49".Translate( target.Character.UUID), 3000);
                    }, 10000);
                }
                else
                {
                    Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_50".Translate( player.Character.UUID), 3000);
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core1_51".Translate( target.Character.UUID), 3000);
                    target.Health = 100;
                }
                return;
            }
            catch (Exception e) {_logger.WriteError("playerHealTarget: " + e.ToString()); }
        }

        public static void payHeal(ExtPlayer player)
        {
            int price = player.GetData<int>("PRICE");
            if (!Wallet.TryChange(player.Character, -price))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_1", 3000);
                return;
            }
            ExtPlayer seller = player.GetData<ExtPlayer>("SELLER");
            if (player.Position.DistanceTo(seller.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_101", 3000);
                return;
            }
            if (NAPI.Player.IsPlayerInAnyVehicle(seller) && NAPI.Player.IsPlayerInAnyVehicle(player))
            {
                var pveh = seller.Vehicle;
                var tveh = player.Vehicle;
                ExtVehicle extVehicle = pveh as ExtVehicle;
                if (extVehicle.Data.OwnerType != OwnerType.Fraction || extVehicle.Data.OwnerID != 8) //TODO: change check id holder
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_320", 3000);
                    return;
                }
                if (pveh != tveh)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_103", 3000);
                    return;
                }
                Notify.Send(seller, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_104".Translate(player.Character.UUID), 3000);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_105".Translate( seller.Character.UUID), 3000);
                SafeTrigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                NAPI.Player.SetPlayerHealth(player, 100);
                Wallet.TransferMoney(player.Character, seller.Character, price, 0, "Оплата за лечение");
                return;
            }
            else if (seller.GetData<bool>("IN_HOSPITAL") && player.GetData<bool>("IN_HOSPITAL") || player.Position.DistanceTo2D(Ems.HospitalPoint) < Ems.HEAL_DISTANCE_FROM_HOSPITAL)
            {
                Notify.Send(seller, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_106".Translate(player.Character.UUID), 3000);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_105".Translate( seller.Character.UUID), 3000);
                NAPI.Player.SetPlayerHealth(player, 100);
                Wallet.TransferMoney(player.Character, seller.Character, price, 0, "Оплата за лечение");
                SafeTrigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                return;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_107", 3000);
                return;
            }
        }

        private static void ChangeDutyStatus(ExtPlayer player)
        {
            if (player.Character.FractionID == 8)
                SkinManager.OpenSkinMenu(player);
            else 
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_108", 3000);
        }

        //private static List<string> TattooZonesNames = new List<string>() { "tattoo:zone:1", "tattoo:zone:2", "tattoo:zone:3", "tattoo:zone:4", "tattoo:zone:5", "tattoo:zone:6" };

        //[Command("cleartattoo")]
        //private static void callback_tattoodelete(ExtPlayer player, int targetId, int zoneId)
        //{
        //    if (Group.CanUseCmd(player, "cleartattoo"))
        //        return;
        //    var target = Trigger.GetPlayerByUuid(targetId);
        //    if (!target.IsLogged())
        //        return;
        //    if (!Enum.IsDefined(typeof(TattooZones), zoneId))
        //        return;
        //    var zone = (TattooZones)zoneId;
        //    var tattoos = target.Character.Customization.Tattoos;
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
        //    CustomizationService.UpdateTattoos(player.Character.Customization);
        //    Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_142".Translate() + TattooZonesNames[Convert.ToInt32(zone)], 3000);
        //}
    }
}
