using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerGo.Casino.Games.Poker.Cards
{
    /// <summary>
    /// Represents the single poker deck
    /// </summary>
    internal class Deck
    {
        public List<Card> Cards { get; }

        public Deck()
        {
            Cards = new List<Card>();

            for (var suit = Suit.Heart; suit <= Suit.Spade; suit++)
                for (var nominal = Nominal.Two; nominal <= Nominal.Ace; nominal++)
                {
                    Cards.Add(new Card(suit, nominal));
                }
        }

        public Card ExtractCard(Card card)
        {
            var removedCard = Cards.FirstOrDefault(c => c.IsEqualTo(card));
            if (removedCard != null)
                Cards.Remove(removedCard);
            return removedCard;
        }

        public Card GetRandomCard()
        {
            int pos = new Random().Next(0, Cards.Count);
            var randomCard = new Card(Cards[pos].Suit, Cards[pos].Value);
            return randomCard;
        }
    }
}