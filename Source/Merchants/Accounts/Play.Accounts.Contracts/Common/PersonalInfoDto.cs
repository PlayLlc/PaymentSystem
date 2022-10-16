using Play.Globalization.Time;

using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Accounts.Contracts.Common;

public class PersonalInfoDto : IDto
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
    ///     The user's date of birth
    /// </summary>
    [Required]
    public DateTimeUtc DateOfBirth { get; set; }

    #endregion
}