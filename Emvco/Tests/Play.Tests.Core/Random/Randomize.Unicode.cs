namespace Play.Tests.Core.Random
{
    public partial class Randomize
    {
        public class Unicode
        {
            #region Instance Members

            private static int GetRandomDecimal()
            {
                int decimalValue = _Random.Next(char.MinValue, char.MaxValue);
                if ((decimalValue >= 55296) && (decimalValue <= 57343))
                    GetRandomDecimal();

                return decimalValue;
            }

            private static int GetRandomDecimalInRange(int minRange, int maxRange)
            {
                if ((minRange < char.MinValue) || (maxRange > char.MaxValue))
                    throw new Exception();

                int decimalValue = _Random.Next(minRange, maxRange);
                if ((decimalValue >= 55296) && (decimalValue <= 57343))
                    GetRandomDecimal();

                return decimalValue;
            }

            private static char Char(System.Random random) => (char) GetRandomDecimal();
            private static char Char(int minRange, int maxRange) => (char) GetRandomDecimalInRange(minRange, maxRange);

            private static char GetRandomCharNotInRange(int excludeRangeMin, int excludeRangeMax)
            {
                if (excludeRangeMin > excludeRangeMax)
                    throw new ArgumentOutOfRangeException();

                int value = GetRandomDecimal();

                if ((value >= excludeRangeMin) && (value <= excludeRangeMax))
                    GetRandomDecimal();

                return (char) value;
            }

            public static char[] Chars(int length)
            {
                char[] result = new char[length];
                for (int i = 0; i <= (length - 1); i++)
                    result[i] = Char(_Random);

                return result;
            }

            private static char[] Chars(int length, int minRange, int maxRange)
            {
                char[] result = new char[length];
                for (int i = 0; i <= (length - 1); i++)
                    result[i] = Char(minRange, maxRange);

                return result;
            }

            private static char[] Chars(int length, char excludeRangeMin, char excludeRangeMax)
            {
                if (excludeRangeMin > excludeRangeMax)
                    throw new ArgumentOutOfRangeException();

                char[] result = new char[length];
                for (int i = 0; i <= (length - 1); i++)
                    result[i] = GetRandomCharNotInRange(excludeRangeMin, excludeRangeMax);

                return result;
            }

            public static string String(int length)
            {
                char[] result = new char[length];
                for (int i = 0; i <= (length - 1); i++)
                    result[i] = Char(_Random);

                return new string(result);
            }

            public static string GetRandomStringNotInRange(int length, char excludeRangeMin, char excludeRangeMax)
            {
                if (excludeRangeMin > excludeRangeMax)
                    throw new ArgumentOutOfRangeException();

                char[] result = new char[length];

                for (int i = 0; i <= (length - 1); i++)
                    result[i] = GetRandomCharNotInRange(excludeRangeMin, excludeRangeMax);

                return new string(result);
            }

            public static string GetRandomStringInRanges(int minRange1, int maxRange1, int minRange2, int maxRange2)
            {
                int chunks = _Random.Next(2, 10);
                int hexadecimalChunks = _Random.Next(1, chunks - 1);
                int unicodeChunks = chunks - hexadecimalChunks;

                char[][] range1 = new char[hexadecimalChunks][];
                char[][] range2 = new char[unicodeChunks][];

                for (int i = 0; i <= (hexadecimalChunks - 1); i++)
                    range1[i] = Chars(_Random.Next(1, 10), minRange1, maxRange1);

                for (int i = 0; i <= (unicodeChunks - 1); i++)
                    range2[i] = Chars(_Random.Next(1, 10), minRange2, maxRange2);

                IEnumerable<char> flat = range1.SelectMany(a => a).ToArray().Concat(range2.SelectMany(a => a).ToArray());

                return new string(flat.OrderBy(s => (_Random.Next(2) % 2) == 0).ToArray());
            }

            #endregion
        }
    }
}