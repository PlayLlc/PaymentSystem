﻿using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Dtos;

namespace Play.Loyalty.Contracts.Dtos;

public record RewardsProgramDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public MoneyDto RewardAmount { get; set; } = null!;

    [Required]
    public uint PointsPerDollar { get; set; }

    [Required]
    public uint RewardThreshold { get; set; }

    [Required]
    public bool IsActive { get; set; }

    #endregion
}