using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Emv.Ber.ValueTypes.Card;

internal readonly record struct PciiValue
{
    #region Static Metadata

    private const uint _UnrelatedBits = 0xFF000000;

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    public PciiValue(uint value)
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

    public static explicit operator uint(PciiValue value) => value._Value;
    public static bool operator ==(PciiValue left, uint right) => left._Value == right;
    public static bool operator !=(PciiValue left, uint right) => left._Value != right;
    public static bool operator ==(uint left, PciiValue right) => left == right._Value;
    public static bool operator !=(uint left, PciiValue right) => left != right._Value;

    #endregion
}