using Play.Domain.Aggregates;
using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Services;

namespace Play.Identity.Domain.Aggregates.Rules;

/// <summary>
///     When a user updates their address we have to make sure that we are allowed to operate at the new location and that
///     the location has not been flagged by government or regulatory bodies for sanctions, terrorism, money laundering,
///     and other prohibited behavior
/// </summary>
internal class UserMustNotBeProhibited : BusinessRule<User, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "User must not be prohibited by regulatory or government entities";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    internal UserMustNotBeProhibited(IUnderwriteMerchants merchantUnderwriter, PersonalDetail personalDetail, Address address, Contact contact)
    {
        Task<bool> isUserProhibited = merchantUnderwriter.IsUserProhibited(personalDetail, address, contact);
        Task.WhenAll(isUserProhibited);
        _IsValid = isUserProhibited.Result;
    }

    #endregion

    #region Instance Members

    public override UserIsProhibited CreateBusinessRuleViolationDomainEvent(User merchant) => new UserIsProhibited(merchant, this);

    public override bool IsBroken() => _IsValid;

    #endregion
}