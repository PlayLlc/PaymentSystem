using System;

using Play.Ber.Exceptions;
using Play.Core.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Icc.Exceptions;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class S910 : CommonProcessing
{
    #region Instance Values

    private readonly ResponseHandler _ResponseHandler;
    private readonly AuthHandler _AuthHandler;
    protected override StateId[] _ValidStateIds { get; } = {WaitingForGenerateAcResponse1.StateId, WaitingForRecoverAcResponse.StateId};

    #endregion

    #region Constructor

    public S910(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver, IEndpointClient endpointClient,
        IAuthenticateTransactionSession authenticationService) : base(database, dataExchangeKernelService, kernelStateResolver, endpointClient)
    {
        _ResponseHandler = new ResponseHandler(database, dataExchangeKernelService, endpointClient);
        _AuthHandler = new AuthHandler(database, _ResponseHandler, authenticationService);
    }

    #endregion

    #region Instance Members

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
    {
        HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

        if (TryProcessingInvalidDataResponse(session.GetKernelSessionId()))
            return currentStateIdRetriever.GetStateId();

        if (IsWithCda())
            return _AuthHandler.ProcessWithCda(currentStateIdRetriever, session, (GenerateApplicationCryptogramResponse) message);

        return _AuthHandler.ProcessWithoutCda(currentStateIdRetriever, session, (GenerateApplicationCryptogramResponse) message);
    }

    /// <exception cref="TerminalDataException"></exception>
    private bool TryProcessingInvalidDataResponse(KernelSessionId sessionId)
    {
        ErrorIndication errorIndication = _Database.GetErrorIndication();

        if (!IsInvalidDataResponsePresent(errorIndication))
            return false;

        _ResponseHandler.ProcessInvalidDataResponse(sessionId);

        return false;
    }

    private bool IsInvalidDataResponsePresent(ErrorIndication errorIndication)
    {
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

    /// <exception cref="TerminalDataException"></exception>
    private bool IsWithCda() => _Database.IsPresentAndNotEmpty(SignedDynamicApplicationData.Tag);

    #endregion
}