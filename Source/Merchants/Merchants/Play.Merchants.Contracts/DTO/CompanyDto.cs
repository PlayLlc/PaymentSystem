namespace Play.Merchants.Contracts.DTO;

public record CompanyDto
{
    #region Instance Values

    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    #endregion
}