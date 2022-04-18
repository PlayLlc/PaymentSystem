using Play.Core;
using Play.Core.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Ber;

/// <summary>
///     A short date 'YYMM' value that represents the end of a validity period
/// </summary>
public readonly record struct TrackExpirationDate
{
    #region Instance Values

    private readonly ShortDate _Value;

    #endregion

    #region Constructor

    /// <exception cref="PlayInternalException"></exception>
    public TrackExpirationDate(ReadOnlySpan<Nibble> value)
    {
        _Value = new ShortDate(value);
    }

    #endregion

    #region Instance Members

    public int GetCharCount() => 4;
    public bool IsExpired() => ShortDate.Today > _Value;
    internal ReadOnlySpan<Nibble> AsNibbleArray() => _Value.AsNibbleArray();

    #endregion
}