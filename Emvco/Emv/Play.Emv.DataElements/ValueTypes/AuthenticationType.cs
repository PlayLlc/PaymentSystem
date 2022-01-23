using System.Collections.Immutable;

using Play.Core;

namespace ___TEMP.Play.Emv.Security.Authentications;

public record AuthenticationType : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, AuthenticationType> _ValueObjectMap;
    public static readonly AuthenticationType CombinedDataAuthentication;
    public static readonly AuthenticationType DynamicDataAuthentication;
    public static readonly AuthenticationType None;
    public static readonly AuthenticationType StaticDataAuthentication;

    #endregion

    #region Constructor

    static AuthenticationType()
    {
        const byte staticDataAuthentication = 1 << 1;
        const byte dynamicDataAuthentication = 1 << 2;
        const byte combinedDataAuthentication = 1 << 3;

        None = new AuthenticationType(0);
        StaticDataAuthentication = new AuthenticationType(staticDataAuthentication);
        DynamicDataAuthentication = new AuthenticationType(dynamicDataAuthentication);
        CombinedDataAuthentication = new AuthenticationType(combinedDataAuthentication);
        _ValueObjectMap = new Dictionary<byte, AuthenticationType>
        {
            {staticDataAuthentication, StaticDataAuthentication}, {dynamicDataAuthentication, DynamicDataAuthentication},
            {combinedDataAuthentication, CombinedDataAuthentication}
        }.ToImmutableSortedDictionary();
    }

    private AuthenticationType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool TryGet(byte value, out AuthenticationType result)
    {
        return _ValueObjectMap.TryGetValue(value, out result);
    }

    #endregion

    #region Equality

    public bool Equals(AuthenticationType x, AuthenticationType y)
    {
        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        return 470621 * _Value.GetHashCode();
    }

    public int GetHashCode(AuthenticationType obj)
    {
        return obj.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static explicit operator byte(AuthenticationType registeredApplicationProviderIndicators)
    {
        return registeredApplicationProviderIndicators._Value;
    }

    #endregion
}