using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.MoneySystem.Interface;
using Whistler.Phone.Calls;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.Fractions.PDA
{
    class PersonalDigitalAssistant : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PersonalDigitalAssistant));

        public PersonalDigitalAssistant()
        {
            MakeCallHandler.CallShortNumber += CallPolice;
        }

        public static void CallPolice(Player player, int number)
        {
            try
            {
                if (number == 112)
                    WhistlerTask.Run(() => {
                        PoliceCalls.CreatePoliceCall(player, 999);
                    });
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at CallPolice: {ex}");
            }
        }
        [RemoteEvent("pda:pressOpenMenu")]
        public static void OpenPersonalDigitalAssistant(Player player)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Manager.IsSilovic(player))
                    return;
                if (SubscribeToPda.IsSubscribe(player))
                {
                    player.TriggerEvent("pda:open");
                }
                else
                {
                    SubscribeToPda.Subscribe(player);
                    player.TriggerEventWithLargeList("pda:loadArrests", PoliceArrests.GetArrestedModels());
                    player.TriggerEvent("pda:openAndLoad", Manager.GetFraction(player).Configuration.Name, WantedSystem.GetJsonWantedPlayers(), WantedSystem.GetJsonWantedVehicle(), PoliceCalls.GetAllPoliceCallsDTO());
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at OpenPersonalDigitalAssistant: {ex}");
            }
        }

        //[RemoteEvent("pda:closeMenu")]
        //public static void ClosePersonalDigitalAssistant(Player player)
        //{
        //    try
        //    {
        //        SubscribeToPda.UnSubscribe(player);
        //        //Whistler.Core.Chat.Action(player, "Pda_43");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteError($"Error at ClosePersonalDigitalAssistant: {ex}");
        //    }
        //}

        [RemoteEvent("pda:setVehicleWantedLvl")]
        public static void RemoteEvent_SetVehicleWanted(Player player, int id, int wantedlvl, string reason)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (wantedlvl == 0 && !Manager.CanUseCommand(player, "removewanted") || wantedlvl > 0 && !Manager.CanUseCommand(player, "setwanted"))
                    return;
                if (!VehicleManager.Vehicles.ContainsKey(id) || !(VehicleManager.Vehicles[id] is PersonalBaseVehicle))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_37", 3000);
                    return;
                }
                if (wantedlvl < 0 || wantedlvl > 5)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_38", 3000);
                    return;
                }
                var vData = VehicleManager.Vehicles[id] as PersonalBaseVehicle;
                WantedSystem.SetVehicleWantedLevel(vData, player, wantedlvl, reason);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_SetVehicleWanted: {ex}"); 
            }
        }

        [RemoteEvent("pda:setPlayerWantedLvl")]
        public static void RemoteEvent_SetPlayerWanted(Player player, int uuid, int wantedlvl, string reason)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (wantedlvl == 0 && !Manager.CanUseCommand(player, "removewanted") || wantedlvl > 0 && !Manager.CanUseCommand(player, "setwanted"))
                    return;
                var target = Main.GetPlayerByUUID(uuid);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_39", 3000);
                    return; 
                }
                if (wantedlvl < 0 || wantedlvl > 5)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_38", 3000);
                    return;
                }
                WantedSystem.SetPlayerWantedLevel(target, player, wantedlvl, reason);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_SetPlayerWanted: {ex}");
            }
        }
        [RemoteEvent("pda:searchPlayer")]
        public static void RemoteEvent_SearchPlayer(Player player, string text, string type)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Manager.IsSilovic(player))
                    return;
                Player target = null;
                switch (type)
                {
                    case "passport":
                        if (!int.TryParse(text, out int uuid))
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_41", 3000);
                            return;
                        }
                        target = Main.GetPlayerByUUID(uuid);
                        break;
                    case "nickname":
                        target = NAPI.Player.GetPlayerFromName(text);
                        break;
                    case "number":
                        if (!int.TryParse(text, out int number))
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_41", 3000);
                            return;
                        }
                        target = Main.GetPlayerGoByPredicate(item => (item.Character.PhoneTemporary?.Simcard?.Number ?? -1) == number);
                        break;
                }
                if (target == null || !target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_40", 3000);
                    return;
                }
                player.TriggerEvent("pda:returnFindPlayer", WantedSystem.GetSearchPlayer(target.GetCharacter()));
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_SearchPlayer: {ex}");
            }
        }
        [RemoteEvent("pda:searchVehicle")]
        public static void RemoteEvent_SearchVehicle(Player player, string text)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Manager.IsSilovic(player))
                    return;
                var vData = VehicleManager.Vehicles.FirstOrDefault(veh => veh.Value.Number == text && veh.Value is PersonalBaseVehicle).Value as PersonalBaseVehicle;
                if (vData == null)
                {
                    return;
                }
                player.TriggerEvent("pda:returnFindVehicle", WantedSystem.GetSearchVehicle(vData));
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_SearchVehicle: {ex}");
            }
        }
        [RemoteEvent("pda:acceptCall")]
        public static void RemoteEvent_AcceptCall(Player player, int id)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Manager.IsSilovic(player))
                    return;
                PoliceCalls.AcceptCall(player, id);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_AcceptCall: {ex}");
            }
        }
        [RemoteEvent("pda:callNeedHelp")]
        public static void RemoteEvent_CallNeedHelp(Player player, int code)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Manager.IsSilovic(player))
                    return;
                PoliceCalls.CreatePoliceCall(player, code);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_AcceptCall: {ex}");
            }
        }
        [RemoteEvent("pda:releaseFromKPZ")]
        public static void RemoteEvent_ReleasePromKPZ(PlayerGo player, int arrestId, int amount)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Manager.IsPoliceAndFIB(player))
                    return;
                ArrestedModel arrest = PoliceArrests.GetArrested(arrestId);
                if (arrest == null)
                    return;
                if (!arrest.CanBeIssue)
                    return;
                Player target = Main.GetPlayerByUUID(arrest.Uuid);
                if (!target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_45", 3000);
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
                if (target.GetCharacter().ArrestDate <= DateTime.UtcNow)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_235", 3000);
                    return;
                }
                ArrestedModel lastestArrest = PoliceArrests.GetLastArrestedModel(target);
                if (lastestArrest != null && lastestArrest.Id != arrest.Id)
                {
                    return;
                }
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_48".Translate(target.Name), 3000);
                DialogUI.Open(target, "Frac_545".Translate(player.GetCharacter().UUID, amount), new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "House_58",// Да
                        Icon = "confirm",
                        Action = p =>
                        {
                            if (Wallet.TransferMoney(p.GetCharacter(), new List<(IMoneyOwner, int)>
                                {
                                    (Manager.GetFraction(7), amount/2),
                                    (player.GetCharacter(), amount/2),
                                }, "Money_ReleaseFromKpz"))
                            {
                                PoliceArrests.ReleasePlayer(p, player, amount, arrest);
                            }
                            else
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "police:pda:1", 3000);
                        }
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "House_59",
                        Icon = "cancel",
                        Action = p =>
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "police:pda:2", 3000);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_ReleasePromKPZ: {ex}");
            }
        }
        [RemoteEvent("pda:blockCanBeIssue")]
        public static void RemoteEvent_BlockCanBeIssue(PlayerGo player, int arrestId)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Manager.IsPoliceAndFIB(player))
                    return;
                var arrest = PoliceArrests.GetArrested(arrestId);
                if (arrest == null)
                    return;
                if (arrest.BlockCanBeIssue(player.GetCharacter().UUID))
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_46".Translate(Main.PlayerNames[arrest.Uuid]), 3000);
                else
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Pda_47".Translate(Main.PlayerNames[arrest.Uuid]), 3000);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_ReleasePromKPZ: {ex}");
            }
        }

        public static void OnPlayerDisconnectedhandler(Player player, DisconnectionType type, string reason)
        {
            try
            {
                SubscribeToPda.UnSubscribe(player);
                PoliceCalls.SubHelperForAllCalls(player, false);
                PoliceCalls.DeletePoliceCallOfPlayer(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_AcceptCall: {ex}");
            }
        }

        public static void OnPlayerRemoveFromFraction(Player player)
        {
            try
            {
                SubscribeToPda.UnSubscribe(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at RemoteEvent_AcceptCall: {ex}");
            }
        }

        public static void UpdateArrestData(ArrestedModel arrest)
        {
            SubscribeToPda.TriggerEventToSubscribers(PDAConstants.UPDATE_ARREST_DATA, JsonConvert.SerializeObject(arrest.GetArrestedModelDTO()));
        }
    }
}