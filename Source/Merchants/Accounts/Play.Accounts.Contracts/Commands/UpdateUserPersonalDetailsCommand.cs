using System.ComponentModel.DataAnnotations;

using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands;

public record UpdateUserPersonalDetailsCommand
{
    #region Instance Values

    [Required]
    public PersonalDetailDto PersonalDetail { get; set; } = new();

    #endregion
}