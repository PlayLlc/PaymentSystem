using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

namespace Play.Persistence.Sql
{
    public static class ValueConverters
    {
        public static class FromDateTimeUtc
        {
            #region Static Metadata

            public static readonly ValueConverter<DateTimeUtc, DateTime> Convert = new(x => (DateTime) x, y => ToDateTimeUtc(y));

            #endregion

            #region Instance Members

            /// <exception cref="InvalidOperationException"></exception>
            private static DateTimeUtc ToDateTimeUtc(DateTime value)
            {
                if (new DateTimeOffset(value).Offset > new TimeSpan(0))
                    throw new InvalidOperationException($"The {nameof(DateTime)} type persisted to the database was not {nameof(DateTimeKind.Utc)}");

                return new DateTimeUtc(value.ToUniversalTime());
            }

            #endregion
        }
    }
}