﻿using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Entities;

public class ConfirmationCode : Entity<string>
{
    #region Instance Values

    public DateTimeUtc SentDate;
    public uint Code;

    public string Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public ConfirmationCode(ConfirmationCodeDto dto)
    {
        Id = dto.Id!;
        SentDate = new DateTimeUtc(dto.SentDate);
        Code = dto.Code!;
    }

    /// <exception cref="ValueObjectException"></exception>
    public ConfirmationCode(string id, DateTime sentDate, uint code)
    {
        Id = id;
        SentDate = new DateTimeUtc(sentDate);
        Code = code;
    }

    #endregion

    #region Instance Members

    public override string GetId()
    {
        return Id;
    }

    public bool IsExpired(TimeSpan validityPeriod)
    {
        return (DateTimeUtc.Now - SentDate) < validityPeriod;
    }

    public override ConfirmationCodeDto AsDto()
    {
        return new ConfirmationCodeDto()
        {
            Id = Id,
            SentDate = SentDate,
            Code = Code
        };
    }

    #endregion
}