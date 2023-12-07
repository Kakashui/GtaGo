namespace Whistler.Core.Models
{
    internal class RewardData
    {
        public readonly int Money = 0;
        public readonly int Donate = 0;

        public RewardData(int money = 0, int donate = 0)
        {
            Money = money;
            Donate = donate;
        }
    }
}
