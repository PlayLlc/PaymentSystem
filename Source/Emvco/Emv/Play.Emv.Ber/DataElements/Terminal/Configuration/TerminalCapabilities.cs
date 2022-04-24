using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: Indicates the card data input, CVM, and security capabilities of the Terminal and Reader. The CVM
///     capability (Byte 2) is instantiated with values depending on the transaction amount.
/// </summary>
public record TerminalCapabilities : DataElement<uint>, IEqualityComparer<TerminalCapabilities>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F33;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public TerminalCapabilities(uint value) : base(value)
    { }

    public TerminalCapabilities(CardDataInputCapability cardDataInputCapability, SecurityCapability securityCapability) : base((uint) (securityCapability << 16)
        | cardDataInputCapability)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool IsCardCaptureSupported() => _Value.IsBitSet(22);
    public bool IsCombinedDataAuthenticationSupported() => _Value.IsBitSet(21);
    public bool IsDynamicDataAuthenticationSupported() => _Value.IsBitSet(23);
    public bool IsEncipheredPinForOfflineVerificationSupported() => _Value.IsBitSet(13);
    public bool IsEncipheredPinForOnlineVerificationSupported() => _Value.IsBitSet(15);
    public bool IsIcWithContactsSupported() => _Value.IsBitSet(6);
    public bool IsMagneticStripeSupported() => _Value.IsBitSet(7);
    public bool IsManualKeyEntrySupported() => _Value.IsBitSet(8);
    public bool IsNoCardVerificationMethodRequiredSet() => _Value.IsBitSet(12);
    public bool IsPlaintextPinForIccVerificationSupported() => _Value.IsBitSet(16);
    public bool IsSignaturePaperSupported() => _Value.IsBitSet(14);
    public bool IsStaticDataAuthenticationSupported() => _Value.IsBitSet(24);
    public static Builder GetBuilder() => new();

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalCapabilities Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TerminalCapabilities Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalCapabilities Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new TerminalCapabilities(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(TerminalCapabilities? x, TerminalCapabilities? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalCapabilities obj) => obj.GetHashCode();

    #endregion

    public class Builder : PrimitiveValueBuilder<uint>
    {
        #region Constructor

        internal Builder(TerminalCapabilities outcomeParameterSet)
        {
            _Value = outcomeParameterSet._Value;
        }

        internal Builder()
        { }

        #endregion

        #region Instance Members

        public void Reset(TerminalCapabilities value)
        {
            _Value = value._Value;
        }

        public override TerminalCapabilities Complete() => new(_Value);

        public void SetCardVerificationMethodNotRequired(bool value)
        {
            if (value)
                _Value.SetBit(12);

            _Value.ClearBit(Bits.Four, 2);
        }

        protected override void Set(uint bitsToSet)
        {
            _Value |= bitsToSet;
        }

        #endregion
    }
}