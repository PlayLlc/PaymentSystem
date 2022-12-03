using System.ComponentModel.DataAnnotations;

namespace TemporaryWebClient.Models;

public class ContactUsModel
{
    #region Instance Values

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string EmailBody { get; set; } = null!;

    #endregion
}