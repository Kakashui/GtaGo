using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.SDK;

namespace Whistler.MoneySystem.Models
{
    class BankPoint
    {
        public int Id { get; protected set; }
        public uint Dimension { get; protected set; }
        public Vector3 Position { get; protected set; }
        public InteractShape Shape { get; protected set; }
        public Blip Blip { get; set; }
        public BankPoint(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            Dimension = Convert.ToUInt32(row["dimension"]);
            Position = JsonConvert.DeserializeObject<Vector3>(row["position"].ToString());
            CreateInteract();
        }

        public BankPoint(Vector3 position, uint dimension)
        {
            Position = position;
            Dimension = dimension;
            var dataQuery = MySQL.QueryRead("INSERT INTO `bankpoints`(`dimension`, `position`) " +
                "VALUES (@prop0, @prop1); SELECT @@identity;",
                Dimension, JsonConvert.SerializeObject(Position));
            var id = Convert.ToInt32(dataQuery.Rows[0][0]);
            Id = id;
            CreateInteract();
        }
        private void CreateInteract()
        {
            Shape?.Destroy();
            Shape = InteractShape.Create(Position, 2, 3, Dimension).AddInteraction(ATM.OpenBankMenu, "Чтобы открыть меню банка");
            Blip = NAPI.Blip.CreateBlip(605, Position, 1, 0, Main.StringToU16("Банк"), 255, 0, true, 0, 0);
            //Blip = NAPI.Blip.CreateBlip(605, Position, 1, 0, Main.StringToU16("Bank"), 255, 0, true, 0, 0);
        }
        public void Destroy()
        {
            Shape?.Destroy();
            Blip?.Delete();
            MySQL.Query("DELETE FROM `bankpoints` WHERE id = @prop0", Id);
        }
    }
}
