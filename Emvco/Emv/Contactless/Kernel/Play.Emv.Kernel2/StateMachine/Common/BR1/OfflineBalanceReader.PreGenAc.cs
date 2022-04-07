using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

using KernelDatabase = Play.Emv.Kernel.Databases.KernelDatabase;

namespace Play.Emv.Kernel2.StateMachine;

public partial class OfflineBalanceReader : CommonProcessing
{
    /// <summary>
    ///     A Card may have an offline balance, and some products require the balance to be read and made available to the
    ///     customer, either on a receipt or on a display.<see cref="BalanceReadBeforeGenAc" /> is often used for offline
    ///     transaction to determine if the customer has enough money to cover the amount of the transaction. This is often the
    ///     case for prepaid debit cards, for example
    /// </summary>
    /// <remarks>Emv Book C-2 Section 3.10</remarks>
    private class PreGenAcBalanceReader : CommonProcessing
    {
        #region Instance Values

        protected override StateId[] _ValidStateIds { get; } =
        {
            WaitingForEmvReadRecordResponse.StateId, WaitingForGetDataResponse.StateId, WaitingForEmvModeFirstWriteFlag.StateId
        };

        #endregion

        #region Constructor

        public PreGenAcBalanceReader(
            KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
            IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint) : base(database, dataExchangeKernelService, kernelStateResolver,
                                                                                   pcdEndpoint, kernelEndpoint)
        { }

        #endregion

        #region Instance Members

        public StateId[] GetValidStateIds() => _ValidStateIds;

        /// <exception cref="RequestOutOfSyncException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
        {
            HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

            if (!_Database.TryGet(ApplicationCapabilitiesInformation.Tag, out PrimitiveValue? applicationCapabilitiesInformation))
                return currentStateIdRetriever.GetStateId();

            if (!((ApplicationCapabilitiesInformation) applicationCapabilitiesInformation!).SupportForBalanceReading())
                return currentStateIdRetriever.GetStateId();

            if (!_Database.IsPresent(BalanceReadBeforeGenAc.Tag))
                return currentStateIdRetriever.GetStateId();

            GetDataRequest capdu = GetDataRequest.Create(OfflineAccumulatorBalance.Tag, session.GetTransactionSessionId());
            _PcdEndpoint.Request(capdu);

            return WaitingForPreGenAcBalanace.StateId;
        }

        #endregion
    }
}