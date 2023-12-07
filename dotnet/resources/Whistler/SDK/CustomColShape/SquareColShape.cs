using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.SDK.CustomColShape.DTO;

namespace Whistler.SDK.CustomColShape
{
    class SquareColShape : BaseColShape
    {
        private readonly Vector3 _center;
        private readonly float _width;
        private readonly float _rotation;
        public SquareColShape(Vector3 center, float width, float rotation, uint dimension)
        {
            _center = center;
            _width = width;
            _rotation = rotation;
            _dimension = dimension;
        }
        internal override BaseDTO GetDTO(int id)
        {
            return new SquareShapeDTO
            {
                Center = _center,
                Width = _width,
                Rotation = _rotation,
                Dimension = _dimension,
                ID = id,
                Type = ColShapeTypes.SquareShape
            };
        }
    }
}
