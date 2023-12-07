namespace ServerGo.Casino.Games.Roulette
{
    public struct BetsStat
    {
        public int WinNumber { get; }//37 - 0, 38 - 00
        public string Result { get; }
        public uint Winning { get; }

        public BetsStat(int winNumber, string result, uint winning)
        {
            Winning = winning;
            WinNumber = winNumber;
            Result = result;
        }
    }
}