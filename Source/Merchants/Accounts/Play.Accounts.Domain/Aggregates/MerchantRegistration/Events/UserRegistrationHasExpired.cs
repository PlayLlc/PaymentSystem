using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Events;
using Play.Globalization.Time;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationHasExpired : DomainEvent
{
    #region Instance Values

    public readonly string Id;

    #endregion

    #region Constructor

    public UserRegistrationHasExpired(string id) : base($"The user registration has expired for {nameof(UserRegistration)} with the {nameof(Id)}: [{id}];")
    {
        Id = id;
    }

    #endregion
}