using Play.Underwriting.Application.Parser.TypeConverters;
using Play.Underwriting.Domain.Aggregates;
using Play.Underwriting.Domain.Enums;

using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace Play.Underwriting.Application.Parser.Mappings;

public sealed class IndividualCsvMapping : CsvMapping<Individual>
{
    #region Constructor

    public IndividualCsvMapping()
    {
        MapProperty(0, x => x.Number);
        MapProperty(1, x => x.Name, CsvParser.DefaultStringConverter);

        MapUsing((mapping, row) =>
        {
            const int colIndex = 2;

            if (string.IsNullOrEmpty(row.Tokens[colIndex]))
                return false;

            if (row.Tokens[colIndex].Equals(CustomStringTypeConverter._Null))
                return true;

            mapping.EntityType = new EntityType(row.Tokens[colIndex]);

            return true;
        });

        MapProperty(3, x => x.Program, CsvParser.DefaultStringConverter);
        MapProperty(4, x => x.Title, CsvParser.DefaultStringConverter);
        MapProperty(5, x => x.VesselCallSign, CsvParser.DefaultStringConverter);
        MapProperty(6, x => x.VesselType, CsvParser.DefaultStringConverter);
        MapProperty(7, x => x.Tonnage, CsvParser.DefaultStringConverter);
        MapProperty(8, x => x.GrossRegisteredTonnage, CsvParser.DefaultStringConverter);
        MapProperty(9, x => x.VesselFlag, CsvParser.DefaultStringConverter);
        MapProperty(10, x => x.VesselOwner, CsvParser.DefaultStringConverter);
        MapProperty(11, x => x.Remarks, CsvParser.DefaultStringConverter);
    }

    public IndividualCsvMapping(ITypeConverterProvider typeConverterProvider) : base(typeConverterProvider)
    { }

    #endregion
}