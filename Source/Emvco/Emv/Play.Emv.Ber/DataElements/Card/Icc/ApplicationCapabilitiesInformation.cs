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

    #region Instance Members

    public bool CombinedDataAuthenticationIndicator() => _Value.IsBitSet(9);
    public override PlayEncodingId GetEncodingId() => EncodingId;

    public SdsSchemeIndicator GetSdsSchemeIndicator()
    {
        const uint sdsBitMask = 0b1111_1111_1111_1111_0000_0000;
        byte input = (byte)_Value.GetMaskedValue(sdsBitMask);

        return SdsSchemeIndicator.Get(input);
    }

    public DataStorageVersionNumber GetDataStorageVersionNumber()
    {
        const byte bitOffset = 16;
        const byte bitMask = 0b11110000;

        byte input = (byte)(_Value >> bitOffset).GetMaskedValue(bitMask);

        return new DataStorageVersionNumber(input);
    }

    public override Tag GetTag() => Tag;
    public bool SupportForBalanceReading() => _Value.IsBitSet(10);
    public bool IsSupportForFieldOffDetectionSet() => _Value.IsBitSet(11);
    public byte[] Encode() => PlayCodec.BinaryCodec.Encode(_Value, _ByteLength);

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
}