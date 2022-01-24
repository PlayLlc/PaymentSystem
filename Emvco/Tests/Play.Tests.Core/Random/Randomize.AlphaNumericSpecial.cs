using System.Collections.Immutable;

namespace Play.Tests.Core.Random;

public partial class Randomize
{
    public class AlphaNumericSpecial
    {
        #region Static Metadata

        // 32 - 126
        private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
            Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

        private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
            Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

        #endregion

        #region Instance Members

        public static char Char() => GetRandomChar();

        public static char[] Chars(int length)
        {
            char[] result = new char[length];

            for (short i = 0; i <= (length - 1); i++)
                result[i] = GetRandomChar();

            return result;
        }

        public static string String(int length)
        {
            char[] result = new char[length];

            for (short i = 0; i <= (length - 1); i++)
                result[i] = GetRandomChar();

            return new string(result);
        }

        private static char GetRandomChar() => _CharMap[(byte) _Random.Next(32, 126)];
        public static byte GetRandomByte() => _ByteMap[GetRandomChar()];

        #endregion
    }
}