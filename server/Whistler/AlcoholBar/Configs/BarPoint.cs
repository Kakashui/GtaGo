using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.SDK;
using Newtonsoft.Json;

namespace Whistler.AlcoholBar.Configs
{
    class BarPoint
    {
        public int Id { get; set; }
        public Vector3 Position { get; set; }
        public int Radius { get; set; }
        public InteractShape Sahpe { get; set; }
        public Blip Blip { get; set; }

        public void Load(Action<Player> callback)
        {
            Sahpe = InteractShape.Create(Position, Radius, 2)
             //.AddDefaultMarker()
             .AddInteraction(callback, "interact_43");
            Blip = NAPI.Blip.CreateBlip(93, Position, 1, 2, "Бар", shortRange: true);
        }
        public void Destroy()
        {
            Sahpe?.Destroy();
            Blip?.Delete();
            MySQL.Query("DELETE FROM `alcobars` WHERE `id`=@prop0", Id);
        }

        internal void Create(Vector3 position, int radius, Action<Player> callback)
        {
            Position = position;
            Radius = radius;
            var responce = MySQL.QueryRead("INSERT INTO `alcobars`(`position`, `radius`) VALUES(@prop0, @prop1); SELECT @@identity;", JsonConvert.SerializeObject(Position), Radius);
            Id = Convert.ToInt32(responce.Rows[0][0]);
            Load(callback);
        }
    }
}
