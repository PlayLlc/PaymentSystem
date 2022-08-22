using System.Collections.Immutable;

using Play.Core;

namespace Play.Encryption.Ciphers.Symmetric;

/// <summary>
///     The block size used by a block cipher
/// </summary>
public sealed record BlockSize : EnumObject<byte>, IEqualityComparer<byte>
{
    #region Static Metadata

    public static readonly BlockSize Empty = new();
    public static readonly BlockSize _16;
    public static readonly BlockSize _8;
    private static readonly ImmutableSortedDictionary<byte, BlockSize> _ValueObjectMap;

    #endregion

    #region Constructor

    public BlockSize()
    { }

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

    public byte GetBlockSize() => (byte)(_Value * 8);

    public override BlockSize[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out BlockSize? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public int GetBlockSize() => _Value * 8;
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