using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Security.Encryption.Ciphers;

/// <summary>
///     The block size used by a block cipher
/// </summary>
public sealed record BlockSize : EnumObject<byte>, IEqualityComparer<BlockSize>
{
    #region Static Metadata

    public static readonly BlockSize _16;
    public static readonly BlockSize _8;
    private static readonly ImmutableSortedDictionary<byte, BlockSize> _ValueObjectMap;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static BlockSize()
    {
        const byte __8 = 8;
        const byte __16 = 16;

        _8 = new BlockSize(__8);
        _16 = new BlockSize(__16);

        _ValueObjectMap = new Dictionary<byte, BlockSize> {{__8, _8}, {__16, _16}}.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    private BlockSize(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int GetByteSize() => _Value;
    public static bool TryGet(byte value, out BlockSize? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(BlockSize? other) => other is not null && (_Value == other._Value);

    public bool Equals(BlockSize? x, BlockSize? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(BlockSize other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(_Value.GetHashCode() * 31153);

    #endregion
}