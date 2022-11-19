using Play.Underwriting.Common.Exceptions;
using Play.Underwriting.Parser.TypeConverters;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Play.Underwriting.Parser;

public class CsvParser
{
    public static CustomStringTypeConverter DefaultStringConverter = new CustomStringTypeConverter();

    public static IEnumerable<CsvMappingResult<T>> ParseFromString<T>(string input, CsvParserOptions options, CsvMapping<T> mapping, string fileName) where T : class, new()
    {
        CsvParser<T> parser = new CsvParser<T>(options, mapping);

        var result = parser.ReadFromString(new CsvReaderOptions(new[] { "\n" }), input).ToList();

        if (result.Any(x => !x.IsValid))
            throw new ParsingException(
                $"The {nameof(CsvParser)} encountered an exception parsing {fileName} file with the following errors: " +
                $"[{string.Join('\n', result.Where(x => !x.IsValid).Select(x => $"Error parsing row {x.RowIndex}: {x.Error.Value}"))}]");

        return result.Where(x => x.IsValid);
    }
}
