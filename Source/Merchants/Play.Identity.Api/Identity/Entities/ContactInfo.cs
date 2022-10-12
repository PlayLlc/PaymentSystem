using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Api.Identity.Entities;

public class ContactInfo
{
    #region Instance Values

    public int Id { get; set; }

    /// <summary>
    ///     The user's first name
    /// </summary>
    [Required]
    [MinLength(1)]
    [RegularExpression("[\x32-\x7E]{2,26}")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     The user's last name
    /// </summary>
    [Required]
    [MinLength(1)]
    [RegularExpression("[\x32-\x7E]{2,26}")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     The user's mobile phone number
    /// </summary>
    [Required]
    [MinLength(10)]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    ///     The user's email address
    /// </summary>
    [Required]
    [MinLength(1)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    #endregion

    #region Instance Members

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    #endregion
}