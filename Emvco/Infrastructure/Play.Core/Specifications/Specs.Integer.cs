using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Play.Core.Specifications;

public partial class Specs
{
    public static class Integer
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<char, byte> _CharMapper = new Dictionary<char, byte>
        {
            {'0', 0},
            {'1', 1},
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9}
        }.ToImmutableSortedDictionary();

        private static readonly ImmutableSortedDictionary<byte, char> _DigitMapper = new Dictionary<byte, char>
        {
            {0, '0'},
            {1, '1'},
            {2, '2'},
            {3, '3'},
            {4, '4'},
            {5, '5'},
            {6, '6'},
            {7, '7'},
            {8, '8'},
            {9, '9'}
        }.ToImmutableSortedDictionary();

        #endregion

        #region Instance Members

        public static byte CharToDecimalLiteral(char value)
        {
            if (value > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"The argument {nameof(value)} was out of the range of digit values. Please provide a digit from 0 - 9");
            }

            return _CharMapper[value];
        }

        public static char DecimalLiteralToChar(byte value)
        {
            if (value > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"The argument {nameof(value)} was out of the range of digit values. Please provide a digit from 0 - 9");
            }

            return _DigitMapper[value];
        }

        #endregion

        public static class UInt64
        {
            #region Static Metadata

            public const byte ByteCount = 8;
            public const byte MaxDigits = 20;
            public const byte BitCount = 64;
            public const byte CompressedNumericByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte HexadecimalByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte BitStringByteSize = (MaxDigits / 2) + (MaxDigits % 2);

            #endregion
        }

        public static class UInt32
        {
            #region Static Metadata

            public const byte BitSize = 32;
            public const byte ByteSize = 4;
            public const byte MaxDigits = 10;
            public const byte BitCount = 32;
            public const byte CompressedNumericByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte HexadecimalByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte BitStringByteSize = (MaxDigits / 2) + (MaxDigits % 2);

            #endregion
        }

        public static class UInt16
        {
            #region Static Metadata

            public const byte ByteSize = 2;
            public const byte MaxDigits = 5;
            public const byte BitCount = 16;
            public const byte CompressedNumericByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte HexadecimalByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte BitStringByteSize = (MaxDigits / 2) + (MaxDigits % 2);

            #endregion
        }

        public static class UInt8
        {
            #region Static Metadata

            public const byte ByteSize = 1;
            public const byte MaxDigits = 3;
            public const byte BitCount = 8;
            public const byte CompressedNumericByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte HexadecimalByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte BitStringByteSize = (MaxDigits / 2) + (MaxDigits % 2);

            #endregion
        }

        public static class Int64
        {
            #region Static Metadata

            public const byte ByteSize = 8;
            public const byte MaxDigits = 19;
            public const byte BitCount = 64;
            public const byte CompressedNumericByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte HexadecimalByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte BitStringByteSize = (MaxDigits / 2) + (MaxDigits % 2);

            #endregion
        }

        public static class Int32
        {
            #region Static Metadata

            public const byte ByteSize = 4;
            public const byte MaxDigits = 9;
            public const byte BitCount = 32;
            public const byte CompressedNumericByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte HexadecimalByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte BitStringByteSize = (MaxDigits / 2) + (MaxDigits % 2);

            #endregion
        }

        public static class Int16
        {
            #region Static Metadata

            public const byte ByteSize = 2;
            public const byte MaxDigits = 4;
            public const byte BitCount = 16;
            public const byte CompressedNumericByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte HexadecimalByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte BitStringByteSize = (MaxDigits / 2) + (MaxDigits % 2);

            #endregion
        }

        public static class Int8
        {
            #region Static Metadata

            public const byte ByteSize = 1;
            public const byte MaxDigits = 2;
            public const byte BitCount = 8;
            public const byte CompressedNumericByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte HexadecimalByteSize = (MaxDigits / 2) + (MaxDigits % 2);
            public const byte BitStringByteSize = (MaxDigits / 2) + (MaxDigits % 2);

            #endregion
        }
    }
}