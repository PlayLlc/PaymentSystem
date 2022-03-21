using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine
{
    internal partial class OfflineBalanceReader : CommonProcessing
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
                KernelDatabase kernelDatabase,
                DataExchangeKernelService dataExchangeKernelService,
                IGetKernelState kernelStateResolver,
                IHandlePcdRequests pcdEndpoint) : base(kernelDatabase, dataExchangeKernelService, kernelStateResolver, pcdEndpoint)
            { }

            #endregion

            #region Instance Members

            public StateId[] GetValidStateIds() => _ValidStateIds;

            /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
            /// <exception cref="TerminalDataException"></exception>
            public override KernelState Process(IGetKernelStateId kernelStateId, Kernel2Session session)
            {
                HandleRequestOutOfSync(kernelStateId.GetStateId());

                if (!_KernelDatabase.TryGet(ApplicationCapabilitiesInformation.Tag, out PrimitiveValue? applicationCapabilitiesInformation))
                    return _KernelStateResolver.GetKernelState(kernelStateId.GetStateId());

                if (!((ApplicationCapabilitiesInformation) applicationCapabilitiesInformation!).SupportForBalanceReading())
                    return _KernelStateResolver.GetKernelState(kernelStateId.GetStateId());

                if (!_KernelDatabase.IsPresent(BalanceReadAfterGenAc.Tag))
                    return _KernelStateResolver.GetKernelState(kernelStateId.GetStateId());

                GetDataRequest capdu = GetDataRequest.Create(OfflineAccumulatorBalance.Tag, session.GetTransactionSessionId());
                _PcdEndpoint.Request(capdu);

                return _KernelStateResolver.GetKernelState(WaitingForPostGenAcBalance.StateId);
            }

            #endregion
        }
    }
}