namespace ServerGo.Casino.Games.Poker
{    
    internal enum BidRoundType
    {
        Preflop,
        Flop,
        Tern,
        River,
        OpenCard
    }

    internal enum BetType
    {
        Check,
        Raise,
        Fold,
        Call,
        AllIn
    }

}