using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Globalization.Time;

namespace Play.Identity.Contracts.Dtos;

public class PersonalDetailDto : IDto
{
    #region Instance Values

    public string? Id { get; set; }

    /// <summary>
    ///     The last four digits of the user's Social Security Number
    /// </summary>
    [Required]
    [MinLength(4)]
    [MaxLength(4)]
    [RegularExpression("[\\d]{4}")]
    public string LastFourOfSocial { get; set; } = string.Empty;

    /// <summary>
    ///     The user's date of birth in UTC
    /// </summary>
    [Required]
    [DateTimeUtc]
    public DateTime DateOfBirth { get; set; }

    #endregion
}