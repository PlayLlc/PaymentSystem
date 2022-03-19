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
public record Track1Data : DataElement<byte[]>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    public static readonly Tag Tag = 0x56;
    private const byte _MaxByteLength = 76;

    #endregion

    #region Constructor

    public Track1Data(ReadOnlySpan<byte> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="CardDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public PrimaryAccountNumber GetPrimaryAccountNumber(PunatcTrack1 value)
    {
        const byte startSentinel = (byte) '%';
        const byte formatCode = (byte) 'B';
        const byte fieldSeparator = (byte) '=';

        int offset = 0;

        if (_Value[0] == startSentinel)
            offset++;

        if (_Value[offset++] != formatCode)
            throw new CardDataException($"The {nameof(PrimaryAccountNumber)} was provided in an unknown format");

        int startRange = offset;

        for (; offset < PrimaryAccountNumber.GetMaxByteLength(); offset++)
        {
            if (_Value[offset] == fieldSeparator)
                return PrimaryAccountNumber.Decode(_Value.AsSpan()[startRange..offset]);
        }

        throw new CardDataException($"The {nameof(PrimaryAccountNumber)} was provided in an unknown format");
    }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track1Data Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track1Data Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        char[] result = PlayCodec.AlphaNumericSpecialCodec.DecodeToChars(value);

        return new Track1Data(value);
    }

    #endregion
}