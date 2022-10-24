using Play.Accounts.Contracts.Dtos;
using Play.Core.Exceptions;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Entities;

public class PersonalDetail : Entity<string>
{
    #region Instance Values

    /// <summary>
    ///     The last four digits of the user's Social Security Number
    /// </summary>
    public readonly string LastFourOfSocial;

    public readonly DateTimeUtc DateOfBirth;

    public string Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private PersonalDetail()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public PersonalDetail(PersonalDetailDto dto)
    {
        Id = dto.Id!;
        LastFourOfSocial = dto.LastFourOfSocial;

        try
        {
            DateOfBirth = new DateTimeUtc(dto.DateOfBirth);
        }
        catch (PlayInternalException e)
        {
            throw new ValueObjectException($"The {nameof(DateOfBirth)} must be in {nameof(DateTimeKind.Utc)} format", e);
        }
    }

    /// <exception cref="ValueObjectException"></exception>
    public PersonalDetail(string id, string lastFourOfSocial, DateTime dateOfBirth)
    {
        Id = id;
        LastFourOfSocial = lastFourOfSocial;

        try
        {
            DateOfBirth = new DateTimeUtc(dateOfBirth);
        }
        catch (PlayInternalException e)
        {
            throw new ValueObjectException($"The {nameof(DateOfBirth)} must be in {nameof(DateTimeKind.Utc)} format", e);
        }
    }

    #endregion

    #region Instance Members

    public override string GetId()
    {
        return Id;
    }

    public override PersonalDetailDto AsDto()
    {
        return new PersonalDetailDto
        {
            Id = Id,
            DateOfBirth = new DateTimeUtc(DateOfBirth),
            LastFourOfSocial = LastFourOfSocial
        };
    }

    #endregion
}