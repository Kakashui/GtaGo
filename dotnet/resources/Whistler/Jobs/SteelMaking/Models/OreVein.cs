using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.SDK;

namespace Whistler.Jobs.SteelMaking.Models
{    class OreVein
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(OreMining));
        public ColShape Shape { get; set; }
        public Vector3 Position { get; set; }
        public int Point { get; set; }
        private ColShape ShapeDrops { get; set; }
        private int CountOresDrop { get; set; }
        public bool Status { get; set; }
        private Marker _marker;
        private Blip _blip;
        public OreVein(int point, Vector3 position)
        {
            Point = point;
            Position = position;
            ShapeDrops = null;
            CountOresDrop = 0;
            Status = true;
            Shape = NAPI.ColShape.CreateCylinderColShape(Position, OreMiningSettings.VeinRange, 6, 0);
            Shape.OnEntityEnterColShape += (ColShape colShape, Player player) => EnterOreVeinColshapeHandler((ExtPlayer)player);
            Shape.OnEntityExitColShape += (ColShape colShape, Player player) => ExitOreVeinColshapeHandler((ExtPlayer)player);
            //_marker = NAPI.Marker.CreateMarker(1, Position, new Vector3(), new Vector3(), OreMiningSettings.VeinRange * 2, new Color(200, 0, 200, 50), false, 0);
            //_blip = NAPI.Blip.CreateBlip(0, Position, 2, 47, "Vein", 255, 0, true, 0, NAPI.GlobalDimension);
        }

        public void Destroy()
        {
            Shape?.Delete();
            _marker?.Delete();
            _blip?.Delete();
            SafeTrigger.ClientEventInRange(Position, OreMiningSettings.VeinRange + 4, "OreVein:ExitPoint", Point);
        }

        private void EnterOreVeinColshapeHandler(ExtPlayer player)
        {
            try
            {
                player.OreVeinID = Point;
                SafeTrigger.ClientEvent(player, "OreVein:EnterPoint", Point, Position);
            }
            catch (Exception e) { _logger.WriteError("Unhandled error on EnterInteractColshapeHandler: " + e.ToString()); }
        }

        private void ExitOreVeinColshapeHandler(ExtPlayer player)
        {
            try
            {
                if (player.OreVeinID == Point)
                {
                    player.OreVeinID = -1;
                    SafeTrigger.ClientEvent(player, "OreVein:ExitPoint", Point);
                }
            }
            catch (Exception e) { _logger.WriteError("Unhandled error on ExitInteractColshapeHandler: " + e.ToString()); }
        }

        public void ExplodeDynamite(Vector3 position)
        {
            if (!Status)
                return;
            Status = false;
            NAPI.Task.Run(() =>
            {
                Destroy();

                var dist = (int)position.DistanceTo2D(Position) + 1;
                CountOresDrop = new Random().Next(OreMiningSettings.MinOres, OreMiningSettings.MaxOres) / dist;


                SafeTrigger.ClientEventInRange(position, 200, "OreVein:explodeDynamit", position, Point, CountOresDrop);
                NAPI.Task.Run(() =>
                {
                    ShapeDrops = NAPI.ColShape.CreateCylinderColShape(position - new Vector3(0, 0, 30), 70, 100, 0);
                    ShapeDrops.OnEntityEnterColShape += (ColShape colShape, Player client) =>
                    {
                        if (!(client is ExtPlayer player)) return;

                        SafeTrigger.ClientEvent(player, "OreVein:GetDropPositions", Position, Point, CountOresDrop);
                    };
                }, 6000);
            }, 10000);
        }

        public void DropOresAndCoals(string dataPos)
        {

            if (ShapeDrops == null)
                return;
            List<Vector3> positions = JsonConvert.DeserializeObject<List<Vector3>>(dataPos);
            if (positions.Count != CountOresDrop)
                return;
            ShapeDrops.Delete();
            ShapeDrops = null;
            for (int i = 0; i < CountOresDrop; i++)
            {
                var randomItem = OreMiningSettings.ProbabilitySpawn.GetRandomElementWithProbability(item => item.Propability);
                switch (randomItem.ItemTypes)
                {
                    case ItemTypes.Coals:
                        var itemCoal = ItemsFabric.CreateCoals(randomItem.Name, 1, false);
                        DropSystem.DropItem(itemCoal, positions[i] + new Vector3(0, 0, 0.97), 0, false);
                        break;
                    case ItemTypes.Ore:
                        var itemOre = ItemsFabric.CreateOre(randomItem.Name, 1, false);
                        DropSystem.DropItem(itemOre, positions[i] + new Vector3(0, 0, 0.97), 0, false);
                        break;
                    default:
                        break;
                }
            }
            CountOresDrop = 0;
        }
    }
}
