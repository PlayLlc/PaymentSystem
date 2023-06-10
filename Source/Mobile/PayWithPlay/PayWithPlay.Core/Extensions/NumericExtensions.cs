namespace PayWithPlay.Core.Extensions
{
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }

        public static double ToRadians(this float val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
