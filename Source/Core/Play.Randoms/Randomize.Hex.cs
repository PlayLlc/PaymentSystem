using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Randoms;

public static partial class Randomize
{
    public class Hex
    {
        #region Static Metadata

        internal const string HexValues = "0123456789ABCDEF";

        #endregion

        #region Instance Members

        public static byte[] Bytes(int length)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = GetRandomHexByte();

            return buffer.ToArray();
        }

        /// <summary>
        ///     Returns a HexadecimalCodec encoded char array
        /// </summary>
        /// <param name="length">
        ///     The length of hexadecimal characters returned
        /// </param>
        /// <returns></returns>
        public static char[] Chars(int length)
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (short i = 0; i < buffer.Length; i++)
                buffer[i] = GetRandomHexChar();

            return buffer.ToArray();
        }

        /// <summary>
        ///     Returns a HexadecimalCodec encoded string
        /// </summary>
        /// <param name="length">
        ///     The length of hexadecimal characters returned
        /// </param>
        /// <returns></returns>
        public static string String(int length)
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (short i = 0; i < buffer.Length; i++)
                buffer[i] = GetRandomHexChar();

            return new string(buffer);
        }

        private static char GetRandomHexChar() => HexValues[Random.Next(0, HexValues.Length - 1)];
        public static byte GetRandomHexByte() => (byte) HexValues[Random.Next(0, HexValues.Length - 1)];

        #endregion
    }
}