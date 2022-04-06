using System;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class S910 : CommonProcessing
{
    #region Instance Values

    // HACK: Maybe convert these response handlers into one
    private readonly InvalidResponseHandler _InvalidResponseHandler;
    private readonly ValidResponseHandler _ValidResponseHandler;
    private readonly IAuthenticateTransactionSession _AuthenticationService;

    protected override StateId[] _ValidStateIds { get; } =
    {
        WaitingForMagStripeReadRecordResponse.StateId, WaitingForMagstripeFirstWriteFlag.StateId
    };

    #endregion

    #region Constructor

    public S910(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
        IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint, IAuthenticateTransactionSession authenticationService) :
        base(database, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint)
    {
        _InvalidResponseHandler = new InvalidResponseHandler(database, dataExchangeKernelService, kernelEndpoint);
        _ValidResponseHandler = new ValidResponseHandler(database, dataExchangeKernelService, kernelEndpoint);
        _AuthenticationService = authenticationService;
    }

    #endregion

    #region Instance Members

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
    {
        HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

        if (IsInvalidResponse(currentStateIdRetriever, session))
            return _InvalidResponseHandler.ProcessInvalidResponse1(currentStateIdRetriever, session.GetKernelSessionId());

        if (_Database.IsPresentAndNotEmpty(SignedDynamicApplicationData.Tag))
            return ProcessWithCda();

        return ProcessWithoutCda();
    }

    /// <exception cref="TerminalDataException"></exception>
    private bool IsInvalidResponse(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
    {
        ErrorIndication errorIndication = _Database.GetErrorIndication();

        if (errorIndication.IsErrorPresent(Level2Error.StatusBytes))
            return true;

        if (errorIndication.IsErrorPresent(Level2Error.ParsingError))
            return true;

        if (errorIndication.IsErrorPresent(Level2Error.CardDataError))
            return true;

        if (errorIndication.IsErrorPresent(Level2Error.CardDataMissing))
            return true;

        return false;
    }

    #endregion
}