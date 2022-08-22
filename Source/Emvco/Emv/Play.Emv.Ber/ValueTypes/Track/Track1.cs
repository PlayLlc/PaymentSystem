using Play.Ber.Exceptions;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.ValueTypes;

public readonly struct Track1
{
    #region Static Metadata

    private const byte _MaxByteCount = 76;
    private const byte _StartSentinel = (byte) '%';
    private const byte _FormatCode = (byte) 'B';
    private const byte _FieldSeparator1 = (byte) '^';
    private const byte _EndSentinel = (byte) '?';

    #endregion

    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    public Track1(ReadOnlySpan<byte> value)
    {
        if (value.Length > _MaxByteCount)
        {
            throw new DataElementParsingException(
                $"The {nameof(Track1)} could not be initialized because the char count was out of range. The char count provided was: [{value.Length}] but must be no greater than {_MaxByteCount}");
        }

        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    public byte[] Encode() => _Value.CopyValue();

    public ushort GetByteCount() => (ushort)_Value.Length;

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public TrackPrimaryAccountNumber GetPrimaryAccountNumber()
    {
        if (_Value[0] == _StartSentinel)
            return new TrackPrimaryAccountNumber(_Value[GetFirstFieldSeparatorOffset(_Value)..1].AsNibbleArray()[1..]);

        return new TrackPrimaryAccountNumber(_Value[1..(GetFirstFieldSeparatorOffset(_Value)-1)].AsNibbleArray());
    }

    /// <exception cref="BerParsingException"></exception>
    private int GetFirstFieldSeparatorOffset(ReadOnlySpan<byte> value)
    {
        int offset = 0;
        int j;
        for (j = 0; j < 1; offset++)
        {
            if (value[offset] == _FieldSeparator1)
                j++;
        }

        if (j != 1)
            throw new BerParsingException($"The {nameof(Track1Data)} could not find the 2nd field separator");

        return offset;
    }

    /// <exception cref="BerParsingException"></exception>
    private int GetExpiryDateOffset(ReadOnlySpan<byte> value) => GetSecondFieldSeparatorOffset(value);

    /// <exception cref="BerParsingException"></exception>
    private int GetServiceCodeOffset(ReadOnlySpan<byte> value) => GetExpiryDateOffset(value) + 4;

    /// <exception cref="BerParsingException"></exception>
    private int GetDiscretionaryDataOffset(ReadOnlySpan<byte> value) => GetServiceCodeOffset(value) + 3;

    /// <exception cref="BerParsingException"></exception>
    private int GetSecondFieldSeparatorOffset(ReadOnlySpan<byte> value)
    {
        int offset = 0;
        int j;
        for (j = 0; j < value.Length && offset != 2; j++)
        {
            if (value[j] == _FieldSeparator1)
                offset++;
        }

        if (offset != 2)
            throw new BerParsingException($"The {nameof(Track1Data)} could not find the 2nd field separator");

        return j;
    }

    /// <exception cref="BerParsingException"></exception>
    public Track1 CreateUpdate(Track1DiscretionaryData discretionaryData)
    {
        int offset = GetDiscretionaryDataOffset(_Value);

        byte[] result = new byte[offset + discretionaryData.GetValueByteCount()];
        _Value[..offset].CopyTo(result.AsSpan());
        discretionaryData.EncodeValue().CopyTo(result[offset..].AsSpan());

        return new Track1(result);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public Track1DiscretionaryData GetDiscretionaryData()
    {
        if (_Value[^1] == _EndSentinel)
            return Track1DiscretionaryData.Decode(_Value[GetDiscretionaryDataOffset(_Value)..1].AsSpan());

        return Track1DiscretionaryData.Decode(_Value[GetDiscretionaryDataOffset(_Value)..].AsSpan());
    }

    #region Service Code

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ServiceCode GetServiceCode() => new(PlayCodec.NumericCodec.DecodeToUInt16(_Value[GetServiceCodeOffset(_Value)..]));

    #endregion

    #endregion
}