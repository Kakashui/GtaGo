using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Jobs.Farm.Models
{
    class FarmZone
    {
        public FarmZone(int id, Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4)
        {
            ID = id;
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            Point4 = point4;
        }
        public int ID { get; set; }
        public Vector3 Point1 { get; set; }
        public Vector3 Point2 { get; set; }
        public Vector3 Point3 { get; set; }
        public Vector3 Point4 { get; set; }

    }
}
