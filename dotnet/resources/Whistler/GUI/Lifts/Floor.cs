using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.GUI.Lifts
{
    class Floor
    {
        public string Name { get; private set; }
        private Vector3 Position { get; set; }
        private Vector3 Rotation { get; set; }
        private Func<ExtPlayer, bool> _enterPredicate { get; set; }
        private Func<ExtPlayer, bool> _exitPredicate { get; set; }
        private bool Exit { get; set; }
        private uint Dimension { get; set; }
        private InteractShape _shape { get; set; }
        public Floor(string name, Vector3 position, Vector3 rotation, uint dimension, Func<ExtPlayer, bool> exitPredicate, bool exit)
        {
            Name = name;
            Position = position;
            Rotation = rotation;
            Exit = exit;
            Dimension = dimension;
            _exitPredicate = exitPredicate;
        }
        public bool IsExit(ExtPlayer player)
        {
            if (_exitPredicate != null)
                return _exitPredicate(player) && Exit;
            return Exit;
        }
        public void GoToFloor(ExtPlayer player)
        {
            player.ChangePosition(Position + new Vector3(0, 0, 1.12));
            if (Rotation != null)
                player.Rotation = Rotation;
            SafeTrigger.UpdateDimension(player,  Dimension);

            Main.PlayerEnterInterior(player, Position + new Vector3(0, 0, 1.12));
        }
        public Floor AddInteract(Action<ExtPlayer> action, Func<ExtPlayer, bool> enterPredicate, bool marker)
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
