using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Models;
using Whistler.Inventory;
using Whistler.Helpers;
using Whistler.Fractions;
using Whistler.Common;
using Whistler.Inventory.Enums;
using Whistler.Entities;

namespace Whistler.Voice.Radio
{
    class RadioEvents : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RadioEvents));
        private static List<ReservedWave> _reservedWaves = new List<ReservedWave>();

        private static bool RESERVERD_WAVES_ENABLED = true;

        private class ReservedWave
        {
            public int StartWave { get; }
            public int EndWave { get; }

            private int FractionType;
            private int FractionID;

            private bool CheckByFractionType = false;

            public ReservedWave(int startWave, int endWave, int fractionType, int fractionId, bool checkbytype)
            {
                StartWave = startWave;
                EndWave = endWave;
                FractionType = fractionType;
                FractionID = fractionId;
                CheckByFractionType = checkbytype;
            }

            public bool CheckIsHaveAccess(ExtPlayer player)
            {
                if (!player.IsLogged()) return false;

                if (CheckByFractionType)
                    return (int)(Manager.GetFraction(player)?.OrgActiveType ?? OrgActivityType.Invalid) == FractionType;
                else
                    return player.Character.FractionID == FractionID;
            }
        }

        public RadioEvents()
        {
            if (!RESERVERD_WAVES_ENABLED) return;

            _logger.WriteInfo("Creating reserved radio waves");

            foreach (var fractionID in Fractions.Manager.FractionTypes.Keys)
            {
                if (Fractions.Manager.FractionTypes[fractionID] != 2) continue;

                var reservedWave = new ReservedWave(100000 + fractionID * 1000, 100000 + fractionID * 1000 + 999, 3, fractionID, false);
                _reservedWaves.Add(reservedWave);
            }

            // special reserved wave for all gov fractions
            var specialWave = new ReservedWave(150000, 151000, 3, -1, true);
            _reservedWaves.Add(specialWave);

            InventoryService.OnUseOtherItem += OnUseRadio;

            _logger.WriteInfo("Reserved radio waves created succesfully");
        }

        public static void OnPlayerDisconnected(ExtPlayer player)
        {
            if (!player.HasData("Voip")) return;

            VoiceMetaData voiceMeta = player.GetData<VoiceMetaData>("Voip");

            if (voiceMeta.RadioRoom != "")
            {
                RadioRoom room = RadioController.GetRoom(voiceMeta.RadioRoom);
                if (room == null) return;

                room.DisconnectPlayerById(player);
            }
        }
        public static void OnUseRadio(ExtPlayer player, BaseItem item)
        {
            if (item.Name == ItemNames.Radio)
                SafeTrigger.ClientEvent(player,"voice.radio:open");
        }

        [RemoteEvent("voice.radio::switchState")]
        public void RemoteEvent_SwitchVoiceState(ExtPlayer player, string newState)
        {
            try
            {
                VoiceMetaData voiceMeta = player.GetData<VoiceMetaData>("Voip");

                SafeTrigger.SetData(player, "voicechat.state", newState);

                var radioRoom = RadioController.GetRoom(voiceMeta.RadioRoom);
                if (radioRoom == null) return;

                switch (newState)
                {
                    case "ONLY_LOCAL":
                        radioRoom.ToggleMutePlayer(player, true);
                        Chat.SendTo(player, "Switched to Local chat");
                        break;
                    case "WITH_RADIO":
                        radioRoom.ToggleMutePlayer(player, false);
                        Chat.SendTo(player, "Switched to Radio chat");
                        break;
                }
            }
            catch (Exception e) { _logger.WriteError($"SOME ERROR CATCHED ON \"voice.radio::switchState\": {e.ToString()}"); }
        }

        [RemoteEvent("voice.radio::connectWave")]
        public void RemoteEvent_ConnectToWave(ExtPlayer player, string wave)
        {
            try
            {
                VoiceMetaData voiceMeta = player.GetData<VoiceMetaData>("Voip");

                if (voiceMeta.RadioRoom != "") return;

                if (RESERVERD_WAVES_ENABLED)
                {
                    var intWave = int.Parse(wave);
                    var reservedWave = _reservedWaves.Find(w => w.StartWave <= intWave && intWave <= w.EndWave);

                    if (reservedWave != null && !reservedWave.CheckIsHaveAccess(player))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Voice_17", 3000);
                        return;
                    }
                }

                RadioController.CreateRoom(wave);
                var radioRoom = RadioController.GetRoom(wave);

                radioRoom.ConnectPlayer(player);

                voiceMeta.RadioRoom = wave;
                SafeTrigger.SetData(player, "Voip", voiceMeta);
            }
            catch (Exception e) { _logger.WriteError($"SOME ERROR CATCHED ON \"voice.radio::connectWave\": {e.ToString()}"); }
        }

        [RemoteEvent("voice.radio::clearWave")]
        public static void RemoteEvent_ClearWave(ExtPlayer player)
        {
            try
            {
                VoiceMetaData voiceMeta = player.GetData<VoiceMetaData>("Voip");

                var radioRoom = RadioController.GetRoom(voiceMeta.RadioRoom);
                if (radioRoom == null) return;

                radioRoom.DisconnectPlayer(player);

                voiceMeta.RadioRoom = "";
                SafeTrigger.SetData(player, "Voip", voiceMeta);
            }
            catch (Exception e) { _logger.WriteError($"SOME ERROR CATCHED ON \"voice.radio::clearWave\": {e.ToString()}"); }
        }
    }
}
