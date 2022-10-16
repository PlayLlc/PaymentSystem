using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Play.Identity.Application.Entities;

public class Address
{
    #region Instance Values

    public int Id { get; set; }

    /// <summary>
    ///     The street address of the user's home
    /// </summary>
    [Required]
    [MinLength(1)]
    public string StreetAddress { get; set; } = string.Empty;

    /// <summary>
    ///     The apartment number of the user's home
    /// </summary>
    public string? ApartmentNumber { get; set; }

    /// <summary>
    ///     The zipcode of the user's home
    /// </summary>
    [Required]
    [MinLength(5)]
    [MaxLength(10)]
    public string Zipcode { get; set; } = string.Empty;

    /// <summary>
    ///     The state the user lives in
    /// </summary>
    [Required]
    [MinLength(1)]
    public string State { get; set; } = string.Empty;

    /// <summary>
    ///     The city where the user lives
    /// </summary>
    [Required]
    [MinLength(1)]
    public string City { get; set; } = string.Empty;

    #endregion

    #region Instance Members

    /// <exception cref="NotSupportedException"></exception>
    public string Normalize()
    {
        return JsonSerializer.Serialize(new
        {
            street_address = StreetAddress,
            locality = City,
            region = State,
            postal_code = Zipcode,
            country = "United States"
        });
    }

    #endregion
}