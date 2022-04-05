using System;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public class S910 : CommonProcessing
{
    #region Instance Values

    private readonly IGenerateUnpredictableNumber _UnpredictableNumberGenerator;

    protected override StateId[] _ValidStateIds { get; } =
    {
        WaitingForMagStripeReadRecordResponse.StateId, WaitingForMagstripeFirstWriteFlag.StateId
    };

    #endregion

    #region Constructor

    public S910(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
        IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint, IGenerateUnpredictableNumber unpredictableNumberGenerator) :
        base(database, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint)
    {
        _UnpredictableNumberGenerator = unpredictableNumberGenerator;
    }

    #endregion

    public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
    {
        HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

        ErrorIndication errorIndication = _Database.GetErrorIndication();

        if (errorIndication.IsErrorPresent(Level2Error.StatusBytes))
            return ProcessC(currentStateIdRetriever, session); 

        if (errorIndication.IsErrorPresent(Level2Error.ParsingError))
            return ProcessC(currentStateIdRetriever, session);

        if (errorIndication.IsErrorPresent(Level2Error.CardDataError))
            return ProcessC(currentStateIdRetriever, session);

        if (errorIndication.IsErrorPresent(Level2Error.CardDataMissing))
            return ProcessC(currentStateIdRetriever, session);

        if (_Database.IsPresentAndNotEmpty(SignedDynamicApplicationData.Tag))
            return ProcessA(currentStateIdRetriever, session);

        return ProcessB(currentStateIdRetriever, session);  
    }


    private StateId ProcessC(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
    {
        throw new NotImplementedException();
    }

    private StateId ProcessA(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
    {
        throw new NotImplementedException();
    }

    private StateId ProcessB(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
    {
        throw new NotImplementedException();
    }
}


 