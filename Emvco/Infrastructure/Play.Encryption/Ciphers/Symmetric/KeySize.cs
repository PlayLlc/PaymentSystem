using System.Collections.Immutable;

using Play.Core;

namespace Play.Encryption.Ciphers.Symmetric;

/// <summary>
///     A Block Cipher algorithm key size in bits
/// </summary>
public sealed record KeySize : EnumObject<ushort>, IEqualityComparer<KeySize>
{
    #region Static Metadata

    public static readonly KeySize _128;
    public static readonly KeySize _192;
    public static readonly KeySize _256;
    public static readonly KeySize _64;
    private static readonly ImmutableSortedDictionary<ushort, KeySize> _ValueObjectMap;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static KeySize()
    {
        const ushort __64 = 64;
        const ushort __128 = 128;
        const ushort __192 = 192;
        const ushort __256 = 256;

        _64 = new KeySize(__64);
        _128 = new KeySize(__128);
        _192 = new KeySize(__192);
        _256 = new KeySize(__256);

        _ValueObjectMap =
            new Dictionary<ushort, KeySize> {{__64, _64}, {__128, _128}, {__192, _192}, {__256, _256}}.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    private KeySize(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static KeySize[] GetAll() => _ValueObjectMap.Values.ToArray();
    public int GetBitSize() => _Value;
    public int GetByteSize() => _Value / 8;
    public static bool TryGet(ushort value, out KeySize? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(KeySize? other) => other is not null && (_Value == other._Value);

    public bool Equals(KeySize? x, KeySize? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(KeySize other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(_Value.GetHashCode() * 31153);

    #endregion
}