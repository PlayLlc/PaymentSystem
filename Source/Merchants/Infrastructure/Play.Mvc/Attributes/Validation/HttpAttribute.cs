using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Play.Mvc.Attributes.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class HttpAttribute : ValidationAttribute
{
    #region Static Metadata

    private const string _Message = "The Url field is not a valid fully-qualified http, or https URL.";

    #endregion

    #region Constructor

    public HttpAttribute() : base(_Message)
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

        return str.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
               || str.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
               || CheckIsLocalUrl(str);
    }

    internal static bool CheckIsLocalUrl([NotNullWhen(true)] string? url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        // Allows "/" or "/foo" but not "//" or "/\".
        if (url[0] == '/')
        {
            // url is exactly "/"
            if (url.Length == 1)
                return true;

            // url doesn't start with "//" or "/\"
            if ((url[1] != '/') && (url[1] != '\\'))
                return !HasControlCharacter(url.AsSpan(1));

            return false;
        }

        // Allows "~/" or "~/foo" but not "~//" or "~/\".
        if ((url[0] == '~') && (url.Length > 1) && (url[1] == '/'))
        {
            // url is exactly "~/"
            if (url.Length == 2)
                return true;

            // url doesn't start with "~//" or "~/\"
            if ((url[2] != '/') && (url[2] != '\\'))
                return !HasControlCharacter(url.AsSpan(2));

            return false;
        }

        return false;

        static bool HasControlCharacter(ReadOnlySpan<char> readOnlySpan)
        {
            // URLs may not contain ASCII control characters.
            for (int i = 0; i < readOnlySpan.Length; i++)
                if (char.IsControl(readOnlySpan[i]))
                    return true;

            return false;
        }
    }

    #endregion
}