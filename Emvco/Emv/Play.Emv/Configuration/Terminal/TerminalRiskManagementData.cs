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

    public bool CardCapture() => _Value.IsBitSet(22);
    public bool CdCvmBypassRequested() => _Value.IsBitSet(32);
    public bool CdCvmContact() => _Value.IsBitSet(11);
    public bool CdCvmContactless() => _Value.IsBitSet(3);
    public bool CdCvmWithoutCdaSupported() => _Value.IsBitSet(22);
    public bool CombinedDataAuthentication() => _Value.IsBitSet(20);
    public bool CvmLimitExceeded() => _Value.IsBitSet(16);
    public bool Dda() => _Value.IsBitSet(23);
    public bool EmvModeContactlessTransactionsNotSupported() => _Value.IsBitSet(23);
    public bool EncipheredPinForOfflineVerification() => _Value.IsBitSet(13);
    public bool EncipheredPinForOnlineVerification() => _Value.IsBitSet(15);
    public bool EncipheredPinVerificationPerformedByIccContact() => _Value.IsBitSet(13);
    public bool EncipheredPinVerificationPerformedByIccContactless() => _Value.IsBitSet(5);
    public bool EncipheredPinVerifiedOnlineContact() => _Value.IsBitSet(15);
    public bool EncipheredPinVerifiedOnlineContactless() => _Value.IsBitSet(7);
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public bool IcWithContacts() => _Value.IsBitSet(6);
    public bool MagneticStripe() => _Value.IsBitSet(7);
    public bool MagstripeModeContactlessTransactionsNotSupported() => _Value.IsBitSet(24);
    public bool ManualKeyEntry() => _Value.IsBitSet(8);
    public bool NoCardVerificationMethodRequired() => _Value.IsBitSet(12);
    public bool NoCardVerificationMethodRequiredContact() => _Value.IsBitSet(12);
    public bool NoCardVerificationMethodRequiredContactless() => _Value.IsBitSet(4);
    public bool PlaintextPinForIccVerification() => _Value.IsBitSet(16);
    public bool PlaintextPinVerificationPerformedByIccContact() => _Value.IsBitSet(10);
    public bool PlaintextPinVerificationPerformedByIccContactless() => _Value.IsBitSet(2);
    public bool PresentAndHoldSupported() => _Value.IsBitSet(1);
    public bool RestartSupported() => _Value.IsBitSet(8);
    public bool ScaExempt() => _Value.IsBitSet(31);
    public bool SignaturePaper() => _Value.IsBitSet(14);
    public bool SignaturePaperContact() => _Value.IsBitSet(14);
    public bool SignaturePaperContactless() => _Value.IsBitSet(6);
    public bool StaticDataAuthentication() => _Value.IsBitSet(24);

    #endregion

    #region Serialization

    public static TerminalRiskManagementData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

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

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

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

    public int GetHashCode(TerminalRiskManagementData obj) => obj.GetHashCode();

    #endregion
}