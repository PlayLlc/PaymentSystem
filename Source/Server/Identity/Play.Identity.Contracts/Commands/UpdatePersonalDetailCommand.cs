using System.ComponentModel.DataAnnotations;

using Play.Identity.Contracts.Dtos;

namespace Play.Identity.Contracts.Commands;

public record UpdatePersonalDetailCommand
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public PersonalDetailDto PersonalDetail { get; set; } = new();

    #endregion
}