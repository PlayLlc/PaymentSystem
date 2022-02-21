using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

public record ApplicationEffectiveDate : DataElement<uint>, IEqualityComparer<ApplicationEffectiveDate>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x5F25;
    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public ApplicationEffectiveDate(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ApplicationEffectiveDate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationEffectiveDate Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationEffectiveDate)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(ApplicationEffectiveDate)} could not be initialized because the {nameof(NumericDataElementCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new ApplicationEffectiveDate(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(GetBerEncodingId(), _Value, _ByteLength);
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ApplicationEffectiveDate? x, ApplicationEffectiveDate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationEffectiveDate obj) => obj.GetHashCode();

    #endregion
}