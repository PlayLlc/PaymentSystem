using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

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

    public bool ApplicationNotYetEffective() => _Value.IsBitSet(30);
    public bool CardAppearsOnTerminalExceptionFile() => _Value.IsBitSet(37);
    public bool CardholderVerificationWasNotSuccessful() => _Value.IsBitSet(24);
    public bool CombinationDataAuthenticationFailed() => _Value.IsBitSet(35);
    public bool DefaultTransactionCertificateDataObjectListUsed() => _Value.IsBitSet(8);
    public bool DynamicDataAuthenticationFailed() => _Value.IsBitSet(36);
    public bool ExpiredApplication() => _Value.IsBitSet(31);
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public bool IccAndTerminalHaveDifferentApplicationVersions() => _Value.IsBitSet(32);
    public bool IccDataMissing() => _Value.IsBitSet(38);
    public bool IssuerAuthenticationFailed() => _Value.IsBitSet(7);
    public bool LowerConsecutiveOfflineLimitExceeded() => _Value.IsBitSet(15);
    public bool MerchantForcedTransactionOnline() => _Value.IsBitSet(12);
    public bool NewCard() => _Value.IsBitSet(28);
    public bool OfflineDataAuthenticationWasNotPerformed() => _Value.IsBitSet(40);
    public bool OnlinePinEntered() => _Value.IsBitSet(19);
    public bool PinEntryRequiredAndPinPadNotPresentOrNotWorking() => _Value.IsBitSet(21);
    public bool PinEntryRequiredPinPadPresentButPinWasNotEntered() => _Value.IsBitSet(20);
    public bool PinTryLimitExceeded() => _Value.IsBitSet(22);
    public bool RelayResistanceThresholdExceeded() => _Value.IsBitSet(4);
    public bool RelayResistanceTimeLimitsExceeded() => _Value.IsBitSet(3);
    public bool RequestedServiceNotAllowedForCardProduct() => _Value.IsBitSet(29);
    public bool ScriptProcessingFailedAfterFinalGenerateAc() => _Value.IsBitSet(5);
    public bool ScriptProcessingFailedBeforeFinalGenerateAc() => _Value.IsBitSet(6);

    public void SetBits(TerminalVerificationResult terminalVerificationResult)
    {
        _Value.SetBits((ulong) terminalVerificationResult);
    }

    public bool StaticDataAuthenticationFailed() => _Value.IsBitSet(39);
    public bool TransactionExceedsFloorLimit() => _Value.IsBitSet(16);
    public bool TransactionSelectedRandomlyForOnlineProcessing() => _Value.IsBitSet(13);
    public bool UnrecognizedCvm() => _Value.IsBitSet(23);
    public bool UpperConsecutiveOfflineLimitExceeded() => _Value.IsBitSet(14);

    #endregion

    #region Serialization

    public static TerminalVerificationResults Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalVerificationResults Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TerminalVerificationResults)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<ulong> result = codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalVerificationResults)} could not be initialized because the {nameof(NumericDataElementCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new TerminalVerificationResults(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

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

    public int GetHashCode(TerminalVerificationResults obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator ulong(TerminalVerificationResults value) => value._Value;

    //public RelayResistancePerformed GetRelayResistancePerformed()
    //{
    //    const byte bitOffset = 1;
    //    return RelayResistancePerformed.Get((byte) (_Value >> bitOffset));
    //}

    public static TerminalVerificationResults operator |(TerminalVerificationResults left, TerminalVerificationResults right) =>
        new(left._Value | right._Value);

    #endregion
}