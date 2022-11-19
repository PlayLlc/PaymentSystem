using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.StateMachine;
using Play.Messaging;

namespace Play.Testing.Emv.Contactless.Stubs;

public class WaitingForPdolDataStub : WaitingForPdolData
{
    public WaitingForPdolDataStub() : base(null, null, null, null, null) { }

    public WaitingForPdolDataStub(
        KernelDatabase database,
        DataExchangeKernelService dataExchangeKernelService,
        IEndpointClient endpointClient,
        IManageTornTransactions tornTransactionLog,
        IGetKernelState kernelStateResolver)
        : base(database, dataExchangeKernelService, endpointClient, tornTransactionLog, kernelStateResolver)
    {
    }
}
