using Play.Underwriting.Domain.ValueObjects;

namespace Play.Underwriting.Domain.Entities;

public class Alias
{
    #region Instance Values

    public ulong IndividualNumber { get; set; }

    public ulong Number { get; set; }

    public AliasName AliasName { get; set; } = default!;

    public string Remarks { get; set; } = string.Empty;

    #endregion
}