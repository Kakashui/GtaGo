using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.SDK.CustomColShape
{
    class ShapeManager : Script
    {
        private static int _id = 0;
        private static Dictionary<int, BaseColShape> _shapes = new Dictionary<int, BaseColShape>();

        private static void AddShape(BaseColShape shape)
        {
            _shapes.Add(++_id, shape);
            Trigger.ClientEventToPlayers(NAPI.Pools.GetAllPlayers().ToArray(), "customShape:add", JsonConvert.SerializeObject(shape.GetDTO(_id)));
        }

        public static BaseColShape CreateQuadColShape(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, uint dimension = uint.MaxValue)
        {
            BaseColShape shape = new QuadColShape(point1, point2, point3, point4, dimension);
            AddShape(shape);
            return shape;
        }

        public static BaseColShape CreateSquareColShape(Vector3 center, float width, float rotation, uint dimension = uint.MaxValue)
        {
            rotation %= 90;
            if (rotation < 0)
                rotation += 90;
            rotation += 45;
            BaseColShape shape = new SquareColShape(center, width, rotation, dimension);
            AddShape(shape);
            return shape;
        }

        [ServerEvent(Event.PlayerConnected)]
        public static void OnPlayerDisconnected(PlayerGo player)
        {
            player.TriggerEvent("customShape:load", JsonConvert.SerializeObject(_shapes.Select(item => item.Value.GetDTO(item.Key))));
        }

        [RemoteEvent("customShape:enterShape")]
        public static void OnPlayerEnterCustomColShape(PlayerGo player, int id)
        {
            _shapes.GetValueOrDefault(id)?.EnterColShape(player);
        }

        [RemoteEvent("customShape:exitShape")]
        public static void OnPlayerExitCustomColShape(PlayerGo player, int id)
        {
            _shapes.GetValueOrDefault(id)?.ExitColShape(player);
        }

        [Command("loadshape")]
        public static void LoadCustomShapes(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "loadshape")) return;
            player.TriggerEvent("loadCustomShapes", true);
        }

        [Command("unloadshape")]
        public static void UnloadCustomShapes(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "loadshape")) return;
            player.TriggerEvent("loadCustomShapes", false);
        }
    }
}
