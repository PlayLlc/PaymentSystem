using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Loyalty.Contracts.Commands;

public record ActivateProgram
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }

    #endregion
}