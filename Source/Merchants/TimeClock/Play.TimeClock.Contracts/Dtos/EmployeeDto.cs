using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Inventory.Contracts.Dtos;

namespace Play.TimeClock.Contracts.Dtos;

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
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public TimeClockDto TimeClock { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string CompensationType { get; set; } = null!;

    #endregion
}