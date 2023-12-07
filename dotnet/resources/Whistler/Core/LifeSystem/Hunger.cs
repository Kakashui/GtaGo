using System;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs.Models;
using Whistler.SDK;

namespace Whistler.Core.LifeSystem
{
    public class Hunger
    {
        private const string GaspEffectName = "FocusIn";
        private DateTime _nextNotify = DateTime.Now;
        private bool _isGaspEffectStarted = false;
        public int Level;

        public Hunger(int level = 100)
        {
            Level = level;
        }

        public void DecreaseLevel(ExtPlayer player)
        {
            if (Level <= 0)
            {
                ApplyEffects(player);
                return;
            }

            Level -= 1;
            ApplyEffects(player);
            Sync(player);
        }

        private void ApplyEffects(ExtPlayer player, bool negativeAllowed = true)
        {
            int healthPenalty = !negativeAllowed ? 0 : Level >= 50 ? 0 : Level >= 30 ? 2 : 5;
            bool gaspEffect = negativeAllowed && Level < 30;
            bool fallEffect = negativeAllowed && Level <= 20;
            if (negativeAllowed && healthPenalty == 0) return;

            DateTime now = DateTime.Now;
            bool notify = false;
            if (now >= _nextNotify && Level <= 55)
            {
                notify = true;
                _nextNotify = now.AddMinutes(5);
            }

            if (healthPenalty > 0)
            {
                int pHealth = player.Health - healthPenalty;
                if (pHealth < 1) pHealth = 1;
                player.Health = pHealth;
            }
            if (notify) Notify.SendAlert(player, "Вы голодны. Когда голод упадёт слишком сильно, то Вы начнёте терять сознание.");
            if (fallEffect) player.TriggerEventSafe("lifesystem:enableRagdoll");

            if (gaspEffect && !_isGaspEffectStarted)
            {
                _isGaspEffectStarted = true;
                player.TriggerEventSafe("lifesystem:startScreenEffect", GaspEffectName);
            }
            else if (!gaspEffect && _isGaspEffectStarted)
            {
                _isGaspEffectStarted = false;
                player.TriggerEventSafe("lifesystem:stopScreenEffect", GaspEffectName);
            }
        }

        public void HandleItemUse(ExtPlayer player, LifeActivityData data)
        {
            int tempLevel = data == null ? Level : Level + data.HungerIncrease;
            Level = tempLevel > 100 ? 100 : tempLevel < 0 ? 0 : tempLevel;
            Sync(player);
            ApplyEffects(player, false);
        }

        public void Sync(ExtPlayer player)
        {
            if (player == null || !player.IsLogged()) return;

            SafeTrigger.ClientEvent(player, "lifesystem:setStatsByKey", "hungerLevel", Level);
        }
    }
}
