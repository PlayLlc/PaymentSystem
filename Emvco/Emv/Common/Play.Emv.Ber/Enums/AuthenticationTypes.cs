using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums;

public record AuthenticationTypes : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, AuthenticationTypes> _ValueObjectMap;
    public static readonly AuthenticationTypes CombinedDataAuthentication;
    public static readonly AuthenticationTypes DynamicDataAuthentication;
    public static readonly AuthenticationTypes None;
    public static readonly AuthenticationTypes StaticDataAuthentication;

    #endregion

    #region Constructor

    static AuthenticationTypes()
    {
        const byte staticDataAuthentication = 1 << 1;
        const byte dynamicDataAuthentication = 1 << 2;
        const byte combinedDataAuthentication = 1 << 3;

        None = new AuthenticationTypes(0);
        StaticDataAuthentication = new AuthenticationTypes(staticDataAuthentication);
        DynamicDataAuthentication = new AuthenticationTypes(dynamicDataAuthentication);
        CombinedDataAuthentication = new AuthenticationTypes(combinedDataAuthentication);
        _ValueObjectMap = new Dictionary<byte, AuthenticationTypes>
        {
            {staticDataAuthentication, StaticDataAuthentication},
            {dynamicDataAuthentication, DynamicDataAuthentication},
            {combinedDataAuthentication, CombinedDataAuthentication}
        }.ToImmutableSortedDictionary();
    }

    private AuthenticationTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override AuthenticationTypes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out AuthenticationTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Equality

    public bool Equals(AuthenticationTypes x, AuthenticationTypes y) => x.Equals(y);
    public override int GetHashCode() => 470621 * _Value.GetHashCode();
    public int GetHashCode(AuthenticationTypes obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(AuthenticationTypes registeredApplicationProviderIndicators) => registeredApplicationProviderIndicators._Value;

    #endregion
}