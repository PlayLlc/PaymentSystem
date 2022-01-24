using Play.Emv.Selection.Contracts;

namespace Play.Emv.Selection.Services;

internal interface ISendSelectionResponses
{
    public void Send(OutSelectionResponse message);
}