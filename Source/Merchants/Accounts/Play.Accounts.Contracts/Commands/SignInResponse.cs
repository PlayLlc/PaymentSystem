using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands;

public class SignInResponse
{
    #region Instance Values

    public bool Success { get; set; }
    public SignInError Error { get; set; }
    public string? Message { get; set; }
    public UserDto? User { get; set; }

    #endregion
}