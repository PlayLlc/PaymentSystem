namespace Play.Underwriting.Domain.Entities;

public class Alias
{
    public ulong IndividualNumber { get; set; }

    public ulong Number { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Remarks { get; set; } = string.Empty;
}
