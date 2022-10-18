using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Events;
using Play.Globalization.Time;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Aggregates;

namespace Play.Accounts.Domain;

public record UserRegistrationCreated : DomainEvent
{
    #region Instance Values

    public readonly string Id;
    public readonly string Email;

    #endregion

    #region Constructor

    public UserRegistrationCreated(string id, string email) : base(
        $"The {nameof(UserRegistration)} with {nameof(Id)}: [{id}]; and {nameof(Email)}: [{email}] has begun the registration process")
    {
        Id = id;
        Email = email;
    }

    #endregion
}