using System;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MP.Arena.Battles;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.Racing;
using Whistler.MP.Arena.UI;
using Whistler.MP.Arena.UI.DTO;

namespace Whistler.MP.Arena.Events
{
    public class TestEvents : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(TestEvents));
        
        [Command("ta")]
        public void t(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "test")) return;

                ArenaUiUpdateHandler.OpenMenuForPlayer(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(t)}: {ex}");
            }
        }
        
        [Command("la")]
        [RemoteEvent("ab:leave")]
        private static void OnPlayerLeaved(ExtPlayer player)
        {
            try
            {
                //if (!Group.CanUseCmd(player, "test")) return;

                BattleManager.GetPlayerBattle(player, out var leaverMemberModel);
                BattleManager.PlayerLeave(leaverMemberModel);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(OnPlayerLeaved)}: {ex}");
            }
        }

        [Command("rstart")]
        private static void OnPlayerStart(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "test")) return;

                RacingManager.StartNextRace();
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(OnPlayerLeaved)}: {ex}");
            }
        }
        
        [Command("reg")]
        private static void OnPlayerReg(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "test")) return;

                RacingManager.CurrentMap.RegisterPlayer(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(OnPlayerLeaved)}: {ex}");
            }
        }
    }
}