using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Parser.TypeConverters;
using TinyCsvParser.Mapping;

namespace Play.Underwriting.Parser.Mappings;

//15268(0), 22761(1),"172 Xibin Rd, Ranghulu District, (Daqing, Heilongjiang Branch)"(2),"Daqing 163453"(3),"China"(4),-0-(5) \
//17013,52484,"43, bld.1, Vorontsovskaya str.","Moscow 109147","Russia",-0- 
internal sealed class AddressCsvMapping : CsvMapping<Address>
{
    public AddressCsvMapping()
    {
        MapProperty(0, x => x.IndividualNumber);
        MapProperty(1, x => x.Number);
        MapProperty(2, x => x.StreetAddress, CsvParser.DefaultStringConverter);
        MapProperty(4, x => x.Country, CsvParser.DefaultStringConverter);
        MapProperty(5, x => x.Remarks, CsvParser.DefaultStringConverter);

        MapUsing((mapping, row) =>
        {
            const int index = 3;

            if (string.IsNullOrEmpty(row.Tokens[index])) 
                return false;

            if (row.Tokens[index].Equals(CustomStringTypeConverter.@null))
                return true;

            string[] addressItems = row.Tokens[index].Split(' ').Select(s => s.Trim()).ToArray();

            if (addressItems.Length == 2)
            {
                mapping.City = addressItems[0];
                mapping.ZipCode = addressItems[1];
            }

            if (addressItems.Length == 3)
            {
                mapping.City = addressItems[0];
                mapping.State = addressItems[1];
                mapping.ZipCode = addressItems[2];
            }

            return true;
        });
    }
}