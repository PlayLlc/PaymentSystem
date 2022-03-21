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

public partial class WaitingForEmvModeFirstWriteFlag : KernelState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForEmvModeFirstWriteFlag));

    #endregion

    #region Instance Values

    private readonly IGenerateUnpredictableNumber _UnpredictableNumberGenerator;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly ICleanTornTransactions _KernelCleaner;
    private readonly S456 _S456;

    #endregion

    #region Constructor

    public WaitingForEmvModeFirstWriteFlag(
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchange,
        IKernelEndpoint kernelEndpoint,
        IGenerateUnpredictableNumber unpredictableNumberGenerator,
        IHandleTerminalRequests terminalEndpoint,
        IHandlePcdRequests pcdEndpoint,
        IGetKernelState kernelStateResolver,
        ICleanTornTransactions kernelCleaner,
        S456 s456) : base(kernelDatabase, dataExchange, kernelEndpoint)
    {
        _UnpredictableNumberGenerator = unpredictableNumberGenerator;
        _TerminalEndpoint = terminalEndpoint;
        _PcdEndpoint = pcdEndpoint;
        _KernelStateResolver = kernelStateResolver;
        _KernelCleaner = kernelCleaner;
        _S456 = s456;
    }

    #endregion

    public override StateId GetStateId() => StateId;

    #region ACT

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region CLEAN

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(CleanKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region RAPDU

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region DET

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion
}