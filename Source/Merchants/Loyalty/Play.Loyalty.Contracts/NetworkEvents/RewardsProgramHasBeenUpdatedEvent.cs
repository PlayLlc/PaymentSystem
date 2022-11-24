using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Loyalty.Contracts.Dtos;
using Play.Messaging.NServiceBus;

namespace Play.Loyalty.Contracts;

public class RewardsProgramHasBeenUpdatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string LoyaltyProgramId { get; set; } = null!;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = null!;

    [Required]
    public RewardsProgramDto RewardsProgram { get; set; } = null!;

    #endregion
}