using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;
using Play.Globalization.Time;

namespace Play.Loyalty.Contracts.Dtosd;

public record PaycheckDto : IDto
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
    public MoneyDto Amount { get; set; } = null!;

    [Required]
    [DateTimeUtc]
    public DateTime DateIssued { get; set; }

    [Required]
    public TimeSheetDto TimeSheet { get; set; } = null!;

    [Required]
    public DirectDepositDto? DirectDeposit { get; set; } = null!;

    [Required]
    public PayPeriodDto PayPeriod { get; set; } = null!;

    #endregion
}

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
    [StringLength(20)]
    [AlphaNumericSpecial]
    public PayPeriodDto PayPeriod { get; set; } = null!;

    [Required]
    public IEnumerable<TimeEntryDto> TimeEntries { get; set; } = Array.Empty<TimeEntryDto>();

    #endregion
}