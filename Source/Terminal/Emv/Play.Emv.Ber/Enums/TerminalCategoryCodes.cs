using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums;

/// <summary>
///     Indicates the terminal category to which the terminal belongs. For terminals that do not belong to a terminal
///     category listed below, the Terminal Category ID L V is not present in the POI Information data object.
///     '00 01' = Transit gate; the terminal at the entrance or exit to a transit network(e.g., a metro gate) or
///     vehicle(e.g., a bus) that is used to accept cards for transit network access.This category does not include
///     terminals present in transit acceptance environments but that do not control access to the transit network(e.g.,
///     unattended ticketing kiosks). '00 02' = Loyalty; the terminal facilitates a loyalty program using POI Information.
///     All other values are RFU for this specification.
/// </summary>
public record TerminalCategoryCodes : EnumObject<ushort>
{
    #region Static Metadata

    public static readonly TerminalCategoryCodes Empty = new();
    private static readonly ImmutableSortedDictionary<ushort, TerminalCategoryCodes> _ValueObjectMap;

    /// <value>decimal: 2; hexadecimal: 0x02</value>
    public static readonly TerminalCategoryCodes Loyalty;

    /// <value>decimal: 1; hexadecimal: 0x01</value>
    public static readonly TerminalCategoryCodes TransitGate;

    /// <value>decimal: 0; hexadecimal: 0x00</value>
    public static readonly TerminalCategoryCodes Unknown;

    #endregion

    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public TerminalCategoryCodes()
    { }

    static TerminalCategoryCodes()
    {
        const ushort unknown = byte.MinValue;
        const ushort terminalGateValue = 1;
        const ushort loyaltyValue = 2;

        Unknown = new TerminalCategoryCodes(0);
        TransitGate = new TerminalCategoryCodes(1);
        Loyalty = new TerminalCategoryCodes(2);

        _ValueObjectMap = new Dictionary<ushort, TerminalCategoryCodes> {{unknown, Unknown}, {terminalGateValue, TransitGate}, {loyaltyValue, Loyalty}}
            .ToImmutableSortedDictionary();
    }

    protected TerminalCategoryCodes(ushort value) : base(value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override TerminalCategoryCodes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(ushort value, out EnumObject<ushort>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out TerminalCategoryCodes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public byte[] AsByteArray() => Encode(_Value);

    public static TerminalCategoryCodes Get(ReadOnlySpan<byte> value)
    {
        if (value.Length != 2)
            return Unknown;

        if (!_ValueObjectMap.ContainsKey((ushort) ((value[0] << 8) | value[1])))
            return Unknown;

        return _ValueObjectMap[(ushort) ((value[0] << 8) | value[1])];
    }

    public static TerminalCategoryCodes Get(ushort value)
    {
        if (!_ValueObjectMap.ContainsKey(value))
            return Unknown;

        return _ValueObjectMap[value];
    }

    public static bool TryGet(ushort value, out TerminalCategoryCodes result) => _ValueObjectMap.TryGetValue(value, out result);
    public static byte[] Encode(TerminalCategoryCodes codes) => Encode((ushort) codes);

    private static byte[] Encode(ushort code)
    {
        byte[] buffer = new byte[5];

        buffer[0] = 0;
        buffer[1] = 1;
        buffer[2] = 2;
        buffer[3] = (byte) (code >> 8);
        buffer[4] = (byte) code;

        return buffer;
    }

    #endregion

    #region Equality

    public bool Equals(TerminalCategoryCodes x, TerminalCategoryCodes y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 7354873;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(TerminalCategoryCodes left, ushort right) => left._Value == right;
    public static bool operator ==(ushort left, TerminalCategoryCodes right) => left == right._Value;
    public static explicit operator ushort(TerminalCategoryCodes value) => value._Value;
    public static bool operator !=(TerminalCategoryCodes left, ushort right) => !(left == right);
    public static bool operator !=(ushort left, TerminalCategoryCodes right) => !(left == right);

    #endregion
}