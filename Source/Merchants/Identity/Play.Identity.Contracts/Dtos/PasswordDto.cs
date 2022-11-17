using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Globalization.Time;

namespace Play.Identity.Contracts.Dtos;

public class PasswordDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string HashedPassword { get; set; } = string.Empty;

    [Required]
    [DateTimeUtc]
    public DateTime CreatedOn { get; set; }

    #endregion
}