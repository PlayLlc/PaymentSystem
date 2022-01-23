using System.Threading.Tasks;

namespace Play.Emv.Timeouts;

public abstract class TimeoutManager
{
    #region Instance Values

    protected readonly TimeoutConfigurationFactory _TimeoutConfigurationFactory;

    #endregion

    #region Instance Members

    public abstract Task<K> Execute<T, K>(T action);

    #endregion
}