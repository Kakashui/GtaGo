using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Voice.Radio
{
    class RadioRoom
    {
        public string ID { get; }

        private List<ExtPlayer> _connectedPlayers = new List<ExtPlayer>();

        public RadioRoom(string id)
        {
            ID = id;
        }

        public void ConnectPlayer(ExtPlayer player)
        {
            var idsToConnect = new List<int>();
            var playersMuteState = new List<bool>();

            var isMuted = (player.GetData<string>("voicechat.state") == "ONLY_LOCAL");

            _connectedPlayers.ForEach(p =>
            {
                if (p != null && p.Exists)
                {
                    p.EnableVoiceTo(player);
                    player.EnableVoiceTo(p);

                    SafeTrigger.ClientEvent(p, "voice.radio:add", player, isMuted);

                    playersMuteState.Add(p.GetData<string>("voicechat.state") == "ONLY_LOCAL");
                    idsToConnect.Add(p.Value);
                }
            });

            SafeTrigger.ClientEvent(player,"voice.radio:addRange", idsToConnect, playersMuteState);

            _connectedPlayers.Add(player);
        }

        public void DisconnectPlayer(ExtPlayer player)
        {
            _connectedPlayers.Remove(player);

            _connectedPlayers.ForEach(p =>
            {
                if (p != null && p.Exists)
                {
                    if (p.Position.DistanceTo(player.Position) > 11)
                        p.DisableVoiceTo(player);
                    SafeTrigger.ClientEvent(p, "voice.radio:remove", player);
                }
            });

            SafeTrigger.ClientEvent(player,"voice.radio:disconnect");
        }

        public void DisconnectPlayerById(ExtPlayer player)
        {
            if (!_connectedPlayers.Contains(player)) return;

            _connectedPlayers.Remove(player);

            _connectedPlayers.ForEach(p =>
            {
                if (p != null && p.Exists)
                {
                    if (p.Position.DistanceTo(player.Position) > 11)
                        p.DisableVoiceTo(player);
                    SafeTrigger.ClientEvent(p, "voice.radio:removeById", player.Value);
                }
            });

            SafeTrigger.ClientEvent(player,"voice.radio:disconnect");
        }

        public void ToggleMutePlayer(ExtPlayer player, bool mute)
        {
            if (!_connectedPlayers.Contains(player)) return;

            _connectedPlayers.ForEach(p =>
            {
                if (p != null && p.Exists && p != player)
                    SafeTrigger.ClientEvent(p, "voice.radio:toggleMute", player, mute);
            });
        }

        public void OnRoomRemove()
        {
            _connectedPlayers.ForEach(p => SafeTrigger.ClientEvent(p, "voice.radio:disconnect"));
        }
    }
}
