using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Inventory.Contracts.Dtos;

namespace Play.Persistence.Sql;

public static class ValueConverters
{
    #region Instance Members

    public static void ConfigureCommonConverters(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<SimpleStringId>().HaveConversion<SimpleStringIdConverter>();

        configurationBuilder.Properties<IEnumerable<SimpleStringId>>().HaveConversion<EnumerableSimpleStringIdConverter>();
        configurationBuilder.Properties<Name>().HaveConversion<NameConverter>();
        configurationBuilder.Properties<Email>().HaveConversion<EmailConverter>();
        configurationBuilder.Properties<Phone>().HaveConversion<PhoneConverter>();
        configurationBuilder.Properties<State>().HaveConversion<StateConverter>();
        configurationBuilder.Properties<Money>().HaveConversion<MoneyConverter>();
        configurationBuilder.Properties<Zipcode>().HaveConversion<ZipcodeConverter>();
        configurationBuilder.Properties<DateTimeUtc>().HaveConversion<DateTimeUtcConverter>();
        configurationBuilder.Properties<NumericCurrencyCode>().HaveConversion<NumericCurrencyCodeConverter>();
        configurationBuilder.Properties<HashSet<SimpleStringId>>().HaveConversion<HashSetSimpleStringIdConverter>();
    }

    #endregion

    public class SimpleStringIdConverter : ValueConverter<SimpleStringId, string>
    {
        #region Constructor

        public SimpleStringIdConverter() : base(x => x.Value, y => new SimpleStringId(y))
        { }

        #endregion
    }

    public class EnumerableSimpleStringIdConverter : ValueConverter<IEnumerable<SimpleStringId>, IEnumerable<string>>
    {
        #region Constructor

        public EnumerableSimpleStringIdConverter() : base(x => x.Select(a => a.Value), y => y.Select(a => new SimpleStringId(a)))
        { }

        #endregion
    }

    public class HashSetSimpleStringIdConverter : ValueConverter<HashSet<SimpleStringId>, IEnumerable<string>>
    {
        #region Constructor

        public HashSetSimpleStringIdConverter() : base(x => x.Select(a => a.Value), y => y.Select(a => new SimpleStringId(a)).ToHashSet())
        { }

        #endregion
    }

    public class NameConverter : ValueConverter<Name, string>
    {
        #region Constructor

        public NameConverter() : base(x => x.Value, y => new Name(y))
        { }

        #endregion
    }

    public class EmailConverter : ValueConverter<Email, string>
    {
        #region Constructor

        public EmailConverter() : base(x => x.Value, y => new Email(y))
        { }

        #endregion
    }

    public class PhoneConverter : ValueConverter<Phone, string>
    {
        #region Constructor

        public PhoneConverter() : base(x => x.Value, y => new Phone(y))
        { }

        #endregion
    }

    public class StateConverter : ValueConverter<State, string>
    {
        #region Constructor

        public StateConverter() : base(x => x.Value, y => new State(y))
        { }

        #endregion
    }

    public class ZipcodeConverter : ValueConverter<Zipcode, string>
    {
        #region Constructor

        public ZipcodeConverter() : base(x => x.Value, y => new Zipcode(y))
        { }

        #endregion
    }

    public class DateTimeUtcConverter : ValueConverter<DateTimeUtc, DateTime>
    {
        #region Constructor

        public DateTimeUtcConverter() : base(x => (DateTime) x, y => ToDateTimeUtc(y))
        { }

        #endregion

        #region Instance Members

        /// <exception cref="RepositoryException"></exception>
        private static DateTimeUtc ToDateTimeUtc(DateTime value)
        {
            try
            {
                if (new DateTimeOffset(value).Offset > new TimeSpan(0))
                    throw new InvalidOperationException($"The {nameof(DateTime)} type persisted to the database was not {nameof(DateTimeKind.Utc)}");

                return new DateTimeUtc(value.ToUniversalTime());
            }
            catch (Exception e)
            {
                throw new RepositoryException($"An exception occured converting a database {nameof(DateTime)} to a {nameof(DateTimeUtc)} or visa versa", e);
            }
        }

        #endregion
    }

    public class NumericCurrencyCodeConverter : ValueConverter<NumericCurrencyCode, ushort>
    {
        #region Constructor

        public NumericCurrencyCodeConverter() : base(x => (ushort) x, y => new NumericCurrencyCode(y))
        { }

        #endregion
    }

    public class MoneyConverter : ValueConverter<Money, MoneyDto>
    {
        #region Constructor

        public MoneyConverter() : base(x => new MoneyDto(x), y => y.AsMoney())
        { }

        #endregion
    }

    //NumericCurrencyCode
}