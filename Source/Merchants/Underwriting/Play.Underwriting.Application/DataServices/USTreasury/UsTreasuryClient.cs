namespace Play.Underwriting.DataServices.USTreasury;

public class UsTreasuryClient : IUsTreasuryClient
{
    private readonly HttpClient _HttpClient;

    public UsTreasuryClient(HttpClient httpClient)
    {
        _HttpClient = httpClient;
    }

    public async Task<string> GetConsolidatedCsvFile(string fileName)
    {
        HttpResponseMessage response = await _HttpClient.GetAsync($"ofac/downloads/consolidated/{fileName}").ConfigureAwait(false);

        var content = await response.Content.ReadAsStringAsync();

        return content;
    }

    public async Task<string> GetCsvFile(string fileName)
    {
        HttpResponseMessage response = await _HttpClient.GetAsync($"ofac/downloads/{fileName}").ConfigureAwait(false);

        var content = await response.Content.ReadAsStringAsync();

        return content;
    }
}
