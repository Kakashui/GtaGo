using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Common.CommonClasses;

namespace Whistler.Fractions.Models
{
    class FractionGarage
    {
        private static Dictionary<GarageTypes, PositionWithHeading> Garages = new Dictionary<GarageTypes, PositionWithHeading>
        {
            [GarageTypes.Garage30Place] = new PositionWithHeading(new Vector3(4621.93507, -0.1716583, 200.1200002), new Vector3(0, 0, 90)),
            [GarageTypes.Garage50Place] = new PositionWithHeading(new Vector3(3782.54144, 476.12762, 202.644107), new Vector3(0, 0, 180)),
        };
        public Vector3 EnterPoint { get; set; }
        public Vector3 ExitPoint { get; set; }
        public Vector3 ExitRotation { get; set; }
        public GarageTypes GarageType { get; set; }
        public Vector3 GaragePosition => Garages[GarageType].Position;
        public Vector3 GarageRotation => Garages[GarageType].Rotation;
        public int Fraction { get; set; }
        public uint Dimension { get; set; }
        public FractionGarage(Vector3 enterPoint, Vector3 exitPoint, Vector3 exitRotation, GarageTypes garageType, int fraction, uint dimension)
        {
            EnterPoint = enterPoint;
            ExitPoint = exitPoint;
            ExitRotation = exitRotation;
            GarageType = garageType;
            Fraction = fraction;
            Dimension = dimension;
        }
        public enum GarageTypes
        {
            Garage50Place,
            Garage30Place,
        }
    }
}
