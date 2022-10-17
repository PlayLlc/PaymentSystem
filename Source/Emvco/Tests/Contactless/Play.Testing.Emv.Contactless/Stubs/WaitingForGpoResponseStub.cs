using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.StateMachine;
using Play.Messaging;

namespace Play.Testing.Emv.Contactless.Stubs;

public class WaitingForGpoResponseStub : WaitingForGpoResponse
{
    public WaitingForGpoResponseStub() : base(null, null, null, null, null, null, null) {}

    public WaitingForGpoResponseStub(
        KernelDatabase database,
        DataExchangeKernelService dataExchangeKernelService,
        IEndpointClient endpointClient,
        IManageTornTransactions tornTransactionLog,
        IGetKernelState kernelStateResolver,
        IGenerateUnpredictableNumber unpredictableNumberGenerator, S3R1 s3R1)
        : base(database, dataExchangeKernelService, endpointClient, tornTransactionLog, kernelStateResolver, unpredictableNumberGenerator, s3R1)
    {
    }
}
