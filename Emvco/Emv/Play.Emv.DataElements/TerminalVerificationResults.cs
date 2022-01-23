using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: Status of the different functions from the Terminal perspective. The Terminal Verification Results is
///     coded according to Annex C.5 of [EMV Book 3]. BitCount that have been reserved for use by contactless
///     specifications are defined as shown.
/// </summary>
public record TerminalVerificationResults : DataElement<ulong>, IEqualityComparer<TerminalVerificationResults>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    public static readonly Tag Tag = 0x95;
    private const byte _ByteLength = 0x05;

    #endregion

    #region Constructor

    public TerminalVerificationResults(ulong value) : base(value)
    { }

    public TerminalVerificationResults(TerminalVerificationResult value) : base((ulong) value)
    { }

    #endregion

    #region Instance Members

    public bool ApplicationNotYetEffective()
    {
        return _Value.IsBitSet(30);
    }

    public bool CardAppearsOnTerminalExceptionFile()
    {
        return _Value.IsBitSet(37);
    }

    public bool CardholderVerificationWasNotSuccessful()
    {
        return _Value.IsBitSet(24);
    }

    public bool CombinationDataAuthenticationFailed()
    {
        return _Value.IsBitSet(35);
    }

    public bool DefaultTransactionCertificateDataObjectListUsed()
    {
        return _Value.IsBitSet(8);
    }

    public bool DynamicDataAuthenticationFailed()
    {
        return _Value.IsBitSet(36);
    }

    public bool ExpiredApplication()
    {
        return _Value.IsBitSet(31);
    }

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return codec.GetByteCount(GetBerEncodingId(), _Value);
    }

    public bool IccAndTerminalHaveDifferentApplicationVersions()
    {
        return _Value.IsBitSet(32);
    }

    public bool IccDataMissing()
    {
        return _Value.IsBitSet(38);
    }

    public bool IssuerAuthenticationFailed()
    {
        return _Value.IsBitSet(7);
    }

    public bool LowerConsecutiveOfflineLimitExceeded()
    {
        return _Value.IsBitSet(15);
    }

    public bool MerchantForcedTransactionOnline()
    {
        return _Value.IsBitSet(12);
    }

    public bool NewCard()
    {
        return _Value.IsBitSet(28);
    }

    public bool OfflineDataAuthenticationWasNotPerformed()
    {
        return _Value.IsBitSet(40);
    }

    public bool OnlinePinEntered()
    {
        return _Value.IsBitSet(19);
    }

    public bool PinEntryRequiredAndPinPadNotPresentOrNotWorking()
    {
        return _Value.IsBitSet(21);
    }

    public bool PinEntryRequiredPinPadPresentButPinWasNotEntered()
    {
        return _Value.IsBitSet(20);
    }

    public bool PinTryLimitExceeded()
    {
        return _Value.IsBitSet(22);
    }

    public bool RelayResistanceThresholdExceeded()
    {
        return _Value.IsBitSet(4);
    }

    public bool RelayResistanceTimeLimitsExceeded()
    {
        return _Value.IsBitSet(3);
    }

    public bool RequestedServiceNotAllowedForCardProduct()
    {
        return _Value.IsBitSet(29);
    }

    public bool ScriptProcessingFailedAfterFinalGenerateAc()
    {
        return _Value.IsBitSet(5);
    }

    public bool ScriptProcessingFailedBeforeFinalGenerateAc()
    {
        return _Value.IsBitSet(6);
    }

    public void SetBits(TerminalVerificationResult terminalVerificationResult)
    {
        _Value.SetBits((ulong) terminalVerificationResult);
    }

    public bool StaticDataAuthenticationFailed()
    {
        return _Value.IsBitSet(39);
    }

    public bool TransactionExceedsFloorLimit()
    {
        return _Value.IsBitSet(16);
    }

    public bool TransactionSelectedRandomlyForOnlineProcessing()
    {
        return _Value.IsBitSet(13);
    }

    public bool UnrecognizedCvm()
    {
        return _Value.IsBitSet(23);
    }

    public bool UpperConsecutiveOfflineLimitExceeded()
    {
        return _Value.IsBitSet(14);
    }

    #endregion

    #region Serialization

    public static TerminalVerificationResults Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalVerificationResults Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        if (value.Length != _ByteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(TerminalVerificationResults)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<ulong> result = codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new
                InvalidOperationException($"The {nameof(TerminalVerificationResults)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new TerminalVerificationResults(result.Value);
    }

    public new byte[] EncodeValue()
    {
        return _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);
    }

    #endregion

    #region Equality

    public bool Equals(TerminalVerificationResults? x, TerminalVerificationResults? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalVerificationResults obj)
    {
        return obj.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ulong(TerminalVerificationResults value)
    {
        return value._Value;
    }

    //public RelayResistancePerformed GetRelayResistancePerformed()
    //{
    //    const byte bitOffset = 1;
    //    return RelayResistancePerformed.Get((byte) (_Value >> bitOffset));
    //}

    public static TerminalVerificationResults operator |(TerminalVerificationResults left, TerminalVerificationResults right)
    {
        return new(left._Value | right._Value);
    }

    #endregion
}