using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.TimeClock.Contracts.Commands;

public record RemoveEmployee
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string EmployeeId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    #endregion
}