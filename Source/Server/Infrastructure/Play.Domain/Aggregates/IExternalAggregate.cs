namespace Play.Domain.Aggregates;

/// <summary>
///     An object that is an aggregate in a different bounded context but not the one it is currently implemented in
/// </summary>
public interface IExternalAggregate : IAggregate
{ }