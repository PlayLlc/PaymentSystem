using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForGpoResponse));

    #endregion

    #region Instance Values

    private readonly S3R1 _S3R1;
    private readonly IGenerateUnpredictableNumber _UnpredictableNumberGenerator;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly ICleanTornTransactions _KernelCleaner;

    #endregion

    #region Constructor

    public WaitingForGpoResponse(
        IKernelEndpoint kernelEndpoint,
        IHandleTerminalRequests terminalEndpoint,
        IHandlePcdRequests pcdEndpoint,
        IGetKernelState kernelStateResolver,
        ICleanTornTransactions kernelCleaner,
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchangeKernelService,
        S3R1 s3R1,
        IGenerateUnpredictableNumber unpredictableNumberGenerator) : base(kernelDatabase, dataExchangeKernelService, kernelEndpoint)
    {
        _TerminalEndpoint = terminalEndpoint;
        _PcdEndpoint = pcdEndpoint;
        _KernelStateResolver = kernelStateResolver;
        _KernelCleaner = kernelCleaner;
        _S3R1 = s3R1;
        _UnpredictableNumberGenerator = unpredictableNumberGenerator;
    }

    #endregion

    public override StateId GetStateId() => StateId;

    #region ACT

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region CLEAN

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(CleanKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region DET

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion
}