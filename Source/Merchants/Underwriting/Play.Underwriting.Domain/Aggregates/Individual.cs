using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Domain.Enums;

namespace Play.Underwriting.Domain.Aggregates;

public class Individual
{
    #region Instance Values

    private readonly List<Address> _Addresses = new();

    private readonly List<Alias> _Aliases = new();

    public IReadOnlyList<Address> Addresses => _Addresses.AsReadOnly();

    public IReadOnlyList<Alias> Aliases => _Aliases.AsReadOnly();

    public ulong Number { get; set; }

    public string Name { get; set; } = string.Empty;

    public EntityType EntityType { get; set; } = EntityType.Empty;

    public string Program { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string VesselCallSign { get; set; } = string.Empty;

    public string VesselType { get; set; } = string.Empty;

    public string Tonnage { get; set; } = string.Empty;

    public string GrossRegisteredTonnage { get; set; } = string.Empty;

    public string VesselFlag { get; set; } = string.Empty;

    public string VesselOwner { get; set; } = string.Empty;

    public string Remarks { get; set; } = string.Empty;

    #endregion

    #region Instance Members

    public void AddAddresses(Address[] addresses)
    {
        _Addresses.AddRange(addresses);
    }

    public void AddAlternateIdentities(Alias[] alternateIdentities)
    {
        _Aliases.AddRange(alternateIdentities);
    }

    #endregion
}