using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;

namespace Play.Payroll.Contracts.Dtos;

public record EmployeeDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public CompensationDto Compensation { get; set; } = null!;

    [Required]
    public DirectDepositDto? DirectDeposit { get; set; } = null!;

    [Required]
    public IEnumerable<TimeEntryDto> TimeEntries { get; set; } = Array.Empty<TimeEntryDto>();

    [Required]
    public IEnumerable<PaycheckDto> Paychecks { get; set; } = Array.Empty<PaycheckDto>();

    #endregion
}