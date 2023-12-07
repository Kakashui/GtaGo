using System;
using System.Collections.Generic;
using System.Linq;
using ServerGo.Casino.ChipModels;

namespace ServerGo.Casino.Games.Roulette
{
    class RouletteHelper
    {
        List<Tuple<int, string>> _board = new List<Tuple<int, string>>();
        public ChipList GetWinning(int number, Bet bet)
        {
            if (_board.Count != 36)
            {
                AddBoard();
            }

            var winning = new ChipList(bet.Cost.Chips);
            var digit = bet.BetName.All(char.IsDigit);
            if (digit)
            {
                var num = int.Parse(bet.BetName);
                if (num == number)
                {
                    winning.AddChipsToList(36);
                }
            }
            else
            {
                var color = string.Empty;
                foreach (var pare in _board)
                {
                    if (pare.Item1 == number)
                        color = pare.Item2;
                }
                switch (bet.BetName)
                    {
                    case "red":
                        if (color == bet.BetName)
                            winning.AddChipsToList(2);
                        break;
                    case "black":
                        if (color == bet.BetName)
                            winning.AddChipsToList(2);
                        break;
                    case "firstdozen":
                        if (number >= 1 && number <= 12)
                            winning.AddChipsToList(3);
                        break;
                    case "seconddozen":
                        if (number >= 13 && number <= 24)
                            winning.AddChipsToList(3);
                        break;
                    case "thirddozen":
                        if (number >= 25 && number <= 36)
                            winning.AddChipsToList(3);
                        break;
                    case "even":
                        if (number % 2 == 0)
                            winning.AddChipsToList(2);
                        break;
                    case "odd":
                        if (number % 2 != 0)
                            winning.AddChipsToList(2);
                        break;
                    case "firsthalf":
                        if (number >= 1 && number <= 18)
                            winning.AddChipsToList(2);
                        break;
                    case "lasthalf":
                        if (number >= 19 && number <= 36)
                            winning.AddChipsToList(2);
                        break;
                    case "firstrow":
                        if (number % 3 == 0)
                            winning.AddChipsToList(3);
                        break;
                    case "secondrow":
                        if (number % 3 == 2)
                            winning.AddChipsToList(3);
                        break;
                    case "thirdrow":
                        if (number % 3 == 1)
                            winning.AddChipsToList(3);
                        break;
                    case "dzero":
                        if (number == 37)
                            winning.AddChipsToList(36);
                        break;
                    case "zero":
                        if (number == 0)
                            winning.AddChipsToList(36);
                        break;
                }
            }
            
            //empty array if there were no changes
            return winning.Chips.Count <= bet.Cost.Chips.Count ? new ChipList(new Chip[0]) : winning;
        }
        
        
        public void AddBoard()
        {
            _board.Clear();
            _board.Add(new Tuple<int, string>(1, "red"));
            _board.Add(new Tuple<int, string>(2, "black"));
            _board.Add(new Tuple<int, string>(3, "red"));
            _board.Add(new Tuple<int, string>(4, "black"));
            _board.Add(new Tuple<int, string>(5, "red"));
            _board.Add(new Tuple<int, string>(6, "black"));
            _board.Add(new Tuple<int, string>(7, "red"));
            _board.Add(new Tuple<int, string>(8, "black"));
            _board.Add(new Tuple<int, string>(9, "red"));
            _board.Add(new Tuple<int, string>(10, "black"));
            _board.Add(new Tuple<int, string>(11, "black"));
            _board.Add(new Tuple<int, string>(12, "red"));
            _board.Add(new Tuple<int, string>(13, "black"));
            _board.Add(new Tuple<int, string>(14, "red"));
            _board.Add(new Tuple<int, string>(15, "black"));
            _board.Add(new Tuple<int, string>(16, "red"));
            _board.Add(new Tuple<int, string>(17, "black"));
            _board.Add(new Tuple<int, string>(18, "red"));
            _board.Add(new Tuple<int, string>(19, "red"));
            _board.Add(new Tuple<int, string>(20, "black"));
            _board.Add(new Tuple<int, string>(21, "red"));
            _board.Add(new Tuple<int, string>(22, "black"));
            _board.Add(new Tuple<int, string>(23, "red"));
            _board.Add(new Tuple<int, string>(24, "black"));
            _board.Add(new Tuple<int, string>(25, "red"));
            _board.Add(new Tuple<int, string>(26, "black"));
            _board.Add(new Tuple<int, string>(27, "red"));
            _board.Add(new Tuple<int, string>(28, "black"));
            _board.Add(new Tuple<int, string>(29, "black"));
            _board.Add(new Tuple<int, string>(30, "red"));
            _board.Add(new Tuple<int, string>(31, "black"));
            _board.Add(new Tuple<int, string>(32, "red"));
            _board.Add(new Tuple<int, string>(33, "black"));
            _board.Add(new Tuple<int, string>(34, "red"));
            _board.Add(new Tuple<int, string>(35, "black"));
            _board.Add(new Tuple<int, string>(36, "red"));
            _board.Add(new Tuple<int, string>(37, "green"));
            _board.Add(new Tuple<int, string>(38, "green"));
        }
    }
}
