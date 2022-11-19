using Play.Domain.ValueObjects;

namespace Play.Inventory.Domain.ValueObjects;

/// <summary>
///     Stock Keeping Unit (SKU) is a unique identifier for a product. It can contain up to 15 characters, using only
///     numbers, letters, and dashes (no spaces, punctuation, or other special characters).
/// </summary>
public record Sku : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private Sku()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Sku(string value) : base(value)
    {
        Validate(value);
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    public void Validate(string value)
    {
        if (value.Length > 15)
            throw new ValueObjectException($"The {nameof(Sku)} must be 15 characters or less");

        for (int i = 0; i < value.Length; i++)
        {
            if ((value[i] >= 'a') && (value[i] <= 'z'))
                continue;
            if ((value[i] >= 'A') && (value[i] <= 'Z'))
                continue;
            if ((value[i] >= '0') && (value[i] <= '9'))
                continue;
            if (value[i] == '-')
                continue;

            throw new ValueObjectException($"The {nameof(Sku)} must contain only numbers, letters, and dashes");
        }
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(Sku value)
    {
        return value.Value;
    }

    #endregion
}