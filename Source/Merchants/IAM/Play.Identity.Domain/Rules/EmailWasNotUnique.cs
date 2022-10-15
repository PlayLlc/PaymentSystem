using Play.Domain.Events;

namespace Play.Identity.Domain.Rules;

public record EmailWasNotUnique : DomainEvent
{ }