using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Accounts.Contracts.Dtos;

public class TerminalDto : IDto
{
    #region Instance Values

    /// <summary>
    ///     The identifier for this terminal
    /// </summary>
    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     The identifier of the Merchant that this terminal belongs to
    /// </summary>
    [Required]
    [MinLength(1)]
    public string MerchantId { get; set; } = string.Empty;

    /// <summary>
    ///     The identifier of the User that this terminal belongs to
    /// </summary>
    [MinLength(1)]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    ///     The identifier of the device this terminal is hosted on
    /// </summary>
    [Required]
    [MinLength(1)]
    public string DeviceId { get; set; } = string.Empty;

    #endregion
}