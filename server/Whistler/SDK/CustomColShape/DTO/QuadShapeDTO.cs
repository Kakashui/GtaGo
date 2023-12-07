using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.SDK.CustomColShape.DTO
{
    class QuadShapeDTO : BaseDTO
    {
        public Vector3 Point1 { get; set; }
        public Vector3 Point2 { get; set; }
        public Vector3 Point3 { get; set; }
        public Vector3 Point4 { get; set; }
    }
}
