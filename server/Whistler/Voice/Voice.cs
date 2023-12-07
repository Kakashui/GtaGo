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

namespace Whistler.Voice
{
    public class Voice : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Voice));
        public Voice()
        {
            VehicleManager.OnPlayerExitVehicle += Event_PlayerExitVehicle;
        }
        
        public static void PlayerJoin(Player player)
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

                player.SetData("voicechat.state", "ONLY_LOCAL");
                player.SetData("Voip", DefaultVoiceMeta);
                player.SetData("PhoneVoip", DefaultVoicePhoneMeta);

            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }
        
        public static void PlayerQuit(Player player)
        {
            try
            {
                VoiceMetaData voiceMeta = player.GetData<VoiceMetaData>("Voip");

                VoicePhoneMetaData playerPhoneMeta = player.GetData<VoicePhoneMetaData>("PhoneVoip");

                Radio.RadioEvents.OnPlayerDisconnected(player);

                if (playerPhoneMeta.Target != null)
                {
                    Player target = playerPhoneMeta.Target;
                    VoicePhoneMetaData targetPhoneMeta = target.GetData<VoicePhoneMetaData>("PhoneVoip");

                    Notify.Send(target, NotifyType.Alert, NotifyPosition.BottomCenter, "local_78".Translate( player.Name), 3000);
                    targetPhoneMeta.Target = null;
                    targetPhoneMeta.CallingState = "nothing";

                    target.ResetData("AntiAnimDown");
                    if (!target.IsInVehicle) 
                        target.StopAnimation();
                    else 
                        target.SetData("ToResetAnimPhone", true);

                    player.ResetSharedData("attachmentsData");

                    Trigger.ClientEvent(target, "voice.phoneStop");

                    target.SetData("PhoneVoip", targetPhoneMeta);
                }

            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }
       
        [RemoteEvent("add_voice_listener")]
        public void add_voice_listener(Player player, params object[] arguments)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!(arguments[0] is Player)) return;
                Player target = (Player)arguments[0];
                if (!target.IsLogged()) return;
                player.EnableVoiceTo(target);
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }

        [RemoteEvent("remove_voice_listener")]
        public void remove_voice_listener(Player player, params object[] arguments)
        {
            try
            {
                if (arguments.Length < 1 || !player.IsLogged()) return;
                if (!(arguments[0] is Player)) return;
                Player target = (Player)arguments[0];
                if (!target.IsLogged()) return;
                player.DisableVoiceTo(target);
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
            }
        }
                
        public void Event_PlayerExitVehicle(Player player, Vehicle veh)
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