using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.SDK.CustomColShape.DTO
{
    class SquareShapeDTO : BaseDTO
    {
        public Vector3 Center { get; set; }
        public float Width { get; set; }
        public float Rotation { get; set; }
    }
}
