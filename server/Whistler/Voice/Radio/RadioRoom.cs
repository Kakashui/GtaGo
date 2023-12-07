using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Voice.Radio
{
    class RadioRoom
    {
        public string ID { get; }

        private List<Player> _connectedPlayers = new List<Player>();

        public RadioRoom(string id)
        {
            ID = id;
        }

        public void ConnectPlayer(Player player)
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

                    p.TriggerEvent("voice.radio:add", player, isMuted);

                    playersMuteState.Add(p.GetData<string>("voicechat.state") == "ONLY_LOCAL");
                    idsToConnect.Add(p.Value);
                }
            });

            player.TriggerEvent("voice.radio:addRange", idsToConnect, playersMuteState);

            _connectedPlayers.Add(player);
        }

        public void DisconnectPlayer(Player player)
        {
            _connectedPlayers.Remove(player);

            _connectedPlayers.ForEach(p =>
            {
                if (p != null && p.Exists)
                {
                    if (p.Position.DistanceTo(player.Position) > 11)
                        p.DisableVoiceTo(player);
                    p.TriggerEvent("voice.radio:remove", player);
                }
            });

            player.TriggerEvent("voice.radio:disconnect");
        }

        public void DisconnectPlayerById(Player player)
        {
            _connectedPlayers.Remove(player);

            _connectedPlayers.ForEach(p =>
            {
                if (p != null && p.Exists)
                {
                    if (p.Position.DistanceTo(player.Position) > 11)
                        p.DisableVoiceTo(player);
                    p.TriggerEvent("voice.radio:removeById", player.Value);
                }
            });

            player.TriggerEvent("voice.radio:disconnect");
        }

        public void ToggleMutePlayer(Player player, bool mute)
        {
            if (!_connectedPlayers.Contains(player)) return;

            _connectedPlayers.ForEach(p =>
            {
                if (p != null && p.Exists && p != player)
                    p.TriggerEvent("voice.radio:toggleMute", player, mute);
            });
        }

        public void OnRoomRemove()
        {
            _connectedPlayers.ForEach(p => p.TriggerEvent("voice.radio:disconnect"));
        }
    }
}
