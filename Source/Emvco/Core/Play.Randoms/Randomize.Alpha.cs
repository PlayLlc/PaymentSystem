using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Randoms;

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
            if (length < Specs.ByteArray.StackAllocateCeiling)
            {
                Span<byte> buffer = stackalloc byte[length];

                for (short i = 0; i <= (length - 1); i++)
                    buffer[i] = GetRandomByte();

                return buffer.ToArray();
            }
            else
            {
                using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
                Span<byte> buffer = stackalloc byte[length];

                for (short i = 0; i <= (length - 1); i++)
                    buffer[i] = GetRandomByte();

                return buffer.ToArray();
            }
        }

        public static char Char() => GetRandomChar();

        public static char[] Chars(int length)
        {
            if (length < Specs.ByteArray.StackAllocateCeiling)
            {
                Span<char> buffer = stackalloc char[length];

                for (short i = 0; i <= (length - 1); i++)
                    buffer[i] = GetRandomChar();

                return buffer.ToArray();
            }
            else
            {
                using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
                Span<char> buffer = stackalloc char[length];

                for (short i = 0; i <= (length - 1); i++)
                    buffer[i] = GetRandomChar();

                return buffer.ToArray();
            }
        }

        public static string String(int length) => new(Chars(length));
        private static char GetRandomChar() => _AlphabeticValues[_Random.Next(0, _AlphabeticValues.Length - 1)];
        public static byte GetRandomByte() => (byte) _AlphabeticValues[_Random.Next(0, _AlphabeticValues.Length - 1)];

        #endregion
    }
}