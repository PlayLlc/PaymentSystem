using System.Linq;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine
{
    /// <summary>
    ///     A Card may have an offline balance, and some products require the balance to be read and made available to the
    ///     customer, either on a receipt or on a display. Not all cards support balance reading and those that do explicitly
    ///     indicate it in the Application Capabilities Information.
    /// </summary>
    /// <remarks>Emv Book C-2 Section 3.10</remarks>
    internal partial class OfflineBalanceReader : CommonProcessing
    {
        #region Instance Values

        private readonly PreGenAcBalanceReader _PreGenAcBalanceReader;
        private readonly PreGenAcBalanceReader _PostGenAcBalanceReader;

        protected override StateId[] _ValidStateIds { get; } =
        {
            WaitingForEmvReadRecordResponse.StateId, WaitingForGetDataResponse.StateId, WaitingForEmvModeFirstWriteFlag.StateId,
            WaitingForGenerateAcResponse1.StateId, WaitingForRecoverAcResponse.StateId, WaitingForGenerateAcResponse2.StateId,
            WaitingForPutDataResponseAfterGenerateAc.StateId
        };

        #endregion

        #region Constructor

        public OfflineBalanceReader(
            KernelDatabase kernelDatabase,
            DataExchangeKernelService dataExchangeKernelService,
            IGetKernelState kernelStateResolver,
            IHandlePcdRequests pcdEndpoint) : base(kernelDatabase, dataExchangeKernelService, kernelStateResolver, pcdEndpoint)
        {
            _PreGenAcBalanceReader = new PreGenAcBalanceReader(kernelDatabase, dataExchangeKernelService, kernelStateResolver, pcdEndpoint);
            _PostGenAcBalanceReader =
                new PreGenAcBalanceReader(kernelDatabase, dataExchangeKernelService, kernelStateResolver, pcdEndpoint);
        }

        #endregion

        #region Instance Members

        /// <summary>
        /// </summary>
        /// <param name="kernelStateId"></param>
        /// <param name="session"></param>
        /// <exception cref="RequestOutOfSyncException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        /// >
        public override KernelState Process(IGetKernelStateId kernelStateId, Kernel2Session session)
        {
            HandleRequestOutOfSync(kernelStateId.GetStateId());

            if (_PreGenAcBalanceReader.GetValidStateIds().Any(a => a == kernelStateId.GetStateId()))
                return _PreGenAcBalanceReader.Process(kernelStateId, session);

            return _PostGenAcBalanceReader.Process(kernelStateId, session);
        }

        #endregion
    }
}