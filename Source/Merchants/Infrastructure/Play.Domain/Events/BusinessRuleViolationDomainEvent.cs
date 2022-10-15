using System.Text.Json;

using Play.Domain.Aggregates;

namespace Play.Domain.Events;

public abstract record BusinessRuleViolationDomainEvent<_Aggregate, _TId> : DomainEvent where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Constructor

    protected BusinessRuleViolationDomainEvent(_Aggregate aggregate, IBusinessRule rule) : base($"{GetRuleDescription(aggregate, rule)}")
    { }

    protected BusinessRuleViolationDomainEvent(_Aggregate aggregate, IBusinessRule rule, string message) : base(
        $"{GetRuleDescription(aggregate, rule)}. \n\n{message}")
    { }

    #endregion

    #region Instance Members

    protected static string GetRuleDescription(_Aggregate aggregate, IBusinessRule rule)
    {
        return
            $"The {nameof(_Aggregate)} with has violated the [{nameof(IBusinessRule)} business rule: [{rule.Message}]; {nameof(_Aggregate)}: [{JsonSerializer.Serialize(aggregate.AsDto())}]";
    }

    #endregion
}