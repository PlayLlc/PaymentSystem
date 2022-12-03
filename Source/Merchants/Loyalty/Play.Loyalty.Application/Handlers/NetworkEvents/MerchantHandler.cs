using NServiceBus;

using Play.Domain.Common.ValueObjects;
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

    private readonly IProgramsRepository _ProgramsRepository;
    private readonly IMemberRepository _MemberRepository;
    private readonly IRetrieveUsers _UserRetriever;

    #endregion

    #region Constructor

    public MerchantHandler(IProgramsRepository programsRepository, IMemberRepository memberRepository, IRetrieveUsers userRetriever)
    {
        _ProgramsRepository = programsRepository;
        _MemberRepository = memberRepository;
        _UserRetriever = userRetriever;
    }

    #endregion

    #region Instance Members

    /// <exception cref=" NotFoundException"></exception>
    /// <exception cref=" BusinessRuleValidationException"></exception>
    /// <exception cref=" ValueObjectException"></exception>
    public async Task Handle(MerchantHasBeenRemovedEvent message, IMessageHandlerContext context)
    {
        Programs programs = await _ProgramsRepository.GetByMerchantIdAsync(new(message.MerchantId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        await _MemberRepository.RemoveAll(new(message.MerchantId)).ConfigureAwait(false);
        await programs.Remove(_UserRetriever, new() {MerchantId = message.MerchantId}).ConfigureAwait(false);
    }

    #endregion
}