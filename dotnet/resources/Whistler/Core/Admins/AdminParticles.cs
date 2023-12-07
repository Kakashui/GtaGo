using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.CustomSync;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core.Admins
{
    public class AdminParticles : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AdminParticles));
        private static readonly Dictionary<string, (string assetName, string effectName)> AppearanceEffectByName = new Dictionary<string, (string assetName, string effectName)>();

        private static readonly (string assetName, string effectName) DeafultAppearanceEffect = (assetName: "core", effectName: "bul_gravel_heli");

        [Command("apfx")]
        public void SetAdminAppearanceEffect(ExtPlayer player, string effectName)
        {
            try
            {
                var appearanceEffect = (assetName: "core", effectName: effectName);
                if (!AppearanceEffectByName.ContainsKey(player.Name))
                {
                    AppearanceEffectByName.Add(player.Name, appearanceEffect);
                }
                else
                {
                    AppearanceEffectByName[player.Name] = appearanceEffect;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
        }

        public static void PlayAdminAppearanceEffect(ExtPlayer player, float zPosition = default)
        {
            if (player.Session.Invisible) return;

            var particlesPosition = player.Position;
            if (zPosition != default)
            {
                particlesPosition.Z = zPosition;
            }

            var appearanceEffect = AppearanceEffectByName.GetValueOrDefault(player.Name, DeafultAppearanceEffect);
            Particles.PlayParticlesAtPosition(particlesPosition, appearanceEffect.assetName, appearanceEffect.effectName, 3, 1000);
        }
    }
}
