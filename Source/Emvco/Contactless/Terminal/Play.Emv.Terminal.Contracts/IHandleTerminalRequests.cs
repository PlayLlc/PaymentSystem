using Play.Emv.Terminal.Contracts.SignalIn;

namespace Play.Emv.Terminal.Contracts;

public interface IHandleTerminalRequests
{
    #region Instance Members

    public void Request(QueryTerminalRequest message);
    public void Request(ActivateTerminalRequest message);

    #endregion
}