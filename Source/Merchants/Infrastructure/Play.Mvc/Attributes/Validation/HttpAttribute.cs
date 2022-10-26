using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Mvc.Attributes.Validation
{
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

            return str.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || str.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}