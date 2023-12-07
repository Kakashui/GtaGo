using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Businesses.Manager.DTOs
{
    public class BizInfoDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public int Price { get; set; }
        public string Owner { get; set; }
        public string Overseer { get; set; }
        public bool Purchaseable { get; set; }
        public int Type { get; set; }
        public float CamPositionX { get; set; }
        public float CamPositionY { get; set; }
        public float CamPositionZ { get; set; }
    }
}
