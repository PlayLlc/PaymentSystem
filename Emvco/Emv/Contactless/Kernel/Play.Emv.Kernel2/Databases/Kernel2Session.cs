using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel2.Configuration;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2Session : KernelSession
{
    #region Instance Values

    private readonly Kernel2SessionConfiguration _Kernel2SessionConfiguration;

    #endregion

    #region Constructor

    public Kernel2Session(
        KernelSessionId kernelSessionId,
        IHandleTerminalRequests terminalRequests,
        KernelDatabase kernelDatabase,
        ISendTerminalQueryResponse kernelEndpoint) : base(kernelSessionId, terminalRequests, kernelDatabase, kernelEndpoint)
    {
        _Kernel2SessionConfiguration = new Kernel2SessionConfiguration();
    }

    #endregion
}