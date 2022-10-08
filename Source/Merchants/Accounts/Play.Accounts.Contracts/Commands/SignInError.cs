namespace Play.Accounts.Contracts.Commands;

public enum SignInError
{
    ExpiredPassword,
    InvalidUsername,
    InvalidPassword,
    LockedOut,
    TwoFactorRequired,
    NotAuthorized
}