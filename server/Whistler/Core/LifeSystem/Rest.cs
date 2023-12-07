using GTANetworkAPI;
using System;

namespace Whistler.Core.LifeSystem
{
    public sealed class Rest : LifePart
    {
        protected override string StatusDisplayKey => "restLevel";

        public Rest(int level, LifeActivity lifeActivity) : base(level, lifeActivity) { }

        public override void ApplyEffects()
        {
        }

        public override void DecreaseLevel()
        {
        }

    }
}
