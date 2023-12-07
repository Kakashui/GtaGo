using System;
using GTANetworkAPI;

namespace Whistler.SDK
{
    public class Area
    {
        private Vector3 _center;
        private int _radius;
        
        public Area(Vector3 center, int radius)
        {
            _center = center;
            _radius = radius;
        }
        
        /// <summary>
        /// Return true if entity in the circle area 
        /// </summary>
        public bool IsEntityOnArea(Entity entity) =>
            Math.Pow(_center.X - entity.Position.X, 2) + Math.Pow(_center.Y - entity.Position.Y, 2) <=
            _radius * _radius;
    }
}