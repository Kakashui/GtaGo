using System;
using GTANetworkAPI;
using Whistler.Core.Admins;
using Whistler.Helpers;

namespace Whistler.Core
{
    class AdminFly : Script
    {
        public static WhistlerLogger _logger = new WhistlerLogger(typeof(AdminFly));

        [RemoteEvent("FlyToggle")]
        public void Admin_FlyToogle(Player player, bool toggle, float zOffset)
        {
            try
            {
                if (toggle)
                {
                    player.SetSharedData("INVISIBLE", true);
                    player.Transparency = 0;
                    //player.SetData("FLY", true);
                }
                else
                {
                    player.SetSharedData("INVISIBLE", false);
                    player.Transparency = 255;
                    //player.SetData("FLY", false);
                    AdminParticles.PlayAdminAppearanceEffect(player, zOffset);
                }
            }
            catch (Exception e) {
                _logger.WriteError($"Admin_FlyToogle:\n{e}");
            }

        }
    }
}
