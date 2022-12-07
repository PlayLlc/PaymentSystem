using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Identity.Contracts.Dtos;

public class UserRoleDto : IDto
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string? Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    #endregion
}