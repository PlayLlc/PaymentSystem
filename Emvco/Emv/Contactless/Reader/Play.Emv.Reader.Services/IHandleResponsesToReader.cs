using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Reader.Services;

public interface IHandleResponsesToReader
{
    public void Handle(OutSelectionResponse message);
    public void Handle(OutKernelResponse message);
    public void Handle(StopPcdAcknowledgedResponse message);
}