using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Inventory.Configs.Models
{
    public class LifeActivityData
    {
        public int HungerIncrease { get; set; } = 0;
        public int ThirstIncrease { get; set; } = 0;
        public int Hp { get; set; } = 0;
        public LifeActivityData(){}

        public LifeActivityData(int hungerIncrease = 0, int thirstIncrease = 0, int hp = 0)
        {
            HungerIncrease = hungerIncrease;
            ThirstIncrease = thirstIncrease;
            Hp = hp;
        }

        public LifeActivityData GetMultipled(int multiple)
        {
            if (multiple < 1) multiple = 1;
            return new LifeActivityData(HungerIncrease * multiple, ThirstIncrease * multiple, Hp * multiple);
        }
    }
}
