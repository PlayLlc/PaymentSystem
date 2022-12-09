using Play.Domain.Events;

namePlay.Loyalty.Domain.Entitiesgregates;

public record LoyaltyProgramHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly Programs Programs;

    #endregion

    #region Constructor

    public LoyaltyProgramHasBeenCreated(Programs programs, string merchantId) : base(
        $"A {nameof(Programs)} has 
        ed with the ID: [{programs.Id}] for The {nameof(Merchant)} with the ID: [{merchantId}]")
    {
        Programs = programs;
    }

    #endregion
}