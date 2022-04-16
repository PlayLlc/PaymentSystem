using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.Ber;

public readonly record struct TrackPrimaryAccountNumber
{
    #region Static Metadata

    public const byte MaxNumberOfDigits = 19;
    public const byte MaxByteCount = 10;

    #endregion

    #region Instance Values

    private readonly Nibble[] _Value;

    #endregion

    #region Constructor

    public TrackPrimaryAccountNumber(ReadOnlySpan<Nibble> value)
    {
        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    public int GetCharCount() => _Value.Length;

    /// <exception cref="OverflowException"></exception>
    public bool IsCheckSumValid()
    {
        Span<Nibble> buffer = stackalloc Nibble[_Value.Length];
        _Value.CopyTo(buffer);

        for (int i = buffer.Length - 1, j = 0; i >= 0; i--, j++)
        {
            if ((j % 2) != 0)
                buffer[i] *= 2;
        }

        int sum = 0;

        for (int i = 0; i < buffer.Length; i++)
            sum += (buffer[i] % 10) + ((buffer[i] / 10) % 10);

        return (sum % 10) == 0;
    }

    /// <exception cref="OverflowException"></exception>
    public byte[] Encode() => _Value.AsByteArray();

    #endregion
}