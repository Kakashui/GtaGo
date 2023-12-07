using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop;
using Whistler.NewDonateShop.Configs;
using Whistler.NewDonateShop.Models;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class DonateInventoryReward : RewardBase
    {
        List<ItemModel> _items;
        public DonateInventoryReward(ItemModel item) : base("Reward_Inventory", 0, "InventoryReward")
        {
            _items = new List<ItemModel> { item };
        }
        public DonateInventoryReward(ItemModel item, string desc, string image) : base(desc, 0, image)
        {
            _items = new List<ItemModel> { item };
        }
        public DonateInventoryReward(List<ItemModel> item) : base("Reward_Inventory", 0, "InventoryReward")
        {
            _items = item;
        }
        public DonateInventoryReward(List<ItemModel> item, string desc, string image) : base(desc, 0, image)
        {
            _items = item;
        }
        public override bool GiveReward(ExtPlayer player, string commentParam)
        {
            return GiveItem(player.Character.DonateInventory, _items.GetRandomElement());
        }
        public override bool GiveReward(int playerUUID, string commentParam)
        {
            var player = Trigger.GetPlayerByUuid(playerUUID);
            if (player.IsLogged())
                return GiveReward(player, commentParam);
            else
            {
                var result = MySQL.QueryRead("SELECT donateInventoryId FROM characters WHERE `uuid` = @prop0", playerUUID);
                if (result == null || result.Rows.Count == 0)
                    return false;
                if (!result.Rows[0].IsNull("donateInventoryId"))
                {
                    var donateInventory = DonateService.GetInventoryById(Convert.ToInt32(result.Rows[0]["donateInventoryId"]));
                    if (donateInventory != null)
                        return GiveItem(donateInventory, _items.GetRandomElement());
                }
            }
            return false;
        }

        private bool GiveItem(DonateInventoryModel donateInventory, ItemModel item)
        {
            if (item.Data is ComplectDonateItems)
            {
                foreach (var id in (item.Data as ComplectDonateItems).Items)
                    donateInventory.AddItem(id, true, true);
            }
            else if (item.Data is ComplectGenderDonateItem)
            {
                foreach (var id in (item.Data as ComplectGenderDonateItem).Items)
                    donateInventory.AddItem(id, true, true);
            }
            else
            {
                donateInventory.AddItem(item.Id, true, true);
            }
            return true;
        }
    }
}
