using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Parser.TypeConverters;
using TinyCsvParser.Mapping;

namespace Play.Underwriting.Parser.Mappings;

internal sealed class AliasCsvMapping : CsvMapping<Alias>
{
    public AliasCsvMapping()
    {
        MapProperty(0, x => x.IndividualNumber);
        MapProperty(1, x => x.Number);

        MapUsing((mapping, row) =>
        {
            const int aliasTypeIndex = 2;
            const int aliasNameIndex = 3;

            mapping.AliasName = new Domain.ValueObjects.AliasName
            {
                Type = row.Tokens[aliasTypeIndex].Equals(CustomStringTypeConverter.@null) ? string.Empty : row.Tokens[aliasTypeIndex],
                Name = row.Tokens[aliasNameIndex].Equals(CustomStringTypeConverter.@null) ? string.Empty : row.Tokens[aliasNameIndex],
            };

            return true;
        });

        MapProperty(4, x => x.Remarks, CsvParser.DefaultStringConverter);
    }
}
