using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.SDK.CustomColShape.DTO
{
    class BaseDTO
    {
        public uint Dimension { get; set; }
        public int ID { get; set; }
        public ColShapeTypes Type { get; set; }
    }
}
