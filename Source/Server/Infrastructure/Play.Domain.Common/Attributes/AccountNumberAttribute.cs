using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Play.Codecs;

namespace Play.Domain.Common.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class AccountNumberAttribute : ValidationAttribute
{
    #region Static Metadata

    private const string _Message = "The field must contain 9 numeric characters;";

    #endregion

    #region Constructor

    public AccountNumberAttribute() : base(_Message)
    { }

    #endregion

    #region Instance Members

    public override string FormatErrorMessage(string name) => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, 1);

    public override bool IsValid(object? value)
    {
        if (value is not string str)
            return false;

        if (!PlayCodec.AlphaNumericCodec.IsValid(str))
            return false;

        return true;
    }

    #endregion
}