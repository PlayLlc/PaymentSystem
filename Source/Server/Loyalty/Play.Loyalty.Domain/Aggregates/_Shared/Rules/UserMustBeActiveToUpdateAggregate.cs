﻿using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;

namePlay.Loyalty.Domain.Entitiesgregates;

public class UserMustBeActiveToUpdateAggregate<_Aggregate> : BusinessRule<_Aggregate, SimpleStringId> where _Aggregate : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(User)} cannot update the {typeof(_Aggregate).Name} because the {nameof(User)} has been deactivated;";

    #endregion

    #region Constructor

    internal UserMustBeActiveToUpdateAggregate(User user)
    {
        _IsValid = user.IsActive;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override DeactivatedUserAttemptedToUpdateAggregate<_Aggregate> CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate) => new(aggregate, this);

    #endregion
}