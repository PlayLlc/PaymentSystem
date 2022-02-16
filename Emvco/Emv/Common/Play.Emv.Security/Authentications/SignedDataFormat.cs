using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Security.Authentications;

public sealed record SignedDataFormat : EnumObject<byte>, IEqualityComparer<SignedDataFormat>
{
    #region Static Metadata

    /// <summary>
    ///     Specifies that the signed data is signed using dynamic application data
    /// </summary>
    public static readonly SignedDataFormat _3;

    /// <summary>
    ///     Specifies that the signed data is signed using dynamic application data
    /// </summary>
    public static readonly SignedDataFormat _5;

    private static readonly ImmutableSortedDictionary<byte, SignedDataFormat> _ValueObjectMap;
    public static readonly SignedDataFormat NotAvailable;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static SignedDataFormat()
    {
        const byte notAvailable = 0;
        const byte __3 = 3;
        const byte __5 = 5;

        NotAvailable = new SignedDataFormat(notAvailable);
        _3 = new SignedDataFormat(__3);
        _5 = new SignedDataFormat(__5);

        _ValueObjectMap =
            new Dictionary<byte, SignedDataFormat> {{notAvailable, NotAvailable}, {__3, _3}, {__5, _5}}.ToImmutableSortedDictionary(
                a => a.Key, b => b.Value);
    }

    private SignedDataFormat(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int GetByteSize() => _Value;
    public static bool TryGet(byte value, out SignedDataFormat? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(SignedDataFormat? other) => other is not null && (_Value == other._Value);

    public bool Equals(SignedDataFormat? x, SignedDataFormat? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(SignedDataFormat other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(_Value.GetHashCode() * 31153);

    #endregion
}