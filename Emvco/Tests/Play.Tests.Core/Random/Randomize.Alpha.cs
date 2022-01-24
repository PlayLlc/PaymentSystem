namespace Play.Tests.Core.Random;

public partial class Randomize
{
    public class Alpha
    {
        #region Static Metadata

        private const string _AlphabeticValues = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        #endregion

        #region Instance Members

        public static byte[] Bytes(int length)
        {
            byte[] result = new byte[length];

            for (short i = 0; i <= (length - 1); i++)
                result[i] = GetRandomByte();

            return result;
        }

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

        private static char GetRandomChar() => _AlphabeticValues[_Random.Next(0, _AlphabeticValues.Length - 1)];
        public static byte GetRandomByte() => (byte) _AlphabeticValues[_Random.Next(0, _AlphabeticValues.Length - 1)];

        #endregion
    }
}