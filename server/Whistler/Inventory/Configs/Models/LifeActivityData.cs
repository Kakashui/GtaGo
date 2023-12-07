using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Inventory.Configs.Models
{
    public class LifeActivityData
    {
        public int HungerIncrease { get; set; }
        public int ThirstIncrease { get; set; }
        public int JoyIncrease { get; set; }
        public int RestIncrease { get; set; }
        public int Hp { get; set; }
        public LifeActivityData(){}

        public LifeActivityData(int hungerIncrease, int thirstIncrease, int joyIncrease, int restIncrease, int hp)
        {
            HungerIncrease = hungerIncrease;
            ThirstIncrease = thirstIncrease;
            JoyIncrease = joyIncrease;
            RestIncrease = restIncrease;
            Hp = hp;
        }

        public LifeActivityData GetMultipled(int multiple)
        {
            if (multiple < 1) multiple = 1;
            return new LifeActivityData(HungerIncrease * multiple, ThirstIncrease * multiple, JoyIncrease * multiple, RestIncrease * multiple, Hp * multiple);
        }
    }
}
