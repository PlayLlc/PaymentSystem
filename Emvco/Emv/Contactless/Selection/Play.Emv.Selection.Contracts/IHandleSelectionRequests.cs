using Play.Emv.Selection.Contracts.SignalIn;

namespace Play.Emv.Selection.Contracts;

public interface IHandleSelectionRequests
{
    public void Request(ActivateSelectionRequest message);
    public void Request(StopSelectionRequest message);
}