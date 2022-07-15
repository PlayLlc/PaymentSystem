using Play.Ber.Exceptions;
using Play.Codecs;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Ber.ValueTypes;

public readonly struct Track2
{
    #region Static Metadata

    private const byte _MaxNibbleCount = 38;
    private const byte _StartSentinel1 = 0xB;
    private const byte _StartSentinel2 = (byte) ';';
    private const byte _FieldSeparator1 = 0xD;
    private const byte _FieldSeparator2 = (byte) '=';
    private const byte _EndSentinel1 = 0xF;
    private const byte _EndSentinel2 = (byte) '?';

    #endregion

    #region Instance Values

    private readonly Nibble[] _Value;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    public Track2(ReadOnlySpan<Nibble> value)
    {
        if (value.Length > _MaxNibbleCount)
        {
            throw new DataElementParsingException(
                $"The {nameof(Track2)} could not be initialized because the char count was out of range. The char count provided was: [{value.Length}] but must be no greater than {_MaxNibbleCount}");
        }

        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    #region Serialize

    public byte[] Encode() => _Value.AsByteArray();

    public int GetByteCount() => _Value.AsByteArray().Length;

    #endregion

    #endregion

    #region Primary Account Number

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public TrackPrimaryAccountNumber GetPrimaryAccountNumber()
    {
        if ((_Value[0] == _StartSentinel1) || (_Value[0] == _StartSentinel2))
            return new TrackPrimaryAccountNumber(_Value[1..GetPrimaryAccountNumberNibbleOffset(_Value)]);

        return new TrackPrimaryAccountNumber(_Value[..GetPrimaryAccountNumberNibbleOffset(_Value)]);
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private int GetPrimaryAccountNumberNibbleOffset(ReadOnlySpan<Nibble> value)
    {
        for (int i = 0; i < TrackPrimaryAccountNumber.MaxNumberOfDigits; i++)
        {
            if ((value[i] == _FieldSeparator1) || (value[i] == _FieldSeparator2))
                return i;
        }

        throw new DataElementParsingException($"A valid {nameof(ApplicationPan)} could not be resolved from the {nameof(Track2EquivalentData)}");
    }

    #endregion

    #region Expiration Date

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private int GetExpirationDateNibbleOffset(ReadOnlySpan<Nibble> buffer) => GetPrimaryAccountNumberNibbleOffset(buffer) + 1;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public ShortDate GetExpirationDate() =>
        new(PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(_Value[..GetExpirationDateNibbleOffset(_Value)].AsByteArray().AsSpan()));

    #endregion

    #region Service Code

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private int GetServiceCodeNibbleOffset(ReadOnlySpan<Nibble> buffer) => GetExpirationDateNibbleOffset(buffer) + 4;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public ServiceCode GetServiceCode() =>
        new(PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(_Value[..GetServiceCodeNibbleOffset(_Value)].AsByteArray().AsSpan()));

    #endregion

    #region Discretionary Data

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private int GetDiscretionaryDataNibbleOffset(ReadOnlySpan<Nibble> buffer) => GetServiceCodeNibbleOffset(buffer) + 3;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public TrackDiscretionaryData GetDiscretionaryData()
    {
        if ((_Value[^1] == _EndSentinel1) || (_Value[^1] == _EndSentinel2))
            return new TrackDiscretionaryData(_Value[GetDiscretionaryDataNibbleOffset(_Value)..^1]);

        return new TrackDiscretionaryData(_Value[GetDiscretionaryDataNibbleOffset(_Value)..]);
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public Track2 CreateUpdate(TrackDiscretionaryData discretionaryData)
    {
        ReadOnlySpan<Nibble> valueBuffer = _Value;
        ReadOnlySpan<Nibble> discretionaryDataBuffer = discretionaryData.AsNibbleArray();
        int discretionaryDataOffset = GetDiscretionaryDataNibbleOffset(valueBuffer);

        int nibbleCount = discretionaryDataOffset + discretionaryDataBuffer.Length;

        Span<Nibble> resultBuffer = stackalloc Nibble[nibbleCount + (nibbleCount % 2)];

        valueBuffer[..discretionaryDataOffset].CopyTo(resultBuffer);
        discretionaryDataBuffer.CopyTo(resultBuffer[discretionaryDataOffset..]);

        if ((nibbleCount % 2) != 0)
            resultBuffer[^1] = Nibble.MaxValue;

        return new Track2(resultBuffer);
    }

    #endregion
}