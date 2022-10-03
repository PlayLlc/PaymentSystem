﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.ValueObjects
{
    public record Email : ValueObject<string>
    {
        #region Constructor

        /// <exception cref="ValueObjectException"></exception>
        public Email(string value) : base(value)
        {
            if (!new EmailAddressAttribute().IsValid(Value))
                throw new ValueObjectException($"The {nameof(Email)} provided was invalid: [{value}]");
        }

        #endregion
    }
}