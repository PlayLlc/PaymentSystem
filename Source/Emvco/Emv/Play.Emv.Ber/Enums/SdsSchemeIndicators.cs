using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.Ber.Enums;

public record SdsSchemeIndicators : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, SdsSchemeIndicators> _ValueObjectMap;
    public static readonly SdsSchemeIndicators Empty = new();
    public static readonly SdsSchemeIndicators All10tags128bytes;
    public static readonly SdsSchemeIndicators All10tags160bytes;
    public static readonly SdsSchemeIndicators All10tags192bytes;
    public static readonly SdsSchemeIndicators All10tags32bytes;
    public static readonly SdsSchemeIndicators All10tags48bytes;
    public static readonly SdsSchemeIndicators All10tags64bytes;
    public static readonly SdsSchemeIndicators All10tags96bytes;
    public static readonly SdsSchemeIndicators AllSdstags32bytesexcept;
    public static readonly SdsSchemeIndicators UndefinedSDSconfiguration;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public SdsSchemeIndicators()
    { }

    static SdsSchemeIndicators()
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

        All10tags128bytes = new SdsSchemeIndicators(all10tags128bytes);
        All10tags160bytes = new SdsSchemeIndicators(all10tags160bytes);
        All10tags192bytes = new SdsSchemeIndicators(all10tags192bytes);
        All10tags32bytes = new SdsSchemeIndicators(all10tags32bytes);
        All10tags48bytes = new SdsSchemeIndicators(all10tags48bytes);
        All10tags64bytes = new SdsSchemeIndicators(all10tags64bytes);
        All10tags96bytes = new SdsSchemeIndicators(all10tags96bytes);
        AllSdstags32bytesexcept = new SdsSchemeIndicators(allSdstags32bytesexcept);
        UndefinedSDSconfiguration = new SdsSchemeIndicators(undefinedSDSconfiguration);
        _ValueObjectMap = new Dictionary<byte, SdsSchemeIndicators>
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

    private SdsSchemeIndicators(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override SdsSchemeIndicators[] GetAll() => _ValueObjectMap.Values.ToArray();

    public static SdsSchemeIndicators Get(byte value)
    {
        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(SdsSchemeIndicators)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value];
    }

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out SdsSchemeIndicators? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Equality

    public bool Equals(SdsSchemeIndicators x, SdsSchemeIndicators y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 281273;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(SdsSchemeIndicators left, byte right) => left._Value == right;
    public static bool operator ==(byte left, SdsSchemeIndicators right) => left == right._Value;
    public static explicit operator byte(SdsSchemeIndicators value) => value._Value;
    public static explicit operator short(SdsSchemeIndicators value) => value._Value;
    public static explicit operator ushort(SdsSchemeIndicators value) => value._Value;
    public static explicit operator int(SdsSchemeIndicators value) => value._Value;
    public static explicit operator uint(SdsSchemeIndicators value) => value._Value;
    public static explicit operator long(SdsSchemeIndicators value) => value._Value;
    public static explicit operator ulong(SdsSchemeIndicators value) => value._Value;
    public static bool operator !=(SdsSchemeIndicators left, byte right) => !(left == right);
    public static bool operator !=(byte left, SdsSchemeIndicators right) => !(left == right);

    #endregion
}