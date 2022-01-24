using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Terminal;

public interface ISendTerminalQueryResponse
{
    public void Send(QueryTerminalResponse message);
}