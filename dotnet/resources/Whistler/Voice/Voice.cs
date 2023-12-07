using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Whistler.GUI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Helpers;
using Whistler.VehicleSystem;
using Whistler.Scenes;
using Whistler.Scenes.Configs;
using Whistler.Entities;
using Whistler.VehicleSystem.Models;

namespace Whistler.Voice
{
    public class Voice : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Voice));
        public Voice()
        {
            VehicleManager.OnPlayerExitVehicle += Event_PlayerExitVehicle;
        }
        
        public static void PlayerJoin(ExtPlayer player)
        {
            try
            {
                VoiceMetaData DefaultVoiceMeta = new VoiceMetaData
                {
                    IsEnabledMicrophone = false,
                    RadioRoom = "",
                    StateConnection = "closed",
                    MicrophoneKey = 78 // N
                };

                VoicePhoneMetaData DefaultVoicePhoneMeta = new VoicePhoneMetaData
                {
                    CallingState = "nothing",
                    Target = null
                };

                SafeTrigger.SetData(player, "voicechat.state", "ONLY_LOCAL");
                SafeTrigger.SetData(player, "Voip", DefaultVoiceMeta);
                SafeTrigger.SetData(player, "PhoneVoip", DefaultVoicePhoneMeta);

            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }
        
        public static void PlayerQuit(ExtPlayer player)
        {
            try
            {
                VoiceMetaData voiceMeta = player.GetData<VoiceMetaData>("Voip");

                VoicePhoneMetaData playerPhoneMeta = player.GetData<VoicePhoneMetaData>("PhoneVoip");

                Radio.RadioEvents.OnPlayerDisconnected(player);

                if (playerPhoneMeta.Target != null)
                {
                    ExtPlayer target = playerPhoneMeta.Target;
                    VoicePhoneMetaData targetPhoneMeta = target.GetData<VoicePhoneMetaData>("PhoneVoip");

                    Notify.Send(target, NotifyType.Alert, NotifyPosition.BottomCenter, "local_78".Translate( player.Name), 3000);
                    targetPhoneMeta.Target = null;
                    targetPhoneMeta.CallingState = "nothing";

                    target.ResetData("AntiAnimDown");
                    if (!target.IsInVehicle) 
                        target.StopAnimation();
                    else 
                        SafeTrigger.SetData(target, "ToResetAnimPhone", true);

                    SafeTrigger.ResetSharedData(player, "attachmentsData");

                    SafeTrigger.ClientEvent(target, "voice.phoneStop");

                    SafeTrigger.SetData(target, "PhoneVoip", targetPhoneMeta);
                }

            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }
       
        [RemoteEvent("add_voice_listener")]
        public void add_voice_listener(ExtPlayer player, params object[] arguments)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!(arguments[0] is ExtPlayer)) return;
                ExtPlayer target = (ExtPlayer)arguments[0];
                if (!target.IsLogged()) return;
                player.EnableVoiceTo(target);
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }

        [RemoteEvent("remove_voice_listener")]
        public void remove_voice_listener(ExtPlayer player, params object[] arguments)
        {
            try
            {
                if (arguments.Length < 1 || !player.IsLogged()) return;
                if (!(arguments[0] is ExtPlayer)) return;
                ExtPlayer target = (ExtPlayer)arguments[0];
                if (!target.IsLogged()) return;
                player.DisableVoiceTo(target);
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }
                
        public void Event_PlayerExitVehicle(ExtPlayer player, Vehicle veh)
        {
            try
            {
                if (player.HasData("ToResetAnimPhone"))
                {
                    player.StopAnimation();
                    player.ResetData("ToResetAnimPhone");
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"PlayerExitVehicle: {e.ToString()}");
            }
        }

    }
}