using System;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.MP.Arena.AdminEvents
{
    internal class AdminGameEventsManager : Script
    {
        private WhistlerLogger _logger = new WhistlerLogger(typeof(AdminGameEventsManager));
        public (Vector3, string, uint)? CurrentTeleport;
        
        [Command("opentp", GreedyArg = true)]
        public void CreateTeleport(ExtPlayer player, string teleportName)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "opentp")) return;
                CurrentTeleport = (player.Position + new Vector3(0, 0, 1), teleportName, player.Dimension);
                Chat.AdminToAll("events_14".Translate(teleportName));
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception at {nameof(CreateTeleport)}: {ex}");
            }
        }
        
        [Command("closetp")]
        public void DeleteTeleport(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "opentp")) return;
                CurrentTeleport = null;
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception at {nameof(DeleteTeleport)}: {ex}");
            }
        }

        [Command("usetp")]
        public void UseTeleport(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (CurrentTeleport == null)
                {
                    Notify.SendError(player, "usetp:close");
                    return;
                }

                if (player.HasData("usedTp") && player.GetData<string>("usedTp") == CurrentTeleport?.Item2)
                {
                    Notify.SendError(player, "usetp:exists");
                    return;
                }

                if (player.Character.Cuffed)
                {
                    Notify.SendError(player, "Frac_180");
                    return;
                }
                
                SafeTrigger.UpdateDimension(player,  CurrentTeleport?.Item3 ?? 0);
                player.ChangePosition(CurrentTeleport?.Item1);
                SafeTrigger.SetData(player, "usedTp", CurrentTeleport?.Item2);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception at {nameof(UseTeleport)}: {ex}");
            }
        }
    }
}