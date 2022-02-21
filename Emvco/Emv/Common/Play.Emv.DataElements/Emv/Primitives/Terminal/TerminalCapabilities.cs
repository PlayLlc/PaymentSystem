using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Description: Indicates the card data input, CVM, and security capabilities of the Terminal and Reader. The CVM
///     capability (Byte 2) is instantiated with values depending on the transaction amount.
/// </summary>
public record TerminalCapabilities : DataElement<uint>, IEqualityComparer<TerminalCapabilities>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x9F33;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public TerminalCapabilities(uint value) : base(value)
    { }

    public TerminalCapabilities(CardDataInputCapability cardDataInputCapability, SecurityCapability securityCapability) : base(
        (uint) (securityCapability << 16) | cardDataInputCapability)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public bool IsCardCaptureSupported() => _Value.IsBitSet(22);
    public bool IsCombinedDataAuthenticationSupported() => _Value.IsBitSet(21);
    public bool IsDynamicDataAuthenticationSupported() => _Value.IsBitSet(23);
    public bool IsEncipheredPinForOfflineVerificationSupported() => _Value.IsBitSet(13);
    public bool IsEncipheredPinForOnlineVerificationSupported() => _Value.IsBitSet(15);
    public bool IsIcWithContactsSupported() => _Value.IsBitSet(6);
    public bool IsMagneticStripeSupported() => _Value.IsBitSet(7);
    public bool IsManualKeyEntrySupported() => _Value.IsBitSet(8);
    public bool IsNoCardVerificationMethodRequiredSupported() => _Value.IsBitSet(12);
    public bool IsPlaintextPinForIccVerificationSupported() => _Value.IsBitSet(16);
    public bool IsSignaturePaperSupported() => _Value.IsBitSet(14);
    public bool IsStaticDataAuthenticationSupported() => _Value.IsBitSet(24);

    #endregion

    #region Serialization

    public static TerminalCapabilities Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalCapabilities Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalCapabilities)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new TerminalCapabilities(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

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
}