using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

public record CvmCodes : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CvmCodes> _ValueObjectMap;
    public static readonly CvmCodes Fail;

    /// <summary>
    ///     The PIN is entered in plaintext format on the PIN pad, encrypted and sent tot the terminal
    /// </summary>
    public static readonly CvmCodes OfflinePlaintextPin;

    public static readonly CvmCodes OnlineEncipheredPin;
    public static readonly CvmCodes OfflinePlaintextPinAndSignature;

    /// <summary>
    ///     The PIN is entered on a tamper proof secure device and handled by the ICC for verification
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