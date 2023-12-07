using System.Collections.Generic;
using System;

namespace ServerGo.Casino.Games.Poker.Cards
{
    /// <summary>
    /// Poker combinations
    /// </summary>
    internal class Combins : IComparer<Combins>, IComparable<Combins>
    {
        private CombsType Comb { get; }
        private List<Nominal> Highcards { get; }
        private int powerComb { get; }
        public List<Card> Cards { get; }
        public int _place { get; } //all available cards for player
        public Combins(CombsType comb, List<Card> cards, int place)
        {
            Comb = comb;
            Cards = new List<Card>(cards);
            _place = place;
            int power = 0;
            switch (comb)
            {
                case CombsType.RoyalFlush:
                    power += 1300000;
                    Highcards = null;
                    break;
                case CombsType.StraightFlush:
                    power += 1259000;
                    Highcards = new List<Nominal>() { cards[0].Value };
                    break;
                case CombsType.Four:
                    power += 1258000;
                    Highcards = new List<Nominal>() { cards[0].Value, cards[4].Value };
                    break;
                case CombsType.FullHouse:
                    power += 1257000;
                    Highcards = new List<Nominal>() { cards[0].Value, cards[3].Value };
                    break;
                case CombsType.Flush:
                    power += 657000;
                    Highcards = new List<Nominal>() { cards[0].Value, cards[1].Value, cards[2].Value, cards[3].Value, cards[4].Value };
                    break;
                case CombsType.Straight:
                    power += 656000;
                    Highcards = new List<Nominal>() { cards[0].Value };
                    break;
                case CombsType.Three:
                    power += 653000;
                    Highcards = new List<Nominal>() { cards[0].Value, cards[3].Value, cards[4].Value };
                    break;
                case CombsType.TwoPair:
                    power += 650000;
                    Highcards = new List<Nominal>() { cards[0].Value, cards[2].Value, cards[4].Value };
                    break;
                case CombsType.Pair:
                    power += 600000;
                    Highcards = new List<Nominal>() { cards[0].Value, cards[2].Value, cards[3].Value, cards[4].Value };
                    break;
                case CombsType.Highcard:
                    Highcards = new List<Nominal>() { cards[0].Value, cards[1].Value, cards[2].Value, cards[3].Value, cards[4].Value };
                    break;
            }
            if (Highcards != null)
            {
                int k = 1;
                for (int i = Highcards.Count - 1; i >= 0; i--)
                {
                    power += ((int)Highcards[i]) * k;
                    k *= 14;
                }
            }
            powerComb = power;
        }

        public int Compare(Combins x, Combins y)
        {
            if (x.powerComb > y.powerComb)  //sorted desc
                return -1;
            else if (x.powerComb < y.powerComb)
                return 1;
            else if (x._place > y._place)
                return -1;
            else if (x._place < y._place)
                return 1;
            return 0;
        }
        public int CompareTo(Combins obj)
        {
            if (this.powerComb > obj.powerComb)
                return -1;
            if (this.powerComb < obj.powerComb)
                return 1;
            else if (this._place > obj._place)
                return -1;
            else if (this._place < obj._place)
                return 1;
            return 0;
        }
        public static bool operator ==(Combins x, Combins y)
        {
            return x.powerComb == y.powerComb;
        }

        public static bool operator !=(Combins x, Combins y)
        {
            return x.powerComb != y.powerComb;
        }

        public override int GetHashCode()
        {
            return _place;
        }

    }

    internal enum CombsType
    {
        Highcard,
        Pair,
        TwoPair,
        Three,
        Straight,
        Flush,
        FullHouse,
        Four,
        StraightFlush,
        RoyalFlush
    }
}