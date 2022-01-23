using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Icc.Messaging.Apdu;

public sealed record StatusWord2 : EnumObject<StatusWord>, IEqualityComparer<StatusWord2>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<StatusWord, StatusWord2> _ValueObjectMap;

    /// <summary>
    ///     The Status Word 2 value is unknown to this code base
    /// </summary>
    public static readonly StatusWord2 Unknown;

    #endregion

    #region Instance Values

    private readonly string _Description;
    private readonly StatusWordInfo _StatusWordInfo;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static StatusWord2()
    {
        const byte unknown = 0;

        Unknown = new StatusWord2(unknown, StatusWordInfo.Info, $"The {nameof(StatusWord2)} value is unknown to this code base");

        _ValueObjectMap = GetValues(typeof(StatusWord2)).ToImmutableSortedDictionary(a => a.Key, b => (StatusWord2) b.Value);
    }

    private StatusWord2(StatusWord value, StatusWordInfo statusWordInfo, string description = "") : base(value)
    {
        _StatusWordInfo = statusWordInfo;
        _Description = description;
    }

    #endregion

    #region Instance Members

    public string GetDescription() => _Description;
    public StatusWordInfo GetStatusWordInfo() => _StatusWordInfo;
    public static bool TryGet(StatusWord value, out StatusWord2? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(StatusWord2? other) => other is not null && (_Value == other._Value);

    public bool Equals(StatusWord2? x, StatusWord2? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(StatusWord2 other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(63247 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static explicit operator byte(StatusWord2 statusWord2) => statusWord2._Value;

    #endregion
}