using System.Collections.Generic;
using GTANetworkAPI;
using Whistler;
using Whistler.Casino.Dtos;
using ServerGo.Casino.Business;
using ServerGo.Casino.ChipModels;
using ServerGo.Casino.Gamblers;
using ServerGo.Casino.Games.Poker.Cards;
using System.Linq;
using System;
using Whistler.SDK;
using Whistler.Helpers;

namespace ServerGo.Casino.Games.Poker
{
    internal class Combinations
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Combinations));
        public static Combins GetComb(PokerPlayer playerP, List<Card> tableCards)
        {
            try
            {
                //full collection available cards
                List<Card> allCard = new List<Card>(tableCards);
                allCard.Add(playerP.Cards[0]);
                allCard.Add(playerP.Cards[1]);
                List<Card> WinCards;
                //check fash and street
                WinCards = IsStraightFlush(allCard);
                if (WinCards != null)
                {
                    if (WinCards[0].Value == Nominal.Ace)
                        return new Combins(CombsType.RoyalFlush, WinCards, playerP._place);
                    else
                        return new Combins(CombsType.StraightFlush, WinCards, playerP._place);
                }
                WinCards = IsFour(allCard);
                if (WinCards != null)
                {
                    return new Combins(CombsType.Four, WinCards, playerP._place);
                }
                WinCards = IsFullHouse(allCard);
                if (WinCards != null)
                {
                    return new Combins(CombsType.FullHouse, WinCards, playerP._place);
                }
                WinCards = IsFlush(allCard);
                if (WinCards != null)
                {
                    return new Combins(CombsType.Flush, WinCards, playerP._place);
                }
                WinCards = IsStraight(allCard);
                if (WinCards != null)
                {
                    return new Combins(CombsType.Straight, WinCards, playerP._place);
                }
                WinCards = IsThree(allCard);
                if (WinCards != null)
                {
                    return new Combins(CombsType.Three, WinCards, playerP._place);
                }
                WinCards = IsTwoPair(allCard);
                if (WinCards != null)
                {
                    return new Combins(CombsType.TwoPair, WinCards, playerP._place);
                }
                WinCards = IsPair(allCard);
                if (WinCards != null)
                {
                    return new Combins(CombsType.Pair, WinCards, playerP._place);
                }
                WinCards = IsHighcard(allCard);
                return new Combins(CombsType.Highcard, WinCards, playerP._place);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | GetComb: " + e.ToString());
                return null;
            }
        }

        private static List<Card> IsStraightFlush(List<Card> cards)
        {
            try
            {
                CompSuit<Card> sortSuit = new CompSuit<Card>();
                cards.Sort(sortSuit); //sort by suit
                for (int i = 0; i < cards.Count - 4; i++)
                {
                    if (((cards[i].Value == cards[i + 4].Value + 4) || (cards[i].Value == Nominal.Five && cards[i + 3].Value == Nominal.Two && cards[i + 4].Value == Nominal.Ace)) &&
                        cards[i].Suit == cards[i + 4].Suit) //one suit and 5 cards in row
                        return new List<Card>() { cards[i], cards[i + 1], cards[i + 2], cards[i + 3], cards[i + 4] };
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | IsStraightFlush: " + e.ToString());
                return null;
            }
        }

        private static List<Card> IsFour(List<Card> cards)
        {
            try
            {
                CompVal<Card> sortVal = new CompVal<Card>();
                cards.Sort(sortVal); //sort by value
                for (int i = 0; i < cards.Count - 3; i++)
                {
                    if (cards[i].Value == cards[i + 3].Value) //4 equal cards
                        if (i == 0)
                            return new List<Card>() { cards[i], cards[i + 1], cards[i + 2], cards[i + 3], cards[i + 4] };
                        else
                            return new List<Card>() { cards[0], cards[i], cards[i + 1], cards[i + 2], cards[i + 3] };
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | IsFour: " + e.ToString());
                return null;
            }
        }

        private static List<Card> IsFullHouse(List<Card> cards)
        {
            try
            {
                CompVal<Card> sortVal = new CompVal<Card>();
                cards.Sort(sortVal); //sort by value
                for (int i = 0; i < cards.Count - 2; i++)
                    if (cards[i].Value == cards[i + 2].Value) //3 equal cards
                        for (int j = 0; j < cards.Count - 1; j++)
                            if (cards[j].Value == cards[j + 1].Value && cards[j].Value != cards[i].Value)
                                return new List<Card>() { cards[i], cards[i + 1], cards[i + 2], cards[j], cards[j + 1] };
                return null;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | IsFullHouse: " + e.ToString());
                return null;
            }
        }


        private static List<Card> IsFlush(List<Card> cards)
        {
            try
            {
                CompSuit<Card> sortSuit = new CompSuit<Card>();
                cards.Sort(sortSuit); //sort by suit
                for (int i = 0; i < cards.Count - 4; i++)
                {
                    if (cards[i].Suit == cards[i + 4].Suit)
                        return new List<Card>() { cards[i], cards[i + 1], cards[i + 2], cards[i + 3], cards[i + 4] };
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | IsFlush: " + e.ToString());
                return null;
            }
        }

        private static List<Card> IsStraight(List<Card> cards)
        {
            try
            {
                for (Nominal n = Nominal.Ace; n > Nominal.Five; n--)
                {
                    if (cards.FirstOrDefault(x => x.Value == n) != null &&
                        cards.FirstOrDefault(x => x.Value == n - 1) != null &&
                        cards.FirstOrDefault(x => x.Value == n - 2) != null &&
                        cards.FirstOrDefault(x => x.Value == n - 3) != null &&
                        cards.FirstOrDefault(x => x.Value == n - 4) != null)
                    {
                        return new List<Card>() {
                        cards.FirstOrDefault(x => x.Value == n),
                        cards.FirstOrDefault(x => x.Value == n - 1),
                        cards.FirstOrDefault(x => x.Value == n - 2),
                        cards.FirstOrDefault(x => x.Value == n - 3),
                        cards.FirstOrDefault(x => x.Value == n - 4) };
                    }
                }
                if (cards.FirstOrDefault(x => x.Value == Nominal.Five) != null &&
                    cards.FirstOrDefault(x => x.Value == Nominal.Four) != null &&
                    cards.FirstOrDefault(x => x.Value == Nominal.Three) != null &&
                    cards.FirstOrDefault(x => x.Value == Nominal.Two) != null &&
                    cards.FirstOrDefault(x => x.Value == Nominal.Ace) != null)
                {
                    return new List<Card>() {
                        cards.FirstOrDefault(x => x.Value == Nominal.Five),
                        cards.FirstOrDefault(x => x.Value == Nominal.Four),
                        cards.FirstOrDefault(x => x.Value == Nominal.Three),
                        cards.FirstOrDefault(x => x.Value == Nominal.Two),
                        cards.FirstOrDefault(x => x.Value == Nominal.Ace) };
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | IsStraight: " + e.ToString());
                return null;
            }
        }

        private static List<Card> IsThree(List<Card> cards)
        {
            try
            {
                CompVal<Card> sortVal = new CompVal<Card>();
                cards.Sort(sortVal); //sort by value
                for (int i = 0; i < cards.Count - 2; i++)
                    if (cards[i].Value == cards[i + 2].Value)
                        switch (i)
                        {
                            case 0:
                                return new List<Card>() { cards[i], cards[i + 1], cards[i + 2], cards[i + 3], cards[i + 4] };
                            case 1:
                                return new List<Card>() { cards[i], cards[i + 1], cards[i + 2], cards[0], cards[4] };
                            default:
                                return new List<Card>() { cards[i], cards[i + 1], cards[i + 2], cards[0], cards[1] };
                        }
                return null;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | IsThree: " + e.ToString());
                return null;
            }
        }

        private static List<Card> IsTwoPair(List<Card> cards)
        {
            try
            {
                CompVal<Card> sortVal = new CompVal<Card>();
                cards.Sort(sortVal); //sort by value
                for (int i = 0; i < cards.Count - 1; i++)
                    if (cards[i].Value == cards[i + 1].Value)
                        for (int j = i + 1; j < cards.Count - 1; j++)
                            if (cards[j].Value == cards[j + 1].Value && cards[j].Value != cards[i].Value)
                                switch (i)
                                {
                                    case 0:
                                        if (j == 2)
                                            return new List<Card>() { cards[i], cards[i + 1], cards[j], cards[j + 1], cards[4] };
                                        else
                                            return new List<Card>() { cards[i], cards[i + 1], cards[j], cards[j + 1], cards[2] };
                                    default:
                                        return new List<Card>() { cards[i], cards[i + 1], cards[j], cards[j + 1], cards[0] };
                                }
                return null;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | IsTwoPair: " + e.ToString());
                return null;
            }
        }

        private static List<Card> IsPair(List<Card> cards)
        {
            try
            {
                CompVal<Card> sortVal = new CompVal<Card>();
                cards.Sort(sortVal); //sort by vale
                for (int i = 0; i < cards.Count - 1; i++)
                    if (cards[i].Value == cards[i + 1].Value)
                        switch (i)
                        {
                            case 0:
                                return new List<Card>() { cards[i], cards[i + 1], cards[2], cards[3], cards[4] };
                            case 1:
                                return new List<Card>() { cards[i], cards[i + 1], cards[0], cards[3], cards[4] };
                            case 2:
                                return new List<Card>() { cards[i], cards[i + 1], cards[0], cards[1], cards[4] };
                            default:
                                return new List<Card>() { cards[i], cards[i + 1], cards[0], cards[1], cards[2] };
                        }
                return null;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | IsPair: " + e.ToString());
                return null;
            }
        }
        private static List<Card> IsHighcard(List<Card> cards)
        {
            try
            {
                CompVal<Card> sortVal = new CompVal<Card>();
                cards.Sort(sortVal);
                return new List<Card>() { cards[0], cards[1], cards[2], cards[3], cards[4] };
            }
            catch (Exception e)
            {
                _logger.WriteError($"Combinations | IsHighcard: " + e.ToString());
                return null;
            }
        }

    }
}