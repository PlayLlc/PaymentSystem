using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Payroll.Contracts.Commands;

public record CreateEmployer
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    #endregion
}

public record ClaimRewards
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string RewardId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public uint TransactionId { get; set; }

    [Required]
    public MoneyDto RewardAmount { get; set; } = null!;

    #endregion
}