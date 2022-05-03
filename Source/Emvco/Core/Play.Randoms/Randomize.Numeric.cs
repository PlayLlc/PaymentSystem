using System.Collections.Immutable;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Core.Specifications;

namespace Play.Randoms;

public partial class Randomize
{
    /// <summary>
    ///     NumericCodec data elements consist of two numeric digits (having values in the range Hex '0' – '9') per byte.
    ///     These digits are right justified and padded with leading hexadecimal zeroes. Other specifications sometimes
    ///     refer to this data format as BinaryCodec Coded Decimal (“BCD”) or unsigned packed.
    ///     Example: Amount, Authorized(NumericCodec) is defined as “n 12” with a length of six bytes.
    ///     A value of 12345 is stored in Amount, Authorized (NumericCodec) as Hex '00 00 00 01 23 45'.
    /// </summary>
    public class Numeric
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
            Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

        private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
            Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

        #endregion

        #region Instance Members

        public static byte Byte() => Bytes(1)[0];

        public static ushort UShort()
        {
            Span<byte> buffer = Bytes(Specs.Integer.UInt16.ByteCount);

            return PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(buffer);
        }

        public static uint UInt(int maxByteCount)
        {
            Span<byte> buffer = Bytes(maxByteCount);

            return PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(buffer);
        }

        public static uint UInt()
        {
            Span<byte> buffer = Bytes(Specs.Integer.UInt32.ByteCount);

            return PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(buffer);
        }

        public static ulong ULong()
        {
            Span<byte> buffer = Bytes(Specs.Integer.UInt64.ByteCount);

            return PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(buffer);
        }

        /// <summary>
        ///     Returns a NumericCodec encoded byte array
        /// </summary>
        /// <param name="length">
        ///     The length of char pairs returned. NumericCodec char length must consist of a multiple of 2
        /// </param>
        /// <returns></returns>
        public static byte[] Bytes(int length)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < length; i++)
                buffer[j++] = GetRandomByte();

            return buffer.ToArray();
        }

        /// <summary>
        ///     Returns a NumericCodec encoded string
        /// </summary>
        /// <param name="length">
        ///     The length of char pairs returned. NumericCodec char length must consist of a multiple of 2
        /// </param>
        /// <returns></returns>
        public static string String(int length)
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length * 2);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < length; i++)
            {
                buffer[j++] = GetRandomChar();
                buffer[j++] = GetRandomChar();
            }

            string? a = new(buffer);

            return new string(buffer);
        }

        /// <summary>
        ///     Returns a NumericCodec encoded string
        /// </summary>
        /// <param name="leftPad">Specifies the amount of '0' characters that will prepend this result</param>
        /// <param name="length">
        ///     The length of char pairs returned. NumericCodec char length must consist of a multiple of 2
        /// </param>
        /// <returns></returns>
        public static string String(int leftPad, int length)
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length * 2);
            Span<char> buffer = spanOwner.Span;

            int offset = 0;
            for (; offset < leftPad; offset++)
                buffer[offset] = '0';

            for (int i = length - (offset / 2); i < length; offset++, i += offset % 2)
                buffer[offset] = GetRandomChar();

            return new string(buffer);
        }

        private static char GetRandomChar() => _CharMap[(byte) _Random.Next(0, 9)];

        private static byte GetRandomByte()
        {
            byte result = _ByteMap[GetRandomChar()];
            result *= 10;
            result += _ByteMap[GetRandomChar()];

            return result;
        }

        private static byte GetRandomNibble() => _ByteMap[GetRandomChar()];

        #endregion
    }
}