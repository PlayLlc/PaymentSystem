using Play.Underwriting.Application.Common.Exceptions;
using Play.Underwriting.Application.Parser.TypeConverters;

using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Play.Underwriting.Application.Parser;

public class CsvParser
{
    #region Static Metadata

    public static CustomStringTypeConverter DefaultStringConverter = new();

    #endregion

    #region Instance Members

    public static IEnumerable<CsvMappingResult<_>> ParseFromString<_>(string input, CsvParserOptions options, CsvMapping<_> mapping, string fileName)
        where _ : class, new()
    {
        CsvParser<_> parser = new CsvParser<_>(options, mapping);

        List<CsvMappingResult<_>> result = parser.ReadFromString(new CsvReaderOptions(new[] {"\n"}), input).ToList();

        if (result.Any(x => !x.IsValid))
            throw new ParsingException($"The {nameof(CsvParser)} encountered an exception parsing {fileName} file with the following errors: "
                                       + $"[{string.Join('\n', result.Where(x => !x.IsValid).Select(x => $"Error parsing row {x.RowIndex}: {x.Error.Value}"))}]");

        return result.Where(x => x.IsValid);
    }

    #endregion
}