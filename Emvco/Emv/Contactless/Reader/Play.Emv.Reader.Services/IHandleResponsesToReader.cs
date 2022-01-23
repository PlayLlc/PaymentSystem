using Play.Emv.Kernel.Contracts.SignalOut;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts.SignalOut;

namespace Play.Emv.Reader.Services;

public interface IHandleResponsesToReader
{
    public void Handle(OutSelectionResponse message);
    public void Handle(OutKernelResponse message);
    public void Handle(StopPcdAcknowledgedResponse message);
}