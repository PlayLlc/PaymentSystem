using Play.Emv.Terminal.Contracts.SignalIn;

namespace Play.Emv.Terminal.Contracts;

public interface IHandleTerminalRequests
{
    public void Request(QueryTerminalRequest message);
    public void Request(ActivateTerminalRequest message);
    public void Request(InitiateSettlementRequest message);
}