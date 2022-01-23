using System.Collections.Immutable;

using Play.Core;

namespace Play.Encryption.Encryption.Ciphers.Symmetric;

/// <summary>
///     Specifies the block cipher mode to use for encryption
/// </summary>
public sealed record BlockCipherAlgorithm : EnumObject<byte>, IEqualityComparer<BlockCipherAlgorithm>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, BlockCipherAlgorithm> _ValueObjectMap;

    /// <summary>
    ///     Also known by it's original name Rijndael, is a symmetric block cipher
    ///     ==============================================================
    ///     Emv Specifications
    ///     ==============================================================
    ///     Valid Key Sizes: 128, 192, 256 bits
    ///     Valid Block Sizes: 16
    /// </summary>
    public static readonly BlockCipherAlgorithm Aes;

    /// <summary>
    ///     A symmetric block cipher which applies the DES cipher algorithm three times to each data block
    ///     ==============================================================
    ///     Emv Specifications
    ///     ==============================================================
    ///     Valid Key Sizes: 128
    ///     Valid Block Sizes: 8
    /// </summary>
    public static readonly BlockCipherAlgorithm TripleDes;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static BlockCipherAlgorithm()
    {
        const byte tripleDes = 1;
        const byte aes = 2;

        TripleDes = new BlockCipherAlgorithm(tripleDes);
        Aes = new BlockCipherAlgorithm(aes);

        _ValueObjectMap =
            new Dictionary<byte, BlockCipherAlgorithm> {{tripleDes, TripleDes}, {aes, Aes}}.ToImmutableSortedDictionary(a => a.Key,
             b => b.Value);
    }

    private BlockCipherAlgorithm(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool TryGet(byte value, out BlockCipherAlgorithm? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(BlockCipherAlgorithm? other) => other is not null && (_Value == other._Value);

    public bool Equals(BlockCipherAlgorithm? x, BlockCipherAlgorithm? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(BlockCipherAlgorithm other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(_Value.GetHashCode() * 31771);

    #endregion
}