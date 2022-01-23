using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Transactions;

namespace Play.Emv.Kernel.Databases;

public interface IActivateKernelDatabase
{
    #region Instance Members

    public void Activate(
        KernelSessionId kernelSessionId,
        IHandleTerminalRequests terminalEndpoint,
        ISendTerminalQueryResponse kernelEndpoint,
        Transaction transaction);

    #endregion
}