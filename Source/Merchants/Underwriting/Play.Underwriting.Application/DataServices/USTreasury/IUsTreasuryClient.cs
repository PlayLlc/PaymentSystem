namespace Play.Underwriting.DataServices.USTreasury;

public interface IUsTreasuryClient
{
    Task<string> GetConsolidatedCsvFile(string fileName);

    Task<string> GetCsvFile(string fileName);
}
