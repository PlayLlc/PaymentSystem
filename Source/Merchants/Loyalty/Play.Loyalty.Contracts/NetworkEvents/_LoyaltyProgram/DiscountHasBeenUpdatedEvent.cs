using System.ComponentModel.DataAnnotations;

using Play.Loyalty.Contracts.Dtos;
using Play.Messaging.NServiceBus;

namespace Play.Loyalty.Contracts;

public class DiscountHasBeenUpdatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    public DiscountDto Discount { get; set; } = null!;

    #endregion
}