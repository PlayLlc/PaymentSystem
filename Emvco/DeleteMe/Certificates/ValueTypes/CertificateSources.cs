using System.Collections.Immutable;

using Play.Core;

namespace DeleteMe.Certificates;

internal sealed record CertificateSources : EnumObject<byte>, IEqualityComparer<CertificateSources>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CertificateSources> _ValueObjectMap;

    /// <value>Decimal: 0; Hexadecimal: 0x00</value>
    public static readonly CertificateSources CertificateAuthority;

    /// <value>Decimal: 04; Hexadecimal: 0x04</value>
    public static readonly CertificateSources Icc;

    /// <value>Decimal: 02; Hexadecimal: 0x02</value>
    public static readonly CertificateSources Issuer;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static CertificateSources()
    {
        CertificateAuthority = new CertificateSources(0);
        Issuer = new CertificateSources(0x02);
        Icc = new CertificateSources(0x04);

        _ValueObjectMap =
            new Dictionary<byte, CertificateSources> {{Issuer, Issuer}, {Icc, Icc}, {CertificateAuthority, CertificateAuthority}}
                .ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    private CertificateSources(byte value) : base(value)
    { }

    #endregion

    #region Equality

    public bool Equals(CertificateSources? other) => other is not null && (_Value == other._Value);

    public bool Equals(CertificateSources? x, CertificateSources? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CertificateSources other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(_Value.GetHashCode() * 13879);

    #endregion

    #region Instance Members

    public int GetByteSize() => _Value;
    public static bool TryGet(byte value, out CertificateSources? result) => _ValueObjectMap.TryGetValue(value, out result);

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static CertificateSources Get(byte value)
    {
        if (!_ValueObjectMap.TryGetValue(value, out CertificateSources? result))
            throw new ArgumentOutOfRangeException(nameof(value));

        return result!;
    }

    #endregion
}