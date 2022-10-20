using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record MerchantRegistrationRejected : DomainEvent
{
    #region Instance Values

    public readonly MerchantRegistration MerchantRegistration;

    #endregion

    #region Constructor

    public MerchantRegistrationRejected(MerchantRegistration merchantRegistration) : base(
        $"The {nameof(MerchantRegistration)} for the {nameof(Merchant)} with ID: [{merchantRegistration.GetId()}] has been rejected")
    {
        MerchantRegistration = merchantRegistration;
    }

    #endregion
}