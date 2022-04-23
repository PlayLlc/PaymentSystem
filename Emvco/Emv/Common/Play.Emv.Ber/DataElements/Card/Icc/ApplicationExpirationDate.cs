using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: Lists a number of card features beyond regular payment.
/// </summary>
public record ApplicationExpirationDate : DataElement<uint>, IEqualityComparer<ApplicationExpirationDate>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x5F24;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public ApplicationExpirationDate(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool CombinedDataAuthenticationIndicator() => _Value.IsBitSet(9);
    public override PlayEncodingId GetEncodingId() => EncodingId;

    public SdsSchemeIndicator GetSdsSchemeIndicator()
    {
        const byte bitOffset = 1;

        return SdsSchemeIndicator.Get((byte) (_Value >> bitOffset));
    }

    public override Tag GetTag() => Tag;
    public bool SupportForBalanceReading() => _Value.IsBitSet(10);
    public bool SupportForFieldOffDetection() => _Value.IsBitSet(11);

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationExpirationDate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ApplicationExpirationDate Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationExpirationDate Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.NumericCodec.DecodeToUInt32(value);

        return new ApplicationExpirationDate(result);
    }

    public new byte[] EncodeValue() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ApplicationExpirationDate? x, ApplicationExpirationDate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationExpirationDate obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator uint(ApplicationExpirationDate value) => value._Value;

    #endregion
}