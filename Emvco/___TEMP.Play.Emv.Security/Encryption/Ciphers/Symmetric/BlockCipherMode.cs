using System.Collections.Immutable;
using System.Security.Cryptography;

using Play.Core;

namespace ___TEMP.Play.Emv.Security.Encryption.Ciphers.Symmetric;

/// <summary>
///     Specifies the block cipher mode to use for encryption
/// </summary>
public sealed record BlockCipherMode : EnumObject<byte>, IEqualityComparer<BlockCipherMode>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, BlockCipherMode> _ValueObjectMap;

    /// <summary>
    ///     'Cipher Block Chaining' mode encrypts each block is OR'd to the previous block before encrypting
    ///     which requires successfully decrypting each subsequent block to correctly decipher the next
    /// </summary>
    /// <value>Decimal: 1; Hexadecimal: 0x01</value>
    public static readonly BlockCipherMode Cbc;

    /// <summary>
    ///     'Electronic Codebook' mode enciphers each block individually without chaining
    /// </summary>
    /// <value>Decimal: 2; Hexadecimal: 0x02</value>
    public static readonly BlockCipherMode Ecb;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static BlockCipherMode()
    {
        const byte cbc = 1;
        const byte ecb = 2;

        Cbc = new BlockCipherMode(cbc);
        Ecb = new BlockCipherMode(ecb);

        _ValueObjectMap =
            new Dictionary<byte, BlockCipherMode> {{cbc, Cbc}, {ecb, Ecb}}.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    private BlockCipherMode(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public CipherMode AsCipherMode()
    {
        return (CipherMode) _Value;
    }

    public static bool TryGet(byte value, out BlockCipherMode? result)
    {
        return _ValueObjectMap.TryGetValue(value, out result);
    }

    #endregion

    #region Equality

    public bool Equals(BlockCipherMode? other)
    {
        return other is not null && (_Value == other._Value);
    }

    public bool Equals(BlockCipherMode? x, BlockCipherMode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(BlockCipherMode other)
    {
        return other.GetHashCode();
    }

    public override int GetHashCode()
    {
        return unchecked(_Value.GetHashCode() * 31771);
    }

    #endregion
}