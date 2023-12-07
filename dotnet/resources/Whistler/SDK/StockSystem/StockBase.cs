using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.Possessions;
using Whistler.SDK;

namespace Whistler.SDK.StockSystem
{
    abstract class StockBase
    {
        public int Id { get; protected set; }
        public int InventoryId { get; protected set; }
        public int OwnerId { get; protected set; }
        public string Password { get; protected set; }
        public uint Dimension { get; protected set; }
        public int Size { get; protected set; }
        public OwnerType TypeOwner { get; protected set; }
        public Vector3 Position { get; protected set; }
        public InteractShape Shape { get; protected set; }
        public StockBase(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            InventoryId = Convert.ToInt32(row["inventoryid"]);
            OwnerId = Convert.ToInt32(row["fractionid"]);
            Password = row["password"] == DBNull.Value ? "h4d7fds" : Convert.ToString(row["password"]);
            Dimension = Convert.ToUInt32(row["dimension"]);
            Size = Convert.ToInt32(row["size"]);
            Position = JsonConvert.DeserializeObject<Vector3>(row["position"].ToString());
            CreateInteract();
        }

        public StockBase(int fractionId, string password, StockConfig config, Vector3 position, uint dimension, OwnerType typeOwner, int size = 1)
        {
            InventoryId = new InventoryModel(config.MaxWeight, config.Size, config.Type).Id;
            OwnerId = fractionId;
            Password = password;
            Position = position;
            Dimension = dimension;
            TypeOwner = typeOwner;
            Size = size;
            var dataQuery = MySQL.QueryRead("INSERT INTO `fractionstock`(`inventoryid`, `fractionid`, `password`, `dimension`, `position` , `typeowner`, `size`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6); SELECT @@identity;",
                InventoryId, OwnerId, Password, Dimension, JsonConvert.SerializeObject(Position), (int)TypeOwner, Size);
            var id = Convert.ToInt32(dataQuery.Rows[0][0]);
            Id = id;
            CreateInteract();
        }
        private void CreateInteract()
        {
            if (Shape != null)
                Shape.Destroy();
            Shape = InteractShape.Create(Position, Size, 3, Dimension)
                .AddInteraction(OpenStock, "interact_1".Translate(Id))
                .AddInteraction(ChangePassword, "stock:passwd:change", Key.VK_I)
                .AddEnterPredicate(EnterPredicate)
                .AddOnExitColshapeExtraAction(ExitShape)
                .AddMarker(27, Position + new Vector3(0, 0, 0.01), Size * 2, InteractShape.DefaultMarkerColor);
        }

        public abstract void OpenStock(ExtPlayer player);
        public abstract void ExitShape(ColShape shape, ExtPlayer player);
        public abstract void ChangePassword(ExtPlayer player);
        public abstract bool EnterPredicate(ColShape shape, ExtPlayer player);

        public void ChangePassword(string pass)
        {
            Password = pass;
            MySQL.Query("UPDATE `fractionstock` SET `password` = @prop0 WHERE id = @prop1", Password, Id);
        }

        public void ChangePosition(Vector3 pos, uint dim)
        {
            Position = pos;
            Dimension = dim;
            CreateInteract();
            MySQL.Query("UPDATE `fractionstock` SET `dimension` = @prop0, `position` = @prop1 WHERE id = @prop2", Dimension, JsonConvert.SerializeObject(Position), Id);
        }
        public void Destroy()
        {
            Shape.Destroy();
            InventoryService.DestroyInventory(InventoryId);
            MySQL.Query("DELETE FROM `fractionstock` WHERE id = @prop0", Id);
        }
    }
}
