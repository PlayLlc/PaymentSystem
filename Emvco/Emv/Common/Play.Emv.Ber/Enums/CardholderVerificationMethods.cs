using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.Ber.Enums;

public record CardholderVerificationMethods : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CardholderVerificationMethods> _ValueObjectMap;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) plaintext for offline PIN verification is performed by the
    ///     terminal and delivered to the ICC without the additional encryption and tamper proof requirements of the Offline
    ///     Encrypted Pin CVM
    /// </summary>
    public static readonly CardholderVerificationMethods OfflinePlaintextPin;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment is performed by the terminal using an asymmetric
    ///     based encipherment mechanism in order to ensure the secure transfer of a PIN from a secure tamper-evident PIN pad
    ///     and sent to the Acquirer for validation
    /// </summary>
    public static readonly CardholderVerificationMethods OnlineEncipheredPin;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment for offline PIN verification is performed by the
    ///     terminal using an asymmetric based encipherment mechanism in order to ensure the secure transfer of a PIN from a
    ///     secure tamper-evident PIN pad to the ICC wrapped in RSA encryption.
    /// </summary>
    public static readonly CardholderVerificationMethods OfflineEncipheredPin;

    /// <summary>
    ///     Someone makes a half-assed attempt to sign their name on a terminal screen that results in a digital signature that
    ///     is unrecognizable from their actual signature. Cough* Cough*, signature validation is a pseudo science
    /// </summary>
    public static readonly CardholderVerificationMethods SignaturePaper;

    private const byte _UnrelatedBits = 0b11000000;

    #endregion

    #region Constructor

    static CardholderVerificationMethods()
    {
        OfflinePlaintextPin = new CardholderVerificationMethods(0b00000001);
        ;
        OnlineEncipheredPin = new CardholderVerificationMethods(0b00000010);
        OfflineEncipheredPin = new CardholderVerificationMethods(0b00000100);
        SignaturePaper = new CardholderVerificationMethods(0b00011110);

        _ValueObjectMap = new Dictionary<byte, CardholderVerificationMethods>
        {
            {OfflinePlaintextPin, OfflinePlaintextPin},
            {OnlineEncipheredPin, OnlineEncipheredPin},
            {OfflineEncipheredPin, OfflineEncipheredPin},
            {SignaturePaper, SignaturePaper}
        }.ToImmutableSortedDictionary();
    }

    private CardholderVerificationMethods(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="InvalidOperationException"></exception>
    public static CardholderVerificationMethods Get(CvmCode cvmCode)
    {
        if (_ValueObjectMap.TryGetValue(((byte) cvmCode).GetMaskedValue(_UnrelatedBits), out CardholderVerificationMethods? result))
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