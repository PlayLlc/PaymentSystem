using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Globalization.Time;
using Play.Mvc.Attributes;

namespace Play.Accounts.Contracts.Dtos;

public class ConfirmationCodeDto : IDto
{
    #region Instance Values

    public string Id { get; set; } = string.Empty;

    [Required]
    [DateTimeUtc]
    public DateTime SentDate { get; set; }

    [Required]
    [Range(000001, 999999)]
    public uint Code { get; set; }

    #endregion
}