using Play.Emv.Selection.Contracts.SignalOut;

namespace Play.Emv.Selection.Services;

internal interface ISendSelectionResponses
{
    public void Send(OutSelectionResponse message);
}