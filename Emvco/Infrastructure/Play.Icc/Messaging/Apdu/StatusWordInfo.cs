using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Icc.Messaging.Apdu;

public sealed record StatusWordInfo : EnumObject<byte>, IEqualityComparer<StatusWordInfo>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, StatusWordInfo> _ValueObjectMap;
    public static readonly StatusWordInfo Error;
    public static readonly StatusWordInfo Info;
    public static readonly StatusWordInfo Security;
    public static readonly StatusWordInfo Unavailable;
    public static readonly StatusWordInfo Warning;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static StatusWordInfo()
    {
        const byte unavailable = 0;
        const byte info = 1;
        const byte warning = 2;
        const byte error = 3;
        const byte security = 4;

        Unavailable = new StatusWordInfo(unavailable);
        Info = new StatusWordInfo(info);
        Warning = new StatusWordInfo(warning);
        Error = new StatusWordInfo(error);
        Security = new StatusWordInfo(security);

        _ValueObjectMap = new Dictionary<byte, StatusWordInfo>
        {
            {unavailable, Unavailable}, {info, Info}, {warning, Warning}, {error, Error}, {security, Security}
        }.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    private StatusWordInfo(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool TryGet(byte value, out StatusWordInfo? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(StatusWordInfo? other) => other is not null && (_Value == other._Value);

    public bool Equals(StatusWordInfo? x, StatusWordInfo? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(StatusWordInfo other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 48677;

        return hash + (_Value.GetHashCode() * 3);
    }

    #endregion

    #region Operator Overrides

    public static explicit operator byte(StatusWordInfo statusWordInfo) => statusWordInfo._Value;

    #endregion
}