﻿using System.ComponentModel.DataAnnotations;

namespace Play.Domain.Common.Dtos;

public class AddressDto : IDto
{
    #region Instance Values

    public string? Id { get; set; }

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
    ///     The state or province of the address
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
}