using System;

using Play.Core.Extensions;

namespace Play.Core;

internal readonly record struct LeftNibble
{
    #region Static Metadata

    private const byte _UnrelatedBits = 0x0F;
    public const byte _MaxValue = 0xF0;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public LeftNibble(byte value)
    {
        if (value.AreAnyBitsSet(_UnrelatedBits))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(Nibble)} could not be initialized because the argument provided has more bits set than a nibble");
        }

        _Value = value;
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public LeftNibble(Nibble value)
    {
        _Value = (byte) (value << 4);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(byte left, LeftNibble right) => left.GetMaskedValue(_UnrelatedBits) == right._Value;
    public static bool operator !=(byte left, LeftNibble right) => left.GetMaskedValue(_UnrelatedBits) != right._Value;
    public static bool operator <(byte left, LeftNibble right) => left.GetMaskedValue(_UnrelatedBits) < right._Value;
    public static bool operator >(byte left, LeftNibble right) => left.GetMaskedValue(_UnrelatedBits) > right._Value;
    public static bool operator <=(byte left, LeftNibble right) => left.GetMaskedValue(_UnrelatedBits) <= right._Value;
    public static bool operator >=(byte left, LeftNibble right) => left.GetMaskedValue(_UnrelatedBits) >= right._Value;
    public static bool operator ==(LeftNibble left, byte right) => left._Value == right.GetMaskedValue(_UnrelatedBits);
    public static bool operator !=(LeftNibble left, byte right) => left._Value != right.GetMaskedValue(_UnrelatedBits);
    public static bool operator <(LeftNibble left, byte right) => left._Value < right.GetMaskedValue(_UnrelatedBits);
    public static bool operator >(LeftNibble left, byte right) => left._Value > right.GetMaskedValue(_UnrelatedBits);
    public static bool operator <=(LeftNibble left, byte right) => left._Value <= right.GetMaskedValue(_UnrelatedBits);
    public static bool operator >=(LeftNibble left, byte right) => left._Value >= right.GetMaskedValue(_UnrelatedBits);

    #endregion
}