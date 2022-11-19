using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Play.Codecs;

namespace Play.Domain.Common.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class AlphaNumericSpecialAttribute : ValidationAttribute
{
    #region Static Metadata

    private const string _Message = "The field must only contain Alphabetic, Numeric, and Ascii Special characters.";

    #endregion

    #region Constructor

    public AlphaNumericSpecialAttribute() : base(_Message)
    { }

    #endregion

    #region Instance Members

    public override string FormatErrorMessage(string name)
    {
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, ErrorMessageString, (object) name, (object) 1);
    }

    public override bool IsValid(object? value)
    {
        if (value is not string str)
            return false;

        return PlayCodec.AlphaNumericSpecialCodec.IsValid(str);
    }

    #endregion
}