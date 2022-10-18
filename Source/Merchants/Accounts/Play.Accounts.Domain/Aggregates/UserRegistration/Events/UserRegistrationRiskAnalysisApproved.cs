using Play.Accounts.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain;

public record UserRegistrationRiskAnalysisApproved : DomainEvent
{
    #region Instance Values

    public readonly string Id;
    public readonly string Username;

    #endregion

    #region Constructor

    public UserRegistrationRiskAnalysisApproved(string id, string username) : base(
        $"The {nameof(UserRegistration)} with {nameof(Id)}: [{id}]; and {nameof(Username)}: [{username}] has been approved")
    {
        Id = id;
        Username = username;
    }

    #endregion
}