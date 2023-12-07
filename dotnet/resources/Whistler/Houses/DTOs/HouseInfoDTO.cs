using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Houses.DTOs
{
    public class HouseInfoDTO
    {
        public int ID { get; set; }
        public string Owner { get; set; }
        public string Class { get; set; }
        public int Roommates { get; set; }
        public int GarageSpace { get; set; }
        public int Price { get; set; }
        public bool IsSelled { get; set; }
        public bool IsLocked { get; set; }
        public bool CanEnter { get; set; }
        public bool IsTarget { get; set; }
        public float CamPositionX { get; set; }
        public float CamPositionY { get; set; }
        public float CamPositionZ { get; set; }
    }
}
