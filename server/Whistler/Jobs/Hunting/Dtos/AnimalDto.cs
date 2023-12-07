using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Jobs.Hunting.Dtos
{
    internal class AnimalDto
    {
        public int ID { get; set; }
        public int Model { get; set; }
        public Vector3 Position { get; set; }
        public Animal.State State { get; set; }
        public bool IsController { get; set; }
    }
}
