using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Selection.Services;

public interface IHandleResponsesToSelection
{
    #region Instance Members

    public void Handle(QueryPcdResponse message);
    public void Handle(ActivatePcdResponse message);

    #endregion
}