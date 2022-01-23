using System;

namespace Play.Icc.FileSystem.ElementaryFiles;

/// <summary>
///     Value Type representing the short file identifier of an Elementary File in the ICC
/// </summary>
public readonly struct ShortFileId
{
    #region Static Metadata

    private const byte _MaxValue = 30;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public ShortFileId(byte value)
    {
        if (value > _MaxValue)
        {
            throw new ArgumentOutOfRangeException(
                $"The argument {nameof(value)} was out of range. ShortFileIdentifier (SFI) can not be more than five bits in length");
        }

        _Value = value;
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is ShortFileId shortFileId && Equals(shortFileId);
    public bool Equals(ShortFileId other) => _Value == other._Value;
    public bool Equals(ShortFileId x, ShortFileId y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 10544431;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(ShortFileId left, ShortFileId right) => left._Value == right._Value;
    public static implicit operator byte(ShortFileId value) => value._Value;
    public static bool operator !=(ShortFileId left, ShortFileId right) => !(left == right);

    #endregion
}