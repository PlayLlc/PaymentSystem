using Play.Accounts.Contracts.Dtos;
using Play.Domain.Entities;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Entities;

public class PersonalDetail : Entity<string>
{
    #region Instance Values

    /// <summary>
    ///     The last four digits of the user's Social Security Number
    /// </summary>
    public readonly string LastFourOfSocial;

    public readonly DateTime DateOfBirth;

    public string Id { get; }

    #endregion

    #region Constructor

    public PersonalDetail(PersonalDetailDto dto)
    {
        Id = dto.Id!;
        LastFourOfSocial = dto.LastFourOfSocial;
        DateOfBirth = dto.DateOfBirth;
    }

    /// <exception cref="ArgumentException"></exception>
    public PersonalDetail(string id, string lastFourOfSocial, DateTime dateOfBirth)
    {
        if (dateOfBirth.Kind != DateTimeKind.Utc)
            throw new ArgumentException($"The {nameof(dateOfBirth)} must be in {nameof(DateTimeKind.Utc)} format");

        Id = id;
        LastFourOfSocial = lastFourOfSocial;
        DateOfBirth = dateOfBirth;
    }

    #endregion

    #region Instance Members

    public override string GetId()
    {
        return Id;
    }

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
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