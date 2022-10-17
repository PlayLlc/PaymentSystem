using Play.Codecs;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

/// <summary>
///     PCI-DSS Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters, and be unique
///     when updated
/// </summary>
internal class PasswordMustBeStrong : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters";

    #endregion

    #region Constructor

    internal PasswordMustBeStrong(string password)
    {
        _IsValid = IsAlphabeticCharacterPresent(password)
                   && IsNumericPresent(password)
                   && IsSevenCharactersInLength(password)
                   && IsSpecialCharacterPresent(password);
    }

    #endregion

    #region Instance Members

    private static bool IsSpecialCharacterPresent(string password)
    {
        return password.Any(a => SpecialCodec.SpecialCodec.IsValid(a));
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

    public override UsernameWasNotAValidEmail CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new UsernameWasNotAValidEmail(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}