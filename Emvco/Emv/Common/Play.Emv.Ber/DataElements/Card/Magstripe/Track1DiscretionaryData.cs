using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     If Track 1 Data is present, then DD Card (Track1) contains a copy of the discretionary data field of Track 1 Data
///     as returned by the Card in the file read using the READ RECORD command during a mag-stripe mode transaction (i.e.
///     without Unpredictable Number (Numeric), Application Transaction Counter, CVC3 (Track1) and nUN included).
/// </summary>
public record Track1DiscretionaryData : DataElement<char[]>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF812A;
    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    private const byte _MaxByteLength = 56;

    #endregion

    #region Constructor

    public Track1DiscretionaryData(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static Track1DiscretionaryData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override Track1DiscretionaryData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static Track1DiscretionaryData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value) as DecodedResult<char[]>
            ?? throw new DataElementParsingException(
                $"The {nameof(Track1DiscretionaryData)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        return new Track1DiscretionaryData(result.Value);
    }

    #endregion
}