using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Terminal.Services;

public interface ISendTerminalQueryResponse
{
    internal void Send(QueryTerminalResponse message);
}