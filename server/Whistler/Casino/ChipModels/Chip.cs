namespace ServerGo.Casino.ChipModels
{
    /// <summary>
    /// The Chip class represents a generic casino chip.
    /// It is an abstract class.
    /// </summary>
    public abstract class Chip
    {
        /// <summary>
        /// Creates a new Chip.
        /// An optional position parameter is used to determine a y-axis offset, when stacking chips.
        /// </summary>
        /// <param name="position"></param>
        public Chip(ChipType chipType, int value)
        {
            ChipType = chipType;
            Value = value;
        }

        /// <summary>
        /// Gets the chip type.
        /// </summary>
        public ChipType ChipType { get; }

        /// <summary>
        /// Gets the chip value (in dollars).
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// The GetChipValue method is called to return the dollar value for a provided chip type.
        /// </summary>
        /// <param name="chipType"></param>
        /// <returns></returns>
        public static int GetChipValue(ChipType chipType)
        {
            int value = 0;

            switch (chipType)
            {
                case ChipType.Black:
                    value = Constants.OneDollar;
                    break;
                case ChipType.Red:
                    value = Constants.TwentyFiveDollar;
                    break;
                case ChipType.Blue:
                    value = Constants.OneHundredDollar;
                    break;
                case ChipType.Green:
                    value = Constants.FiveHundredDollar;
                    break;
                case ChipType.Yellow:
                    value = Constants.OneThousandDollar;
                    break;
            }

            return value;
        }
    }
}
