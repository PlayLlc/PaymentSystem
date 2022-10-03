using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.ValueObjects
{
    public record Name : ValueObject<string>
    {
        #region Constructor

        /// <exception cref="ValueObjectException"></exception>
        public Name(string value) : base(value)
        {
            if (value == string.Empty)
                throw new ValueObjectException($"The {nameof(Name)} provided was empty: [{value}]");
            if (value.Length > 50)
                throw new ValueObjectException($"The {nameof(Name)} provided was too long: [{value}]");
        }

        #endregion
    }
}