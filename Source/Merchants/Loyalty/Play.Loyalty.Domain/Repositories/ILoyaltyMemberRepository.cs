﻿using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Domain.Repositories;

public interface ILoyaltyMemberRepository : IRepository<Member, SimpleStringId>
{
    #region Instance Members

    public Task RemoveAll(SimpleStringId merchantId);

    #endregion
}