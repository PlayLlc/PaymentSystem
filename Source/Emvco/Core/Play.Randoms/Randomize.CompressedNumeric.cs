using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Core;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Randoms
{
    public partial class Randomize
    {
        /// <summary>
        ///     NumericCodec data elements consist of two numeric digits (having values in the range Hex '0' – '9') per byte.
        ///     These digits are right justified and padded with leading hexadecimal zeroes. Other specifications sometimes
        ///     refer to this data format as BinaryCodec Coded Decimal (“BCD”) or unsigned packed.
        ///     Example: Amount, Authorized(NumericCodec) is defined as “n 12” with a length of six bytes.
        ///     A value of 12345 is stored in Amount, Authorized (NumericCodec) as Hex '00 00 00 01 23 45'.
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

            public static byte Byte() => Numeric.Byte();
            public static ushort UShort() => Numeric.UShort();
            public static uint UInt() => Numeric.UInt();
            public static ulong ULong() => Numeric.ULong();

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

                using SpanOwner<Nibble> spanOwner = SpanOwner<Nibble>.Allocate(length);
                Span<Nibble> buffer = spanOwner.Span;

                int padCount = _Random.Next(1, length);

                for (int i = 0; i < padCount; i++)
                    buffer[i] = GetRandomNibble();

                for (int i = length - padCount; i < length; i++)
                    buffer[i] = _PaddedNibble;

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
}