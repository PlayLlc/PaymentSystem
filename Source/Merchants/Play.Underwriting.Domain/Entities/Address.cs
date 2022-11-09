namespace Play.Underwriting.Domain.Entities;

//15268(0), 22761(1),"172 Xibin Rd, Ranghulu District, (Daqing, Heilongjiang Branch)"(2),"Daqing 163453"(3),"China"(4),-0-(5) 
public class Address
{
    public ulong Number { get; set; }

    public ulong IndividualNumber { get; set; }

    public string StreetAddress { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Remarks { get; set; } = string.Empty;
}
