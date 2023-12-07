using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.VehicleRent.Configs
{
    class CategoryModel
    {
        public List<RentModel> Cars { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public List<GTANetworkAPI.Color> Colors { get; set; }
        public CategoryModel(string icon, string name, List<GTANetworkAPI.Color> colors, List<RentModel> cars)
        {
            Cars = cars;
            Icon = icon;
            Name = name;
            Colors = colors;
        }
    }
}
