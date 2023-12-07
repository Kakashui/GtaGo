using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Inventory.Models
{
    public class CostumeElement
    {
        public int Drawable { get; }
        public int Texture { get; }
        public CostumeElement(int drawable, int texture)
        {
            Drawable = drawable;
            Texture = texture;
        }
    }
}
