using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Globalization.Time;

namespace Play.Globalization.Extensions;

public static class DateTimeUtcExtensions
{
    #region Instance Members

    /// <exception cref="OverflowException"></exception>
    public static int GetBusinessDays(this DateTimeUtc from, DateTimeUtc to)
    {
        int dayDifference = (int) to.Subtract(from).TotalDays;

        return Enumerable.Range(1, dayDifference).Select(from.AddDays)
            .Count(x => (x.GetDayOfTheWeek() != DayOfWeek.Saturday) && (x.GetDayOfTheWeek() != DayOfWeek.Sunday));
    }

    #endregion
}