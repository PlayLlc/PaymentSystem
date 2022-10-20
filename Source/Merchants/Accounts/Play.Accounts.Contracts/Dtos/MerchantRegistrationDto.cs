﻿using Play.Domain;
using Play.Globalization.Time;

namespace Play.Accounts.Contracts.Dtos;

public class MerchantRegistrationDto : IDto
{
    #region Instance Values

    public string Id { get; set; }
    public string? CompanyName { get; set; }
    public AddressDto? AddressDto { get; set; }
    public string? BusinessType { get; set; }
    public ushort? MerchantCategoryCode { get; set; }
    public DateTimeUtc RegisteredDate { get; set; }
    public string RegistrationStatus { get; set; } = string.Empty;

    #endregion
}