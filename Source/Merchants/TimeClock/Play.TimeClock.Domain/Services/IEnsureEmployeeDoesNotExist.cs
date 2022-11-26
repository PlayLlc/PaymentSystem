namespace Play.TimeClock.Domain.Services;

public interface IEnsureEmployeeDoesNotExist
{
    #region Instance Members

    public bool DoesEmployeeAlreadyExist(string merchantId, string userId);

    #endregion
}