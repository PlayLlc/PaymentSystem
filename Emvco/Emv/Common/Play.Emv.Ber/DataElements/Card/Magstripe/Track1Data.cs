using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Track 1 Data contains the data objects of the track 1 according to [ISO/IEC 7813] Structure B, excluding start
///     sentinel, end sentinel and LRC. The Track 1 Data may be present in the file read using the READ RECORD command
///     during a mag-stripe mode transaction. It is made up of the following sub-fields:
/// </summary>
public record Track1Data : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    public static readonly Tag Tag = 0x56;
    private const byte _MaxByteLength = 76;
    private const byte _StartSentinel = (byte) '%';
    private const byte _FormatCode = (byte) 'B';
    private const byte _FieldSeparator = (byte) '^';
    private const byte _EndSentinel = (byte) '?';

    #endregion

    #region Constructor

    public Track1Data(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <summary>
    ///     GetTrack1DiscretionaryData
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public Track1DiscretionaryData GetTrack1DiscretionaryData()
    {
        ReadOnlySpan<byte> value = _Value.ToByteArray();

        return Track1DiscretionaryData.Decode(value[GetDiscretionaryDataOffset(value)..]);
    }

    /// <summary>
    ///     GetSecondFieldSeparatorOffset
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    private int GetSecondFieldSeparatorOffset(ReadOnlySpan<byte> value)
    {
        int offset = 0;

        for (int j = 0; j < 2; offset++)
        {
            if (value[offset] == _FieldSeparator)
                j++;
        }

        if (offset != 2)
            throw new BerParsingException($"The {nameof(Track1Data)} could not find the 2nd field separator");

        return offset;
    }

    private int GetExpiryDateOffset(ReadOnlySpan<byte> value) => GetSecondFieldSeparatorOffset(value) + 1;
    private int GetServiceCodeOffset(ReadOnlySpan<byte> value) => GetExpiryDateOffset(value) + 4;
    private int GetDiscretionaryDataOffset(ReadOnlySpan<byte> value) => GetExpiryDateOffset(value) + 3;

    /// <exception cref="CardDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public PrimaryAccountNumber GetPrimaryAccountNumber(PunatcTrack1 value)
    {
        int offset = 0;

        ReadOnlySpan<byte> buffer = _Value.ToByteArray();

        if (buffer[0] == _StartSentinel)
            offset++;

        if (buffer[offset++] != _FormatCode)
            throw new CardDataException($"The {nameof(PrimaryAccountNumber)} was provided in an unknown format");

        int startRange = offset;

        for (; offset < PrimaryAccountNumber.GetMaxByteLength(); offset++)
        {
            if (buffer[offset] == _FieldSeparator)
                return PrimaryAccountNumber.Decode(buffer[startRange..offset]);
        }

        throw new CardDataException($"The {nameof(PrimaryAccountNumber)} was provided in an unknown format");
    }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track1Data Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override Track1Data Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track1Data Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        char[] result = PlayCodec.AlphaNumericSpecialCodec.DecodeToChars(value);

        return new Track1Data(new BigInteger(value));
    }

    #endregion
}