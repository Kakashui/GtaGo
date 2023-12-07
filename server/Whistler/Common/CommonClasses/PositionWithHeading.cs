using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Common.CommonClasses
{
    public class PositionWithHeading
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public PositionWithHeading(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }
        public PositionWithHeading(Vector3 position)
        {
            Position = position;
            Rotation = new Vector3();
        }
    }
}
