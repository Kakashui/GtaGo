using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Families.FamilyMenu;
using Whistler.Families.FamilyWars;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.MP.RoyalBattle.Models;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.Inventory.Enums;
using Whistler.Core.QuestPeds;
using Whistler.Entities;

namespace Whistler.Families.FamilyMP.Models.MPModels
{
    class IslandCaptureMP : FamilyMPModel
    {

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(IslandCaptureMP));
        public IslandCaptureMP(DataRow row) : base(row)
        {

        }
        public IslandCaptureMP(DateTime date, BattleLocation location) : base(date, location, FamilyMPType.IslandCapture)
        {
            var dataQuery = MySQL.QueryRead("INSERT INTO `familymp`(`date`, `location`, `winner`, `finished`, `type`, `kills`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5); SELECT @@identity;",
                MySQL.ConvertTime(Date), (int)Location, WinnerFamily, IsFinished, (int)Type, JsonConvert.SerializeObject(KillLog));
            ID = Convert.ToInt32(dataQuery.Rows[0][0]);
        }


        private static readonly List<Vector3> _outPoints = new List<Vector3>
        {
            new Vector3(4904.932, -5192.819, 2.437315),
            new Vector3(4977.199, -5173.623, 2.462548),
            new Vector3(5115.781, -5146.249, 2.179648),
            new Vector3(5167.873, -4703.077, 2.176523),
            new Vector3(5075.777, -4632.782, 2.318387),
            new Vector3(5088.298, -4529.072, 2.288183),
            new Vector3(4827.925, -4289.955, 2.709095),
            new Vector3(4649.615, -4415.957, 3.103191),
            new Vector3(3909.867, -4630.218, 2.157498),
            new Vector3(4344.149, -4590.955, 4.571273),
            new Vector3(4776.337, -4729.882, 2.607516),
        };

        private static Vector3 _enterPoint = new Vector3(1289.666, -3337.071, 5.902167);
        private static Vector3 _exitPoint = new Vector3(5075.777, -4632.782, 2.318387);
        private static Vector3 _dropPoint = new Vector3(5018.811, -5735.828, 17.67747);

        private static DateTime StartDate = DateTime.Now;
        private static int _minuteBeforeEndMP = 15; 
        private static int _minuteFullTimeMP = 45;
        private static ZoneModel _startZone = new ZoneModel(new Vector3(4991, -4886, -100), 1200);
        private static ZoneModel _endZone = new ZoneModel(new Vector3(5013.369, -5742.534, 19.88035), 0);
        private static Dictionary<ExtPlayer, int> Players = new Dictionary<ExtPlayer, int>();

        private static bool IsPlayingIslandCapt = false;

        public static QuestPedParamModel enterIslandPed = new QuestPedParamModel(PedHash.Genfat01AMM, new Vector3(1294.3691, -3341.113, 5.901952), "Cristobal Paredes", "Незнакомец", 53, 0, 2);
        public static QuestPedParamModel exitIslandPed = new QuestPedParamModel(PedHash.JimmyBoston, new Vector3(5076.5366, -4627.8584, 2.4132693), "Matias Zepeda", "Незнакомец", 100, 0, 2);

        public static Action<ExtPlayer, QuestPed> WorkPed_PlayerInteracted { get; private set; }

        public static void Init()
        {
            InteractShape.Create(_enterPoint, 3, 2, 0)
                .AddMarker(27, _enterPoint + new Vector3(0, 0, -0.9), 6, InteractShape.DefaultMarkerColor)
                .AddInteraction(EnterIsland, "Чтобы отправиться на остров");
            InteractShape.Create(_exitPoint, 3, 2, 0)
                .AddMarker(27, _exitPoint + new Vector3(0, 0, -0.9), 6, InteractShape.DefaultMarkerColor)
                .AddInteraction(ExitIsland, "Чтобы покинуть остров");
            NAPI.Blip.CreateBlip(766, _enterPoint, 1, 25, Main.StringToU16("Cayo Perico"), 255, 0, true, 0, 0);
            NAPI.Blip.CreateBlip(766, _exitPoint, 1, 25, Main.StringToU16("Los Santos"), 255, 0, true, 0, 0);


            var ped = new QuestPed(enterIslandPed);
            ped.PlayerInteracted += (player, ped) =>
            {
                try
                {
                    var introPage = new DialogPage("", ped.Name, ped.Role);
                    introPage.AddAnswer("Хочу попасть на остров", EnterIsIslandPed);
                    introPage.AddCloseAnswer("Досвидания");
                    introPage.OpenForPlayer(player);
                }
                catch (Exception e)
                {
                    _logger.WriteError("IslandQuestPed: " + e.ToString());
                }
            };

            var exitped = new QuestPed(exitIslandPed);
            exitped.PlayerInteracted += (player, ped) =>
            {
                try
                {
                    var introPage = new DialogPage("", ped.Name, ped.Role);
                    introPage.AddAnswer("Хочу покинуть остров", ExitIsIslandPed);
                    introPage.AddCloseAnswer("Досвидания");
                    introPage.OpenForPlayer(player);
                }
                catch (Exception e)
                {
                    _logger.WriteError("IslandQuestPed: " + e.ToString());
                }
            };
        }

        private static void EnterIsIslandPed(ExtPlayer player)
        {
            bool isIslandCapt = (ManagerMP.CurrentMP?.Type ?? FamilyMPType.Invalid) == FamilyMPType.IslandCapture;
            Vector3 targetPosition = isIslandCapt ? _outPoints.GetRandomElement() : _exitPoint;
            player.ChangePosition(targetPosition + new Vector3(0, 0, 2.5), 2000);
        }

        private static void ExitIsIslandPed(ExtPlayer player)
        {
            player.ChangePosition(_enterPoint + new Vector3(0, 0, 1.5), 2000);
        }

        private static void EnterIsland(ExtPlayer player)
        {
            if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat)
            {
                Notify.SendError(player, "Вы должны быть в машине");
                return;
            }
            bool isIslandCapt = (ManagerMP.CurrentMP?.Type ?? FamilyMPType.Invalid) == FamilyMPType.IslandCapture;
            Vector3 targetPosition = isIslandCapt ? _outPoints.GetRandomElement() : _exitPoint;
            player.ChangePositionWithCar(targetPosition + new Vector3(0, 0, 0.9), null, 1000);
        }

        private static void ExitIsland(ExtPlayer player)
        {
            if (!player.IsInVehicle)
            {
                Notify.SendError(player, "Вы должны быть в машине");
                return;
            }
            player.ChangePositionWithCar(_enterPoint + new Vector3(0, 0, 0.9), null, 1000);
        }
        public void PlayerEnterZone(ExtPlayer player, BattleLocation location)
        {
            if (location != Location)
                return;
            if (IsPlayingIslandCapt)
                LoadZone(player);
            var familyId = player.GetFamily()?.Id ?? 0;

            if (!Players.ContainsKey(player))
                Players.Add(player, familyId);
            if (IsPlayingIslandCapt)
                TryEndMP();
        }

        public void PlayerExitZone(ExtPlayer player, BattleLocation location)
        {
            if (location != Location)
                return;
            RemoveMember(player);
        }
        private static bool RemoveMember(ExtPlayer player)
        {
            if (Players.ContainsKey(player))
            {
                Players.Remove(player);
                if (IsPlayingIslandCapt)
                {
                    UnloadZone(player);
                    ManagerMP.CurrentMP?.TryEndMP();
                }
                return true;
            }
            return false;
        }
        public override bool IsMember(ExtPlayer player)
        {
            if (!player.IsLogged())
                return false;
            return Players.ContainsKey(player);
        }

        public override bool TryStartMP()
        {
            if (IsFinished || IsPlaying)
                return false;
            IsPlaying = true;
            IsPlayingIslandCapt = true;
            StartDate = DateTime.Now;
            Players = new Dictionary<ExtPlayer, int>();
            var players = Main.GetExtPlayersListByPredicate((player) => player.Character.IsAlive && player.Character.WarZone == Location);
            foreach (var player in players)
            {
                int familyId = player.Character.FamilyID;
                Players.Add(player, familyId);
                LoadZone(player);
            }
            Chat.AdminToAll($"Мероприятие \"{NameMP}\" началось!");
            WarManager.SubscribeToBattleEvent(Location, PlayerEnterZone, PlayerExitZone);
            Timers.StartOnce((_minuteBeforeEndMP * 60 + 1) * 1000, TryEndMP);
            return true;
        }

        public override void TryEndMP()
        {
            if (StartDate.AddMinutes(_minuteBeforeEndMP) > DateTime.Now)
                return;
            var familyPlayers = Players.Where(item => item.Value > 0 && !item.Key.Session.Invisible);
            if (familyPlayers.Count() > 0 && familyPlayers.Max(item => item.Value) != familyPlayers.Min(item => item.Value))
                return;
            ManagerMP.CurrentMP = null;
            IsPlayingIslandCapt = false;
            var familyWin = familyPlayers.FirstOrDefault().Value;
            foreach (var player in familyPlayers)
            {
                MoneySystem.Wallet.MoneyAdd(player.Key.Character, 2000, "Захват острова");
            }
            foreach (var player in Players.Keys)
            {
                UnloadZone(player);
            }
            IsPlaying = false;
            WinnerFamily = familyWin;
            WinnerFamilyName = FamilyManager.GetFamilyName(WinnerFamily);
            IsFinished = true;
            Save();
            FamilyMenuManager.UpdateFamilyMP(this);
            WarManager.UnsubscribeToBattleEvent(Location, PlayerEnterZone, PlayerExitZone);
            var items = new List<Inventory.Models.BaseItem>
            {
                Inventory.ItemsFabric.CreateItemBox(ItemNames.AmmoBox, ItemNames.RiflesAmmo, 150, false),
                Inventory.ItemsFabric.CreateItemBox(ItemNames.WeaponBox, ItemNames.AssaultRifleMk2, 10, false),
                Inventory.ItemsFabric.CreateItemBox(ItemNames.ArmorBox, ItemNames.BodyArmor, 100, false),
            };
            foreach (var item in items)
            {
                if (item != null)
                    Inventory.DropSystem.DropItem(item, _dropPoint, 0);
            }
            if (WinnerFamily > 0)
                Chat.AdminToAll($"Мероприятие \"Захват острова\" завершено! Победила семья {WinnerFamilyName}");
            else
                Chat.AdminToAll("Мероприятие \"Захват острова\" завершено!");
        }

        private static void LoadZone(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            var startTime = (int)(DateTime.Now - StartDate).TotalSeconds;
            var totalTime = _minuteFullTimeMP * 60;
            var constrictionTime = (_minuteFullTimeMP - _minuteBeforeEndMP) * 60;
            SafeTrigger.ClientEvent(player,"islandCapt::loadZone", startTime, totalTime, constrictionTime, _startZone.Center, _startZone.Range, _endZone.Center, _endZone.Range);
        }

        private static void UnloadZone(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"islandCapt::unloadZone");
        }

        public override bool PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            if (!RemoveMember(player))
                return false;
            if (killer != null && killer != player && IsMember(killer))
            {
                var killerCharacter = killer.Character;
                var killerFamily = killer.GetFamily();
                if (killerFamily != null && killerCharacter != null)
                    AddKill(killerCharacter.UUID, killer.Name, killerFamily.Id);
            }
            else if (killer.IsLogged() && killer != player)
                Chat.SendToAdmins(1, $"{killer.Name} убил игрока {player.Name} на захвате острова, не являясь участником МП");
            NAPI.Task.Run(() =>
            {
                player.Health = 0;
            }, 5000);
            return true;
        }

        public static void PlayerDisconnected(ExtPlayer player)
        {
            RemoveMember(player);
        }
    }
}
