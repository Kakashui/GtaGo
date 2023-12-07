namespace Whistler.MP.Arena.Helpers
{
    internal static class ArenaDimensionHelper
    {
        private static uint _dimensionPoolHead = 123;
        
        public static uint GetUniqueDimension() =>_dimensionPoolHead++;
    }
}