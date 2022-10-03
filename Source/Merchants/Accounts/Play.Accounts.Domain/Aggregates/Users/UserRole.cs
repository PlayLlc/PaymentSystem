namespace Play.Merchants.Onboarding.Domain.Aggregates;

public record UserRole
{
    #region Instance Values

    public static UserRole Member => new(nameof(Member));

    public static UserRole Administrator => new(nameof(Administrator));

    public string Value { get; }

    #endregion

    #region Constructor

    private UserRole(string value)
    {
        Value = value;
    }

    #endregion
}