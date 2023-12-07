using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.SDK;
using Whistler.Inventory.Enums;
using Whistler.Entities;

namespace Whistler.Fractions.Models
{
    class ArmorGivePoint
    {
        private static Dictionary<int, DateTime> _lastGetArmor = new Dictionary<int, DateTime>();
        private static Dictionary<int, int> _countArmorGetting = new Dictionary<int, int>();
        public int Id { get; set; }
        public Vector3 Position { get; set; }
        public uint Dimension { get; set; }
        public int FractionId { get; set; }
        public InteractShape Shape { get; protected set; }
        public ArmorGivePoint(int fractionId, Vector3 position, uint dimension)
        {
            Position = position;
            FractionId = fractionId;
            Dimension = dimension;
            var dataQuery = MySQL.QueryRead("INSERT INTO `armorpoints`(`fractionid`, `dimension`, `position`) " +
                "VALUES (@prop0, @prop1, @prop2); SELECT @@identity;",
                FractionId, Dimension, JsonConvert.SerializeObject(Position));
            var id = Convert.ToInt32(dataQuery.Rows[0][0]);
            Id = id;
            CreateInteract();
        }
        public ArmorGivePoint(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            FractionId = Convert.ToInt32(row["fractionid"]);
            Dimension = Convert.ToUInt32(row["dimension"]);
            Position = JsonConvert.DeserializeObject<Vector3>(row["position"].ToString());
            CreateInteract();
        }
        private void CreateInteract()
        {
            if (Shape != null)
                Shape.Destroy();
            Shape = InteractShape.Create(Position, 1, 2, Dimension)
                .AddInteraction(GiveArmor, "interact_38".Translate(Id))
                .AddEnterPredicate(EnterPredicate)
                .AddDefaultMarker();
        }
        private void GiveArmor(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.FractionID != FractionId)
                return;
            int uuid = player.Character.UUID;
            if (_lastGetArmor.GetValueOrDefault(uuid, DateTime.Now) > DateTime.Now)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "armpoints_5", 3000);
                return;
            }
            var armor = ItemsFabric.CreateClothes(ItemNames.BodyArmor, true, 0, 0, true);
            if (armor == null)
                return;
            if (player.GetInventory().AddItem(armor))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "armpoints_4", 3000);
                SetOrAddLastGettingArmor(uuid);
            }
        }
        private bool EnterPredicate(ColShape shape, ExtPlayer player)
        {
            return player.IsLogged() && (player.Character.FractionID == FractionId || player.IsAdmin());
        }
        public void Destroy()
        {
            Shape.Destroy();
            MySQL.Query("DELETE FROM `armorpoints` WHERE id = @prop0", Id);
        }

        private static void SetOrAddLastGettingArmor(int uuid)
        {
            if (!_countArmorGetting.ContainsKey(uuid))
                _countArmorGetting.Add(uuid, 1);
            else
            {
                _countArmorGetting[uuid]++;
                if (_countArmorGetting[uuid] >= 3)
                {
                    _countArmorGetting.Remove(uuid);
                    if (_lastGetArmor.ContainsKey(uuid))
                        _lastGetArmor[uuid] = DateTime.Now.AddHours(1);
                    else
                        _lastGetArmor.Add(uuid, DateTime.Now.AddHours(1));
                }
            }
        }
    }
}
