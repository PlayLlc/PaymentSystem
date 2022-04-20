using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Emv.Ber.ValueTypes.Card;

internal readonly record struct PciiMask
{
    #region Static Metadata

    private const uint _UnrelatedBits = 0xFF000000;

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    /// <exception cref="PlayInternalException"></exception>
    public PciiMask(uint value)
    {
        if (value.AreAnyBitsSet(_UnrelatedBits))
            throw new PlayInternalException($"The {nameof(PciiMask)} must not exceed 3 bytes in length");

        _Value = value.ClearBits(_UnrelatedBits);
    }

    #endregion

    #region Serialization

    public void Decode(Span<byte> buffer, ref int offset)
    {
        buffer[offset++] = (byte) (_Value >> 16);
        buffer[offset++] = (byte) (_Value >> 8);
        buffer[offset++] = (byte) _Value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator uint(PciiMask value) => value._Value;

    #endregion
}