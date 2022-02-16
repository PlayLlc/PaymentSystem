using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.Security.Cryptograms;

public record ApplicationCryptogramVersion : EnumObject<byte>, IEqualityComparer<ApplicationCryptogramVersion>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 5; HexValue: 0x05</remarks>
    public static readonly ApplicationCryptogramVersion _5;

    /// <remarks>DecimalValue: 6; HexValue: 0x06</remarks>
    public static readonly ApplicationCryptogramVersion _6;

    private static readonly ImmutableSortedDictionary<byte, ApplicationCryptogramVersion> _ValueObjectMap;
    internal const byte UnrelatedBits = (byte) (Bits.Six | Bits.Five | Bits.Four | Bits.Three | Bits.Two | Bits.One);

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static ApplicationCryptogramVersion()
    {
        const byte __5 = 5;
        const byte __6 = 6;
        _5 = new ApplicationCryptogramVersion(__5);
        _6 = new ApplicationCryptogramVersion(__6);

        _ValueObjectMap = new Dictionary<byte, ApplicationCryptogramVersion> {{__5, _5}, {__6, _6}}.ToImmutableSortedDictionary();
    }

    private ApplicationCryptogramVersion(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool TryGet(byte value, out ApplicationCryptogramVersion result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(ApplicationCryptogramVersion? x, ApplicationCryptogramVersion? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationCryptogramVersion other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 33911;

        return hash + (_Value.GetHashCode() * 3);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(ApplicationCryptogramVersion left, byte right)
    {
        if (left is null)
            return false;

        return left._Value == right;
    }

    public static bool operator ==(byte left, ApplicationCryptogramVersion right)
    {
        if (right is null)
            return false;

        return right._Value == left;
    }

    public static explicit operator byte(ApplicationCryptogramVersion applicationCryptogramVersion) => applicationCryptogramVersion._Value;

    public static explicit operator ApplicationCryptogramVersion(byte cryptogramVersion)
    {
        if (!TryGet(cryptogramVersion, out ApplicationCryptogramVersion result))
        {
            throw new ArgumentOutOfRangeException(nameof(cryptogramVersion),
                $"The {nameof(ApplicationCryptogramVersion)} could not be found from the number supplied to the argument: {cryptogramVersion}");
        }

        return result;
    }

    public static bool operator !=(ApplicationCryptogramVersion left, byte right) => !(left == right);
    public static bool operator !=(byte left, ApplicationCryptogramVersion right) => !(left == right);

    #endregion
}