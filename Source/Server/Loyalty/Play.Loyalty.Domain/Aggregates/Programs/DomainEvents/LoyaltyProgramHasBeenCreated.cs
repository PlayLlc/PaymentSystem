using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyProgramHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly Programs Programs;

    #endregion

    #region Constructor

    public LoyaltyProgramHasBeenCreated(Programs programs, string merchantId) : base(
        $"A {nameof(Programs)} has been created with the ID: [{programs.Id}] for The {nameof(Merchant)} with the ID: [{merchantId}]")
    {
        Programs = programs;
    }

    #endregion
}