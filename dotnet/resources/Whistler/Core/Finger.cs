using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.MP.OrgBattle.BattleModels;
using Whistler.SDK;

namespace Whistler.Core
{
    class Fingerpointing : Script
    {
        [RemoteEvent("server.fpsync.update")]
        public void FingerSyncUpdate(ExtPlayer player, float camPitch, float camHeading)
        {
            if (player == null) return;
            SafeTrigger.ClientEventInRange(player.Position, 250f, "client.fpsync.update", player.Value, camPitch, camHeading);
        }
    }
}
