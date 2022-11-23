using NServiceBus;

using Play.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Enumerables;

using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Identity.Contracts;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Application.Handlers.NetworkEvents;

public class MerchantHandler : IHandleMessages<MerchantHasBeenRemovedEvent>
{
    #region Instance Values

    private readonly ILoyaltyProgramRepository _LoyaltyProgramRepository;
    private readonly ILoyaltyMemberRepository _LoyaltyMemberRepository;
    private readonly IRetrieveUsers _UserRetriever;

    #endregion

    #region Constructor

    public MerchantHandler(ILoyaltyProgramRepository loyaltyProgramRepository, ILoyaltyMemberRepository loyaltyMemberRepository, IRetrieveUsers userRetriever)
    {
        _LoyaltyProgramRepository = loyaltyProgramRepository;
        _LoyaltyMemberRepository = loyaltyMemberRepository;
        _UserRetriever = userRetriever;
    }

    #endregion

    #region Instance Members

    /// <exception cref=" NotFoundException"></exception>
    /// <exception cref=" BusinessRuleValidationException"></exception>
    /// <exception cref=" ValueObjectException"></exception>
    public async Task Handle(MerchantHasBeenRemovedEvent message, IMessageHandlerContext context)
    {
        var loyaltyProgram = await _LoyaltyProgramRepository.GetByMerchantIdAsync(new SimpleStringId(message.MerchantId)).ConfigureAwait(false)
                             ?? throw new NotFoundException(typeof(LoyaltyProgram));

        await _LoyaltyMemberRepository.RemoveAll(new SimpleStringId(message.MerchantId)).ConfigureAwait(false);
        await loyaltyProgram.Remove(_UserRetriever, new RemoveLoyaltyProgram() {MerchantId = message.MerchantId}).ConfigureAwait(false);
    }

    #endregion
}