using Play.Codecs;
using Play.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Domain.Common.ValueObjects;

/// <summary>
///     The account number of an Employee's checking account
/// </summary>
public record AccountNumber : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private AccountNumber()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public AccountNumber(string value) : base(value)
    {
        if (!PlayCodec.AlphaNumericCodec.IsValid(value))
            throw new ValueObjectException($"The {nameof(AccountNumber)} must be comprised of numeric characters only but was not: [{value}]");

        // TODO: Try and track down specifications for an account number that can satisfy both American account numbers and international account numbers. America doesn't participate in IBAN. There's got to be a SWIFT specification for this somewhere
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(AccountNumber value) => value.Value;

    #endregion
}