using Play.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Application.Handlers.DomainEvents.LoyaltyMembers
{
    public partial class LoyaltyMemberHandler : DomainEventHandler, IHandleDomainEvents<LoyaltyMemberEarnedPoints>,
        IHandleDomainEvents<LoyaltyMemberEarnedRewards>, IHandleDomainEvents<LoyaltyMemberLostPoints>, IHandleDomainEvents<LoyaltyMemberLostRewards>,
        IHandleDomainEvents<RewardBalanceIsInsufficient>, IHandleDomainEvents<RewardProgramIsNotActive>
    {
        #region Instance Members

        public Task Handle(LoyaltyMemberEarnedPoints domainEvent) => throw new NotImplementedException();

        public Task Handle(LoyaltyMemberEarnedRewards domainEvent) => throw new NotImplementedException();

        public Task Handle(LoyaltyMemberLostPoints domainEvent) => throw new NotImplementedException();

        public Task Handle(LoyaltyMemberLostRewards domainEvent) => throw new NotImplementedException();

        public Task Handle(RewardBalanceIsInsufficient domainEvent) => throw new NotImplementedException();

        public Task Handle(RewardProgramIsNotActive domainEvent) => throw new NotImplementedException();

        #endregion
    }
}