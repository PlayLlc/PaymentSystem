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
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForExchangeRelayResistanceDataResponse : KernelState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForExchangeRelayResistanceDataResponse));

    #endregion

    #region Instance Values

    private readonly CommonProcessingS3R1 _S3R1;
    private readonly IGenerateUnpredictableNumber _UnpredictableNumberGenerator;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly IHandlePcdRequests _PcdEndpoint;

    #endregion

    #region Constructor

    public WaitingForExchangeRelayResistanceDataResponse(
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchange,
        IKernelEndpoint kernelEndpoint,
        IGetKernelState kernelStateResolver,
        IGenerateUnpredictableNumber unpredictableNumberGenerator,
        CommonProcessingS3R1 s3R1,
        IHandlePcdRequests pcdEndpoint) : base(kernelDatabase, dataExchange, kernelEndpoint)
    {
        _KernelStateResolver = kernelStateResolver;
        _UnpredictableNumberGenerator = unpredictableNumberGenerator;
        _S3R1 = s3R1;
        _PcdEndpoint = pcdEndpoint;
    }

    #endregion

    public override StateId GetStateId() => StateId;

    #region ACT

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region CLEAN

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="signal"></param>
    /// <returns></returns>
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
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

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
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion
}