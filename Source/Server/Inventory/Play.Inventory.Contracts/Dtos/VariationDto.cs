﻿using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Dtos;

namespace Play.Inventory.Contracts.Dtos;

public record VariationDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(15)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    public MoneyDto Price { get; set; } = null!;

    #endregion
}