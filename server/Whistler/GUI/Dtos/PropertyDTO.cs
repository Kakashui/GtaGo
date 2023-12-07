using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.GUI
{
    class PropertyDTO
    {
        public bool house { get; set; }
        public BusinessDTO business { get; set; }
        public List<VehicleDTO> transport { get; set; }
    }

    class HomeDTO
    {
        public int Number { get; set; } = -1;
        public string Style { get; set; } = "-";
        public int GarageSpace { get; set; } = -1;
        public string Tax { get; set; } = "0/0";
    }

    class BusinessDTO
    {
        public int number { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public int tax { get; set; }
        public int taxCount { get; set; }
        public int price { get; set; }
    }

    class VehicleDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string numbers { get; set; }
        public int price { get; set; }
    }
    class ProductDTO
    {
        public string title { get; set; }
        public int maxCount { get; set; }
        public int curCount { get; set; }
        public int price { get; set; }
    }
}
