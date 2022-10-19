using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.ValueObjects;

public record UniversalDateTime : ValueObject<DateTime>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public UniversalDateTime(DateTime value) : base(value)
    {
        if (value.Kind != DateTimeKind.Utc)
            throw new ValueObjectException($"The {nameof(UniversalDateTime)} provided was not in {nameof(DateTimeKind.Utc)} format");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator DateTime(UniversalDateTime value)
    {
        return value.Value;
    }

    #endregion
}