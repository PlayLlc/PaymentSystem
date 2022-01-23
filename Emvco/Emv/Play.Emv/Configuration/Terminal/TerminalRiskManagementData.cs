using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;

namespace Play.Emv.Configuration;

/// <summary>
///     Description: Application-specific value used by the cardholder device for risk management purposes.
/// </summary>
public record TerminalRiskManagementData : DataElement<ulong>, IEqualityComparer<TerminalRiskManagementData>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F1D;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    public TerminalRiskManagementData(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool CardCapture()
    {
        return _Value.IsBitSet(22);
    }

    public bool CdCvmBypassRequested()
    {
        return _Value.IsBitSet(32);
    }

    public bool CdCvmContact()
    {
        return _Value.IsBitSet(11);
    }

    public bool CdCvmContactless()
    {
        return _Value.IsBitSet(3);
    }

    public bool CdCvmWithoutCdaSupported()
    {
        return _Value.IsBitSet(22);
    }

    public bool CombinedDataAuthentication()
    {
        return _Value.IsBitSet(20);
    }

    public bool CvmLimitExceeded()
    {
        return _Value.IsBitSet(16);
    }

    public bool Dda()
    {
        return _Value.IsBitSet(23);
    }

    public bool EmvModeContactlessTransactionsNotSupported()
    {
        return _Value.IsBitSet(23);
    }

    public bool EncipheredPinForOfflineVerification()
    {
        return _Value.IsBitSet(13);
    }

    public bool EncipheredPinForOnlineVerification()
    {
        return _Value.IsBitSet(15);
    }

    public bool EncipheredPinVerificationPerformedByIccContact()
    {
        return _Value.IsBitSet(13);
    }

    public bool EncipheredPinVerificationPerformedByIccContactless()
    {
        return _Value.IsBitSet(5);
    }

    public bool EncipheredPinVerifiedOnlineContact()
    {
        return _Value.IsBitSet(15);
    }

    public bool EncipheredPinVerifiedOnlineContactless()
    {
        return _Value.IsBitSet(7);
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

    public bool IcWithContacts()
    {
        return _Value.IsBitSet(6);
    }

    public bool MagneticStripe()
    {
        return _Value.IsBitSet(7);
    }

    public bool MagstripeModeContactlessTransactionsNotSupported()
    {
        return _Value.IsBitSet(24);
    }

    public bool ManualKeyEntry()
    {
        return _Value.IsBitSet(8);
    }

    public bool NoCardVerificationMethodRequired()
    {
        return _Value.IsBitSet(12);
    }

    public bool NoCardVerificationMethodRequiredContact()
    {
        return _Value.IsBitSet(12);
    }

    public bool NoCardVerificationMethodRequiredContactless()
    {
        return _Value.IsBitSet(4);
    }

    public bool PlaintextPinForIccVerification()
    {
        return _Value.IsBitSet(16);
    }

    public bool PlaintextPinVerificationPerformedByIccContact()
    {
        return _Value.IsBitSet(10);
    }

    public bool PlaintextPinVerificationPerformedByIccContactless()
    {
        return _Value.IsBitSet(2);
    }

    public bool PresentAndHoldSupported()
    {
        return _Value.IsBitSet(1);
    }

    public bool RestartSupported()
    {
        return _Value.IsBitSet(8);
    }

    public bool ScaExempt()
    {
        return _Value.IsBitSet(31);
    }

    public bool SignaturePaper()
    {
        return _Value.IsBitSet(14);
    }

    public bool SignaturePaperContact()
    {
        return _Value.IsBitSet(14);
    }

    public bool SignaturePaperContactless()
    {
        return _Value.IsBitSet(6);
    }

    public bool StaticDataAuthentication()
    {
        return _Value.IsBitSet(24);
    }

    #endregion

    #region Serialization

    public static TerminalRiskManagementData Decode(ReadOnlyMemory<byte> value)
    {
        return Decode(value.Span);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalRiskManagementData Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(TerminalRiskManagementData)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new
                InvalidOperationException($"The {nameof(TerminalRiskManagementData)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new TerminalRiskManagementData(result.Value);
    }

    public new byte[] EncodeValue()
    {
        return EncodeValue(_ByteLength);
    }

    #endregion

    #region Equality

    public bool Equals(TerminalRiskManagementData? x, TerminalRiskManagementData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalRiskManagementData obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}