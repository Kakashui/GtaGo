using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.Core.LifeSystem
{
    public sealed class Thirst : LifePart
    {
        protected override string StatusDisplayKey => "thirstLevel";
      

        private bool _isStaminaDescreased = false;

        public Thirst(int level, LifeActivity lifeActivity) : base(level, lifeActivity)
        {
        }

        public override void ApplyEffects()
        {
            // Уменьшение стамины
            /*if (Level < 20)
            {
                if (!_isStaminaDescreased)
                {
                    _isStaminaDescreased = true;
                    _parent.InvokeAction(p => p.TriggerEventSafe("lifesystem:toggleStaminaDecreased", true));
                }
            }
            else if (_isStaminaDescreased)
            {
                _isStaminaDescreased = false;
                _parent.InvokeAction(p => p.TriggerEventSafe("lifesystem:toggleStaminaDecreased", false));
            }*/
        }

        public override void DecreaseLevel()
        {
            /*if (Level > 0)
            {
                Level--;
            }*/
        }


        public void HandleItemUse(LifeActivityData data)
        {
            Level = Math.Min(100, Math.Max(0, Level + data.ThirstIncrease));
        }
    }
}
