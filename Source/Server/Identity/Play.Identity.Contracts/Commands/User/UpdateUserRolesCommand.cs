using System.ComponentModel.DataAnnotations;

using Play.Identity.Contracts.Dtos;

namespace Play.Identity.Contracts.Commands;

public record UpdateUserRolesCommand
{
    #region Instance Values

    [Required]
    public IEnumerable<UserRoleDto> Roles { get; set; } = new List<UserRoleDto>();

    #endregion
}