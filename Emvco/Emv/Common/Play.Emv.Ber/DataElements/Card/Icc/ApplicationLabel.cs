using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Mnemonic associated with the AID according to ISO/IEC 7816-5
/// </summary>
public record ApplicationLabel : DataElement<char[]>, IEqualityComparer<ApplicationLabel>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    public static readonly Tag Tag = 0x50;

    #endregion

    #region Constructor

    public ApplicationLabel(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override string ToString() => new(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static ApplicationLabel Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override ApplicationLabel Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static ApplicationLabel Decode(ReadOnlySpan<byte> value)
    {
        const ushort minByteLength = 1;
        const ushort maxByteLength = 16;

        if (value.Length is < minByteLength and <= maxByteLength)
        {
            throw new
                DataElementParsingException($"The Primitive Value {nameof(ApplicationLabel)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value) as DecodedResult<char[]>
            ?? throw new
                DataElementParsingException($"The {nameof(ApplicationLabel)} could not be initialized because the {nameof(AlphaNumericSpecialCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        return new ApplicationLabel(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(ApplicationLabel? x, ApplicationLabel? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationLabel obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator ReadOnlySpan<char>(ApplicationLabel value) => value._Value;

    #endregion
}