using Play.Core;

namespace Play.Encryption.Ciphers.Hashing;

public record HashAlgorithmIndicators : EnumObject<byte>
{
    #region Static Metadata

    public static readonly HashAlgorithmIndicators Empty = new();
    private static readonly Dictionary<byte, HashAlgorithmIndicators> _ValueObjectMap;
    public static readonly HashAlgorithmIndicators NotAvailable = new(0x00);

    /// <value>Decimal: 0; HexadecimalCodec; 0x01</value>
    /// <remarks>Book 2 Section B2.1</remarks>
    public static readonly HashAlgorithmIndicators Sha1 = new(0x01);

    #endregion

    #region Constructor

    public HashAlgorithmIndicators()
    { }

    static HashAlgorithmIndicators()
    {
        _ValueObjectMap = new Dictionary<byte, HashAlgorithmIndicators> {{NotAvailable, NotAvailable}, {Sha1, Sha1}};
    }

    private HashAlgorithmIndicators(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override HashAlgorithmIndicators[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out HashAlgorithmIndicators? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static bool IsValid(byte value) => _ValueObjectMap.ContainsKey(value);

    #endregion

    #region Equality

    public int CompareTo(HashAlgorithmIndicators? other) => _Value.CompareTo(other._Value);

    #endregion
}