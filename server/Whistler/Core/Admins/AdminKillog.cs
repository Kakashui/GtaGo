using System;
using System.Linq;
using System.Text.RegularExpressions;
using GTANetworkAPI;
using Whistler.Core.ReportSystem;
using Whistler.GUI;
using Whistler.SDK;

namespace Whistler.Core.Admins
{
    public class AdminKillog : Script
    {
        private const string AdminKillogDataName = "adm_killog_enabled";
        
        [Command("killog")]
        public static void ToggleAdminKillog(Player player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "killog")) return;

                if (!player.HasData(AdminKillogDataName))
                {
                    player.SetData(AdminKillogDataName, true);
                }
                else
                {
                    var currentToggle = Convert.ToBoolean(player.GetData<bool>(AdminKillogDataName));
                    player.SetData(AdminKillogDataName, !currentToggle);
                }
                var toggle = Convert.ToBoolean(player.GetData<bool>(AdminKillogDataName));
                if (toggle)
                {
                    Notify.SendInfo(player, "killog:on");
                    CaptureUI.EnableKillLog(player);
                }
                else
                {
                    Notify.SendInfo(player, "killog:off");
                    CaptureUI.DisableKillog(player);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception at: " + ex);
            }
        }

        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(Player victim, Player killer, uint weapon)
        {
            try
            {
                var adminWithKillog = ReportManager.Admins
                    .Where(a => a.HasData(AdminKillogDataName) && a.GetData<bool>(AdminKillogDataName));
                
                foreach (var admin in adminWithKillog)
                    CaptureUI.AddKillogEmptyItem(admin, killer, victim, weapon);
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception at: " + ex);
            }
        }
    }
}