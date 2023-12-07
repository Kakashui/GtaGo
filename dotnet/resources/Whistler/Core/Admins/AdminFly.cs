using System;
using GTANetworkAPI;
using Whistler.Core.Admins;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core
{
    class AdminFly : Script
    {
        public static WhistlerLogger _logger = new WhistlerLogger(typeof(AdminFly));

        [RemoteEvent("FlyToggle")]
        public void Admin_FlyToogle(ExtPlayer player, bool toggle, float zOffset)
        {
            try
            {
                Admin.SetInvisible(player, toggle);
                if (!toggle) AdminParticles.PlayAdminAppearanceEffect(player, zOffset);
            }
            catch (Exception e) {
                _logger.WriteError($"Admin_FlyToogle:\n{e}");
            }

        }
    }
}
