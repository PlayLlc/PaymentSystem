using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public record CardholderVerificationMethodCodes : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CardholderVerificationMethodCodes> _ValueObjectMap;

    /// <remarks>HEx: 0x00; Decimal: 0</remarks>
    public static readonly CardholderVerificationMethodCodes Fail;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) plaintext for offline PIN verification is performed by the
    ///     terminal and delivered to the ICC without the additional encryption and tamper proof requirements of the Offline
    ///     Encrypted Pin CVM
    /// </summary>
    /// <remarks>HEx: 0x01; Decimal: 1</remarks>
    public static readonly CardholderVerificationMethodCodes OfflinePlaintextPin;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment is performed by the terminal using an asymmetric
    ///     based encipherment mechanism in order to ensure the secure transfer of a PIN from a secure tamper-evident PIN pad
    ///     and sent to the Acquirer for validation
    /// </summary>
    /// <remarks>HEx: 0x02; Decimal: 2</remarks>
    public static readonly CardholderVerificationMethodCodes OnlineEncipheredPin;

    /// <summary>
    ///     Both Offline Plaintext Pin and Signature verification are performed
    /// </summary>
    /// <remarks>Hex: 0x03; Decimal: 3</remarks>
    public static readonly CardholderVerificationMethodCodes OfflinePlaintextPinAndSignature;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment for offline PIN verification is performed by the
    ///     terminal using an asymmetric based encipherment mechanism in order to ensure the secure transfer of a PIN from a
    ///     secure tamper-evident PIN pad to the ICC wrapped in RSA encryption.
    /// </summary>
    /// <remarks>HEx: 0x04; Decimal: 4</remarks>
    public static readonly CardholderVerificationMethodCodes OfflineEncipheredPin;

    /// <summary>
    ///     Both Offline Enciphered Pin and Signature verification are performed
    /// </summary>
    /// <remarks>HEx: 05; Decimal: 5</remarks>
    public static readonly CardholderVerificationMethodCodes OfflineEncipheredPinAndSignature;

    /// <summary>
    ///     Someone makes a half-assed attempt to sign their name on a terminal screen that results in a digital signature that
    ///     is unrecognizable from their actual signature. Cough* Cough*, signature validation is a pseudo science
    /// </summary>
    /// <remarks>HEx: 0x1E; Decimal: 30</remarks>
    public static readonly CardholderVerificationMethodCodes SignaturePaper;

    /// <summary>
    ///     No CVM is required for this transaction
    /// </summary>
    /// <remarks>HEx: 0x3F; Decimal: 63</remarks>
    public static readonly CardholderVerificationMethodCodes None;

    private const byte _UnrelatedBits = 0b11000000;

    #endregion

    #region Constructor

    static CardholderVerificationMethodCodes()
    {
        Fail = new CardholderVerificationMethodCodes(0b00000000);
        OfflinePlaintextPin = new CardholderVerificationMethodCodes(0b00000001);
        OnlineEncipheredPin = new CardholderVerificationMethodCodes(0b00000010);
        OfflinePlaintextPinAndSignature = new CardholderVerificationMethodCodes(0b00000011);
        OfflineEncipheredPin = new CardholderVerificationMethodCodes(0b00000100);
        OfflineEncipheredPinAndSignature = new CardholderVerificationMethodCodes(0b00000101);
        SignaturePaper = new CardholderVerificationMethodCodes(0b00011110);
        None = new CardholderVerificationMethodCodes(0b00011111);

        _ValueObjectMap = new Dictionary<byte, CardholderVerificationMethodCodes>
        {
            {Fail, Fail},
            {OfflinePlaintextPin, OfflinePlaintextPin},
            {OnlineEncipheredPin, OnlineEncipheredPin},
            {OfflinePlaintextPinAndSignature, OfflinePlaintextPinAndSignature},
            {OfflineEncipheredPin, OfflineEncipheredPin},
            {OfflineEncipheredPinAndSignature, OfflineEncipheredPinAndSignature},
            {SignaturePaper, SignaturePaper},
            {None, None}
        }.ToImmutableSortedDictionary();
    }

    private CardholderVerificationMethodCodes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="InvalidOperationException"></exception>
    public static CardholderVerificationMethodCodes Get(CvmCode cvmCode)
    {
        if (_ValueObjectMap.TryGetValue(((byte) cvmCode).GetMaskedValue(_UnrelatedBits), out CardholderVerificationMethodCodes? result))
            throw new InvalidOperationException($"The {nameof(cvmCode)} with the value: [{cvmCode}] could not be recognized");

        return result!;
    }

    public static bool Exists(byte value) => _ValueObjectMap.ContainsKey(value);
    public static bool Exists(CvmCode value) => _ValueObjectMap.ContainsKey((byte) value);

    #endregion

    #region Equality

    public bool Equals(CvmCode value) => _Value == ((byte) value).GetMaskedValue(_UnrelatedBits);

    #endregion

    #region Operator Overrides

    public static implicit operator CvmCode(CardholderVerificationMethodCodes value) => new(value._Value);
    public static bool operator ==(CardholderVerificationMethodCodes left, CvmCode right) => left.Equals(right);
    public static bool operator !=(CardholderVerificationMethodCodes left, CvmCode right) => !left.Equals(right);
    public static bool operator ==(CvmCode left, CardholderVerificationMethodCodes right) => right.Equals(left);
    public static bool operator !=(CvmCode left, CardholderVerificationMethodCodes right) => !right.Equals(left);

    #endregion
}