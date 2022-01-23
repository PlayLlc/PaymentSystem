using System.Collections.Immutable;

using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public readonly struct SdsSchemeIndicator
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, SdsSchemeIndicator> _ValueObjectMap;
    public static readonly SdsSchemeIndicator All10tags128bytes;
    public static readonly SdsSchemeIndicator All10tags160bytes;
    public static readonly SdsSchemeIndicator All10tags192bytes;
    public static readonly SdsSchemeIndicator All10tags32bytes;
    public static readonly SdsSchemeIndicator All10tags48bytes;
    public static readonly SdsSchemeIndicator All10tags64bytes;
    public static readonly SdsSchemeIndicator All10tags96bytes;
    public static readonly SdsSchemeIndicator AllSdstags32bytesexcept;
    public static readonly SdsSchemeIndicator UndefinedSDSconfiguration;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static SdsSchemeIndicator()
    {
        const byte all10tags128bytes = 5;
        const byte all10tags160bytes = 6;
        const byte all10tags192bytes = 7;
        const byte all10tags32bytes = 1;
        const byte all10tags48bytes = 2;
        const byte all10tags64bytes = 3;
        const byte all10tags96bytes = 4;
        const byte allSdstags32bytesexcept = 8;
        const byte undefinedSDSconfiguration = 0;

        All10tags128bytes = new SdsSchemeIndicator(all10tags128bytes);
        All10tags160bytes = new SdsSchemeIndicator(all10tags160bytes);
        All10tags192bytes = new SdsSchemeIndicator(all10tags192bytes);
        All10tags32bytes = new SdsSchemeIndicator(all10tags32bytes);
        All10tags48bytes = new SdsSchemeIndicator(all10tags48bytes);
        All10tags64bytes = new SdsSchemeIndicator(all10tags64bytes);
        All10tags96bytes = new SdsSchemeIndicator(all10tags96bytes);
        AllSdstags32bytesexcept = new SdsSchemeIndicator(allSdstags32bytesexcept);
        UndefinedSDSconfiguration = new SdsSchemeIndicator(undefinedSDSconfiguration);
        _ValueObjectMap = new Dictionary<byte, SdsSchemeIndicator>
        {
            {all10tags128bytes, All10tags128bytes},
            {all10tags160bytes, All10tags160bytes},
            {all10tags192bytes, All10tags192bytes},
            {all10tags32bytes, All10tags32bytes},
            {all10tags48bytes, All10tags48bytes},
            {all10tags64bytes, All10tags64bytes},
            {all10tags96bytes, All10tags96bytes},
            {allSdstags32bytesexcept, AllSdstags32bytesexcept},
            {undefinedSDSconfiguration, UndefinedSDSconfiguration}
        }.ToImmutableSortedDictionary();
    }

    private SdsSchemeIndicator(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static SdsSchemeIndicator Get(byte value)
    {
        const byte bitMask = 0b11111100;

        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(SdsSchemeIndicator)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is SdsSchemeIndicator sdsSchemeIndicator && Equals(sdsSchemeIndicator);
    public bool Equals(SdsSchemeIndicator other) => _Value == other._Value;
    public bool Equals(SdsSchemeIndicator x, SdsSchemeIndicator y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 281273;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(SdsSchemeIndicator left, SdsSchemeIndicator right) => left._Value == right._Value;
    public static bool operator ==(SdsSchemeIndicator left, byte right) => left._Value == right;
    public static bool operator ==(byte left, SdsSchemeIndicator right) => left == right._Value;
    public static explicit operator byte(SdsSchemeIndicator value) => value._Value;
    public static explicit operator short(SdsSchemeIndicator value) => value._Value;
    public static explicit operator ushort(SdsSchemeIndicator value) => value._Value;
    public static explicit operator int(SdsSchemeIndicator value) => value._Value;
    public static explicit operator uint(SdsSchemeIndicator value) => value._Value;
    public static explicit operator long(SdsSchemeIndicator value) => value._Value;
    public static explicit operator ulong(SdsSchemeIndicator value) => value._Value;
    public static bool operator !=(SdsSchemeIndicator left, SdsSchemeIndicator right) => !(left == right);
    public static bool operator !=(SdsSchemeIndicator left, byte right) => !(left == right);
    public static bool operator !=(byte left, SdsSchemeIndicator right) => !(left == right);

    #endregion
}