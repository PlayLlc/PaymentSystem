using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Dtos;

namespace Play.Identity.Contracts.Commands;

public record UpdateContactCommand
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public ContactDto Contact { get; set; } = new ContactDto();

    #endregion
}