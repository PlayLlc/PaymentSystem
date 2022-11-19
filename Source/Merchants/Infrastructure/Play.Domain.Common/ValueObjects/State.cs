using Play.Core;
using Play.Domain.Common.Enums;
using Play.Domain.ValueObjects;

namespace Play.Domain.Common.ValueObjects;

public record State : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public State(string value) : base(value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ValueObjectException(
                $"An instance of the {nameof(State)} could not be created because the {nameof(value)} argument: [{value}] is an invalid {nameof(States)}");

        if (!States.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException(
                $"An instance of the {nameof(State)} could not be created because the {nameof(value)} argument: [{value}] is an invalid {nameof(States)}");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(State value)
    {
        return value.Value;
    }

    #endregion
}