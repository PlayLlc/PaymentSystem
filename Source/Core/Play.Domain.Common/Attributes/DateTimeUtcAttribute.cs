using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Globalization.Time;

public class DateTimeUtcAttribute : ValidationAttribute
{
    #region Instance Members

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not DateTime dateTime)
            return new($"This must be a {nameof(DateTime)} object");

        if (dateTime.Kind != DateTimeKind.Utc)
            return new("DateTime is not UTC format");

        return ValidationResult.Success;
    }

    #endregion
}