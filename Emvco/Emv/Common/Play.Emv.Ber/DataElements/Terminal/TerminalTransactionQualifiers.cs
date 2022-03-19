using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the requirements for online and CVM processing as a result of Entry Point processing. The scope of this
///     tag is limited to Entry Point. Kernels may use this tag for different purposes.
/// </summary>
public record TerminalTransactionQualifiers : DataElement<uint>, IEqualityComparer<TerminalTransactionQualifiers>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F66;
    private const byte _ByteLength = 4;

    #endregion

    #region Constructor

    public TerminalTransactionQualifiers(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public TerminalTransactionQualifiers AsValueCopy() => new(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public TerminalTransactionQualifiers GetValueCopy() => new(_Value);
    public bool IsCvmRequired() => _Value.IsBitSet(15);
    public bool IsCvmSupportedOnConsumerDevice() => _Value.IsBitSet(23);
    public bool IsEmvContactSupported() => _Value.IsBitSet(5);
    public bool IsEmvModeSupported() => _Value.IsBitSet(6);
    public bool IsIssuerUpdateProcessingSupported() => _Value.IsBitSet(24);
    public bool IsMagStripeModeSupported() => _Value.IsBitSet(8);
    public bool IsOfflineDataAuthenticationForOnlineAuthorizationsSupported() => _Value.IsBitSet(1);
    public bool IsOfflinePinSupportedForEmvContact() => _Value.IsBitSet(14);
    public bool IsOnlineCryptogramRequired() => _Value.IsBitSet(16);
    public bool IsOnlinePinSupported() => _Value.IsBitSet(3);
    public bool IsReaderOfflineOnly() => _Value.IsBitSet(4);
    public bool IsReaderOnlineCapable() => !_Value.IsBitSet(4);
    public bool IsSignatureSupported() => _Value.IsBitSet(2);

    /// <summary>
    ///     Resets the flags <see cref="IsOnlineCryptogramRequired" /> and <see cref="IsCvmRequired" /> in preparation for
    ///     Pre-Processing
    /// </summary>
    /// <remarks>
    ///     This mutates the underlying value of the current instance
    /// </remarks>
    public void ResetForPreProcessingIndicator()
    {
        const uint bitMask = (uint) 0b11000000 << 16;
        _Value = _Value.GetMaskedValue(bitMask);
    }

    public void SetCvmRequired()
    {
        _Value = _Value.SetBit(Bits.Seven, 2);
    }

    public void SetOnlineCryptogramRequired()
    {
        _Value = _Value.SetBit(Bits.Eight, 2);
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TerminalTransactionQualifiers Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TerminalTransactionQualifiers Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new TerminalTransactionQualifiers(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(TerminalTransactionQualifiers? x, TerminalTransactionQualifiers? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalTransactionQualifiers obj) => obj.GetHashCode();

    #endregion
}