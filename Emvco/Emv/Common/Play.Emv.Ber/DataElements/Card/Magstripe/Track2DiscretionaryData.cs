using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     DD Card (Track2) contains a copy of the discretionary data field of Track 2 Data as returned by the Card in the
///     file read using the READ RECORD command during a mag-stripe mode transaction (i.e. without Unpredictable Number
///     (Numeric), Application Transaction Counter, CVC3 (Track2) and nUN included).
/// </summary>
public record Track2DiscretionaryData : DataElement<TrackDiscretionaryData>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF812B;
    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    private const byte _MaxByteLength = 11;

    #endregion

    #region Constructor

    public Track2DiscretionaryData(TrackDiscretionaryData value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public new byte[] Encode() => _Value.Encode();

    #endregion

    #region Serialization

    public static Track2DiscretionaryData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override Track2DiscretionaryData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static Track2DiscretionaryData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        if (!PlayCodec.AlphaNumericSpecialCodec.IsValid(value))
        {
            throw new DataElementParsingException(
                $"The {nameof(Track2DiscretionaryData)} could not be initialized because the format was invalid for the {nameof(AlphaNumericSpecialCodec)}");
        }

        return new Track2DiscretionaryData(new TrackDiscretionaryData(PlayCodec.AlphaNumericSpecialCodec.DecodeToNibbles(value)));
    }

    #endregion
}