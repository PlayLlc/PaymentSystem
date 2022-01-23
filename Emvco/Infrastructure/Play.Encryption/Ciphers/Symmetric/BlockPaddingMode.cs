using System.Collections.Immutable;
using System.Security.Cryptography;

using Play.Core;

namespace Play.Encryption.Encryption.Ciphers.Symmetric;

/// <summary>
///     Specifies the type of padding to add when the message block is shorter than the block size
/// </summary>
public sealed record BlockPaddingMode : EnumObject<byte>, IEqualityComparer<BlockPaddingMode>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, BlockPaddingMode> _ValueObjectMap;
    public static readonly BlockPaddingMode None;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static BlockPaddingMode()
    {
        const byte none = 1;
        const byte pkcs7 = 1;
        const byte zeros = 1;
        const byte ansix923 = 1;
        const byte iso10126 = 1;

        None = new BlockPaddingMode(none);

        //Pkcs7 = new BlockPaddingMode(pkcs7);
        //Zeros = new BlockPaddingMode(zeros);
        //Ansix923 = new BlockPaddingMode(ansix923);
        //Iso10126 = new BlockPaddingMode(iso10126);

        _ValueObjectMap = new Dictionary<byte, BlockPaddingMode>
        {
            {none, None}

            //{ pkcs7, Pkcs7 },
            //{ zeros, Zeros },
            //{ ansix923, Ansix923 },
            //{ iso10126, Iso10126 },
        }.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    private BlockPaddingMode(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public PaddingMode AsPaddingMode()
    {
        return (PaddingMode) _Value;
    }

    public static bool TryGet(byte value, out BlockPaddingMode? result)
    {
        return _ValueObjectMap.TryGetValue(value, out result);
    }

    #endregion

    #region Equality

    public bool Equals(BlockPaddingMode? other)
    {
        return other is not null && (_Value == other._Value);
    }

    public bool Equals(BlockPaddingMode? x, BlockPaddingMode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(BlockPaddingMode other)
    {
        return other.GetHashCode();
    }

    public override int GetHashCode()
    {
        return unchecked(_Value.GetHashCode() * 31771);
    }

    #endregion
}