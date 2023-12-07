using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.NewDonateShop;
using Whistler.NewDonateShop.Configs;
using Whistler.NewDonateShop.Models;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class VehicleReward : DonateInventoryReward
    {
        public VehicleReward(ItemModel item) : base(item, "Reward_Vehicle", "VehicleReward")
        {
        }
        public VehicleReward(List<ItemModel> item) : base(item, "Reward_Vehicle", "VehicleReward")
        {
        }
    }
}
