using System.Collections.Immutable;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Core;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Randoms;

public partial class Randomize
{
    /// <summary>
    ///     CompressedNumeric data elements consist of two numeric digits (having values in the range Hex '0' – '9') per byte.
    ///     These digits are left justified and padded with trailing hexadecimal 'F'. Other specifications sometimes refer to
    ///     this data format as BinaryCodec Coded Decimal (“BCD”) or unsigned packed. Example: Amount, Authorized(NumericCodec)
    ///     is defined as “n 12” with a length of six bytes. A value of 12345 is stored in Amount, Authorized (NumericCodec) as
    ///     Hex '12 34 5F FF FF FF FF FF'.
    /// </summary>
    public class CompressedNumeric
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
            Enumerable.Range(0, 9).ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

        private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
            Enumerable.Range(0, 9).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

        private static readonly Nibble _PaddedNibble = 0xF;
        private const char _PaddingChar = 'F';

        #endregion

        #region Instance Members

        public static byte Byte() => Bytes(Specs.Integer.UInt8.ByteCount)[0];

        public static ushort UShort()
        {
            Span<byte> buffer = Bytes(Specs.Integer.UInt16.ByteCount);

            return PlayCodec.CompressedNumericCodec.DecodeToUInt16(buffer);
        }

        public static uint UInt()
        {
            Span<byte> buffer = Bytes(Specs.Integer.UInt32.ByteCount);

            return PlayCodec.CompressedNumericCodec.DecodeToUInt16(buffer);
        }

        public static ulong ULong()
        {
            Span<byte> buffer = Bytes(Specs.Integer.UInt64.ByteCount);

            return PlayCodec.CompressedNumericCodec.DecodeToUInt16(buffer);
        }

        /// <summary>
        ///     Returns a CompressedNumeric encoded byte array
        /// </summary>
        /// <param name="length">
        ///     The length of bytes returned
        /// </param>
        /// <returns></returns>
        /// <exception cref="OverflowException"></exception>
        public static byte[] Bytes(int length)
        {
            if (length == 0)
                return new byte[] { };

            int nibbleCount = length * 2;
            using SpanOwner<Nibble> spanOwner = SpanOwner<Nibble>.Allocate(nibbleCount);
            Span<Nibble> buffer = spanOwner.Span;

            int padCount = _Random.Next(1, nibbleCount);

            for (int i = 0; i < (nibbleCount - padCount); i++)
                buffer[i] = GetRandomNibble();

            for (int i = nibbleCount - padCount; i < nibbleCount; i++)
                buffer[i] = _PaddedNibble;

            if (buffer[0] == 0)
                buffer[0] = (byte) _Random.Next(1, 9);

            return buffer.AsByteArray().ToArray();
        }

        /// <summary>
        ///     Returns a CompressedNumeric encoded string
        /// </summary>
        /// <param name="length">
        ///     The length of chars returned
        /// </param>
        /// <returns></returns>
        public static string String(int length)
        {
            if (length == 0)
                return string.Empty;

            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            int padCount = _Random.Next(1, length);

            for (int i = 0; i < padCount; i++)
                buffer[i] = GetRandomChar();

            for (int i = padCount; i < length; i++)
                buffer[i] = _PaddingChar;

            return buffer.ToString();
        }

        /// <summary>
        ///     Returns a NumericCodec encoded string
        /// </summary>
        /// <param name="length">
        ///     The length of chars returned
        /// </param>
        /// <param name="padding">Specifies the amount of 'F' characters that will append this result</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string String(int length, int padding)
        {
            if (padding > length)
                throw new ArgumentOutOfRangeException(nameof(padding), $"The {nameof(padding)} argument must be less than the {nameof(length)} argument");

            if (length == 0)
                return string.Empty;

            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length * 2);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0; i < padding; i++)
                buffer[i] = GetRandomChar();

            for (int i = length - padding; i < length; i++)
                buffer[i] = _PaddingChar;

            return buffer.ToString();
        }

        private static char GetRandomChar() => _CharMap[(byte) _Random.Next(0, _CharMap.Count)];
        private static byte GetRandomNibble() => _ByteMap[GetRandomChar()];

        #endregion
    }
}