﻿using Play.Underwriting.Domain.Aggregates;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace Play.Underwriting.Parser.Mappings;

public sealed class IndividualCsvMapping : CsvMapping<Individual>
{

    public IndividualCsvMapping() : base()
    {
        MapProperty(0, x => x.Number);
        MapProperty(1, x => x.Name, CsvParser.DefaultStringConverter);
        MapProperty(2, x => x.EntityType, CsvParser.DefaultStringConverter);
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
    {

    }
}
