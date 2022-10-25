using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Mvc.Attributes
{
    public class DateTimeUtcAttribute : ValidationAttribute
    {
        #region Instance Members

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not DateTime dateTime)
                return new ValidationResult($"This must be a {nameof(DateTime)} object");

            if (dateTime.Kind != DateTimeKind.Utc)
                return new ValidationResult("DateTime is not UTC format");

            return ValidationResult.Success;
        }

        #endregion
    }
}