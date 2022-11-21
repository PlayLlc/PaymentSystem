﻿using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;

using System.ComponentModel.DataAnnotations;

namespace Play.Domain.Common.Entitiesd;

public record AmountOffDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public Money Amount { get; set; } = null!;

    #endregion
}

public record DiscountDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string VariationId { get; set; } = string.Empty;

    [Required]
    public Money Price { get; set; } = null!;

    #endregion
}