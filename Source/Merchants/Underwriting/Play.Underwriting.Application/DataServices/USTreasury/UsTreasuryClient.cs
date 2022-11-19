namespace Play.Underwriting.DataServices.USTreasury;

public class UsTreasuryClient : IUsTreasuryClient
{
    #region Instance Values

    private readonly HttpClient _HttpClient;

    #endregion

    #region Constructor

    public UsTreasuryClient(HttpClient httpClient)
    {
        _HttpClient = httpClient;
    }

    #endregion

    #region Instance Members

    public async Task<string> GetConsolidatedCsvFile(string fileName)
    {
        HttpResponseMessage response = await _HttpClient.GetAsync($"ofac/downloads/consolidated/{fileName}").ConfigureAwait(false);

        string content = await response.Content.ReadAsStringAsync();

        return content;
    }

    public async Task<string> GetCsvFile(string fileName)
    {
        HttpResponseMessage response = await _HttpClient.GetAsync($"ofac/downloads/{fileName}").ConfigureAwait(false);

        string content = await response.Content.ReadAsStringAsync();

        return content;
    }

    #endregion
}