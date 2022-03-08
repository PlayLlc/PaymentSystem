using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Play.Emv.DataElements.Emv;
// TODO: Terminal Category Codes
// TODO: https://wallethub.com/edu/cc/merchant-category-code/25837#:~:text=A%20transaction%20category%20code%20is,MCC%20then%20provides%20the%20specifics.

/// <summary>
///     Indicates the terminal category to which the terminal belongs. For terminals that do not belong to a terminal
///     category listed below, the Terminal Category ID L V is not present in the POI Information data object.
///     '00 01' = Transit gate; the terminal at the entrance or exit to a transit network(e.g., a metro gate) or
///     vehicle(e.g., a bus) that is used to accept cards for transit network access.This category does not include
///     terminals present in transit acceptance environments but that do not control access to the transit network(e.g.,
///     unattended ticketing kiosks). '00 02' = Loyalty; the terminal facilitates a loyalty program using POI Information.
///     All other values are RFU for this specification.
/// </summary>
public readonly struct TerminalCategoryCode
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<ushort, TerminalCategoryCode> _ValueObjectMap;

    /// <value>decimal: 2; hexadecimal: 0x02</value>
    public static readonly TerminalCategoryCode Loyalty;

    /// <value>decimal: 1; hexadecimal: 0x01</value>
    public static readonly TerminalCategoryCode TransitGate;

    /// <value>decimal: 0; hexadecimal: 0x00</value>
    public static readonly TerminalCategoryCode Unknown;

    #endregion

    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    static TerminalCategoryCode()
    {
        const ushort unknown = byte.MinValue;
        const ushort terminalGateValue = 1;
        const ushort loyaltyValue = 2;

        Unknown = new TerminalCategoryCode(0);
        TransitGate = new TerminalCategoryCode(1);
        Loyalty = new TerminalCategoryCode(2);

        _ValueObjectMap =
            new Dictionary<ushort, TerminalCategoryCode> {{unknown, Unknown}, {terminalGateValue, TransitGate}, {loyaltyValue, Loyalty}}
                .ToImmutableSortedDictionary();
    }

    private TerminalCategoryCode(ushort value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => Encode(_Value);

    public static TerminalCategoryCode Get(ReadOnlySpan<byte> value)
    {
        if (value.Length != 2)
            return Unknown;

        if (!_ValueObjectMap.ContainsKey((ushort) ((value[0] << 8) | value[1])))
            return Unknown;

        return _ValueObjectMap[(ushort) ((value[0] << 8) | value[1])];
    }

    public static TerminalCategoryCode Get(ushort value)
    {
        if (!_ValueObjectMap.ContainsKey(value))
            return Unknown;

        return _ValueObjectMap[value];
    }

    public static bool TryGet(ushort value, out TerminalCategoryCode result) => _ValueObjectMap.TryGetValue(value, out result);
    public static byte[] Encode(TerminalCategoryCode code) => Encode((ushort) code);

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

    public override bool Equals(object? obj) => obj is TerminalCategoryCode terminalCategory && Equals(terminalCategory);
    public bool Equals(TerminalCategoryCode other) => _Value == other._Value;
    public bool Equals(ushort other) => _Value == other;
    public bool Equals(TerminalCategoryCode x, TerminalCategoryCode y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 7354873;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(TerminalCategoryCode left, TerminalCategoryCode right) => left._Value == right._Value;
    public static bool operator ==(TerminalCategoryCode left, ushort right) => left._Value == right;
    public static bool operator ==(ushort left, TerminalCategoryCode right) => left == right._Value;
    public static explicit operator ushort(TerminalCategoryCode value) => value._Value;
    public static bool operator !=(TerminalCategoryCode left, TerminalCategoryCode right) => !(left == right);
    public static bool operator !=(TerminalCategoryCode left, ushort right) => !(left == right);
    public static bool operator !=(ushort left, TerminalCategoryCode right) => !(left == right);

    #endregion
}