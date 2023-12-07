using System.Collections.Generic;

namespace ServerGo.Casino.Games.Poker.Cards
{
    /// <summary>
    /// Represents the base poker card from 52 deck
    /// </summary>
    internal class Card
    {
        public Suit Suit { get; }
        public Nominal Value { get; }
        public bool Wins { get; set;}

        public Card(Suit suit, Nominal value)
        {
            Suit = suit;
            Value = value;
            Wins = false;
        }
        public bool IsEqualTo(Card card) =>
            Suit == card.Suit && Value == card.Value;
    }

    internal enum Suit
    {
        Heart,
        Diamond,
        Club,
        Spade
    }

    internal enum Nominal
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace // = 14
    }

    class CompSuit<T> : IComparer<T> //sort by suit, val desc
        where T : Card
    {
        public int Compare(T x, T y)
        {
            if (x.Suit < y.Suit)
                return 1;
            else if (x.Suit > y.Suit)
                return -1;
            else 
            {
                if (x.Value < y.Value)
                    return 1;
                else if (x.Value > y.Value)
                    return -1;
            }            
            return 0;
        }
    }
    
    class CompVal<T> : IComparer<T> //sort by val desc
        where T : Card
    {
        public int Compare(T x, T y)
        {
            if (x.Value < y.Value)
                return 1;
            else if (x.Value > y.Value)
                return -1;
            else 
            {
            if (x.Suit < y.Suit)
                return 1;
            else if (x.Suit > y.Suit)
                return -1;
            }            
            return 0;
        }
    }
}