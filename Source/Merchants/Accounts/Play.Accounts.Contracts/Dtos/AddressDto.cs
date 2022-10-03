using Play.Domain;

namespace Play.Accounts.Contracts.Dtos;

public class AddressDto : IDto
{
    #region Instance Values

    public string StreetAddress { get; set; }
    public string ApartmentNumber { get; set; }
    public string Zipcode { get; set; }
    public string StateAbbreviation { get; set; }
    public string City { get; set; }

    #endregion
}