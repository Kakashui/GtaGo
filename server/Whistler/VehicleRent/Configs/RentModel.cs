using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem;

namespace Whistler.VehicleRent.Configs
{
    class RentModel
    {
        public string Model { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public RentModel(string model, int price)
        {
            Model = model;
            var config = VehicleConfiguration.GetConfig(model);
            Name = config.DisplayName;
            Price = price;
            Image = model;
        }
    }
}
