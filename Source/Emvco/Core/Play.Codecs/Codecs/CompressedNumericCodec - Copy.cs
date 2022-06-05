using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using System;

using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Core.Extensions.Types;
using Play.Core.Specifications;

namespace Play.Codecs;

public partial class CompressedNumericCodec : PlayCodec
{
    #region Encode

    public byte[] Encode(ushort value)
    {
        byte[] buffer = new byte[Specs.Integer.UInt16.ByteCount];
        int mostSignificantByte = (value.GetNumberOfDigits() / 2) + (value.GetNumberOfDigits() % 2);

        if (mostSignificantByte > Specs.Integer.UInt16.ByteCount)
            return Encode((uint) value);

        int padCount = (buffer.Length * 2) - value.GetNumberOfDigits();

        for (int i = mostSignificantByte - 1, j = padCount; j < (Specs.Integer.UInt16.ByteCount * 2); i -= j % 2, j++)
        {
            if ((j % 2) == 0)
            {
                buffer[i] += (byte) (value % 10);
                value /= 10;
            }
            else
            {
                buffer[i] += (byte) ((value % 10) << 4);
                value /= 10;
            }
        }

        PaddShit(buffer, padCount, mostSignificantByte);

        return buffer;
    }

    #endregion

    #region Instance Members

    private void PaddShit(Span<byte> buffer, int padCount, int mostSignificantByte)
    {
        if (padCount == 0)
            return;

        if ((padCount % 2) != 0)
            buffer[mostSignificantByte - 1] += 0x0F;

        if (padCount == 1)
            return;

        buffer[mostSignificantByte..].Fill(0xFF);
    }

    public byte[] Hello(ushort value)
    {
        const byte byteSize = Specs.Integer.UInt16.ByteCount;
        int mostSignificantByte = (value.GetNumberOfDigits() / 2) + (value.GetNumberOfDigits() % 2);
        int padCount = (byteSize * 2) - value.GetNumberOfDigits();

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = mostSignificantByte - 1, j = padCount; j < (Specs.Integer.UInt16.ByteCount * 2); i -= j % 2, j++)
        {
            if ((j % 2) == 0)
            {
                buffer[i] += (byte) (value % 10);
                value /= 10;
            }
            else
            {
                buffer[i] += (byte) ((value % 10) << 4);
                value /= 10;
            }
        }

        if (padCount == 0)
            return buffer.ToArray();

        buffer[mostSignificantByte..].Fill(0xFF);

        if ((padCount % 2) != 0)
            buffer[mostSignificantByte - 1] += 0x0F;

        return buffer.ToArray();
    }

    #endregion
}