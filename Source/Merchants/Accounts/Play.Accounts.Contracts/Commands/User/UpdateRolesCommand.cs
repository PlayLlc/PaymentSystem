using System.ComponentModel.DataAnnotations;

using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands.User;

public record UpdateRolesCommand
{
    #region Instance Values

    [Required]
    public IEnumerable<UserRoleDto> Roles { get; set; } = new List<UserRoleDto>();

    #endregion
}