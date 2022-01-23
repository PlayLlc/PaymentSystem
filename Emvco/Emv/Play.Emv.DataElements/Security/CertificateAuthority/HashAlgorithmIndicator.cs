using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Play.Emv.DataElements.CertificateAuthority;

public class HashAlgorithmIndicator : IEqualityComparer<HashAlgorithmIndicator>, IEquatable<HashAlgorithmIndicator>,
    IComparable<HashAlgorithmIndicator>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, HashAlgorithmIndicator> _ValueObjectMap;
    public static readonly HashAlgorithmIndicator NotAvailable;

    /// <value>Decimal: 0; Hexadecimal; 0x01</value>
    /// >
    /// <remarks>Book 2 Section B2.1</remarks>
    public static readonly HashAlgorithmIndicator Sha1;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static HashAlgorithmIndicator()
    {
        const byte notAvailable = 0x00;
        const byte sha1 = 0x01;

        NotAvailable = new HashAlgorithmIndicator(notAvailable);
        Sha1 = new HashAlgorithmIndicator(sha1);
        _ValueObjectMap = new Dictionary<byte, HashAlgorithmIndicator> {{notAvailable, NotAvailable}, {sha1, Sha1}}
            .ToImmutableSortedDictionary();
    }

    private HashAlgorithmIndicator(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public int CompareTo([AllowNull] HashAlgorithmIndicator other) => _Value.CompareTo(other._Value);

    public static HashAlgorithmIndicator Get(byte value)
    {
        if (!_ValueObjectMap.TryGetValue(value, out HashAlgorithmIndicator result))
            return NotAvailable;

        return result!;
    }

    public static bool IsValid(byte value) => _ValueObjectMap.ContainsKey(value);
    public static bool TryGet(byte value, out HashAlgorithmIndicator? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals([AllowNull] HashAlgorithmIndicator other)
    {
        if (other is null)
            return false;

        return _Value == other._Value;
    }

    public bool Equals([AllowNull] HashAlgorithmIndicator x, [AllowNull] HashAlgorithmIndicator y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals([AllowNull] object obj) =>
        obj is HashAlgorithmIndicator hashAlgorithmIndicator && Equals(hashAlgorithmIndicator);

    public int GetHashCode(HashAlgorithmIndicator obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(297581 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static bool operator ==(HashAlgorithmIndicator left, HashAlgorithmIndicator right) => left._Value == right._Value;
    public static explicit operator byte(HashAlgorithmIndicator value) => value._Value;
    public static bool operator >(HashAlgorithmIndicator left, HashAlgorithmIndicator right) => left._Value > right._Value;
    public static bool operator >=(HashAlgorithmIndicator left, HashAlgorithmIndicator right) => left._Value >= right._Value;
    public static bool operator !=(HashAlgorithmIndicator left, HashAlgorithmIndicator right) => !(left == right);
    public static bool operator <(HashAlgorithmIndicator left, HashAlgorithmIndicator right) => left._Value < right._Value;
    public static bool operator <=(HashAlgorithmIndicator left, HashAlgorithmIndicator right) => left._Value <= right._Value;

    #endregion
}