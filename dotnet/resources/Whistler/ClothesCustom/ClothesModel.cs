using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.ClothesCustom
{
    struct ClothesModel
    {
        public int Drawable { get; set; }
        public int Texture { get; set; }
        public ClothesModel(int drawable, int texture)
        {
            Drawable = drawable;
            Texture = texture;
        }
    }
}
