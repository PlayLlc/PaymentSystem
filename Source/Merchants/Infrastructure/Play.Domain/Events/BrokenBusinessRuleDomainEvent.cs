using System.Text.Json;

using Play.Domain.Aggregates;

namespace Play.Domain.Events;

public abstract record BrokenBusinessRuleDomainEvent<_Aggregate, _TId> : DomainEvent where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Constructor

    protected BrokenBusinessRuleDomainEvent(_Aggregate aggregate, IBusinessRule rule) : base($"{GetRuleDescription(aggregate, rule)}")
    { }

    #endregion

    #region Instance Members

    protected static string GetRuleDescription(_Aggregate aggregate, IBusinessRule rule)
    {
#if DEBUG
        try
        {
            return
                $"The {nameof(_Aggregate)} with has violated the [{nameof(IBusinessRule)} business rule: [{rule.Message}]; {nameof(_Aggregate)}: [{JsonSerializer.Serialize(aggregate.AsDto())}]";
        }
        catch (Exception)
        {
            return
                $"The {nameof(_Aggregate)} with has violated the [{nameof(IBusinessRule)} business rule: [{rule.Message}]; {nameof(_Aggregate)}: [WARNING-{nameof(JsonSerializer)} not supported-WARNING]";
        }
#else
     return $"The {nameof(_Aggregate)} with has violated the [{nameof(IBusinessRule)} business rule: [{rule.Message}]; {nameof(_Aggregate)}: [{aggregate.GetId()}]";
#endif
    }

    #endregion
}