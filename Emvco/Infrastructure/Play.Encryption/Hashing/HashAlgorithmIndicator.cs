using Play.Core;

namespace Play.Encryption.Hashing;

public record HashAlgorithmIndicator : EnumObject<byte>
{
    #region Static Metadata

    private static readonly Dictionary<byte, HashAlgorithmIndicator> _ValueObjectMap;
    public static readonly HashAlgorithmIndicator NotAvailable = new(0x00);

    /// <value>Decimal: 0; Hexadecimal; 0x01</value>
    /// <remarks>Book 2 Section B2.1</remarks>
    public static readonly HashAlgorithmIndicator Sha1 = new(0x01);

    #endregion

    #region Constructor

    static HashAlgorithmIndicator()
    {
        _ValueObjectMap = new Dictionary<byte, HashAlgorithmIndicator> {{NotAvailable, NotAvailable}, {Sha1, Sha1}};
    }

    private HashAlgorithmIndicator(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo(HashAlgorithmIndicator? other) => _Value.CompareTo(other._Value);

    public static HashAlgorithmIndicator Get(byte value)
    {
        if (!_ValueObjectMap.TryGetValue(value, out HashAlgorithmIndicator? result))
            return NotAvailable;

        return result!;
    }

    public static bool IsValid(byte value) => _ValueObjectMap.ContainsKey(value);
    public static bool TryGet(byte value, out HashAlgorithmIndicator? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion
}