using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.Configuration;

/// <summary>
///     Description: Application-specific value used by card for risk management purposes.
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

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TerminalRiskManagementData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalRiskManagementData Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TerminalRiskManagementData)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalRiskManagementData)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<ulong>)}");

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

    #region Byte 1

    public bool PresentAndHoldSupported() => _Value.IsBitSet(1);

    /// <summary>
    ///     The terminal captures the PIN from the user and sends it in clear text to the chip card. The chip compares the
    ///     value received with a witness value stored in its permanent memory since the personalization stage
    /// </summary>
    public bool PlaintextPinVerificationPerformedByIccForContactless() => _Value.IsBitSet(2);

    /// <summary>
    ///     Consumer Device Cardholder Verification Method
    /// </summary>
    /// <returns></returns>
    public bool CdCvmForContactless() => _Value.IsBitSet(3);

    /// <summary>
    ///     This method consists of accepting without proof that the person at the point of service is that to whom the card
    ///     was issued
    /// </summary>
    public bool NoCardVerificationMethodRequiredForContactless() => _Value.IsBitSet(4);

    /// <summary>
    ///     The terminal captures the PIN from the user and sends it encrypted in an RSA envelope to the chip card. The chip
    ///     decrypts the envelope, retrieves the PIN in clear, and compares the retrieved value with a witness value stored in
    ///     its permanent memory since the personalization stage.
    /// </summary>
    public bool EncipheredPinVerificationPerformedByIccForContactless() => _Value.IsBitSet(5);

    /// <summary>
    ///     This CVM can be applied for credit card products at a point of service that is attended by an operator. The method
    ///     consists of comparing the signature produced by the card user on the sales slip against the witness signature of
    ///     the cardholder written on the back side of the card
    /// </summary>
    public bool SignaturePaperForContactless() => _Value.IsBitSet(6);

    /// <summary>
    ///     The cardholder types his or her PIN in the terminal's PIN pad. The terminal encrypts it using a symmetric
    ///     encryption mechanism. The IH receives this cryptogram, decrypts it in a secure module, which computes a PIN image
    ///     control value that is compared against a witness value kept in the cardholder database, referred to as the PIN
    ///     image stored value
    /// </summary>
    public bool EncipheredPinVerifiedOnlineForContactless() => _Value.IsBitSet(7);

    public bool IsRestartSupported() => _Value.IsBitSet(8);

    #endregion

    #region Byte 2

    /// <summary>
    ///     The terminal captures the PIN from the user and sends it in clear text to the chip card. The chip compares the
    ///     value received with a witness value stored in its permanent memory since the personalization stage
    /// </summary>
    public bool PlaintextPinVerificationPerformedByIccForContact() => _Value.IsBitSet(10);

    public bool CdCvmForContact() => _Value.IsBitSet(11);

    /// <summary>
    ///     This method consists of accepting without proof that the person at the point of service is that to whom the card
    ///     was issued
    /// </summary>
    public bool NoCardholderVerificationMethodRequiredForContact() => _Value.IsBitSet(12);

    /// <summary>
    ///     The terminal captures the PIN from the user and sends it encrypted in an RSA envelope to the chip card. The chip
    ///     decrypts the envelope, retrieves the PIN in clear, and compares the retrieved value with a witness value stored in
    ///     its permanent memory since the personalization stage.
    /// </summary>
    public bool EncipheredPinVerificationPerformedByIccForContact() => _Value.IsBitSet(13);

    /// <summary>
    ///     This CVM can be applied for credit card products at a point of service that is attended by an operator. The method
    ///     consists of comparing the signature produced by the card user on the sales slip against the witness signature of
    ///     the cardholder written on the back side of the card
    /// </summary>
    public bool SignaturePaperForContact() => _Value.IsBitSet(14);

    /// <summary>
    ///     The cardholder types his or her PIN in the terminal's PIN pad. The terminal encrypts it using a symmetric
    ///     encryption mechanism. The IH receives this cryptogram, decrypts it in a secure module, which computes a PIN image
    ///     control value that is compared against a witness value kept in the cardholder database, referred to as the PIN
    ///     image stored value
    /// </summary>
    public bool EncipheredPinVerifiedOnlineForContact() => _Value.IsBitSet(15);

    public bool CvmLimitExceeded() => _Value.IsBitSet(16);

    #endregion

    #region Byte 3

    public bool CdCvmWithoutCdaSupported() => _Value.IsBitSet(22);
    public bool EmvModeContactlessTransactionsNotSupported() => _Value.IsBitSet(23);
    public bool MagstripeModeContactlessTransactionsNotSupported() => _Value.IsBitSet(24);
    public bool StaticDataAuthentication() => _Value.IsBitSet(24);

    #endregion

    #region Byte 4

    public bool ScaExempt() => _Value.IsBitSet(31);
    public bool CdCvmBypassRequested() => _Value.IsBitSet(32);

    #endregion
}