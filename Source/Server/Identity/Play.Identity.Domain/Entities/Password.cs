﻿using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Aggregates;

namespace Play.Identity.Domain.Entities;

public class Password : Entity<SimpleStringId>
{
    #region Instance Values

    public string HashedPassword;
    public DateTimeUtc CreatedOn;

    /// <summary>
    ///     A foreign key to the <see cref="User" /> object
    /// </summary>
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Password()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Password(string id, string hashedPassword, DateTimeUtc createdOn)
    {
        Id = new(id);
        HashedPassword = hashedPassword;
        CreatedOn = createdOn;
    }

    /// <exception cref="ValueObjectException"></exception>
    public Password(PasswordDto dto)
    {
        if (dto.CreatedOn.Kind != DateTimeKind.Utc)
            throw new ValueObjectException($"The {nameof(CreatedOn)} date must be in UTC format");

        Id = new(dto.Id);
        HashedPassword = dto.HashedPassword;
        CreatedOn = new(dto.CreatedOn);
    }

    #endregion

    #region Instance Members

    public bool IsExpired(TimeSpan validityPeriod) => (DateTimeUtc.Now - CreatedOn) > validityPeriod;

    public override SimpleStringId GetId() => Id;

    public override PasswordDto AsDto() =>
        new()
        {
            Id = Id,
            CreatedOn = CreatedOn,
            HashedPassword = HashedPassword
        };

    #endregion
}