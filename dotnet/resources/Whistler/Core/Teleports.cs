using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core
{
    public class Teleports : Script
    {
        private static List<Teleport> _Teleports = new List<Teleport>();

        [ServerEvent(Event.ResourceStart)]
        public void LoadTeleports()
        {
            var queryResult = MySQL.QueryRead("SELECT * FROM `teleports`");
            if (queryResult == null)
            {
                return;
            }

            foreach (DataRow row in queryResult.Rows)
            {
                var id = Convert.ToInt32(row["id"]);
                var enterPosition = JsonConvert.DeserializeObject<Vector3>(row["enterPoint"].ToString());
                var enterDimension = Convert.ToUInt32(row["enterDimension"]);

                var exitPosition = JsonConvert.DeserializeObject<Vector3>(row["exitPoint"].ToString());
                var exitDimension = Convert.ToUInt32(row["exitDimension"]);

                _Teleports.Add(new Teleport(id, enterPosition, enterDimension, exitPosition, exitDimension));
            }
        }

        public static void CreateTeleport(Vector3 enterPoint, uint enterDimension, Vector3 exitPoint, uint exitDimension)
        {
            var queryResult = MySQL.QueryRead("INSERT INTO `teleports`(`enterPoint`,`enterDimension`,`exitPoint`,`exitDimension`) " +
                "VALUES(@prop0, @prop1, @prop2, @prop3); SELECT @@identity;",
                JsonConvert.SerializeObject(enterPoint), enterDimension, JsonConvert.SerializeObject(exitPoint), exitDimension);

            var id = Convert.ToInt32(queryResult.Rows[0][0]);

            _Teleports.Add(new Teleport(id, enterPoint, enterDimension, exitPoint, exitDimension));
        }
        public static void DeleteTeleport(ExtPlayer player)
        {
            var teleport = _Teleports.FirstOrDefault(item => item.DistanceLessTwoMeters(player.Position, player.Dimension));
            if (teleport != null)
            {
                _Teleports.Remove(teleport);
                teleport.Destroy();
            }
        }

        private class Teleport
        {
            public int Id { get; }

            private InteractShape enterShape;
            private InteractShape exitShape;

            private Vector3 enterPosition;
            private uint enterDimension;

            private Vector3 exitPosition;
            private uint exitDimension;

            public Teleport(int id, Vector3 enterPosition, uint enterDimension, Vector3 exitPosition, uint exitDimension)
            {
                this.Id = id;
                this.enterPosition = enterPosition;
                this.enterDimension = enterDimension;
                this.exitPosition = exitPosition;
                this.exitDimension = exitDimension;

                enterShape = InteractShape.Create(enterPosition, 1, 2, enterDimension)
                    .AddMarker(0, enterPosition + new Vector3(0, 0, 0.7), 1, InteractShape.DefaultMarkerColor)
                    .AddInteraction(Enter, "Чтобы войти");

                exitShape = InteractShape.Create(exitPosition, 1, 2, exitDimension)
                    .AddMarker(0, exitPosition + new Vector3(0, 0, 0.7), 1, InteractShape.DefaultMarkerColor)
                    .AddInteraction(Exit, "Чтобы выйти");
            }

            private void Enter(ExtPlayer player)
            {
                player.ChangePosition(exitPosition + new Vector3(0, 0, 1.12));
                SafeTrigger.UpdateDimension(player,  exitDimension);
                Main.PlayerEnterInterior(player, exitPosition + new Vector3(0, 0, 1.12));
            }

            private void Exit(ExtPlayer player)
            {
                player.ChangePosition(enterPosition + new Vector3(0, 0, 1.12));
                SafeTrigger.UpdateDimension(player,  enterDimension);
                Main.PlayerEnterInterior(player, enterPosition + new Vector3(0, 0, 1.12));
            }
            public void Destroy()
            {
                enterShape.Destroy();
                exitShape.Destroy();
                MySQL.Query("DELETE FROM `teleports` WHERE `id` = @prop0", Id);
            }
            public bool DistanceLessTwoMeters(Vector3 pos, uint dim)
            {
                return dim == enterDimension && enterPosition.DistanceTo(pos) < 2 || dim == exitDimension && exitPosition.DistanceTo(pos) < 2;
            }
        }
    }
}
