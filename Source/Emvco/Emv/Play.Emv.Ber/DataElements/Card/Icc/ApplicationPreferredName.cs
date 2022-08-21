using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Preferred mnemonic associated with the AID
/// </summary>
public record ApplicationPreferredName : DataElement<char[]>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F12;
    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    private const byte _MinByteLength = 1;
    private const byte _MaxByteLength = 16;

    #endregion

    #region Constructor

    public ApplicationPreferredName(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Serialization

    public static ApplicationPreferredName Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override ApplicationPreferredName Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static ApplicationPreferredName Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length is < _MinByteLength and <= _MaxByteLength)
        {
            throw new DataElementParsingException(
                $"The Primitive Value {nameof(ApplicationPreferredName)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {_MinByteLength}-{_MaxByteLength}");
        }

        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value) as DecodedResult<char[]>
            ?? throw new DataElementParsingException(
                $"The {nameof(ApplicationPreferredName)} could not be initialized because the {nameof(AlphaNumericSpecialCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        return new ApplicationPreferredName(result.Value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}