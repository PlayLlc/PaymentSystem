using System.Collections.Immutable;

using Play.Core;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Enums;

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
        Empty = new("");
        WaitingForRiskAnalysis = new(nameof(WaitingForRiskAnalysis));
        Approved = new(nameof(Approved));
        Expired = new(nameof(Expired));
        Rejected = new(nameof(Rejected));

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

    public override MerchantRegistrationStatuses[] GetAll() => _ValueObjectMap.Values.ToArray();

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

    public static implicit operator MerchantRegistrationStatus(MerchantRegistrationStatuses value) => new(value);

    #endregion
}