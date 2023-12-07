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
    public sealed class Joy : LifePart
    {
        protected override string StatusDisplayKey => "joyLevel";   

        private const string DarkScreenEffectName = "DeathFailNeutralIn";

        private int LifePartLevel = 0;
        private int JoyPartLevel = 0;

        private bool _isDarkScreenEnabled = false;

        public Joy(int level, LifeActivity lifeActivity) : base(level, lifeActivity)
        {

            /*lifeActivity.Hunger.LevelChange += RecalculateLevel;
            lifeActivity.Thirst.LevelChange += RecalculateLevel;
            lifeActivity.Rest.LevelChange += RecalculateLevel;

            JoyPartLevel = Convert.ToInt32(level * 0.4);
            RecalculateLevel();*/
        }

        public void HandleBuyTuning()
        {
            //IncreaseJoyPart(5);
        }

        public override void ApplyEffects()
        {
            /*if (Level < 30)
            {
                if (!_isDarkScreenEnabled)
                {
                    _isDarkScreenEnabled = true;
                    _parent.InvokeAction(p => p.TriggerEventSafe("lifesystem:startScreenEffect", DarkScreenEffectName));
                }
            }
            else if (_isDarkScreenEnabled)
            {
                _isDarkScreenEnabled = false;
                _parent.InvokeAction(p => p.TriggerEventSafe("lifesystem:stopScreenEffect", DarkScreenEffectName));
            }*/
        }

        public override void DecreaseLevel()
        {
            //JoyPartLevel--;
            //RecalculateLevel();
        }


        private void RecalculateLevel()
        {
            var hunger = (int)Math.Round(_parent.Hunger.Level / 100.0 * 20);
            var thirst = (int)Math.Round(_parent.Thirst.Level / 100.0 * 20);
            var rest = (int)Math.Round(_parent.Rest.Level / 100.0 * 20);

            LifePartLevel = hunger + thirst + rest;

            Level = LifePartLevel + JoyPartLevel;
        }

        public void HandleItemUse(LifeActivityData data)
        {
            IncreaseJoyPart(data.JoyIncrease);
        }

        private void IncreaseJoyPart(int increaseOnValue)
        {
            JoyPartLevel = (JoyPartLevel + increaseOnValue <= 40) ? Level + increaseOnValue : 40;
            //RecalculateLevel();
        }
    }
}
