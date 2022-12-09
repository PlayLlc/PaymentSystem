﻿using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Loyalty.Contracts.Commands;

public record CreateLoyaltyMember
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string RewardsNumber { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; } = string.Empty;

    [Required]
    public ushort NumericCurrencyCode { get; set; }

    #endregion
}