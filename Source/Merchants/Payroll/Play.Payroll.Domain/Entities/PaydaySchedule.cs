using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Globalization.Currency;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.ValueObjects;
using Play.Domain;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Enums;

namespace Play.Payroll.Domain.Entities
{
    public class WeeklySchedule : PaydaySchedule
    {
        public WeeklySchedule(SimpleStringId id, RecurrenceType recurrenceType) : base(id, recurrenceType)
        {
        }

        public override IDto AsDto()
        {
            throw new NotImplementedException();
        }

        public override SimpleStringId GetId() => Id;

        public override PayPeriod GetPayPeriod()
        {
            throw new NotImplementedException();
        }
    }
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



    public abstract class PaydaySchedule : Entity<SimpleStringId>
    {
        #region Instance Values

        private readonly RecurrenceType _Recurrence; 
        public override SimpleStringId Id { get; }
        public abstract PayPeriod GetPayPeriod();
        #endregion
         
        // Constructor for EF only
        protected PaydaySchedule(SimpleStringId id, RecurrenceType recurrence)
        {
            Id = id;
            _Recurrence = recurrence;
        }
            
    }
}
