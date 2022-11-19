using TinyCsvParser.TypeConverter;

namespace Play.Underwriting.Parser.TypeConverters;

public class CustomStringTypeConverter : StringConverter
{
    public const string @null = "-0-";
    public override bool TryConvert(string value, out string result)
    {
        if (value.Equals(@null))
        {
            result = string.Empty;
            return true;
        }

        return base.TryConvert(value, out result);
    }
}
