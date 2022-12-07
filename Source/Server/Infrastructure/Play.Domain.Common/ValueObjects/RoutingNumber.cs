using Play.Codecs;
using Play.Domain.ValueObjects;
using Play.Randoms;

namespace Play.Domain.Common.ValueObjects;

public record RoutingNumber : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private RoutingNumber()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public RoutingNumber(string value) : base(value)
    {
        if (value.Length != 9)
            throw new ValueObjectException(
                $"The {nameof(RoutingNumber)} must contain 9 numeric characters, but the value provided has {value.Length} characters;");
        if (!PlayCodec.NumericCodec.IsValid(value))
            throw new ValueObjectException($"The {nameof(RoutingNumber)} must be comprised of numeric characters only but was not: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(RoutingNumber value) => value.Value;

    #endregion
}