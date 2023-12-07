using System.Collections.Generic;
using System;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.GUI;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.GUI.Documents.Enums;
using Whistler.GUI.Documents;
using Whistler.Fractions;
using Whistler.Entities;

namespace Whistler.Core
{
    class DrivingSchool : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DrivingSchool));
        // 0 - Moto
        // 1 - Car,
        // 2 - Truck,
        // 3 - Boat,
        // 4 - Helicopter,
        // 5 - Plane,

        private static Dictionary<int, ExamConfig> _vehicleModelForExam = new Dictionary<int, ExamConfig>()
        {
            {
                0,
                new ExamConfig() {
                    TypeSchool = 0,
                    Models = new List<string>(){ "enduro", "carbonrs", "akuma", "bati", "double", "hakuchou", "pcj",  },
                    PositionCar = new Vector3(-811.0152, -1311.798, 3.880266),
                    RotationCar = new Vector3(0, 0, 349.6678),
                    CheckpointCreateVehicle = new Vector3(-817.3486, -1332.905, 4.230392),
                    PriceTheory = 300,
                    PricePractic = 300,
                    SchoolPoint = new Vector3(-806.4958, -1365.461, 8.03975),
                    PointLabel = "interact_31",
                    StartPracticLocalString = "AutoSchool_40",
                }
            },
            {
                1,
                new ExamConfig() {
                    TypeSchool = 1,
                    Models = new List<string>(){ "felon", "kanjo", "cogcabrio", "exemplar", "jackal", "sentinel", "zion", "oracle2", "faction2", "asea", "fugitive", "intruder", "premier", "primo", "surge", },
                    PositionCar = new Vector3(-811.0152, -1311.798, 3.880266),
                    RotationCar =new Vector3(0, 0, 349.6678),
                    CheckpointCreateVehicle = new Vector3(-817.3486, -1332.905, 4.230392),
                    PriceTheory = 300,
                    PricePractic = 700,
                    SchoolPoint = new Vector3(-800.4074, -1348.753, 4.423733),
                    PointLabel = "interact_18",
                    StartPracticLocalString = "AutoSchool_40",
                }
            },
            {
                2,
                new ExamConfig() {
                    TypeSchool = 2,
                    Models = new List<string>(){ "pounder2", "pounder", "benson" },
                    PositionCar = new Vector3(-811.0152, -1311.798, 3.880266),
                    RotationCar =new Vector3(0, 0, 349.6678),
                    CheckpointCreateVehicle = new Vector3(-817.3486, -1332.905, 4.230392),
                    PriceTheory = 300,
                    PricePractic = 2700,
                    SchoolPoint = new Vector3(-795.1876, -1363.309, 4.429336),
                    PointLabel = "interact_32",
                    StartPracticLocalString = "AutoSchool_40",
                }
            },
            {
                3,
                new ExamConfig() {
                    TypeSchool = 3,
                    Models = new List<string>(){ "dinghy", "suntrap", "dinghy2", "jetmax" },
                    PositionCar = new Vector3(-721.6487, -1328.283, 1.884334),
                    RotationCar = new Vector3(0, 0, 226.4037),
                    CheckpointCreateVehicle = new Vector3(-733.5247, -1313.189, 4.080266),
                    PriceTheory = 1000,
                    PricePractic = 5000,
                    SchoolPoint = new Vector3(-769.0007, -1313.213, 4.030391),
                    PointLabel = "interact_33",
                    StartPracticLocalString = "AutoSchool_41",
                }
            },
            {
                4,
                new ExamConfig() {
                    TypeSchool = 4,
                    Models = new List<string>(){ "supervolito", "frogger", "maverick", "swift", "volatus", },
                    PositionCar = new Vector3(-724.9465, -1444.181, 3.88067),
                    RotationCar = new Vector3(0, 0, 139.4846),
                    CheckpointCreateVehicle = new Vector3(-704.8682, -1418.127, 4.080266),
                    PriceTheory = 2000,
                    PricePractic = 8000,
                    SchoolPoint = new Vector3(-772.3662, -1319.295, 8.494607),
                    PointLabel = "interact_34",
                    StartPracticLocalString = "AutoSchool_42",
                }
            },
            {
                5,
                new ExamConfig() {
                    TypeSchool = 5,
                    Models = new List<string>(){ "mammatus" },
                    PositionCar = new Vector3(1726.815, 3263.354, 40.04296),
                    RotationCar = new Vector3(0, 0, 100.0704),
                    CheckpointCreateVehicle = new Vector3(1758.442, 3296.788, 40.22414),
                    PriceTheory = 2000,
                    PricePractic = 8000,
                    SchoolPoint = new Vector3(-764.6888, -1323.095, 8.494608),
                    PointLabel = "interact_35",
                    StartPracticLocalString = "AutoSchool_43",
                }
            }
        };


        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                foreach (var school in _vehicleModelForExam.Values)
                {
                    InteractShape.Create(school.SchoolPoint, 4, 2)
                        .AddInteraction((client) => OpenSchoolMenu(client, school.TypeSchool), school.PointLabel);
                }

                var blip = NAPI.Blip.CreateBlip(498, new Vector3(-764.3064, -1319.571, 8.494602), 1, 24, Main.StringToU16("Автошкола"), 255, 0, true, 0, 0);
                blip = NAPI.Blip.CreateBlip(498, new Vector3(-804.1927, -1355.273, 5.694611), 1, 24, Main.StringToU16("Автошкола"), 255, 0, true, 0, 0);
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }



        [RemoteEvent("cancelmiss")]
        public static void FinishTask(PlayerGo player, int typeExam, string data, bool result)
        {
            try
            {

                player.Dimension = 0;
                player?.RemoveTempVehicle(VehicleAccess.School)?.CustomDelete();
                player.ResetData("school:typeExam");

                
                if (!Wallet.TransferMoney(player.GetCharacter(), Manager.GetFraction(6), _vehicleModelForExam[typeExam].PricePractic, 0, "Money_School".Translate(typeExam)))
                { 
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "AutoSchool_25", 3000); 
                    return;
                }
                
                player.SetData($"school:resultPracticExamData{typeExam}", data);
                player.SetData($"school:resultPracticExam{typeExam}", result);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "AutoSchool_39", 6000);
                CheckAndGiveLic(player, typeExam);
            }
            catch (Exception e) { _logger.WriteError("FinishTask: " + e.ToString()); }
        }
        [RemoteEvent("school:saveTheoryResult")]
        public static void RemoteEvent_SaveTheorySerult(PlayerGo player, int typeExam, string data, bool result)
        {
            try
            {
                player.SetData($"school:resultTheoryExamData{typeExam}", data);
                player.SetData($"school:resultTheoryExam{typeExam}", result);
                CheckAndGiveLic(player, typeExam);
            }
            catch (Exception e) { _logger.WriteError("RemoteEvent_SaveTheorySerult: " + e.ToString()); }
        }

        [RemoteEvent("school:startTheoryExam")]
        public static void RemoteEvent_StartTheoryExam(Player player, int typeExam)
        {
            try
            {
                if (player.GetCharacter().Mulct > 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "AutoSchool_44", 6000);
                    return;
                }
                player.SetData("school:typeExam", typeExam);
                Trigger.ClientEvent(player, "openDialog", "school:theoryExam", "AutoSchool_23".Translate( _vehicleModelForExam[typeExam].PriceTheory));
            }
            catch (Exception e) { _logger.WriteError("RemoteEvent_StartTheoryExam: " + e.ToString()); }
        }

        [RemoteEvent("school:startPracticExam")]
        public static void RemoteEvent_StartPracticExam(Player player, int typeExam)
        {
            try
            {
                if (player.GetCharacter().Mulct > 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "AutoSchool_44", 6000);
                    return;
                }
                player.SetData("school:typeExam", typeExam);
                Trigger.ClientEvent(player, "openDialog", "school:practicExam", "AutoSchool_24".Translate( _vehicleModelForExam[typeExam].PricePractic));
            }
            catch (Exception e) { _logger.WriteError("RemoteEvent_StartPracticExam: " + e.ToString()); }
        }

        public static void StartPracticExam(Player player, int typeExam)
        {
            try
            {
                if (player.GetCharacter().Money < _vehicleModelForExam[typeExam].PricePractic)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "AutoSchool_25", 3000);
                    return;
                }
                NAPI.ClientEvent.TriggerClientEvent(player, "school:setStartPosition", JsonConvert.SerializeObject(_vehicleModelForExam[typeExam].CheckpointCreateVehicle), typeExam);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, _vehicleModelForExam[typeExam].StartPracticLocalString, 6000);
            }
            catch (Exception e) { _logger.WriteError("StartPracticExam: " + e.ToString()); }

        }

        [RemoteEvent("school:createSchoolVehicle")]
        public static void CreateSchoolVehicle(Player player, int typeExam)
        {
            try
            {
                Vehicle vehicle = VehicleManager.CreateTemporaryVehicle(_vehicleModelForExam[typeExam].GetModel(), _vehicleModelForExam[typeExam].PositionCar, _vehicleModelForExam[typeExam].RotationCar, "SCHOOL", VehicleAccess.School, player);
                uint dim = Dimensions.RequestPrivateDimension();
                VehicleStreaming.SetEngineState(vehicle, false);
                vehicle.CustomPrimaryColor = new Color(new Random().Next(0, 160));
                vehicle.CustomSecondaryColor = new Color(new Random().Next(0, 160));
                vehicle.Dimension = dim;
                player.Dimension = dim;
                player.AddTempVehicle(vehicle, VehicleAccess.School);
                VehicleStreaming.SetVehicleFuel(vehicle, vehicle.GetVehicleGo().Config.MaxFuel);
                player.SetData("school:typeExam", typeExam);
                NAPI.ClientEvent.TriggerClientEvent(player, "school:startLearnTask", typeExam, dim, vehicle, JsonConvert.SerializeObject(_vehicleModelForExam[typeExam].PositionCar));
            }
            catch (Exception e) { _logger.WriteError("CreateSchoolVehicle: " + e.ToString()); }
        }

        public static void StartTheoryExam(Player player, int typeExam)
        {
            try
            {
                if (!Wallet.TransferMoney(player.GetCharacter(), Manager.GetFraction(6), _vehicleModelForExam[typeExam].PriceTheory, 0, "Money_School".Translate(typeExam)))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "AutoSchool_25", 3000);
                    return;
                }

                NAPI.ClientEvent.TriggerClientEvent(player, "school:openTheoryMenu", typeExam);
            }
            catch (Exception e) { _logger.WriteError("StartTheoryExam: " + e.ToString()); }
        }

        public static void OpenSchoolMenu(Player player, int typeExam)
        {
            try
            {
                string theoryExam = player.HasData($"school:resultTheoryExamData{typeExam}") ? player.GetData<string>($"school:resultTheoryExamData{typeExam}") : "{}";
                string practicExam = player.HasData($"school:resultPracticExamData{typeExam}") ? player.GetData<string>($"school:resultPracticExamData{typeExam}") : "{}";
                Trigger.ClientEvent(player, "school:openMenu", theoryExam, practicExam, typeExam);
            }
            catch (Exception e) { _logger.WriteError("OpenSchoolMenu: " + e.ToString()); }
        }

        private static void CheckAndGiveLic(PlayerGo player, int typeLic)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (player.HasData($"school:resultTheoryExam{typeLic}") && player.GetData<bool>($"school:resultTheoryExam{typeLic}") &&
                    player.HasData($"school:resultPracticExam{typeLic}") && player.GetData<bool>($"school:resultPracticExam{typeLic}"))
                {

                    player.GiveLic((LicenseName)typeLic);
                    Notify.Alert(player, "AutoSchool_38".Translate(DocumentConfigs.GetLicenseWord((LicenseName)typeLic)));
                    MainMenu.SendStats(player);
                    if (typeLic == 1 || typeLic == 2)
                        StartQuest.StartQuestManager.EndQuest(player, StartQuest.StartQuestNames.Stage7AutoSchool);
                }
            }
            catch (Exception e) { _logger.WriteError("CheckAndGiveLic: " + e.ToString()); }
        }
    }

    class ExamConfig
    {
        public int TypeSchool { get; set; }
        public List<string> Models { get; set; }
        public Vector3 PositionCar { get; set; }
        public Vector3 RotationCar { get; set; }
        public Vector3 CheckpointCreateVehicle { get; set; }
        public int PriceTheory { get; set; }
        public int PricePractic { get; set; }
        public Vector3 SchoolPoint { get; set; }
        public string PointLabel { get; set; }
        public string StartPracticLocalString { get; set; }
        public ExamConfig()
        {
        }

        public string GetModel()
        {
            int rand = new Random().Next(0, Models.Count);
            return Models[rand];
        }
    }
}