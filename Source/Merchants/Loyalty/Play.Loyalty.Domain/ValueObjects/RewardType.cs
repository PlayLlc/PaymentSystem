using Play.Core;
using Play.Domain.ValueObjects;
using Play.Loyalty.Domain.Enums;

namespace Play.Loyalty.Domain.ValueObjects;

public record RewardType : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private RewardType()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public RewardType(string value) : base(value)
    {
        if (!RewardTypes.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(RewardType)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(RewardType value)
    {
        return value.Value;
    }

    #endregion
}