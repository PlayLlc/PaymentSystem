using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     DD Card (Track2) contains a copy of the discretionary data field of Track 2 Data as returned by the Card in the
///     file read using the READ RECORD command during a mag-stripe mode transaction (i.e. without Unpredictable Number
///     (Numeric), Application Transaction Counter, CVC3 (Track2) and nUN included).
/// </summary>
public record DiscretionaryDataCardTrack2 : DataElement<char[]>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF812B;
    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    private const byte _MaxByteLength = 11;

    #endregion

    #region Constructor

    public DiscretionaryDataCardTrack2(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DiscretionaryDataCardTrack2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override DiscretionaryDataCardTrack2 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DiscretionaryDataCardTrack2 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value) as DecodedResult<char[]>
            ?? throw new
                DataElementParsingException($"The {nameof(DiscretionaryDataCardTrack2)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        return new DiscretionaryDataCardTrack2(result.Value);
    }

    #endregion
}