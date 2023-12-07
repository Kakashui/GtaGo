using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.CustomSync;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Phone
{
    internal class PhoneSync : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PhoneSync));

        [RemoteEvent("phone:getHeld")]
        public void GetHeldPhoneOff(ExtPlayer player)
        {
            try
            {
                player.PlayAnimGo("move_characters@sandy@texting", "sandy_text_loop_base", (AnimFlag) 49);
                AttachmentSync.AddAttachment(player, AttachId.Mobile);
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on phone:getHeld ({player?.Name}) - " + e.ToString());
                });
            }
        }

        [RemoteEvent("phone:offHeld")]
        public void HeldPhoneOff(ExtPlayer player)
        {
            try
            {
                player.StopAnimGo();
                AttachmentSync.RemoveAttachment(player, AttachId.Mobile);
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on phone:offHeld ({player?.Name}) - " + e.ToString());
                });
            }
        }

        [RemoteEvent("phone:setSpeaking")]
        public void SetPlayerSpeaking(ExtPlayer player)
        {
            try
            {
                player.PlayAnimGo("amb@code_human_wander_mobile@male@base", "static", (AnimFlag) 49);
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on phone:setSpeaking ({player?.Name}) - " + e.ToString());
                });
            }
        }
    }
}
