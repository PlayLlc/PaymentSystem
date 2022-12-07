using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Identity.Contracts.Dtos;

namespace Play.Identity.Domain.Entities;

public class ConfirmationCode : Entity<SimpleStringId>
{
    #region Instance Values

    public DateTimeUtc SentDate;
    public uint Code;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private ConfirmationCode()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public ConfirmationCode(ConfirmationCodeDto dto)
    {
        Id = new(dto.Id!);
        SentDate = new(dto.SentDate);
        Code = dto.Code!;
    }

    /// <exception cref="ValueObjectException"></exception>
    public ConfirmationCode(string id, DateTime sentDate, uint code)
    {
        Id = new(id!);
        SentDate = new(sentDate);
        Code = code;
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public bool IsExpired(TimeSpan validityPeriod) => (DateTimeUtc.Now - SentDate) < validityPeriod;

    public override ConfirmationCodeDto AsDto() =>
        new()
        {
            Id = Id,
            SentDate = SentDate,
            Code = Code
        };

    #endregion
}