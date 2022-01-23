using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Tests.Core.Random
{
    public partial class Randomize
    {
        public class Array
        {
            #region Instance Members

            public static byte[] Bytes(int length)
            {
                if (length < Specs.ByteArray.StackAllocateCeiling)
                {
                    Span<byte> buffer = stackalloc byte[length];

                    for (int i = 0; i < length; i++)
                        buffer[i] = Numeric.Byte();

                    return buffer.ToArray();
                }
                else
                {
                    SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
                    Span<byte> buffer = spanOwner.Span;

                    for (int i = 0; i < length; i++)
                        buffer[i] = Numeric.Byte();

                    return buffer.ToArray();
                }
            }

            #endregion
        }
    }
}