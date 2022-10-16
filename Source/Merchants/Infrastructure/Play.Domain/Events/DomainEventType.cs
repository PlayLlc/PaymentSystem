namespace Play.Domain.Events;

public record DomainEventType
{
    #region Instance Values

    public readonly string Value;

    #endregion

    #region Constructor

    public DomainEventType(Type type)
    {
        Value = type.FullName!;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(DomainEventType value)
    {
        return value.Value;
    }

    #endregion
}