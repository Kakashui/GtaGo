using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Entities;
using Whistler.Fishing;
using Whistler.Fishing.Models;
using Whistler.Inventory.Models;
using Whistler.VehicleSystem.Models;

namespace Whistler.Core.Models
{
    public class PSession
    {
        public int Id = -1;
        public uint Dimension = 0;
        public Vector3 Position = new Vector3(0f, 0f, 0f);
        public string Name = "Unknown";
        public string HWID = null;
        public string IP = null;

        public PTimers Timers = new PTimers();

        public int FishingSpotId = -1;
        public FishingActions FishingAction = FishingActions.NoAction;
        public FisherData FisherData = null;

        public bool InWorkCar = false;
        public bool OnWork = false;

        public ExtVehicle RentBus = null;

        public int SchoolExamType = -1;
        public string[] SchoolTheoryExamData = new string[5] { "{}", "{}", "{}", "{}", "{}" };
        public bool[] SchoolTheoryExam = new bool[5] { false, false, false, false, false };
        public string[] SchoolPracticExamData = new string[5] { "{}", "{}", "{}", "{}", "{}" };
        public bool[] SchoolPracticExam = new bool[5] { false, false, false, false, false };

        public ExtPlayer VehicleSeller = null;
        public int VehicleNumber = -1;
        public int VehiclePrice = 0;

        public List<uint> Attachments = new List<uint>();
        public AttachId Container = AttachId.invalid;

        public BaseItem SceneItem = null;
        public int SceneCount = 0;

        public DateTime NextRobbery = DateTime.Now;

        public Vector3 PositionBeforeCreator = null;
        public bool InCreator = false;

        public DateTime NextArmor = DateTime.Now;
        public DateTime NextBandage = DateTime.Now;

        public bool SPActivated = false;
        public Vector3 SPPosition;
        public bool SPInvisible;
        public uint SPDimension;
        public int SPClient = -1;

        public bool Invisible = false;

        public PSession(int id, uint dimension, string name)
        {
            Id = id;
            Dimension = dimension;
            Name = name;
        }

    }
}
