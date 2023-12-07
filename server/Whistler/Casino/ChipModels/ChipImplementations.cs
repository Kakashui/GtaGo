namespace ServerGo.Casino.ChipModels
{
    public class Black : Chip
    {
        public Black(int position = 0)
            : base(ChipType.Black, Constants.FiveHundredDollar){}
    }
    public class Red : Chip
    {
        public Red() 
            : base(ChipType.Red, Constants.OneThousandDollar) { }
    }
    public class Blue : Chip
    {
        public Blue() 
            : base(ChipType.Blue, Constants.TwoThousandDollar) { }
    }
    public class Green : Chip
    {
        public Green() 
            : base(ChipType.Green, Constants.FiveThousandDollar) { }
    }

    public class Yellow : Chip
    {
        public Yellow() 
            : base(ChipType.Yellow, Constants.TenThousandDollar) { }
    }
}