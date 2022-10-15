using Play.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Enums;
using Play.Core;

namespace Play.Accounts.Domain.ValueObjects
{
    public record StateAbbreviation : ValueObject<string>
    {
        #region Constructor

        /// <exception cref="ValueObjectException"></exception>
        public StateAbbreviation(string value) : base(value)
        {
            if (value.Length != 2)
                throw new ValueObjectException(
                    $"An instance of the {nameof(StateAbbreviation)} could not be created because the {nameof(value)} argument: [{value}] is an invalid {nameof(StateAbbreviations)}");

            if (!StateAbbreviations.Empty.TryGet(value, out EnumObjectString? result))
                throw new ValueObjectException(
                    $"An instance of the {nameof(StateAbbreviation)} could not be created because the {nameof(value)} argument: [{value}] is an invalid {nameof(StateAbbreviations)}");
        }

        #endregion
    }
}