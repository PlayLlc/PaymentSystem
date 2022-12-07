using Play.Domain.ValueObjects;

namespace Play.Domain.Common.ValueObjects;

public record Name : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Name(string value) : base(value)
    {
        if (value == string.Empty)
            throw new ValueObjectException($"The {nameof(Name)} provided was empty: [{value}]");
        if (value.Length > 200)
            throw new ValueObjectException($"The {nameof(Name)} provided was too long: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(Name value) => value.Value;

    #endregion
}