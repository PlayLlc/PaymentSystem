using System.Collections.Immutable;

using Play.Core.Extensions;

namespace Play.Emv.Ber;

public readonly struct CvmPerformedOutcome
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CvmPerformedOutcome> _ValueObjectMap;

    /// <value>Decimal: 48; HexadecimalCodec: 0x30</value>
    public static readonly CvmPerformedOutcome ConfirmationCodeVerified;

    /// <value>Decimal: 0; HexadecimalCodec: 0x00</value>
    public static readonly CvmPerformedOutcome NoCvm;

    /// <value>Decimal: 240; HexadecimalCodec: 0xF0</value>
    public static readonly CvmPerformedOutcome NotAvailable;

    /// <value>Decimal: 16; HexadecimalCodec: 0x10</value>
    public static readonly CvmPerformedOutcome ObtainSignature;

    /// <value>Decimal: 32; HexadecimalCodec: 0x20</value>
    public static readonly CvmPerformedOutcome OnlinePin;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static CvmPerformedOutcome()
    {
        const byte noCvm = 0;
        const byte obtainSignature = 16;
        const byte onlinePin = 32;
        const byte confirmationCodeVerified = 48;
        const byte notAvailable = 240;

        NoCvm = new CvmPerformedOutcome(noCvm);
        NotAvailable = new CvmPerformedOutcome(notAvailable);
        ObtainSignature = new CvmPerformedOutcome(obtainSignature);
        OnlinePin = new CvmPerformedOutcome(onlinePin);
        ConfirmationCodeVerified = new CvmPerformedOutcome(confirmationCodeVerified);

        _ValueObjectMap = new Dictionary<byte, CvmPerformedOutcome>
        {
            {noCvm, NoCvm},
            {notAvailable, NotAvailable},
            {obtainSignature, ObtainSignature},
            {onlinePin, OnlinePin},
            {confirmationCodeVerified, ConfirmationCodeVerified}
        }.ToImmutableSortedDictionary();
    }

    private CvmPerformedOutcome(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static CvmPerformedOutcome[] GetAllValues() => _ValueObjectMap.Values.ToArray();

    public static CvmPerformedOutcome Get(byte value)
    {
        const byte bitMask = 0b11111100;

        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(CvmPerformedOutcome)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is CvmPerformedOutcome cVm && Equals(cVm);
    public bool Equals(CvmPerformedOutcome other) => _Value == other._Value;
    public bool Equals(CvmPerformedOutcome x, CvmPerformedOutcome y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 202777;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(CvmPerformedOutcome left, CvmPerformedOutcome right) => left._Value == right._Value;
    public static bool operator ==(CvmPerformedOutcome left, byte right) => left._Value == right;
    public static bool operator ==(byte left, CvmPerformedOutcome right) => left == right._Value;
    public static explicit operator byte(CvmPerformedOutcome value) => value._Value;
    public static explicit operator short(CvmPerformedOutcome value) => value._Value;
    public static explicit operator ushort(CvmPerformedOutcome value) => value._Value;
    public static explicit operator int(CvmPerformedOutcome value) => value._Value;
    public static explicit operator uint(CvmPerformedOutcome value) => value._Value;
    public static explicit operator long(CvmPerformedOutcome value) => value._Value;
    public static explicit operator ulong(CvmPerformedOutcome value) => value._Value;
    public static bool operator !=(CvmPerformedOutcome left, CvmPerformedOutcome right) => !(left == right);
    public static bool operator !=(CvmPerformedOutcome left, byte right) => !(left == right);
    public static bool operator !=(byte left, CvmPerformedOutcome right) => !(left == right);

    #endregion
}