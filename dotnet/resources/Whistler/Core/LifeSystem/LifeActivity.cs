using System;
using System.Data;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Configs.Models;
using Whistler.SDK;

namespace Whistler.Core.LifeSystem
{
    public class LifeActivity
    {
        public Hunger Hunger { get; }
        public Thirst Thirst { get; }

        public LifeActivity(DataRow data)
        {
            Hunger = new Hunger(Convert.ToInt32(data["hungerlevel"]));
            Thirst = new Thirst(Convert.ToInt32(data["thirstlevel"]));
        }

        public LifeActivity()
        {
            Hunger = new Hunger();
            Thirst = new Thirst();
        }

        public static void Initialize()
        {
            InventoryService.OnUseLifeActivityItem += HandleItemUse;
        }

        public static void Subscribe(ExtPlayer player)
        {
            if (player.Session.Timers.LifeTimer != null) Timers.Stop(player.Session.Timers.LifeTimer);
            player.Session.Timers.LifeTimer = Timers.Start(180000, () => 
            {
                CalculateLife(player);
            });
        }

        public static void Destroy(ExtPlayer player)
        {
            if (player == null) return;
            if (player.Session == null) return;
            if (player.Session.Timers.LifeTimer == null) return;

            Timers.Stop(player.Session.Timers.LifeTimer);
            player.Session.Timers.LifeTimer = null;
        }


        private static void HandleItemUse(ExtPlayer player, LifeActivityData data)
        {
            if (player == null || !player.IsLogged())
            {
                Destroy(player);
                return;
            }

            player.Character.LifeActivity.Hunger.HandleItemUse(player, data);
            player.Character.LifeActivity.Thirst.HandleItemUse(player, data);
            if (data.Hp <= 0) return;

            int tempHealth = player.Health + data.Hp;
            tempHealth = tempHealth > 100 ? 100 : tempHealth;
            player.Health = tempHealth;
        }


        public static void Sync(ExtPlayer player)
        {
            if (player == null || !player.Logged()) return;

            player.Character.LifeActivity.Hunger.Sync(player);
            player.Character.LifeActivity.Thirst.Sync(player);
        }

        private static void CalculateLife(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (player.Character.AdminLVL > 0) return;

            player.Character.LifeActivity.Hunger.DecreaseLevel(player);
            player.Character.LifeActivity.Thirst.DecreaseLevel(player);
        }
    }
}