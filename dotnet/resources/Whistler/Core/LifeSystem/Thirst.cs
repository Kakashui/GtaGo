using Whistler.Helpers;
using Whistler.Inventory.Configs.Models;
using Whistler.SDK;
using Whistler.Entities;

namespace Whistler.Core.LifeSystem
{
    public class Thirst
    {
        private bool _isStaminaDescreased = false;
        public int Level;

        public Thirst(int level = 100)
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
            if (Level > 20 && !_isStaminaDescreased) return;

            if (Level <= 20)
            {
                if (_isStaminaDescreased) return;

                _isStaminaDescreased = true;
                SafeTrigger.ClientEvent(player, "lifesystem:toggleStaminaDecreased", true);
                return;
            }
            else if (_isStaminaDescreased)
            {
                _isStaminaDescreased = false;
                SafeTrigger.ClientEvent(player, "lifesystem:toggleStaminaDecreased", false);
            }
        }

        public void HandleItemUse(ExtPlayer player, LifeActivityData data)
        {
            int tempLevel = data == null ? Level : Level + data.ThirstIncrease;
            Level = tempLevel > 100 ? 100 : tempLevel < 0 ? 0 : tempLevel;
            Sync(player);
            ApplyEffects(player, false);
        }

        public void Sync(ExtPlayer player)
        {
            if (player == null || !player.IsLogged()) return;

            SafeTrigger.ClientEvent(player, "lifesystem:setStatsByKey", "thirstLevel", Level);
        }
    }
}
