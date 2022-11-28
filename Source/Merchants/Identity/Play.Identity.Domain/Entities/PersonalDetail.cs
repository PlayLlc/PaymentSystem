using Play.Core.Exceptions;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Identity.Contracts.Dtos;

namespace Play.Identity.Domain.Entities;

public class PersonalDetail : Entity<SimpleStringId>
{
    #region Instance Values

    /// <summary>
    ///     The last four digits of the user's Social Security Number
    /// </summary>
    public readonly string LastFourOfSocial;

    public readonly DateTimeUtc DateOfBirth;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private PersonalDetail()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public PersonalDetail(PersonalDetailDto dto)
    {
        Id = new SimpleStringId(dto.Id!);
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
    public PersonalDetail(string id, string lastFourOfSocial, DateTimeUtc dateOfBirth)
    {
        Id = new SimpleStringId(id);
        LastFourOfSocial = lastFourOfSocial;

        try
        {
            DateOfBirth = dateOfBirth;
        }
        catch (PlayInternalException e)
        {
            throw new ValueObjectException($"The {nameof(DateOfBirth)} must be in {nameof(DateTimeKind.Utc)} format", e);
        }
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public override PersonalDetailDto AsDto() =>
        new PersonalDetailDto
        {
            Id = Id,
            DateOfBirth = new DateTimeUtc(DateOfBirth),
            LastFourOfSocial = LastFourOfSocial
        };

    #endregion
}