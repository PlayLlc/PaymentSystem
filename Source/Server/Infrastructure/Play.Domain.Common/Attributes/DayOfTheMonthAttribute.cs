using System.ComponentModel.DataAnnotations;

using Play.Globalization.Time;

namespace Play.Domain.Common.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class DayOfTheMonthAttribute : ValidationAttribute
{
    #region Static Metadata

    private const string _Message = "The field must only contain a day of the month.";

    #endregion

    #region Constructor

    public DayOfTheMonthAttribute() : base(_Message)
    { }

    #endregion

    #region Instance Members

    public override bool IsValid(object? value)
    {
        if (value is DaysOfTheMonth)
            return true;

        if (value is int intValue)
            return (intValue >= DaysOfTheMonth.Empty.GetMinDayOfTheMonth()) && (intValue <= DaysOfTheMonth.Empty.GetMaxDayOfTheMonth());

        return false;
    }

    #endregion
}