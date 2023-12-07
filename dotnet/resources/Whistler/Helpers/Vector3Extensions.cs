using GTANetworkAPI;
using Whistler.Voice.Radio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Helpers
{
    public static class Vector3Extensions
    {
        private static Random _rnd = new Random();
        /// <summary>
        /// Возвращает нормализованный вектор направления из заданного вектора вращения.
        /// </summary>
        public static Vector3 GetDirectionFromRotation(this Vector3 rotation)
        {
            var z = DegToRad(rotation.Z);
            var x = DegToRad(rotation.X);
            var num = Math.Abs(Math.Cos(x));

            return new Vector3
            {
                X = (float)(-Math.Sin(z) * num),
                Y = (float)(Math.Cos(z) * num),
                Z = (float)Math.Sin(x)
            };
        }

        public static Vector3 DirectionToRotation(this Vector3 direction)
        {
            direction.Normalize();

            var x = Math.Atan2(direction.Z, direction.Y);
            var y = 0;
            var z = -Math.Atan2(direction.X, direction.Y);

            return new Vector3
            {
                X = (float)RadToDeg(x),
                Y = (float)RadToDeg(y),
                Z = (float)RadToDeg(z)
            };
        }

        /// <summary>
        /// Возвращает позицию удаленную на <paramref name="range"/> в направлении <paramref name="rotation"/>
        /// </summary>
        public static Vector3 ApplyDirectionWithRange(this Vector3 position, Vector3 rotation, float range)
        {
            return position + rotation.GetDirectionFromRotation().Multiply(range);
        }

        private static double DegToRad(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        private static double RadToDeg(double deg)
        {
            return deg * 180.0 / Math.PI;
        }

        public static Vector3 GetOffsetPosition(this Vector3 pos, Vector3 rotation, Vector3 offset)
        {
            var newPos = new Vector3(pos.X, pos.Y, pos.Z + offset.Z);
            var dist = Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);
            var offsetUnit = new Vector3(offset.X / dist, offset.Y / dist, 0);
            var acosA = Math.Acos(offsetUnit.X);
            var asinA = Math.Asin(offsetUnit.Y);
            var A = (asinA >= 0) ? acosA : -acosA;
            A += (rotation.Z * Math.PI / 180);
            newPos.X += (float)(Math.Cos(A) * dist);
            newPos.Y += (float)(Math.Sin(A) * dist);

            return newPos;
        }

        public static Vector3 GetRandomPointInRange(this Vector3 center, double radius)
        {
            double t = 2 * Math.PI * _rnd.NextDouble();
            double u = _rnd.NextDouble() + _rnd.NextDouble();
            double r = u > 1 ? 2 - u : u;
            double randomX = radius * r * Math.Cos(t);
            double randomY = radius * r * Math.Sin(t);
            return new Vector3(center.X + randomX, center.Y + randomY, center.Z);
        }
    }
}
