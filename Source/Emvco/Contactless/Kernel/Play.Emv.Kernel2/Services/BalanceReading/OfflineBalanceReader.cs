using System.Linq;

using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services.BalanceReading;

// TODO: This does not need to inherit from CommonProcessing. We need to abstract this service from the Kernel 2 implementation and move this into the common Play.Emv.Kernel module

/// <summary>
///     A Card may have an offline balance, and some products require the balance to be read and made available to the
///     customer, either on a receipt or on a display. Not all cards support balance reading and those that do explicitly
///     indicate it in the Application Capabilities Information.
/// </summary>
/// <remarks>Emv Book C-2 Section 3.10</remarks>
internal partial class OfflineBalanceReader : CommonProcessing, IReadOfflineBalance
{
    #region Instance Values

    private readonly PreGenAcBalanceReader _PreGenAcBalanceReader;
    private readonly PostGenAcBalanceReader _PostGenAcBalanceReader;

    protected override StateId[] _ValidStateIds { get; } =
    {
        WaitingForEmvReadRecordResponse.StateId, WaitingForGetDataResponse.StateId, WaitingForEmvModeFirstWriteFlag.StateId,
        WaitingForGenerateAcResponse1.StateId, WaitingForRecoverAcResponse.StateId, WaitingForGenerateAcResponse2.StateId,
        WaitingForPutDataResponseAfterGenerateAc.StateId
    };

    #endregion

    #region Constructor

    public OfflineBalanceReader(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver, IHandlePcdRequests pcdEndpoint,
        IKernelEndpoint kernelEndpoint) : base(database, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint)
    {
        _PreGenAcBalanceReader = new PreGenAcBalanceReader(database, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint);
        _PostGenAcBalanceReader = new PostGenAcBalanceReader(database, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint);
    }

    #endregion

    #region Instance Members

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// >
    public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
    {
        HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

        if (_PreGenAcBalanceReader.GetValidStateIds().Any(a => a == currentStateIdRetriever.GetStateId()))
            return _PreGenAcBalanceReader.Process(currentStateIdRetriever, session, message);

        return _PostGenAcBalanceReader.Process(currentStateIdRetriever, session, message);
    }

    #endregion
}