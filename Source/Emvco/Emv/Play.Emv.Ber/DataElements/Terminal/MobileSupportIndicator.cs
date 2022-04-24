using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The Mobile Support Indicator informs the Card that the Kernel supports extensions for mobile and requires on device
///     cardholder verification.
/// </summary>
public record MobileSupportIndicator : DataElement<byte>, IEqualityComparer<MobileSupportIndicator>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly MobileSupportIndicator Default = new(0);
    public static readonly Tag Tag = 0x9F7E;
    private const byte _OnDeviceCvmRequiredOffset = (byte) Bits.Two;
    private const byte _IsMobileSupportedOffset = (byte) Bits.One;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public MobileSupportIndicator(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static Builder GetBuilder() => new();
    public bool IsOnDeviceCvmRequired() => _Value.IsBitSet(Bits.Two);
    public bool IsMobileSupported() => _Value.IsBitSet(Bits.One);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MobileSupportIndicator Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MobileSupportIndicator Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MobileSupportIndicator Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new MobileSupportIndicator(result);
    }

    #endregion

    #region Equality

    public bool Equals(MobileSupportIndicator? x, MobileSupportIndicator? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(MobileSupportIndicator obj) => obj.GetHashCode();

    #endregion

    public class Builder : PrimitiveValueBuilder<byte>
    {
        #region Constructor

        internal Builder(MobileSupportIndicator outcomeParameterSet)
        {
            _Value = outcomeParameterSet._Value;
        }

        internal Builder()
        { }

        #endregion

        #region Instance Members

        public void Reset(MobileSupportIndicator value)
        {
            _Value = value._Value;
        }

        public void SetOnDeviceCvmRequired(bool value)
        {
            if (value)
                _Value.SetBits(_OnDeviceCvmRequiredOffset);

            _Value.ClearBits(_OnDeviceCvmRequiredOffset);
        }

        public void SetMobileSupported(bool value)
        {
            if (value)
                _Value.SetBits(_IsMobileSupportedOffset);

            _Value.ClearBits(_IsMobileSupportedOffset);
        }

        public override MobileSupportIndicator Complete() => new(_Value);

        protected override void Set(byte bitsToSet)
        {
            _Value |= bitsToSet;
        }

        #endregion
    }
}