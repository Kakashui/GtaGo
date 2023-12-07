using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.Core.LifeSystem
{
    public sealed class Hunger : LifePart
    {
        protected override string StatusDisplayKey => "hungerLevel";

        private const string GaspEffectName = "FocusIn";

        private DateTime _nextFall = DateTime.Now;
        private DateTime _nextStomachSound = DateTime.Now;
        private bool _isGaspEffectStarted = false;
        private bool _isCantRunJump = false;


        public Hunger(int level, LifeActivity lifeActivity) : base(level, lifeActivity)
        {
        }

        public override void ApplyEffects()
        {
            /*if (Level < 30 && _nextStomachSound < DateTime.Now)
            {
                _parent.InvokeAction(p => p.TriggerEventSafe("lifesystem:playStomachSound"));
                _nextStomachSound = _nextStomachSound.AddMinutes(5);
            }
            if (Level < 20)
            {
                if (!_isGaspEffectStarted)
                {
                    _isGaspEffectStarted = true;
                    _parent.InvokeAction(p => p.TriggerEventSafe("lifesystem:startScreenEffect", GaspEffectName));
                }
            }
            else if (_isGaspEffectStarted)
            {
                _isGaspEffectStarted = false;
                _parent.InvokeAction(p => p.TriggerEventSafe("lifesystem:stopScreenEffect", GaspEffectName));
            }

            if (Level < 15 && _nextFall < DateTime.Now)
            {
                _nextFall = _nextFall.AddMinutes(4);
            }

            if (Level < 10)
            {
                _parent.InvokeAction(p => p.TriggerEventSafe("lifesystem:disableMoveOnTime", 1500));
            }

            if (Level < 5)
            {
                _parent.InvokeAction(p => p.Health -= 10);
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
            Level =  Math.Min(100, Math.Max(0, Level + data.HungerIncrease));
        }
    }
}
