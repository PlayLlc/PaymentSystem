using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the requirements for online and CVM processing as a result of Entry Point processing. The scope of this
///     tag is limited to Entry Point. Kernels may use this tag for different purposes.
/// </summary>
public record TerminalTransactionQualifiers : DataElement<uint>, IEqualityComparer<TerminalTransactionQualifiers>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x9F66;
    private const byte _ByteLength = 4;

    #endregion

    #region Constructor

    public TerminalTransactionQualifiers(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public TerminalTransactionQualifiers AsValueCopy() => new(_Value);
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
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

    public static TerminalTransactionQualifiers Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalTransactionQualifiers Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TerminalTransactionQualifiers)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalTransactionQualifiers)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new TerminalTransactionQualifiers(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

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