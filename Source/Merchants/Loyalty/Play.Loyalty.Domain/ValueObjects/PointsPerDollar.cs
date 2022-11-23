using Play.Domain.ValueObjects;
using Play.Globalization.Currency;

namespace Play.Loyalty.Domain.ValueObjects;

public record PointsPerDollar : ValueObject<uint>
{
    #region Constructor

    // Constructor for Entity Framework
    private PointsPerDollar()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public PointsPerDollar(uint value) : base(value)
    {
        if (value == 0)
            throw new ValueObjectException($"The {nameof(PointsPerDollar)} must have a value that is nonzero and positive");
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Gets the number of rewards that the loyalty customer has earned
    /// </summary>
    /// <param name="rewardThreshold"></param>
    /// <param name="totalSpent"></param>
    /// <returns></returns>
    public uint GetRewards(uint rewardThreshold, Money totalSpent)
    {
        return (uint) (totalSpent.GetMajorCurrencyAmount() * Value) / rewardThreshold;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator uint(PointsPerDollar value)
    {
        return value.Value;
    }

    #endregion
}