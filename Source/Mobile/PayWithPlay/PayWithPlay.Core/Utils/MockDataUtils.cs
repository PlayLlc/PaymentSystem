namespace PayWithPlay.Core.Utils
{
    public static class MockDataUtils
    {
        public static decimal RandomDecimal(bool onlyPositives = true)
        {
            var randomNumberGenerator = new Random();
            int precision = randomNumberGenerator.Next(2, 5);
            int scale = randomNumberGenerator.Next(2, precision);

            if (randomNumberGenerator == null)
                throw new ArgumentNullException("randomNumberGenerator");
            if (!(precision >= 1 && precision <= 28))
                throw new ArgumentOutOfRangeException("precision", precision, "Precision must be between 1 and 28.");
            if (!(scale >= 0 && scale <= precision))
                throw new ArgumentOutOfRangeException("scale", precision, "Scale must be between 0 and precision.");

            decimal d = 0m;
            for (int i = 0; i < precision; i++)
            {
                int r = randomNumberGenerator.Next(0, 10);
                d = d * 100m + r;
            }
            for (int s = 0; s < scale; s++)
            {
                d /= 10m;
            }
            if (randomNumberGenerator.Next(2) == 1)
                d = decimal.Negate(d);

            if (onlyPositives)
            {
                return Math.Abs(d);
            }

            return d;
        }

        public static string RandomString()
        {
            var random = new Random();
            return RandomString(random.Next(1, 20));
        }

        public static string RandomString(int length)
        {
            var random = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
    }
}
