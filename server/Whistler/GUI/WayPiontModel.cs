using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.GUI
{
    class WayPiontModel
    {
        public WayPiontModel(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
