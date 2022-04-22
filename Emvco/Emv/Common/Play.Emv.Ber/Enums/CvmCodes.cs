using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.Enums;

public record CvmCodes : EnumObject<byte> { public override CvmCodes[] GetAll() => _ValueObjectMap.Values.ToArray(); public override bool TryGet(byte value, out EnumObject<byte>? result) { if (_ValueObjectMap.TryGetValue(value, out CvmCodes? enumResult)) { result = enumResult; return true; } result = null; return false; }
 public CvmCodes() : base() { } public static readonly CvmCodes Empty = new(); 
#region Static Metadata

    public static readonly CvmCodes Empty = new();
    private static readonly ImmutableSortedDictionary<byte, CvmCodes> _ValueObjectMap;

    /// <remarks>Hex: 0x00; Decimal: 0; Binary: 0b00000000 </remarks>
    public static readonly CvmCodes Fail;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) plaintext for offline PIN verification is performed by the
    ///     terminal and delivered to the ICC without the additional encryption and tamper proof requirements of the Offline
    ///     Encrypted Pin CVM
    /// </summary>
    /// <remarks>Hex: 0x01; Decimal: 1; Binary: 0b00000001</remarks>
    public static readonly CvmCodes OfflinePlaintextPin;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment is performed by the terminal using an asymmetric
    ///     based encipherment mechanism in order to ensure the secure transfer of a PIN from a secure tamper-evident PIN pad
    ///     and sent to the Acquirer for validation
    /// </summary>
    /// <remarks>Hex: 0x02; Decimal: 2; Binary: 0b00000010</remarks>
    public static readonly CvmCodes OnlineEncipheredPin;

    /// <summary>
    ///     Both Offline Plaintext Pin and Signature verification are performed
    /// </summary>
    /// <remarks>Hex: 0x03; Decimal: 3; Binary: 0b00000011</remarks>
    public static readonly CvmCodes OfflinePlaintextPinAndSignature;

    /// <summary>
    ///     If supported, Personal Identification Number (PIN) encipherment for offline PIN verification is performed by the
    ///     terminal using an asymmetric based encipherment mechanism in order to ensure the secure transfer of a PIN from a
    ///     secure tamper-evident PIN pad to the ICC wrapped in RSA encryption.
    /// </summary>
    /// <remarks>Hex: 0x04; Decimal: 4; Binary: 0b00000100</remarks>
    public static readonly CvmCodes OfflineEncipheredPin;

    /// <summary>
    ///     Both Offline Enciphered Pin and Signature verification are performed
    /// </summary>
    /// <remarks>Hex: 05; Decimal: 5; Binary: 0b00000101</remarks>
    public static readonly CvmCodes OfflineEncipheredPinAndSignature;

    /// <summary>
    ///     Someone makes a half-assed attempt to sign their name on a terminal screen that results in a digital signature that
    ///     is unrecognizable from their actual signature. Cough* Cough*, signature validation is a pseudo science
    /// </summary>
    /// <remarks>Hex: 0x1E; Decimal: 30; Binary: 0b00011110</remarks>
    public static readonly CvmCodes SignaturePaper;

    /// <summary>
    ///     No CVM is required for this transaction
    /// </summary>
    /// <remarks>Hex: 0x1F; Decimal: 63; Binary: 0b00011111</remarks>
    public static readonly CvmCodes None;

    private const byte _UnrelatedBits = 0b11000000;

    #endregion

    #region Constructor

    public CvmCodes() : base()
    { }

    static CvmCodes()
    {
        Fail = new CvmCodes(0b00000000);
        OfflinePlaintextPin = new CvmCodes(0b00000001);
        OnlineEncipheredPin = new CvmCodes(0b00000010);
        OfflinePlaintextPinAndSignature = new CvmCodes(0b00000011);
        OfflineEncipheredPin = new CvmCodes(0b00000100);
        OfflineEncipheredPinAndSignature = new CvmCodes(0b00000101);
        SignaturePaper = new CvmCodes(0b00011110);
        None = new CvmCodes(0b00011111);

        _ValueObjectMap = new Dictionary<byte, CvmCodes>
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

    private CvmCodes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override CvmCodes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out CvmCodes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }
     

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

    public static implicit operator CvmCode(CvmCodes value) => new(value._Value);
    public static bool operator ==(CvmCodes left, CvmCode right) => left.Equals(right);
    public static bool operator !=(CvmCodes left, CvmCode right) => !left.Equals(right);
    public static bool operator ==(CvmCode left, CvmCodes right) => right.Equals(left);
    public static bool operator !=(CvmCode left, CvmCodes right) => !right.Equals(left);

    #endregion
}