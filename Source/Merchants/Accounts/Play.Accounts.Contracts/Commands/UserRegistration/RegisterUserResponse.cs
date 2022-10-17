using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands.UserRegistration;

public class RegisterUserResponse
{
    #region Instance Values

    public bool Success { get; set; }
    public string? Message { get; set; }
    public UserDto? User { get; set; }

    #endregion
}