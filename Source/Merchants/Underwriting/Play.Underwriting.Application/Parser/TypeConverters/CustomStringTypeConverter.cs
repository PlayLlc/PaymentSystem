using TinyCsvParser.TypeConverter;

namespace Play.Underwriting.Parser.TypeConverters;

public class CustomStringTypeConverter : StringConverter
{
    #region Static Metadata

    public const string _Null = "-0-";

    #endregion

    #region Instance Members

    public override bool TryConvert(string value, out string result)
    {
        if (value.Equals(_Null))
        {
            result = string.Empty;

            return true;
        }

        return base.TryConvert(value, out result);
    }

    #endregion
}