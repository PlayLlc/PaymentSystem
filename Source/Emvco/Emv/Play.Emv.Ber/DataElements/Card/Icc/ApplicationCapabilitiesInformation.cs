using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: Lists a number of card features beyond regular payment.
/// </summary>
public record ApplicationCapabilitiesInformation : DataElement<uint>, IEqualityComparer<ApplicationCapabilitiesInformation>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F5D;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public ApplicationCapabilitiesInformation(uint value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationCapabilitiesInformation Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ApplicationCapabilitiesInformation Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationCapabilitiesInformation Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new ApplicationCapabilitiesInformation(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(ApplicationCapabilitiesInformation? x, ApplicationCapabilitiesInformation? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationCapabilitiesInformation obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public bool CombinedDataAuthenticationIndicator() => _Value.IsBitSet(9);
    public override PlayEncodingId GetEncodingId() => EncodingId;

    public SdsSchemeIndicators GetSdsSchemeIndicator()
    {
        const byte bitMask = 0b11110000;

        return SdsSchemeIndicators.Get((byte) (_Value).GetMaskedValue(bitMask));
    }

    public DataStorageVersionNumber GetDataStorageVersionNumber()
    {
        const byte bitOffset = 16;
        const byte bitMask = 0b11111100;

        return new DataStorageVersionNumber((byte) (_Value >> bitOffset).GetMaskedValue(bitMask));
    }

    public override Tag GetTag() => Tag;
    public bool SupportForBalanceReading() => _Value.IsBitSet(10);
    public bool IsSupportForFieldOffDetectionSet() => _Value.IsBitSet(11);
    public byte[] Encode() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);

    #endregion
}