using GTANetworkAPI;
using Whistler.Core.InteractShapes;
using Whistler.Helpers;
using Newtonsoft.Json;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Whistler.Entities;

namespace Whistler.Core
{
    internal class InteractShape
    {
        #region Static
        public readonly static Color DefaultMarkerColor = new Color(182, 211, 0, 200);

        private const string DATA_INTERACT_ID = "INTERACT:ID";

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(InteractShape));
        private readonly static Dictionary<int, InteractShape> InteractionColshapes = new Dictionary<int, InteractShape>();
        private static int LastShapeId = 0;
        private static object _lockObject = new object();

        static InteractShape()
        {
            InteractShapeEvents.PlayerConnected += LoadMarkersForPlayer;
        }

        public static InteractShape Create(Vector3 position, float range, int height, uint dimension = 0)
        {
            int id;

            lock (_lockObject)
            {
                id = LastShapeId++;
            }

            var interactShape = new InteractShape(id, position, range, height, dimension);
            InteractionColshapes.Add(id, interactShape);

            return interactShape;
        }

        private static void LoadMarkersForPlayer(ExtPlayer player)
        {
            var markers = InteractionColshapes.Values
                .Where(i => i._markerInfo != null)
                .Select(i => i._markerInfo);

            player.TriggerEventWithLargeList("interact:loadMarkers", markers);
        }
        public static void PlayerPressInteractKeyHandlerStatic(ExtPlayer player, Key key)
        {
            if (player.DATA_INTERACT_ID > -1)
                InteractionColshapes.GetValueOrDefault(player.DATA_INTERACT_ID)?.PlayerPressInteractKeyHandler(player, key);
        }
        #endregion

        private ColShape _colshape;
        private Marker _marker;
        private MarkerInfo _markerInfo;

        private Action<ColShape, ExtPlayer> _enterColshapeHandler;
        private Action<ColShape, ExtPlayer> _exitColshapeHandler;
        private Func<ColShape, ExtPlayer, bool> _enterPredicate;

        private int _id;
        private uint _dimension = 0;
        private Vector3 _position;
        private Dictionary<Key, Interaction> _interactions = new Dictionary<Key, Interaction>();

        private InteractShape(int id, Vector3 position, float range, int height, uint dimension = 0)
        {
            _id = id;
            _dimension = dimension;
            _position = position;

            _colshape = NAPI.ColShape.CreateCylinderColShape(position, range, height, dimension);
            _colshape.OnEntityEnterColShape += (c, p) => EnterInteractColshapeHandler(c, p as ExtPlayer);
            _colshape.OnEntityExitColShape += (c, p) => ExitInteractColshapeHandler(c, p as ExtPlayer);

        }
        private void PlayerPressInteractKeyHandler(ExtPlayer player, Key key)
        {
            try
            {
                if (!_interactions.ContainsKey(key))
                {
                    return;
                }

                _interactions[key].Action?.Invoke(player);
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on colshape with coords {_position}, error: {e}"); }
        }

        public InteractShape AddEnterPredicate(Func<ColShape, ExtPlayer, bool> enterPredicate)
        {
            if (_enterPredicate != null)
            {
                throw new ArgumentException($"InteractShape check already exists", nameof(enterPredicate));
            }

            _enterPredicate = enterPredicate;
            return this;
        }

        public InteractShape AddDefaultMarker()
        {
            CreateMarker(_markerInfo = new MarkerInfo
            {
                ID = _id,
                Type = 27,
                Position = _position + new Vector3(0, 0, 0.01),
                Scale = 1f,
                Color = DefaultMarkerColor,
                Dimension = _dimension
            });

            return this;
        }

        public InteractShape AddDefaultMarker(Color color)
        {
            CreateMarker(new MarkerInfo
            {
                ID = _id,
                Type = 27,
                Position = _position + new Vector3(0, 0, 0.01),
                Scale = 1f,
                Color = color,
                Dimension = _dimension
            });

            return this;
        }

        public InteractShape AddMarker(int markerType, Vector3 pos, float scale, Color color)
        {
            CreateMarker(new MarkerInfo
            {
                ID = _id,
                Type = markerType,
                Position = pos,
                Scale = scale,
                Color = color,
                Dimension = _dimension
            });

            return this;
        }

        public InteractShape DeleteMarker()
        {
            if (_markerInfo != null)
            {
                SafeTrigger.ClientEventForAll("interact:destroyMarker", _id);
                _markerInfo = null;
            }
            return this;
        }

        public InteractShape AddInteraction(Action<ExtPlayer> action, string text = "Для взаимодействия", Key key = Key.VK_E)
        {
            var interaction = new Interaction(action, key, text);
            if (_interactions.ContainsKey(key))
                _interactions[key] = interaction;
            else
                _interactions.Add(key, interaction);
            return this;
        }

        public InteractShape AddOnExitColshapeExtraAction(Action<ColShape, ExtPlayer> action)
        {
            if (_exitColshapeHandler != null)
            {
                throw new ArgumentException($"InteractShape check exit already", nameof(action));
            }

            _exitColshapeHandler = action;
            return this;
        }

        public InteractShape AddOnEnterColshapeExtraAction(Action<ColShape, ExtPlayer> action)
        {
            if (_enterColshapeHandler != null)
            {
                throw new ArgumentException("InteractShape check enter already", nameof(action));
            }

            _enterColshapeHandler = action;
            return this;
        }
        public InteractShape DeleteInteraction(Key key)
        {
            if (_interactions.ContainsKey(key))
                _interactions.Remove(key);
            return this;
        }

        public void Destroy()
        {
            if (_colshape != null)
            {
                _colshape.Delete();
            }

            InteractionColshapes.Remove(_id);

            if (_markerInfo != null)
            {
                SafeTrigger.ClientEventForAll("interact:destroyMarker", _id);
            }
        }

        private void CreateMarker(MarkerInfo markerInfo)
        {
            _markerInfo = markerInfo;

            var dto = new List<MarkerInfo> { _markerInfo };
            SafeTrigger.ClientEventForAll("interact:loadMarkers", JsonConvert.SerializeObject(dto));
        }

        private void EnterInteractColshapeHandler(ColShape colShape, ExtPlayer client)
        {
            try
            {
                if (!client.IsLogged())
                    return;

                if (_enterPredicate != null)
                {
                    var result = _enterPredicate.Invoke(colShape, client);
                    if (!result)
                    {
                        return;
                    }
                }
                client.DATA_INTERACT_ID = _id;
                SafeTrigger.ClientEvent(client, "interact:enterInteractShape", JsonConvert.SerializeObject(_interactions.Values.Select(i => i.Dto).ToList()));

                _enterColshapeHandler?.Invoke(colShape, client);
            }
            catch (Exception e) { _logger.WriteError("Unhandled error on EnterInteractColshapeHandler: " + e.ToString()); }
        }

        private void ExitInteractColshapeHandler(ColShape colShape, ExtPlayer client)
        {
            try
            {
                if (!client.IsLogged())
                    return;
                if(client.DATA_INTERACT_ID == _id)
                {
                    client.DATA_INTERACT_ID = -1;
                    SafeTrigger.ClientEvent(client, "interact:exitInteractShape");
                }

                _exitColshapeHandler?.Invoke(colShape, client);
            }
            catch (Exception e) { _logger.WriteError("Unhandled error on ExitInteractColshapeHandler: " + e.ToString()); }
        }

        #region Interaction
        private class Interaction
        {
            public Key Key { get; set; }
            public string Text { get; set; }
            public Action<ExtPlayer> Action { get; set; }

            public InteractionDto Dto { get; }

            public Interaction(Action<ExtPlayer> action, Key key, string text)
            {
                Key = key;
                Text = text;
                Action = action;

                Dto = new InteractionDto { Key = (int)Key, Text = text };
            }
        }

        private class InteractionDto
        {
            public int Key { get; set; }
            public string Text { get; set; }
        }
        #endregion
    }

    internal class MarkerInfo
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public Vector3 Position { get; set; }
        public float Scale { get; set; }
        public Color Color { get; set; }
        public uint Dimension { get; set; }
    }

    internal enum Key
    {
        VK_E = 0x45,
        VK_I = 0x49
    }
}
