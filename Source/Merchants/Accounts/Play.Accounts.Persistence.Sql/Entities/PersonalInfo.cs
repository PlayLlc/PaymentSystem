using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Api.Identity.Entities;

public class PersonalInfo
{
    #region Instance Values

    public int Id { get; set; }

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
    public DateTime DateOfBirth { get; set; }

    #endregion
}