using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.GUI.Lifts
{
    class Floor
    {
        public string Name { get; private set; }
        private Vector3 Position { get; set; }
        private Vector3 Rotation { get; set; }
        private Func<Player, bool> _enterPredicate { get; set; }
        private Func<Player, bool> _exitPredicate { get; set; }
        private bool Exit { get; set; }
        private uint Dimension { get; set; }
        private InteractShape _shape { get; set; }
        public Floor(string name, Vector3 position, Vector3 rotation, uint dimension, Func<Player, bool> exitPredicate, bool exit)
        {
            Name = name;
            Position = position;
            Rotation = rotation;
            Exit = exit;
            Dimension = dimension;
            _exitPredicate = exitPredicate;
        }
        public bool IsExit(Player player)
        {
            if (_exitPredicate != null)
                return _exitPredicate(player) && Exit;
            return Exit;
        }
        public void GoToFloor(Player player)
        {
            player.ChangePosition(Position + new Vector3(0, 0, 1.12));
            if (Rotation != null)
                player.Rotation = Rotation;
            player.Dimension = Dimension;

            Main.PlayerEnterInterior(player, Position + new Vector3(0, 0, 1.12));
        }
        public Floor AddInteract(Action<Player> action, Func<Player, bool> enterPredicate, bool marker)
        {
            _enterPredicate = enterPredicate;
            _shape = InteractShape.Create(Position, 1, 2, Dimension)
                .AddInteraction((player) =>
                {
                    if (_enterPredicate != null && !_enterPredicate(player))
                        return;
                    action(player);
                }, "interact_5");
            if (marker)
                _shape.AddDefaultMarker();
            return this;
        }

        public void Destroy()
        {
            if (_shape != null)
                _shape.Destroy();
        }
    }
}
