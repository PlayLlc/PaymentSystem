using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;

namespace Play.Payroll.Contracts.Dtos;

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

    [Required]
    public IEnumerable<EmployeeDto> Employees { get; set; } = Array.Empty<EmployeeDto>();

    #endregion
}