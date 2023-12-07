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
using System.Linq;
using Whistler.VehicleSystem.Models;

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
                    PositionCar = new Vector3(-980.9483, -2060.0168, 9.580816),
                    RotationCar = new Vector3(0, 0, -348),
                    CheckpointCreateVehicle = new Vector3(-980.9483, -2060.0168, 8.580816),
                    PriceTheory = 300,
                    PricePractic = 300,
                    SchoolPoint = new Vector3(-806.4958, -1365.461, 8.03975),
                    PointLabel = "Чтобы открыть меню мотошколы",
                    StartPracticLocalString = "Отправляйтесь на парковку для начала экзамена",
                }
            },
            {
                1,
                new ExamConfig() {
                    TypeSchool = 1,
                    Models = new List<string>(){ "felon", "kanjo", "cogcabrio", "exemplar", "jackal", "sentinel", "zion", "oracle2", "faction2", "asea", "fugitive", "intruder", "premier", "primo", "surge", },
                    PositionCar = new Vector3(-980.9483, -2060.0168, 9.580816),
                    RotationCar =new Vector3(0, 0, -138),
                    CheckpointCreateVehicle = new Vector3(-980.9483, -2060.0168, 8.580816),
                    PriceTheory = 300,
                    PricePractic = 700,
                    SchoolPoint = new Vector3(-800.4074, -1348.753, 4.423733),
                    PointLabel = "Чтобы открыть меню автошколы",
                    StartPracticLocalString = "Отправляйтесь на парковку для начала экзамена",
                }
            },
            {
                2,
                new ExamConfig() {
                    TypeSchool = 2,
                    Models = new List<string>(){ "pounder2", "pounder", "benson" },
                    PositionCar = new Vector3(-980.9483, -2060.0168, 9.580816),
                    RotationCar =new Vector3(0, 0, -138),
                    CheckpointCreateVehicle = new Vector3(-980.9483, -2060.0168, 8.580816),
                    PriceTheory = 300,
                    PricePractic = 2700,
                    SchoolPoint = new Vector3(-795.1876, -1363.309, 4.429336),
                    PointLabel = "Чтобы открыть меню школы вождения грузовиков",
                    StartPracticLocalString = "Отправляйтесь на парковку для начала экзамена",
                }
            },
            {
                3,
                new ExamConfig() {
                    TypeSchool = 3,
                    Models = new List<string>(){ "dinghy", "suntrap", "dinghy2", "jetmax" },
                    PositionCar = new Vector3(-721.6487, -1328.283, 1.884334),
                    RotationCar = new Vector3(0, 0, 226.4037),
                    CheckpointCreateVehicle = new Vector3(-733.5247, -1313.189, 3.080266),
                    PriceTheory = 1000,
                    PricePractic = 5000,
                    SchoolPoint = new Vector3(-769.0007, -1313.213, 4.030391),
                    PointLabel = "Чтобы открыть меню школы судовождения",
                    StartPracticLocalString = "Отправляйтесь на причал для начала экзамена",
                }
            },
            {
                4,
                new ExamConfig() {
                    TypeSchool = 4,
                    Models = new List<string>(){ "supervolito", "frogger", "maverick", "swift", "volatus", },
                    PositionCar = new Vector3(-724.9465, -1444.181, 3.88067),
                    RotationCar = new Vector3(0, 0, 139.4846),
                    CheckpointCreateVehicle = new Vector3(-704.8682, -1418.127, 3.080266),
                    PriceTheory = 2000,
                    PricePractic = 8000,
                    SchoolPoint = new Vector3(-772.3662, -1319.295, 8.494607),
                    PointLabel = "Чтобы открыть меню школы пилотов вертолетов",
                    StartPracticLocalString = "Отправляйтесь на вертолетную площадку для начала экзамена",
                }
            },
            {
                5,
                new ExamConfig() {
                    TypeSchool = 5,
                    Models = new List<string>(){ "mammatus" },
                    PositionCar = new Vector3(1726.815, 3263.354, 40.04296),
                    RotationCar = new Vector3(0, 0, 100.0704),
                    CheckpointCreateVehicle = new Vector3(1758.442, 3296.788, 39.22414),
                    PriceTheory = 2000,
                    PricePractic = 8000,
                    SchoolPoint = new Vector3(-764.6888, -1323.095, 8.494608),
                    PointLabel = "Чтобы открыть меню школы пилотов самолетов",
                    StartPracticLocalString = "Отправляйтесь на аэродром для начала экзамена",
                }
            }
        };


        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                // foreach (var school in _vehicleModelForExam.Values)
                // {
                //     InteractShape.Create(school.SchoolPoint, 4, 2)
                //         .AddInteraction((client) => OpenSchoolMenu(client, school.TypeSchool), school.PointLabel);
                // }
                InteractShape.Create(new Vector3(-927.4753, -2038.4761, 9.386556), 4, 2)
                        .AddInteraction((client) => OpenSchoolMenu(client, 0), "Чтобы открыть меню школы центра лицензирования");
                //-706.4041, -1400.0447, 5.1502624
                var blip = NAPI.Blip.CreateBlip(498, new Vector3(-927.4753, -2038.4761, 9.386556), 1, 24, Main.StringToU16("Центр лицензирования"), 255, 0, true, 0, 0);
                // blip = NAPI.Blip.CreateBlip(498, new Vector3(-804.1927, -1355.273, 5.694611), 1, 24, Main.StringToU16("Автошкола"), 255, 0, true, 0, 0);
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }



        [RemoteEvent("cancelmiss")]
        public static void FinishTask(ExtPlayer player, int typeExam, string data, bool result)
        {
            try
            {

                SafeTrigger.UpdateDimension(player,  0);
                player?.RemoveTempVehicle(VehicleAccess.School)?.CustomDelete();
                player.Session.SchoolExamType = -1;
                if (typeExam < player.Session.SchoolPracticExamData.Length)
                {
                    player.Session.SchoolPracticExamData[typeExam] = data;
                    player.Session.SchoolPracticExam[typeExam] = result;
                }
                if (!result)
                {
                    Notify.SendAlert(player, "К сожалению, Вы не сдали экзамен.");
                    return;
                }

                player.GiveLic((LicenseName)typeExam);
                Notify.SendAlert(player, $"Поздравляем, Вы сдали теоретический и практический экзамен и получаете лицензию {DocumentConfigs.GetLicenseWord((LicenseName)typeExam)}");
                MainMenu.SendStats(player);
                if (typeExam != 1 && typeExam != 2) return;

                StartQuest.StartQuestManager.EndQuest(player, StartQuest.StartQuestNames.Stage7AutoSchool);
            }
            catch (Exception e) { _logger.WriteError("FinishTask: " + e.ToString()); }
        }


        [RemoteEvent("server:school:buyLic")]
        public static void schoolBuyLic(ExtPlayer player, int type)
        {
            if (!Enum.IsDefined(typeof(LicenseName), type)) return;

            LicenseName license = (LicenseName)type;
            int price = DocumentConfigs.GetLicensePrice(license);
            if (price == -1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Покупка данной лицензии не доступна в данный момент.", 3000);
                return;
            }

            if (!Wallet.TransferMoney(player.Character, MoneyManager.ServerMoney, price, 0, $"Покупка лицензии"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно денег для оплаты лицензии", 3000);
                return;
            }
            player.GiveLic(license);
            MainMenu.SendStats(player);
            SafeTrigger.ClientEvent(player, "school:updateMenu", JsonConvert.SerializeObject(player.Character.Licenses.FindAll(item => item.DateEnd > System.DateTime.Now)));
        }

        [RemoteEvent("school:saveTheoryResult")]
        public static void RemoteEvent_SaveTheorySerult(ExtPlayer player, int typeExam, string data, bool result)
        {
            try
            {
                if (typeExam < 0 || typeExam >= player.Session.SchoolTheoryExamData.Length) return;

                player.Session.SchoolTheoryExamData[typeExam] = data;
                player.Session.SchoolTheoryExam[typeExam] = result;
                CheckAndGiveLic(player, typeExam);
            }
            catch (Exception e) { _logger.WriteError("RemoteEvent_SaveTheorySerult: " + e.ToString()); }
        }

        [RemoteEvent("school:startTheoryExam")]
        public static void RemoteEvent_StartTheoryExam(ExtPlayer player, int typeExam)
        {
            try
            {
                if (player.Character.Mulct > 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Оплатите штрафы для сдачи экзаменов", 6000);
                    return;
                }
                player.Session.SchoolExamType = typeExam;
                SafeTrigger.ClientEvent(player, "openDialog", "school:theoryExam", $"Начать теоретический экзамен?");
            }
            catch (Exception e) { _logger.WriteError("RemoteEvent_StartTheoryExam: " + e.ToString()); }
        }

        [RemoteEvent("school:startPracticExam")]
        public static void RemoteEvent_StartPracticExam(ExtPlayer player, int typeExam)
        {
            try
            {
                if (player.Character.Mulct > 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Оплатите штрафы для сдачи экзаменов", 6000);
                    return;
                }
                player.Session.SchoolExamType = typeExam;
                SafeTrigger.ClientEvent(player, "openDialog", "school:practicExam", $"Начать практический экзамен? Стоимость одной попытки - ${_vehicleModelForExam[typeExam].PricePractic}");
            }
            catch (Exception e) { _logger.WriteError("RemoteEvent_StartPracticExam: " + e.ToString()); }
        }

        public static void StartPracticExam(ExtPlayer player, int typeExam)
        {
            try
            {
                if (!Wallet.TransferMoney(player.Character, MoneyManager.ServerMoney, _vehicleModelForExam[typeExam].PricePractic, 0, $"Практический экзамен"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно денег для оплаты экзамена", 3000);
                    return;
                }
                NAPI.ClientEvent.TriggerClientEvent(player, "school:setStartPosition", JsonConvert.SerializeObject(_vehicleModelForExam[typeExam].CheckpointCreateVehicle), typeExam);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, _vehicleModelForExam[typeExam].StartPracticLocalString, 6000);
            }
            catch (Exception e) { _logger.WriteError("StartPracticExam: " + e.ToString()); }

        }

        [RemoteEvent("school:createSchoolVehicle")]
        public static void CreateSchoolVehicle(ExtPlayer player, int typeExam)
        {
            try
            {
                ExtVehicle vehicle = VehicleManager.CreateTemporaryVehicle(_vehicleModelForExam[typeExam].GetModel(), _vehicleModelForExam[typeExam].PositionCar, _vehicleModelForExam[typeExam].RotationCar, "SCHOOL", VehicleAccess.School, player);
                if (vehicle == null || !vehicle.Exists) return;

                uint dim = Dimensions.RequestPrivateDimension();
                VehicleStreaming.SetEngineState(vehicle, false);
                vehicle.CustomPrimaryColor = new Color(new Random().Next(0, 160));
                vehicle.CustomSecondaryColor = new Color(new Random().Next(0, 160));
                vehicle.Dimension = dim;
                SafeTrigger.UpdateDimension(player,  dim);
                player.AddTempVehicle(vehicle, VehicleAccess.School);
                VehicleStreaming.SetVehicleFuel(vehicle, vehicle.Config.MaxFuel);
                player.Session.SchoolExamType = typeExam;
                NAPI.ClientEvent.TriggerClientEvent(player, "school:startLearnTask", typeExam, dim, vehicle, JsonConvert.SerializeObject(_vehicleModelForExam[typeExam].PositionCar));
            }
            catch (Exception e) { _logger.WriteError("CreateSchoolVehicle: " + e.ToString()); }
        }

        public static void StartTheoryExam(ExtPlayer player, int typeExam)
        {
            try
            {
                // if (!Wallet.TransferMoney(player.Character, Manager.GetFraction(6), _vehicleModelForExam[typeExam].PriceTheory, 0, $"Сдача экзамена {typeExam}"))
                // {
                //     Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно денег для оплаты экзамена", 3000);
                //     return;
                // }

                NAPI.ClientEvent.TriggerClientEvent(player, "school:openTheoryMenu", typeExam);
            }
            catch (Exception e) { _logger.WriteError("StartTheoryExam: " + e.ToString()); }
        }

        public static void OpenSchoolMenu(ExtPlayer player, int typeExam)
        {
            try
            {
                string theoryExam = player.Session.SchoolTheoryExamData[typeExam];
                string practicExam = player.Session.SchoolPracticExamData[typeExam];
                SafeTrigger.ClientEvent(player, "school:openMenu", theoryExam, practicExam, typeExam, JsonConvert.SerializeObject(player.Character.Licenses.FindAll(item => item.DateEnd > System.DateTime.Now)));
            }
            catch (Exception e) { _logger.WriteError("OpenSchoolMenu: " + e.ToString()); }
        }

        private static void CheckAndGiveLic(ExtPlayer player, int typeLic)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.Session.SchoolTheoryExam[typeLic] && player.Session.SchoolPracticExam[typeLic])
                {
                    player.GiveLic((LicenseName)typeLic);
                    Notify.SendAlert(player, $"Поздравляем, Вы сдали теоретический и практический экзамен и получаете лицензию {DocumentConfigs.GetLicenseWord((LicenseName)typeLic)}");
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