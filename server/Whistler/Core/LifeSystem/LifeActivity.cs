using GTANetworkAPI;
using System;
using System.Data;
using System.Text;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Configs.Models;
using Whistler.SDK;

namespace Whistler.Core.LifeSystem
{
    public class LifeActivity
    {
        public Joy Joy { get; }
        public Hunger Hunger { get; }
        public Thirst Thirst { get; }
        public Rest Rest { get; }
        public Health Health { get; }

        private Player _player;
        private string _lifeTimer;

        public LifeActivity(DataRow data)
        {
            Hunger = new Hunger(Convert.ToInt32(data["hungerlevel"]), this);
            Thirst = new Thirst(Convert.ToInt32(data["thirstlevel"]), this);
            Rest = new Rest(Convert.ToInt32(data["restlevel"]), this);
            Joy = new Joy(Convert.ToInt32(data["joylevel"]), this);
        }

        public LifeActivity()
        {
            Hunger = new Hunger(100, this);
            Thirst = new Thirst(100, this);
            Rest = new Rest(100, this);
            Joy = new Joy(100, this);

        }

        public void Subscribe(Player player)
        {
            _player = player;

            Main.PlayerPreDisconnect += HandlePlayerDisconnect;
            InventoryService.OnUseLifeActivityItem += HandleItemUse;
            BusinessManager.PlayerBuyTuning += HandleBuyTuning;
            _lifeTimer = Timers.Start(120 * 1000, CalculateLife);
        }

        private void HandleBuyTuning(Player player)
        {
            if (player != _player) return;

            Joy.HandleBuyTuning();
        }

        private void CalculateLife()
        {
            if (!_player.IsLogged())
            {
                Destroy();
                return;
            }

            if (_player.GetCharacter().AdminLVL > 0)
            {
                return;
            }

            Joy.DecreaseLevel();
            Hunger.DecreaseLevel();
            Thirst.DecreaseLevel();
            Rest.DecreaseLevel();
        }

        private void Destroy()
        {
            Main.PlayerPreDisconnect -= HandlePlayerDisconnect;
            InventoryService.OnUseLifeActivityItem -= HandleItemUse;
            Timers.Stop(_lifeTimer);

        }

        private void HandlePlayerDisconnect(Player player)
        {
            if (_player != player) return;

            Destroy();
        }
        private void HandleItemUse(Player player, LifeActivityData data)
        {
            if (player != _player) return;
            Hunger.HandleItemUse(data);
            Thirst.HandleItemUse(data);
            Joy.HandleItemUse(data);
        }

        public void InvokeAction(Action<Player> action)
        {
            if (_player.IsLogged())
                action?.Invoke(_player);
        }
    }

    public abstract class LifePart
    {
        public event Action LevelChange;

        public int Level 
        { 
            get => _level;
            set
            {
                _level = value;
                ApplyEffects();
                LevelChange?.Invoke();
                _parent.InvokeAction(p => p.TriggerEventSafe("lifesystem:setStatsByKey", StatusDisplayKey, value));
            }
        }

        protected abstract string StatusDisplayKey { get; }

        protected LifeActivity _parent;

        private int _level;

        public LifePart(int level, LifeActivity parent)
        {
            _parent = parent;

            Level = level;
        }

        public abstract void DecreaseLevel();

        public abstract void ApplyEffects();
    }
}