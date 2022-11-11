using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Domain.Enums;
using TinyCsvParser.Mapping;

namespace Play.Underwriting.Parser.Mappings;

internal sealed class AliasCsvMapping : CsvMapping<Alias>
{
    public AliasCsvMapping()
    {
        MapProperty(0, x => x.IndividualNumber);
        MapProperty(1, x => x.Number);
        MapProperty(2, x => x.AliasName.Type, CsvParser.DefaultStringConverter);
        MapProperty(3, x => x.AliasName.Name, CsvParser.DefaultStringConverter);
        MapProperty(4, x => x.Remarks, CsvParser.DefaultStringConverter);
    }
}
