using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Terminal.Services;

internal interface ISendTerminalResponses
{
    internal void Send(QueryTerminalResponse message);
}