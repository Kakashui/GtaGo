using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Enviroment.Models
{
    class SitData
    {
        public SitData(int model, uint hash, int place)
        {
            Model = model;
            Hash = hash;
            Place = place;
        }

        public int Model { get; set; }
        public uint Hash { get; set; }
        public int Place { get; set; }
    }
}
