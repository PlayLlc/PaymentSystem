using Play.Codecs;
using Play.Domain.ValueObjects;

namespace Play.Loyalty.Domain.ValueObjects;

public record RewardsNumber : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private RewardsNumber()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public RewardsNumber(string value) : base(value)
    {
        if (value.Length != 10)
            throw new ValueObjectException(
                $"The {nameof(RewardsNumber)} must have a length of 10 but the value provided was {value.Length} characters in length");

        if (!PlayCodec.NumericCodec.IsValid(value))
            throw new ValueObjectException($"The {nameof(RewardsNumber)} must contain only numeric characters");
    }

    #endregion
}