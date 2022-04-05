using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the capabilities of the card to support specific functions in the application
/// </summary>
public record ApplicationInterchangeProfile : DataElement<ushort>, IEqualityComparer<ApplicationInterchangeProfile>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x82;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public ApplicationInterchangeProfile(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool IsStaticDataAuthenticationSupported() => _Value.IsBitSet(15);
    public bool IsDynamicDataAuthenticationSupported() => _Value.IsBitSet(14);
    public bool IsCardholderVerificationSupported() => _Value.IsBitSet(13);
    public bool IsTerminalRiskManagementRequired() => _Value.IsBitSet(12);
    public bool IsIssuerAuthenticationSupported() => _Value.IsBitSet(11);
    public bool IsOnDeviceCardholderVerificationSupported() => _Value.IsBitSet(10);
    public bool IsCombinedDataAuthenticationSupported() => _Value.IsBitSet(9);
    public bool IsEmvModeSupported() => _Value.IsBitSet(8);
    public bool IsRelayResistanceProtocolSupported() => _Value.IsBitSet(1);

    /// <summary>
    ///     Combined Dynamic Data Authentication and Application Cryptogram Generation
    /// </summary>
    public byte[] Encode() => new[] {(byte) (_Value >> 8), (byte) _Value};

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationInterchangeProfile Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ApplicationInterchangeProfile Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationInterchangeProfile Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new ApplicationInterchangeProfile(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ApplicationInterchangeProfile? x, ApplicationInterchangeProfile? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationInterchangeProfile obj) => obj.GetHashCode();

    #endregion
}