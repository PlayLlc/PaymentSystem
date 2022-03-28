using System;

using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine
{
    public class S78 : CommonProcessing
    {
        #region Instance Values

        protected override StateId[] _ValidStateIds { get; } =
        {
            WaitingForMagStripeReadRecordResponse.StateId, WaitingForMagstripeFirstWriteFlag.StateId
        };

        #endregion

        #region Constructor

        public S78(
            KernelDatabase kernelDatabase, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
            IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint) : base(kernelDatabase, dataExchangeKernelService,
                                                                                   kernelStateResolver, pcdEndpoint, kernelEndpoint)
        { }

        #endregion

        public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
        {
            HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

            throw new NotImplementedException();
        }
    }
}