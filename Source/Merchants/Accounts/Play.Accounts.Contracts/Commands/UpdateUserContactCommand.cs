using System.ComponentModel.DataAnnotations;

using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands;

public record UpdateUserContactCommand
{
    #region Instance Values

    [Required]
    public ContactDto Contact { get; set; } = new();

    #endregion
}