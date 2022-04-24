using Play.Emv.Selection.Contracts;

namespace Play.Emv.Selection.Services;

internal interface ISendSelectionResponses
{
    #region Instance Members

    public void Send(OutSelectionResponse message);

    #endregion
}