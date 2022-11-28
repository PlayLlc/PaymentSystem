﻿using NServiceBus;

using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Repositories;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates.Rules;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;
using Play.Loyalty.Domain.ValueObjects;

namespace Play.Loyalty.Domain.Aggregates;

public partial class Member : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    private readonly Rewards _Rewards;

    /// <summary>
    ///     The RewardsNumber is the number that the Loyalty Member provides to the merchant at the time of sale to earn
    ///     rewards or to apply discounted items to the sale
    /// </summary>
    private readonly RewardsNumber _RewardsNumber;

    private Phone _Phone;
    private Name _Name;
    private Email? _Email;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Member()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Member(LoyaltyMemberDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _MerchantId = new SimpleStringId(dto.MerchantId);
        _RewardsNumber = new RewardsNumber(dto.RewardsNumber);
        _Rewards = new Rewards(dto.Rewards);
        _Name = new Name(dto.Name);
        _Phone = new Phone(dto.Phone);
        _Email = dto.Email is null ? null : new Email(dto.Email);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Member(string id, string merchantId, string name, string phone, string rewardsNumber, Rewards rewards, string? email = null)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _RewardsNumber = new RewardsNumber(rewardsNumber);
        _Rewards = rewards;
        _Name = new Name(name);
        _Phone = new Phone(phone);
        _Email = email is null ? null : new Email(email);
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task Update(IRetrieveUsers userRetriever, UpdateLoyaltyMember command)
    {
        // Enforce
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Member>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Member>(_MerchantId, user));
        _Name = new Name(command.Name);
        _Email = command.Email is null ? null : new Email(command.Email);
        _Phone = new Phone(command.Phone);

        Publish(new LoyaltyMemberUpdated(this, _MerchantId, user.Id));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static async Task<Member> Create(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever, IEnsureRewardsNumbersAreUnique uniqueRewardNumberChecker,
        CreateLoyaltyMember command)
    {
        Rewards rewards = new Rewards(GenerateSimpleStringId(), 0, new Money(0, new NumericCurrencyCode(command.NumericCurrencyCode)));
        Member member = new Member(GenerateSimpleStringId(), command.MerchantId, command.Name, command.Phone, command.RewardsNumber, rewards, command.Email);

        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantRetriever.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));
        member.Enforce(new UserMustBeActiveToUpdateAggregate<Member>(user));
        member.Enforce(new AggregateMustBeUpdatedByKnownUser<Member>(command.MerchantId, user));
        member.Enforce(new MerchantMustBeActiveToCreateAggregate<Member>(merchant));
        member.Enforce(new RewardNumberMustNotAlreadyExist(uniqueRewardNumberChecker, merchant.Id, command.RewardsNumber));

        member.Publish(new LoyaltyMemberCreated(member, merchant.Id, user.Id));

        return member;
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task Remove(IRetrieveUsers userRetriever, RemoveLoyaltyMember command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Member>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Member>(_MerchantId, user));

        Publish(new LoyaltyMemberRemoved(this, command.UserId));
    }

    public override SimpleStringId GetId() => Id;

    public override LoyaltyMemberDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = _MerchantId,
            RewardsNumber = _RewardsNumber.Value,
            Rewards = _Rewards.AsDto(),
            Name = _Name,
            Email = _Email?.Value,
            Phone = _Phone
        };

    #endregion
}