using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.ValueTypes;

public readonly record struct TrackDiscretionaryData
{
    #region Static Metadata

    private const byte _MaxNibbleLength = 11;

    #endregion

    #region Instance Values

    private readonly Nibble[] _Value;

    #endregion

    #region Constructor

    /// <exception cref="TerminalDataException"></exception>
    public TrackDiscretionaryData(ReadOnlySpan<Nibble> value)
    {
        if (value.Length > _MaxNibbleLength)
        {
            throw new TerminalDataException(new ArgumentOutOfRangeException(
                $"The {nameof(TrackDiscretionaryData)} could not be initialized because the value was too large"));
        }

        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    /// <exception cref="OverflowException"></exception>
    public byte[] Encode() => _Value.AsByteArray();

    public int GetCharCount() => _Value.Length;

    /// <exception cref="OverflowException"></exception>
    public Nibble[] AsNibbleArray() => _Value.CopyValue();

    /// <exception cref="OverflowException"></exception>
    public int GetByteCount() => (_Value.Length / 2) + (_Value.Length % 2);

    #endregion
}