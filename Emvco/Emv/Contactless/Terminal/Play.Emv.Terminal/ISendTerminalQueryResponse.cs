using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Terminal;

public interface ISendTerminalQueryResponse
{
    #region Instance Members

    public void Send(QueryTerminalResponse message);

    #endregion
}