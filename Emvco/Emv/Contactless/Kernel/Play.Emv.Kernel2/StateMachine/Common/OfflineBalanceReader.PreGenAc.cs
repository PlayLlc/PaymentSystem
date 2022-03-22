﻿using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

using KernelDatabase = Play.Emv.Kernel.Databases.KernelDatabase;

namespace Play.Emv.Kernel2.StateMachine
{
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
                KernelDatabase kernelDatabase, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
                IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint) : base(kernelDatabase, dataExchangeKernelService,
                                                                                       kernelStateResolver, pcdEndpoint, kernelEndpoint)
            { }

            #endregion

            #region Instance Members

            public StateId[] GetValidStateIds() => _ValidStateIds;

            /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
            /// <exception cref="TerminalDataException"></exception>
            public override StateId Process(IGetKernelStateId kernelStateId, Kernel2Session session)
            {
                HandleRequestOutOfSync(kernelStateId.GetStateId());

                if (!_KernelDatabase.TryGet(ApplicationCapabilitiesInformation.Tag, out PrimitiveValue? applicationCapabilitiesInformation))
                    return kernelStateId.GetStateId();

                if (!((ApplicationCapabilitiesInformation) applicationCapabilitiesInformation!).SupportForBalanceReading())
                    return kernelStateId.GetStateId();

                if (!_KernelDatabase.IsPresent(BalanceReadBeforeGenAc.Tag))
                    return kernelStateId.GetStateId();

                GetDataRequest capdu = GetDataRequest.Create(OfflineAccumulatorBalance.Tag, session.GetTransactionSessionId());
                _PcdEndpoint.Request(capdu);

                return WaitingForPreGenAcBalanace.StateId;
            }

            #endregion
        }
    }
}