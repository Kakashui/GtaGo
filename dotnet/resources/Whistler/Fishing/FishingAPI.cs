using GTANetworkAPI;
using Whistler.Fishing.Models;
using System;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Fishing
{
    class FishingAPI: Script
    {
        private static  FishingManager _manager;
        internal static FishShops FishShops;
        internal static Random Random;       

        /*
        [Command("sfs")]
        public void SetFishSpot(ExtPlayer client)
        {
            _manager.AddFishingSpot(client);
        }

        [Command("dfs")]
        public void DeleteFishingSpot(ExtPlayer client)
        {
            _manager.DeleteFishingSpot(client);
        }
        */

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            Random = new Random(DateTime.Now.Millisecond);
            //GearShops = new GearShops(FishingTraders.GearShops);
            FishShops = new FishShops(FishingTraders.FishShops);
            _manager = new FishingManager();
            _manager.LoadConfig();
            //InventoryService.OnUseItem += FishingManager.UseItem;
        }      
               
        [RemoteEvent(Const.EVENT_ACTION)]
        public void OnPressActionButton(ExtPlayer client)
        {
            _manager.DoAction(client);
        }     

        [RemoteEvent(Const.EVENT_BUY_GEAR)]
        public void BuyGear(ExtPlayer client, int id)
        {
            //GearShops.Buy(client, id);
        }

        [RemoteEvent(Const.EVENT_DROP_FISH)]
        public void DropFish(ExtPlayer client, int id, int count)
        {
            _manager.DropFish(client, id, count);
        }

        [RemoteEvent(Const.EVENT_CELL_FISH)]
        public void CellFish(ExtPlayer client, int id)
        {
            FishShops.CellFish(client, id);
        }

        [RemoteEvent(Const.EVENT_MINI_GAME_END)]
        public void EndMiniGame(ExtPlayer client, bool result)
        {
            _manager.EndMiniGame(client, result);
        }
    }
}
