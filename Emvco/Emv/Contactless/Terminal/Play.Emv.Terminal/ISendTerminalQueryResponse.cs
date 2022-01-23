using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Terminal.Services;

public interface ISendTerminalQueryResponse
{
    public void Send(QueryTerminalResponse message);
}