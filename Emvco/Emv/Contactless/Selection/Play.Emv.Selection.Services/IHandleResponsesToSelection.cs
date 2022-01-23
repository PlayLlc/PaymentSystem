using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Selection.Services;

public interface IHandleResponsesToSelection
{
    public void Handle(QueryPcdResponse message);
    public void Handle(ActivatePcdResponse message);
}