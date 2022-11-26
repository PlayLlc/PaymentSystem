using Play.Domain.Common.ValueObjects;

namespace Play.TimeClock.Domain.Services;

public interface IEnsureEmployeeDoesNotExist
{
    #region Instance Members

    public bool DoesEmployeeAlreadyExist(SimpleStringId merchantId, SimpleStringId userId);

    #endregion
}