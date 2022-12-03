using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;

namespace Play.Payroll.Contracts.Dtos;

public record TimeSheetDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string EmployeeId { get; set; } = string.Empty;

    [Required]
    public PayPeriodDto PayPeriod { get; set; } = null!;

    [Required]
    public IEnumerable<TimeEntryDto> TimeEntries { get; set; } = Array.Empty<TimeEntryDto>();

    #endregion
}

public record EmployerDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string MerchantId { get; set; } = string.Empty;

    #endregion
}