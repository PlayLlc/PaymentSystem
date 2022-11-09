using Play.Underwriting.Domain.Entities;

namespace Play.Underwriting.Application.Parser;

public class SdnListParser
{
    public IEnumerable<National> ParseInput(string input)
    {
        IEnumerable<string> items = input.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));

        foreach(string item in items)
        {
            yield return ParseNationalDetails(input);
        }
    }

    private National ParseNationalDetails(string input)
    {
        return National.Default;
    }
}
