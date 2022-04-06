using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Emv.Ber;

internal readonly record struct PciiMask
{
    private readonly uint _Value;
    private const uint _UnrelatedBits = 0xFF000000;

    /// <exception cref="PlayInternalException"></exception>
    public PciiMask(uint value)
    {
        if (value.AreAnyBitsSet(_UnrelatedBits))
            throw new PlayInternalException($"The {nameof(PciiMask)} must not exceed 3 bytes in length");

        _Value = value.ClearBits(_UnrelatedBits);
    }

    public void Decode(Span<byte> buffer, ref int offset)
    {
        buffer[offset++] = (byte) (_Value >> 16);
        buffer[offset++] = (byte) (_Value >> 8);
        buffer[offset++] = (byte) _Value;
    }

    public static explicit operator uint(PciiMask value) => value._Value;
}