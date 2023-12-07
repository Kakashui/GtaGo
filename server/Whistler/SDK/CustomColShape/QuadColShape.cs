using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.SDK.CustomColShape.DTO;

namespace Whistler.SDK.CustomColShape
{
    class QuadColShape : BaseColShape
    {
        public Vector3 Point1 { get; set; }
        public Vector3 Point2 { get; set; }
        public Vector3 Point3 { get; set; }
        public Vector3 Point4 { get; set; }
        public QuadColShape(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, uint dimension)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            Point4 = point4;
            _dimension = dimension;
        }

        internal override BaseDTO GetDTO(int id)
        {
            return new QuadShapeDTO
            {
                Point1 = Point1,
                Point2 = Point2,
                Point3 = Point3,
                Point4 = Point4,
                Dimension = _dimension,
                ID = id,
                Type = ColShapeTypes.QuadShape
            };
        }
    }
}
