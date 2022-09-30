using Play.Domain.Entities;

namespace Play.Domain.Aggregates;

public interface IAggregate<_TId> : IEntity<_TId>
{ }