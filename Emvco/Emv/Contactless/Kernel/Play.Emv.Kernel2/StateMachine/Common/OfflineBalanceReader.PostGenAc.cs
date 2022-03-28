using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

using KernelDatabase = Play.Emv.Kernel.Databases.KernelDatabase;

namespace Play.Emv.Kernel2.StateMachine;

public partial class OfflineBalanceReader : CommonProcessing
{
    /// <summary>
    ///     A Card may have an offline balance, and some products require the balance to be read and made available to the
    ///     customer, either on a receipt or on a display.<see cref="BalanceReadAfterGenAc" /> is often displayed to the user
    ///     when the transaction was approved offline
    /// </summary>
    /// <remarks>Emv Book C-2 Section 3.10</remarks>
    private class PostGenAcBalanceReader : CommonProcessing
    {
        #region Instance Values

        protected override StateId[] _ValidStateIds { get; } =
        {
            WaitingForGenerateAcResponse1.StateId, WaitingForRecoverAcResponse.StateId, WaitingForGenerateAcResponse2.StateId,
            WaitingForPutDataResponseAfterGenerateAc.StateId
        };

        #endregion

        #region Constructor

        public PostGenAcBalanceReader(
            KernelDatabase kernelDatabase, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
            IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint) : base(kernelDatabase, dataExchangeKernelService,
                                                                                   kernelStateResolver, pcdEndpoint, kernelEndpoint)
        { }

        #endregion

        #region Instance Members

        public StateId[] GetValidStateIds() => _ValidStateIds;

        /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
        {
            HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

            if (!_KernelDatabase.TryGet(ApplicationCapabilitiesInformation.Tag, out PrimitiveValue? applicationCapabilitiesInformation))
                return currentStateIdRetriever.GetStateId();

            if (!((ApplicationCapabilitiesInformation) applicationCapabilitiesInformation!).SupportForBalanceReading())
                return currentStateIdRetriever.GetStateId();

            if (!_KernelDatabase.IsPresent(BalanceReadAfterGenAc.Tag))
                return currentStateIdRetriever.GetStateId();

            GetDataRequest capdu = GetDataRequest.Create(OfflineAccumulatorBalance.Tag, session.GetTransactionSessionId());
            _PcdEndpoint.Request(capdu);

            return WaitingForPostGenAcBalance.StateId;
        }

        #endregion
    }
}