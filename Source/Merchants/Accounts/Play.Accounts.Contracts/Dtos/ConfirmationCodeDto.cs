using Play.Domain;

namespace Play.Accounts.Domain.Aggregates;

public class ConfirmationCodeDto : IDto
{
    #region Instance Values

    public string Id { get; set; } = string.Empty;
    public DateTime SentDate { get; set; }
    public uint Code { get; set; }

    #endregion
}