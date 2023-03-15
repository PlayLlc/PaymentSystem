using System.ComponentModel.DataAnnotations;

using Play.Codecs;
using Play.Domain.ValueObjects;

namespace Play.Domain.Common.ValueObjects;

public record Phone : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Phone(string value) : base(value)
    {
        if (!new PhoneAttribute().IsValid(Value))
            throw new ValueObjectException($"The {nameof(Phone)} provided was invalid: [{value}]");

        if (!PlayCodec.NumericCodec.IsValid(value))
            throw new ValueObjectException($"The {nameof(Phone)} provided contained characters that were not numeric: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(Phone value) => value.Value;

    #endregion
}