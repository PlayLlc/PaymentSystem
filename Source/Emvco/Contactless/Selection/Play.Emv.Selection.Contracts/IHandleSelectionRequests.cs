namespace Play.Emv.Selection.Contracts;

public interface IHandleSelectionRequests
{
    #region Instance Members

    public void Request(ActivateSelectionRequest message);
    public void Request(StopSelectionRequest message);

    #endregion
}