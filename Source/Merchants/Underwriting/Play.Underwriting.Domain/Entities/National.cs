using Play.Underwriting.Domain.ValueObjects;

namespace Play.Underwriting.Domain.Entities;

public class National
{
    public static National Default => new(); 

    private readonly List<AlternateName> _alternateNames = new();

    private readonly List<Location> _knownLocations = new();

    private National() { }

    public National(string name, string remarks)
    {
        Name = name;
        Remarks = remarks;
    }

    public string Name { get; }

    public string Remarks { get; }

    public IReadOnlyList<AlternateName> AlternateNames => _alternateNames.AsReadOnly();

    public IReadOnlyList<Location> KnownLocations => _knownLocations.AsReadOnly();

    public void AddAlternateNames(IEnumerable<AlternateName> alternateNames)
    {
        _alternateNames.AddRange(alternateNames);
    }

    public void AddLocations(IEnumerable<Location> locations)
    {
        _knownLocations.AddRange(locations);
    }
}
