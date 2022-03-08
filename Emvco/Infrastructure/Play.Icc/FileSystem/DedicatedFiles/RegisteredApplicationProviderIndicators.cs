using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;
using Play.Icc.Exceptions;

namespace Play.Icc.FileSystem.DedicatedFiles;

public sealed record RegisteredApplicationProviderIndicators : EnumObject<RegisteredApplicationProviderIndicator>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<ulong, RegisteredApplicationProviderIndicators> _ValueObjectMap;
    public static readonly RegisteredApplicationProviderIndicators AmericanExpress;
    public static readonly RegisteredApplicationProviderIndicators ChinaUnionPay;
    public static readonly RegisteredApplicationProviderIndicators Discover;
    public static readonly RegisteredApplicationProviderIndicators Jcb;
    public static readonly RegisteredApplicationProviderIndicators MasterCard;
    public static readonly RegisteredApplicationProviderIndicators VisaInternational;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static RegisteredApplicationProviderIndicators()
    {
        const ulong americanExpress = 0xA000000025;
        const ulong discover = 0xA000000324;
        const ulong jcb = 0xA000000065;
        const ulong masterCard = 0xA000000004;
        const ulong chinaUnionPay = 0xA000000333;
        const ulong visaInternational = 0xA000000003;

        AmericanExpress = new RegisteredApplicationProviderIndicators(new RegisteredApplicationProviderIndicator(americanExpress));
        Discover = new RegisteredApplicationProviderIndicators(new RegisteredApplicationProviderIndicator(discover));
        Jcb = new RegisteredApplicationProviderIndicators(new RegisteredApplicationProviderIndicator(jcb));
        MasterCard = new RegisteredApplicationProviderIndicators(new RegisteredApplicationProviderIndicator(masterCard));
        ChinaUnionPay = new RegisteredApplicationProviderIndicators(new RegisteredApplicationProviderIndicator(chinaUnionPay));
        VisaInternational = new RegisteredApplicationProviderIndicators(new RegisteredApplicationProviderIndicator(visaInternational));

        _ValueObjectMap = new Dictionary<ulong, RegisteredApplicationProviderIndicators>
        {
            {americanExpress, AmericanExpress},
            {discover, Discover},
            {jcb, Jcb},
            {masterCard, MasterCard},
            {chinaUnionPay, ChinaUnionPay},
            {visaInternational, VisaInternational}
        }.ToImmutableSortedDictionary();
    }

    private RegisteredApplicationProviderIndicators(RegisteredApplicationProviderIndicator value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo(RegisteredApplicationProviderIndicators other) => _Value.CompareTo(other._Value);

    public static bool TryGet(RegisteredApplicationProviderIndicator value, out RegisteredApplicationProviderIndicators? result) =>
        _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(RegisteredApplicationProviderIndicators? other) => !(other is null) && (_Value == other._Value);

    public bool Equals(RegisteredApplicationProviderIndicators x, RegisteredApplicationProviderIndicators y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        const int hash = 7354873;

        return hash + (_Value.GetHashCode() * 3);
    }

    public int GetHashCode(RegisteredApplicationProviderIndicators obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(RegisteredApplicationProviderIndicator left, RegisteredApplicationProviderIndicators right) =>
        right.Equals(left);

    public static bool operator ==(RegisteredApplicationProviderIndicators left, RegisteredApplicationProviderIndicator right) =>
        left.Equals(right);

    public static explicit operator RegisteredApplicationProviderIndicator(
        RegisteredApplicationProviderIndicators registeredApplicationProviderIndicators) =>
        registeredApplicationProviderIndicators._Value;

    public static explicit operator RegisteredApplicationProviderIndicators(
        RegisteredApplicationProviderIndicator registeredApplicationProviderIndicator)
    {
        if (!TryGet(registeredApplicationProviderIndicator, out RegisteredApplicationProviderIndicators result))
        {
            throw new IccProtocolException(new ArgumentOutOfRangeException(nameof(registeredApplicationProviderIndicator),
                $"The {nameof(RegisteredApplicationProviderIndicators)} could not be found from the number supplied to the argument: {registeredApplicationProviderIndicator}"));
        }

        return result;
    }

    public static bool operator !=(RegisteredApplicationProviderIndicator left, RegisteredApplicationProviderIndicators right) =>
        !right.Equals(left);

    public static bool operator !=(RegisteredApplicationProviderIndicators left, RegisteredApplicationProviderIndicator right) =>
        !(left == right);

    #endregion
}