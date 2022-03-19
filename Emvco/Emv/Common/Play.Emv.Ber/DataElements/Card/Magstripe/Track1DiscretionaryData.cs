using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Discretionary part of track 1 according to [ISO/IEC 7813].
/// </summary>
public record Track1DiscretionaryData : DataElement<char[]>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    public static readonly Tag Tag = 0x9F1F;
    private const byte _MaxByteLength = 54;

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

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track1DiscretionaryData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track1DiscretionaryData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        char[] result = PlayCodec.AlphaNumericSpecialCodec.DecodeToChars(value);

        return new Track1DiscretionaryData(result);
    }

    #endregion
}