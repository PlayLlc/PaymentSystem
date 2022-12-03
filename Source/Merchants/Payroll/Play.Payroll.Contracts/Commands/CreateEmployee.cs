using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;

namespace Play.Payroll.Contracts.Commands;

public record CreateEmployee
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    ///     The type of compensation the employee receives in either an hourly wage or an annual salary
    /// </summary>
    [Required]
    [MinLength(1)]
    public string CompensationType { get; set; } = string.Empty;

    /// <summary>
    ///     The hourly or salary wage that the employee earns
    /// </summary>
    [Required]
    public MoneyDto CompensationRate { get; set; } = null!;

    #endregion
}