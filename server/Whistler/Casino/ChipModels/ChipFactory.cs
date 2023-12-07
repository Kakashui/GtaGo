namespace ServerGo.Casino.ChipModels
{
    public class ChipFactory
    {
        public static Chip Create(ChipType type)
        {
            switch (type)
            {
                case ChipType.Black:
                    return new Black();
                case ChipType.Red:
                    return new Red();
                case ChipType.Green:
                    return new Green();
                case ChipType.Blue:
                    return new Blue();
                case ChipType.Yellow:
                    return new Yellow();
                default: return null;
            }
        }


    }
}