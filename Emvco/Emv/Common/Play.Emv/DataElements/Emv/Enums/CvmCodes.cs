using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

public record CardholderVerificationMethod : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CardholderVerificationMethod> _ValueObjectMap;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) plaintext for offline PIN verification is performed by the
    ///     terminal and delivered to the ICC without the additional encryption and tamper proof requirements of the Offline
    ///     Encrypted Pin CVM
    /// </summary>
    public static readonly CardholderVerificationMethod OfflinePlaintextPin;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment is performed by the terminal using an asymmetric
    ///     based encipherment mechanism in order to ensure the secure transfer of a PIN from a secure tamper-evident PIN pad
    ///     and sent to the Acquirer for validation
    /// </summary>
    public static readonly CardholderVerificationMethod OnlineEncipheredPin;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment for offline PIN verification is performed by the
    ///     terminal using an asymmetric based encipherment mechanism in order to ensure the secure transfer of a PIN from a
    ///     secure tamper-evident PIN pad to the ICC wrapped in RSA encryption.
    /// </summary>
    public static readonly CardholderVerificationMethod OfflineEncipheredPin;

    /// <summary>
    ///     Someone makes a half-assed attempt to sign their name on a terminal screen that results in a digital signature that
    ///     is unrecognizable from their actual signature. Cough* Cough*, signature validation is a pseudo science
    /// </summary>
    public static readonly CardholderVerificationMethod SignaturePaper;

    private const byte _UnrelatedBits = 0b11000000;

    #endregion

    #region Constructor

    static CardholderVerificationMethod()
    {
        OfflinePlaintextPin = new CardholderVerificationMethod(0b00000001);
        ;
        OnlineEncipheredPin = new CardholderVerificationMethod(0b00000010);
        OfflineEncipheredPin = new CardholderVerificationMethod(0b00000100);
        SignaturePaper = new CardholderVerificationMethod(0b00011110);

        _ValueObjectMap = new Dictionary<byte, CardholderVerificationMethod>
        {
            {OfflinePlaintextPin, OfflinePlaintextPin},
            {OnlineEncipheredPin, OnlineEncipheredPin},
            {OfflineEncipheredPin, OfflineEncipheredPin},
            {SignaturePaper, SignaturePaper}
        }.ToImmutableSortedDictionary();
    }

    private CardholderVerificationMethod(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="InvalidOperationException"></exception>
    public static CardholderVerificationMethod Get(CvmCode cvmCode)
    {
        if (_ValueObjectMap.TryGetValue(((byte) cvmCode).GetMaskedValue(_UnrelatedBits), out CardholderVerificationMethod? result))
            throw new InvalidOperationException($"The {nameof(cvmCode)} with the value: [{cvmCode}] could not be recognized");

        return result!;
    }

    public static bool Exists(byte value) => _ValueObjectMap.ContainsKey(value);
    public static bool Exists(CvmCode value) => _ValueObjectMap.ContainsKey((byte) value);

    #endregion

    #region Equality

    public bool Equals(CvmCode value) => _Value == ((byte) value).GetMaskedValue(_UnrelatedBits);

    #endregion
}

public record CvmCodes : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CvmCodes> _ValueObjectMap;
    public static readonly CvmCodes Fail;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) plaintext for offline PIN verification is performed by the
    ///     terminal and delivered to the ICC without the additional encryption and tamper proof requirements of the Offline
    ///     Encrypted Pin CVM
    /// </summary>
    public static readonly CvmCodes OfflinePlaintextPin;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment is performed by the terminal using an asymmetric
    ///     based encipherment mechanism in order to ensure the secure transfer of a PIN from a secure tamper-evident PIN pad
    ///     and sent to the Acquirer for validation
    /// </summary>
    public static readonly CvmCodes OnlineEncipheredPin;

    /// <summary>
    ///     Both Offline Plaintext Pin and Signature verification are performed
    /// </summary>
    public static readonly CvmCodes OfflinePlaintextPinAndSignature;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment for offline PIN verification is performed by the
    ///     terminal using an asymmetric based encipherment mechanism in order to ensure the secure transfer of a PIN from a
    ///     secure tamper-evident PIN pad to the ICC wrapped in RSA encryption.
    /// </summary>
    public static readonly CvmCodes OfflineEncipheredPin;

    public static readonly CvmCodes OfflineEncipheredPinAndSignature;
    public static readonly CvmCodes SignaturePaper;
    public static readonly CvmCodes NoCvmRequired;
    private const byte _UnrelatedBits = 0b11000000;

    #endregion

    #region Constructor

    static CvmCodes()
    {
        Fail = new CvmCodes(0b00000000);
        OfflinePlaintextPin = new CvmCodes(0b00000001);
        ;
        OnlineEncipheredPin = new CvmCodes(0b00000010);
        OfflinePlaintextPinAndSignature = new CvmCodes(0b00000011);
        OfflineEncipheredPin = new CvmCodes(0b00000100);
        OfflineEncipheredPinAndSignature = new CvmCodes(0b00000101);
        SignaturePaper = new CvmCodes(0b00011110);
        NoCvmRequired = new CvmCodes(0b00011111);

        _ValueObjectMap = new Dictionary<byte, CvmCodes>
        {
            {Fail, Fail},
            {OfflinePlaintextPin, OfflinePlaintextPin},
            {OnlineEncipheredPin, OnlineEncipheredPin},
            {OfflinePlaintextPinAndSignature, OfflinePlaintextPinAndSignature},
            {OfflineEncipheredPin, OfflineEncipheredPin},
            {OfflineEncipheredPinAndSignature, OfflineEncipheredPinAndSignature},
            {SignaturePaper, SignaturePaper},
            {NoCvmRequired, NoCvmRequired}
        }.ToImmutableSortedDictionary();
    }

    private CvmCodes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="InvalidOperationException"></exception>
    public static CvmCodes Get(CvmCode cvmCode)
    {
        if (_ValueObjectMap.TryGetValue(((byte) cvmCode).GetMaskedValue(_UnrelatedBits), out CvmCodes? result))
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

    public static explicit operator CvmCode(CvmCodes value) => new(value._Value);
    public static bool operator ==(CvmCodes left, CvmCode right) => left.Equals(right);
    public static bool operator !=(CvmCodes left, CvmCode right) => !left.Equals(right);
    public static bool operator ==(CvmCode left, CvmCodes right) => right.Equals(left);
    public static bool operator !=(CvmCode left, CvmCodes right) => !right.Equals(left);

    #endregion
}