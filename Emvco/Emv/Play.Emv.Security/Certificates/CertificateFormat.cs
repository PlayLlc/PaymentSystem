using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Security.Certificates;

public sealed record CertificateFormat : EnumObject<byte>, IEqualityComparer<CertificateFormat>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CertificateFormat> _ValueObjectMap;

    /// <value>Decimal: 04; Hexadecimal: 0x04</value>
    public static readonly CertificateFormat Icc;

    /// <value>Decimal: 02; Hexadecimal: 0x02</value>
    public static readonly CertificateFormat Issuer;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static CertificateFormat()
    {
        const byte issuer = 0x02;
        const byte icc = 0x04;

        Issuer = new CertificateFormat(issuer);
        Icc = new CertificateFormat(icc);

        _ValueObjectMap =
            new Dictionary<byte, CertificateFormat> {{issuer, Issuer}, {icc, Icc}}.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    private CertificateFormat(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int GetByteSize()
    {
        return _Value;
    }

    public static bool TryGet(byte value, out CertificateFormat? result)
    {
        return _ValueObjectMap.TryGetValue(value, out result);
    }

    #endregion

    #region Equality

    public bool Equals(CertificateFormat? other)
    {
        return other is not null && (_Value == other._Value);
    }

    public bool Equals(CertificateFormat? x, CertificateFormat? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CertificateFormat other)
    {
        return other.GetHashCode();
    }

    public override int GetHashCode()
    {
        return unchecked(_Value.GetHashCode() * 13879);
    }

    #endregion
}