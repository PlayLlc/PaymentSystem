using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Accounts.Contracts.Dtos;

public class UserRoleDto : IDto
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    #endregion
}