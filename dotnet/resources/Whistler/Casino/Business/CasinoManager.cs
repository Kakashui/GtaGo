using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Whistler;
using Whistler.SDK;
using System.Linq;
using Whistler.Casino.Dtos;
using Whistler.Core.Character;
using ServerGo.Casino.ChipModels;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.Entities;

namespace ServerGo.Casino.Business
{
    /// <summary>
    /// Rage mp script class,
    /// represents all casinos interactions
    /// </summary>
    internal class CasinoManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CasinoManager));

        public const int MinPlayerLvlToEnter = 3;
        public const int MinPlayerBalanceToEnter = 10000;
        public static double StateShare = 0.2; //in percents
        public static double CasinoShare = 0.2; //in percents
        public static readonly Vector3 RoomEnterPoint = new Vector3(1089.808, 205.9162, -50.11974);
        public static readonly Vector3 RoomExitPoint = new Vector3(1089.808, 205.9162, -50.11974);
        public static readonly Vector3 CashBoxPoint = new Vector3(1115.991, 219.9869, -49.55512);

        public static Vector3 EnterToCasinoRoof { get; set; } = new Vector3(1085.48, 214.58, -50.2);
        public static Vector3 ExitFromCasinoRoof { get; set; } = new Vector3(964.7249, 58.64898, 111.553);

     

        public static readonly Vector3 FirstRouletteTablePoint = new Vector3(989.3856, 61.16, 74.491);
        public static readonly Vector3 SecondRouletteTablePoint = new Vector3(983.6421, 61.12032, 74.539);
        public static readonly Vector3 ThirdRouletteTablePoint = new Vector3(989.7935, 56.88181, 74.539);
        public static readonly Vector3 FourRouletteTablePoint = new Vector3(942.100, 54.8711, 74.991);

        public static readonly Vector3 FiveRouletteTablePoint = new Vector3(947.2969, 54.710, 74.991);
        public static readonly Vector3 SixRouletteTablePoint = new Vector3(943.025, 58.485, 74.991);
        public static readonly Vector3 SevenRouletteTablePoint = new Vector3(946.900, 57.791, 74.991);
        public static readonly Vector3 EightRouletteTablePoint = new Vector3(984.260, 56.77, 74.539);
        public static readonly Vector3 NinthRouletteTablePoint = new Vector3(988.6068, 65.0856, 74.5062);        


        public static readonly List<Tuple<Vector3,Vector3, int,int>> PokerTablePoint = new List<Tuple<Vector3,Vector3, int,int>>(){

            new Tuple<Vector3, Vector3, int, int>(new Vector3(1145.9, 261.21, -52.8), new Vector3(0, 0, 40), 5000, 500000), //coords, rotation, small blind, enter money
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1151.63, 266.86, -52.84), new Vector3(0, 0, 40), 500, 10000), 
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1148.63, 269.41, -52.84), new Vector3(0, 0, 54), 500, 10000),
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1143.38, 264.19, -52.84), new Vector3(0, 0, 44), 500, 10000), 
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1129.61, 262.42, -52.03), new Vector3(0, 0, -45), 500, 10000) ,
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1133.50, 266.64, -52.03), new Vector3(0, 0, -30), 500000, 1000000),

            new Tuple<Vector3, Vector3, int, int>(new Vector3(1144.59, 247.47, -52.03), new Vector3(0, 0, -65), 500, 10000),
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1148.68, 251.92, -52.03), new Vector3(0, 0, -50), 500, 10000),
            //new Tuple<Vector3, Vector3, int, int>(new Vector3(962.6018, 49.91366, 74), new Vector3(0, 0, -35), 500, 10000),
            //new Tuple<Vector3, Vector3, int, int>(new Vector3(966.7984, 57.2475, 74), new Vector3(0, 0, 150), 500, 10000),
            //new Tuple<Vector3, Vector3, int, int>(new Vector3(985.1536, 61.3902, 74.4955), new Vector3(0, 0, -125), 5000, 100000),
            //new Tuple<Vector3, Vector3, int, int>(new Vector3(993.7558, 57.0898, 74.4914), new Vector3(0, 0, 65), 5000, 100000)
        };
        public static readonly List<Tuple<Vector3, Vector3, int, int>> RouletteTablePoint = new List<Tuple<Vector3, Vector3, int, int>>(){
             //coords, rotation, small blind, enter money
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1134.9987,256.5799,-52.0607), new Vector3(0,0,0), 5000, 500000),
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1138.1013,252.8016,-52.0477), new Vector3(0,0,0), 500, 10000),
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1131.9513,253.2558,-52.0608), new Vector3(0,0,0), 500, 10000),
            new Tuple<Vector3, Vector3, int, int>(new Vector3(1134.7981,250.1860,-52.0358), new Vector3(0,0,0), 500, 10000)
        };

        public static readonly List<Tuple<Vector3,Vector3, int,int, uint>> PrivatePokerTablePoint = new List<Tuple<Vector3,Vector3, int, int, uint>>()
        {
            //coords, rotation, small blind, enter money, dimansion
            new Tuple<Vector3, Vector3, int, int, uint>(new Vector3(-3419.304, -615.7401, 453.0036) + new Vector3(5060, 110, -345), new Vector3(0, 0, 270), 50000, 5000000, 20428), //Slade
            new Tuple<Vector3, Vector3, int, int, uint>(new Vector3(-3419.304, -615.7401, 453.0036) + new Vector3(5060, 110, -345), new Vector3(0, 0, 270), 50000, 5000000, 20688), //Nevechen
            new Tuple<Vector3, Vector3, int, int, uint>(new Vector3(-3419.304, -615.7401, 453.0036) + new Vector3(5060, 110, -345), new Vector3(0, 0, 270), 50000, 5000000, 20109), //Black_Star

            new Tuple<Vector3, Vector3, int, int, uint>(new Vector3(-3419.344, -585.4146, 453.0035) + new Vector3(5060, 110, -345), new Vector3(0, 0, 270), 5000, 500000, 20428), //Slade
            new Tuple<Vector3, Vector3, int, int, uint>(new Vector3(-3419.344, -585.4146, 453.0035) + new Vector3(5060, 110, -345), new Vector3(0, 0, 270), 5000, 500000, 20688), //Nevechen
            new Tuple<Vector3, Vector3, int, int, uint>(new Vector3(-3419.344, -585.4146, 453.0035) + new Vector3(5060, 110, -345), new Vector3(0, 0, 270), 5000, 500000, 20109), //Black_Star
        };

        public static readonly List<Tuple<Vector3,Vector3, int,int, uint>> PrivateRouletteTablePoint = new List<Tuple<Vector3,Vector3, int, int, uint>>()
        {
            //coords, rotation, small blind, enter money, dimansion
        };

        /// <summary>
        /// Returns a list of all casinos
        /// </summary>
        public static List<Casino> Casinos = new List<Casino>();

        public static Casino FindCasinoByBizId(int bizId)
        {
            foreach (var casino in Casinos)
                if (casino.BizId == bizId)
                    return casino;
            return null;
        }
       
        public static Casino FindFirstCasino()
        {
            if (!Casinos.Any()) return null;
            return Casinos[0];
        }
        
        #region ServerEvents
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            NAPI.Marker.CreateMarker(1, EnterToCasinoRoof - new Vector3(0, 0, 0.5), new Vector3(), new Vector3(), 1, new Color(255, 255, 255, 220));
            NAPI.Marker.CreateMarker(1, ExitFromCasinoRoof - new Vector3(0, 0, 0.5), new Vector3(), new Vector3(), 1, new Color(255, 255, 255, 220));
            
            var questPed = new QuestPed(PedHash.Scdressy01AFY, new Vector3(1115.153, 254.2012, -45.83101), "Kris", "", 280, interactionRange: 2);
            questPed.PlayerInteracted += (player, ped) =>
            {
                if (BusinessManager.GetBusiness(FindFirstCasino().BizId).OwnerID == player.Character.UUID)
                {
                    var page1 =
                        new DialogPage("Конечно, сейчас приготовлю по вашему вкусу из лучшей арабики", ped.Name,
                                ped.Role)
                            .AddCloseAnswer();
                    var page2 =
                        new DialogPage("Большое спасибо, как это мило с вашей стороны!", ped.Name,
                                ped.Role)
                            .AddCloseAnswer();
                    var introPage =
                        new DialogPage("Добрый день, босс! Чем сейчас могу вам помочь?", ped.Name, ped.Role)
                            .AddAnswer("Принеси мне, пожалуйста кофе", page1)
                            .AddAnswer("Открой, пожалуйста, меню управления казино", CasinoMenu)
                            .AddAnswer("Крис, а ты сегодня потрясающе выглядишь!", page2)
                            .AddCloseAnswer("Спасибо, пока ничего не нужно");
                    introPage.OpenForPlayer(player);
                }
                else
                {
                    var introPage =
                        new DialogPage("Чем сейчас могу вам помочь?", ped.Name, ped.Role)
                            .AddCloseAnswer();
                    introPage.OpenForPlayer(player);
                }
            };

            InteractShape.Create(EnterToCasinoRoof, 1, 2)
                .AddDefaultMarker()
                .AddInteraction(RoofExitCasino, "Чтобы выйти");

            InteractShape.Create(ExitFromCasinoRoof, 1, 2)
                .AddDefaultMarker()
                .AddInteraction(RoofEnterCasino, "Чтобы выйти");


        }

        private static void RoofExitCasino(ExtPlayer player)
        {
            player.ChangePosition(ExitFromCasinoRoof + new Vector3(0, 0, .7));
        }

        private static void RoofEnterCasino(ExtPlayer player)
        {
            player.ChangePosition(EnterToCasinoRoof + new Vector3(0, 0, .7));
        }

        [Command("casino")]
        public static void CasinoMenu(ExtPlayer player)
        {
            try
            {
                if (BusinessManager.GetBusiness(FindFirstCasino().BizId).OwnerID != player.Character.UUID) return;
                SafeTrigger.ClientEvent(player,
                    "openCasinoOwnerMenu", player.Name, Convert.ToInt32(StateShare * 100), Convert.ToInt32(CasinoShare * 100), FindFirstCasino().CashBox.Amount
                    );
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | cmenu: {e.Message}");
            }
        }
        
        [RemoteEvent("casino:setTax")]
        public static void SetTax(ExtPlayer player, int tax)
        {
            try
            {
                UpdateLocalShare((double) tax / 100);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | setTax: {e.Message}");
            }
        }
        [RemoteEvent("casinoOwner:toDeposit")]
        public static void Deposit(ExtPlayer player, int charge)
        {
            try
            {
                if (charge <= 0 || player.Character.Money < charge)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Введённая сумма неверна", 3000);
                    return;
                }
                FindFirstCasino().CashBox.Charge(charge);
                SafeTrigger.ClientEvent(player,"casinoOwner:update", FindFirstCasino().CashBox.Amount);
                Wallet.MoneySub(player.Character, charge, "Пополнение счета казино");
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | casinoOwner:toDeposit: {e.Message}");
            }
        }
        [RemoteEvent("casinoOwner:toCredit")]
        public static void Withdraw(ExtPlayer player, int withdraw)
        {
            try
            {
                if (withdraw <= 0 || withdraw > FindFirstCasino().CashBox.Amount) 
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Введённая сумма неверна", 3000);
                    return;
                }
                FindFirstCasino().CashBox.Withdraw(withdraw, player);
                SafeTrigger.ClientEvent(player,"casinoOwner:update", FindFirstCasino().CashBox.Amount);
                Wallet.MoneyAdd(player.Character, withdraw, "Снятие со счета казино");
            }
            catch (Exception e) 
            {
                _logger.WriteError($"Casino | casinoOwner:toCredit: {e.Message}");
            }
        }
        
        [RemoteEvent("player:boughtChips")]
        public static void BoughtChipsCallback(ExtPlayer player, int black, int red, int blue, int green, int yellow)
        {
            try
            {
                var dto = new CashBoxDto
                {
                    Black = black,
                    Red = red,
                    Blue = blue,
                    Green = green,
                    Yellow = yellow
                };
                FindFirstCasino().OnPlayerBoughtChips(player, dto);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | PlayerBoughtChips: {e.Message}");
            }
        }
        [RemoteEvent("player:soldChips")]
        public static void SoldChipsCallback(ExtPlayer player)
        {
            try
            {
                FindFirstCasino().OnPlayerSoldChips(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | PlayerSoldChips: {e.Message} CasinoManager");
            }
        }

        [RemoteEvent("player:leftRoulette")]
        public static void LeftRouletteCallback(ExtPlayer player)
        {
            try
            {
                FindFirstCasino().OnPlayerLeftGame(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:leftRoulette: {e.Message} CasinoManager");
            }
        }

        [RemoteEvent("player:leftPoker")]
        public static void LeftPokerCallback(ExtPlayer player)
        {
            try
            {
                FindFirstCasino()
                    .OnPlayerLeftGame(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:leftRoulette: {e.Message} CasinoManager");
            }
        }
        
        [RemoteEvent("player:sentTimerInfo")]
        public static void SentTimerInfo(ExtPlayer player, int seconds)
        {
            Console.WriteLine($"player:sentTimerInfo: {seconds}");
            try
            {
                var dto = new TimerDto
                {
                    Seconds = seconds
                };
                FindFirstCasino().OnPlayerSentTimerInfo(player, dto);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:sentTimerInfo: {e.Message} CasinoManager");
            }
        }
        
        [RemoteEvent("player:placedBet")]
        public static void PlacedBetCallback(ExtPlayer player, string bet, int black, int red, int blue, int green, int yellow)
        {
            try
            {
                var dto = new CashBoxDto
                {
                    Black = black,
                    Red = red,
                    Blue = blue,
                    Green = green,
                    Yellow = yellow
                };
                if (dto.TotalCount <= 0) return;
                if (dto.Black < 0 || dto.Red < 0 || dto.Black < 0 || dto.Green < 0 || dto.Blue < 0) return;
                FindFirstCasino()
                    .OnPlayerPlacedBets(player, bet, dto.CreateChipList());
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:placedBet: {e.Message} CasinoManager");
            }
        }
        
        [RemoteEvent("player:pokerBet")]
        public static void PokerBetCallback(ExtPlayer player, string bet, int amount)
        {
            try
            {
                FindFirstCasino()
                    .OnPlayerPokerPlacedBets(player, bet, amount);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:pokerBet: {e.Message} CasinoManager");
            }
        }
        
        [RemoteEvent("player:pokerBuyChips")]
        public static void PokerBuyChips(ExtPlayer player, int amount)
        {
            try
            {
                FindFirstCasino()
                    .OnPlayerPokerBuyChips(player, amount);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:pokerBet: {e.Message} CasinoManager");
            }
        }
        [RemoteEvent("player:canceledBet")]
        public static void CanceledBetCallback(ExtPlayer player)
        {
            try
            {
                FindFirstCasino().OnPlayerCanceledBet(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:placedBet: {e.Message} CasinoManager");
            }
        }

        [RemoteEvent("player:playAnim")]
        public static void PlayAnimCallback(ExtPlayer player, int flag, string animDictionary, string animName, int x, int y, int z, int angle)
        {
            try
            {
                //player.PlayAnimation(animDictionary, animName, flag);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:playAnim {e.Message} CasinoManager");
            }
        }
        
        [RemoteEvent("player:stopAnim")]
        public static void StopAnimCallback(ExtPlayer player)
        {
            try
            {
                NAPI.Player.StopPlayerAnimation(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:stopAnim {e.Message} CasinoManager");
            }
        }
        
        [RemoteEvent("player:requestBank")]
        public static void RequestBankCallback(ExtPlayer player)
        {
            try
            {
                FindFirstCasino()
                    .OnPlayerRequestedBank(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | player:requestBank {e.Message} CasinoManager");
            }
        }
        
        [ServerEvent(Event.PlayerConnected)]
        public static void PlayerConnected(ExtPlayer player) //PlayerConnectedCallback
        {
            try
            {
                if (!Casinos.Any()) return;
                var casino = FindFirstCasino();
                casino.LoadBoardObjects(player);
                SafeTrigger.SetData(player, "CURRENTCASINO_ID", casino.BizId);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | PlayerConnected: {e.Message} CasinoManager");
            }
        }
        #endregion

        public static void UpdateStateShare(double percent)
        {
            StateShare = percent;
            MySQL.Query("UPDATE casino SET stateTax = @prop0 WHERE id = 0", StateShare * 100);
        }
        
        public static void UpdateLocalShare(double percent)
        {
            CasinoShare = percent;
            MySQL.Query("UPDATE casino SET casinoTax = @prop0 WHERE id = 0", CasinoShare * 100);
        }

        public static void ChangeImage(ExtPlayer player, string image)
        {
            FindFirstCasino().OnPlayerChangeImage(player, image);
        }
    }
}