using Play.Codecs;
using Play.Domain.ValueObjects;

namespace Play.Identity.Domain.ValueObjects;

/// <summary>
///     Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters
/// </summary>
public record ClearTextPassword : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public ClearTextPassword(string value) : base(value)
    {
        if (!IsValid(value))
            throw new ValueObjectException($"Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters");
    }

    #endregion

    #region Instance Members

    public static bool IsValid(string password)
    {
        return IsAlphabeticCharacterPresent(password)
               && IsNumericPresent(password)
               && IsSevenCharactersInLength(password)
               && IsSpecialCharacterPresent(password);
    }

    private static bool IsSpecialCharacterPresent(string password)
    {
        return password.Any(a => PlayCodec.SpecialCodec.IsValid(a));
    }

    private static bool IsAlphabeticCharacterPresent(string password)
    {
        return password.Any(a => a is >= 'a' and <= 'z' or >= 'A' and <= 'Z');
    }

    private static bool IsNumericPresent(string password)
    {
        return password.Any(a => a is >= (char) 0 and <= (char) 9);
    }

    private static bool IsSevenCharactersInLength(string password)
    {
        return password.Length >= 7;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(ClearTextPassword value)
    {
        return value.Value;
    }

    #endregion
}