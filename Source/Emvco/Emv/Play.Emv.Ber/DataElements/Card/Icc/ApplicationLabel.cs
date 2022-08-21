using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Mnemonic associated with the AID according to ISO/IEC 7816-5
/// </summary>
public record ApplicationLabel : DataElement<char[]>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    public static readonly Tag Tag = 0x50;
    private const byte _MinByteLength = 1;
    private const byte _MaxByteLength = 16;

    #endregion

    #region Constructor

    public ApplicationLabel(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Serialization

    public static ApplicationLabel Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override ApplicationLabel Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static ApplicationLabel Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length is < _MinByteLength and <= _MaxByteLength)
        {
            throw new DataElementParsingException(
                $"The Primitive Value {nameof(ApplicationLabel)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {_MinByteLength}-{_MaxByteLength}");
        }

        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value) as DecodedResult<char[]>
            ?? throw new DataElementParsingException(
                $"The {nameof(ApplicationLabel)} could not be initialized because the {nameof(AlphaNumericSpecialCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        return new ApplicationLabel(result.Value);
    }

    #endregion

    #region Operator Overrides

    public static implicit operator ReadOnlySpan<char>(ApplicationLabel value) => value._Value;

    #endregion

    #region Instance Members

    public override string ToString() => new(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}