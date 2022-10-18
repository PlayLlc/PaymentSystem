using Play.Accounts.Domain.ValueObjects;
using Play.Domain;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

public class ConfirmationCode : Entity<string>
{
    #region Instance Values

    public UniversalDateTime SentDate;
    public uint Code;

    public string Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public ConfirmationCode(ConfirmationCodeDto dto)
    {
        Id = dto.Id!;
        SentDate = new UniversalDateTime(dto.SentDate);
        Code = dto.Code!;
    }

    /// <exception cref="ValueObjectException"></exception>
    public ConfirmationCode(string id, DateTime sentDate, uint code)
    {
        Id = id;
        SentDate = new UniversalDateTime(sentDate);
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
        return (DateTimeUtc.Now - SentDate.Value) < validityPeriod;
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