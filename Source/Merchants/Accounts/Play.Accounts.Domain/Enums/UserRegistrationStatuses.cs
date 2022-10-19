using System.Collections.Immutable;

using Play.Accounts.Domain.ValueObjects;
using Play.Core;

namespace Play.Accounts.Domain.Enums;

public record MerchantRegistrationStatuses : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, MerchantRegistrationStatuses> _ValueObjectMap;
    public static MerchantRegistrationStatuses Empty;
    public static MerchantRegistrationStatuses WaitingForRiskAnalysis;
    public static MerchantRegistrationStatuses Approved;
    public static MerchantRegistrationStatuses Expired;
    public static MerchantRegistrationStatuses Rejected;

    #endregion

    #region Constructor

    private MerchantRegistrationStatuses(string value) : base(value)
    { }

    static MerchantRegistrationStatuses()
    {
        Empty = new MerchantRegistrationStatuses("");
        WaitingForRiskAnalysis = new MerchantRegistrationStatuses(nameof(WaitingForRiskAnalysis));
        Approved = new MerchantRegistrationStatuses(nameof(Approved));
        Expired = new MerchantRegistrationStatuses(nameof(Expired));
        Rejected = new MerchantRegistrationStatuses(nameof(Rejected));

        _ValueObjectMap = new Dictionary<string, MerchantRegistrationStatuses>
        {
            {WaitingForRiskAnalysis, WaitingForRiskAnalysis},
            {Approved, Approved},
            {Expired, Expired},
            {Rejected, Rejected}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override MerchantRegistrationStatuses[] GetAll()
    {
        return _ValueObjectMap.Values.ToArray();
    }

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out MerchantRegistrationStatuses? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator MerchantRegistrationStatus(MerchantRegistrationStatuses value)
    {
        return new(value);
    }

    #endregion
}

public record UserRegistrationStatuses : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, UserRegistrationStatuses> _ValueObjectMap;
    public static UserRegistrationStatuses Empty;
    public static UserRegistrationStatuses WaitingForRiskAnalysis;
    public static UserRegistrationStatuses WaitingForEmailVerification;
    public static UserRegistrationStatuses WaitingForSmsVerification;
    public static UserRegistrationStatuses Approved;
    public static UserRegistrationStatuses Expired;
    public static UserRegistrationStatuses Rejected;

    #endregion

    #region Constructor

    private UserRegistrationStatuses(string value) : base(value)
    { }

    static UserRegistrationStatuses()
    {
        Empty = new UserRegistrationStatuses("");
        WaitingForRiskAnalysis = new UserRegistrationStatuses(nameof(WaitingForRiskAnalysis));
        WaitingForEmailVerification = new UserRegistrationStatuses(nameof(WaitingForEmailVerification));
        WaitingForSmsVerification = new UserRegistrationStatuses(nameof(WaitingForSmsVerification));
        Approved = new UserRegistrationStatuses(nameof(Approved));
        Expired = new UserRegistrationStatuses(nameof(Expired));
        Rejected = new UserRegistrationStatuses(nameof(Rejected));

        _ValueObjectMap = new Dictionary<string, UserRegistrationStatuses>
        {
            {WaitingForRiskAnalysis, WaitingForRiskAnalysis},
            {WaitingForEmailVerification, WaitingForEmailVerification},
            {WaitingForSmsVerification, WaitingForSmsVerification},
            {Approved, Approved},
            {Expired, Expired},
            {Rejected, Rejected}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override UserRegistrationStatuses[] GetAll()
    {
        return _ValueObjectMap.Values.ToArray();
    }

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out UserRegistrationStatuses? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator UserRegistrationStatus(UserRegistrationStatuses value)
    {
        return new(value);
    }

    #endregion
}