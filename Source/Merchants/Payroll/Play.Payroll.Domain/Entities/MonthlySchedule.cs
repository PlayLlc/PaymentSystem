using Play.Domain;
using Play.Domain.Common.ValueObjects;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public class MonthlySchedule : PaydaySchedule
{
    DateTimeUtc _Payday;
    DateTimeUtc? _SecondPayday;
    /// <exception cref="ValueObjectException"></exception>
    public MonthlySchedule(SimpleStringId id,
        DateTimeUtc payday, DateTimeUtc? secondPayday) : base(id, (RecurrenceType)(secondPayday is null ? new RecurrenceType(RecurrenceTypes.Monthly) : new RecurrenceType(RecurrenceTypes.SemiMonthly)))
    {
        _Payday = payday;
        _SecondPayday = secondPayday;
    }

    public override IDto AsDto()
    {  
        throw new NotImplementedException();
    }

    public override SimpleStringId GetId() => Id;

    public override PayPeriod CreateNextPayPeriod(SimpleStringId id)
    {
        int dayOfTheMonth = DateTimeUtc.Now.Day;
        DateTime a = new DateTime()

        DateTime.Now.Subtract()
        if (_Payday.Day < dayOfTheMonth)
            return new PayPeriod(id,)

        if(_Payday.Day)

            throw new NotImplementedException();
    }
}