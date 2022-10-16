using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Play.Identity.Api.Identity.Entities;

[Table(nameof(IdentityProviders))]
public class IdentityProviders
{
    #region Instance Values

    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(200)")]
    public string? Scheme { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string? DisplayName { get; set; }

    [Required]
    [Column(TypeName = "bit")]
    public bool Enabled { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(20)")]
    public bool Type { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public bool Properties { get; set; }

    #endregion
}