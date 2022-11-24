﻿using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Dtos;
using Play.Messaging.NServiceBus;

namespace Play.Loyalty.Contracts.NetworkEvents;

public class DiscountHasBeenUpdatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string DiscountId { get; set; } = null!;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = null!;

    [Required]
    public Money Price { get; set; } = null!;

    #endregion
}