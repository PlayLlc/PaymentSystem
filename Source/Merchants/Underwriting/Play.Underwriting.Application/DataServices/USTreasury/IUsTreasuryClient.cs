namespace Play.Underwriting.Application.DataServices.USTreasury;

public interface IUsTreasuryClient
{
    #region Instance Members

    Task<string> GetConsolidatedCsvFile(string fileName);

    Task<string> GetCsvFile(string fileName);

    #endregion
}