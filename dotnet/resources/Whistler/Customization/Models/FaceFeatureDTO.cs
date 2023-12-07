using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Customization.Models
{
    public class FaceFeatureDTO
    {
        public FaceFeatureDTO(int id, float val)
        {
            OverlayId = id;
            Value = val;
        }
        public FaceFeatureDTO()
        {

        }
        public int OverlayId { get; set; }
        public float Value { get; set; }
    }
}
