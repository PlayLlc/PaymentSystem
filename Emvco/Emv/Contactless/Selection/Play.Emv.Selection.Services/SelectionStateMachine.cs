using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts.SignalIn;
using Play.Emv.Selection.Contracts.SignalOut;
using Play.Emv.Selection.Start;
using Play.Emv.Sessions;
using Play.Emv.Transactions;

namespace Play.Emv.Selection.Services;

internal class SelectionStateMachine
{
    #region Instance Values

    private readonly IHandlePcdRequests _PcdClient;
    private readonly ISendSelectionResponses _EndpointClient;
    private readonly SelectionProcess _SelectionProcess;
    private readonly CandidateList _CandidateList;
    private readonly CombinationSelector _CombinationSelector;
    private readonly PreProcessingIndicators _PreProcessingIndicators;
    private readonly Preprocessor _Preprocessor;
    private readonly ProtocolActivator _ProtocolActivator;
    private readonly SelectionSessionLock _SelectionSessionLock = new();

    #endregion

    #region Constructor

    public SelectionStateMachine(
        IHandlePcdRequests pcdClient,
        IHandleDisplayRequests displayClient,
        TransactionProfile[] transactionProfiles,
        PoiInformation poiInformation,
        SelectionProcess selectionProcess,
        ISendSelectionResponses endpointClient)
    {
        _PcdClient = pcdClient;
        _EndpointClient = endpointClient;

        _PreProcessingIndicators = new PreProcessingIndicators(transactionProfiles);
        _CandidateList = new CandidateList();
        _Preprocessor = new Preprocessor();
        _ProtocolActivator = new ProtocolActivator(pcdClient, displayClient);
        _SelectionProcess = selectionProcess;

        _CombinationSelector = new CombinationSelector(poiInformation, pcdClient);
    }

    #endregion

    #region Instance Members

    public void Handle(StopSelectionRequest request)
    {
        lock (_SelectionSessionLock)
        {
            if (_SelectionSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(StopSelectionRequest)} can't be processed because the {nameof(TransactionSessionId)}: [{_SelectionSessionLock.Session!.GetTransactionSessionId()}] is currently processing");
            }

            if (_SelectionSessionLock.Session.GetTransactionSessionId() != request.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(StopSelectionRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{request.GetTransactionSessionId()}] but the current {nameof(ChannelType.Selection)} session has a {nameof(TransactionSessionId)} of: [{_SelectionSessionLock.Session.GetTransactionSessionId()}]");
            }

            // HACK: Book C-2 Section 2.3.3 doesn't say what to include in the OUT signal that results from a STOP signal. Need to find out
            _EndpointClient.Send(new OutSelectionResponse(default, default, default, default, default));
        }
    }

    public void Handle(ActivatePcdResponse request)
    {
        lock (_SelectionSessionLock)
        {
            if (_SelectionSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivatePcdResponse)} can't be processed because the {nameof(SelectionSession)} no longer exists");
            }

            // HACK: We need to look at the list of appropriate Status Words from the RAPDU response, not just if the Level1Error is set to OK. Check the specs on appropriate status word results
            if (!request.Successful())
                HandleCommunicationsError(_SelectionSessionLock.Session, _SelectionSessionLock.Session.GetTransactionSessionId());

            ProcessAtC(_SelectionSessionLock.Session!.GetTransaction());
            _SelectionSessionLock.Session = null;
        }
    }

    public void Handle(SelectProximityPaymentSystemEnvironmentResponse response)
    {
        lock (_SelectionSessionLock)
        {
            if (_SelectionSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(SelectionSession)} no longer exists");
            }

            if (_SelectionSessionLock.Session.GetTransactionSessionId() != response.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(ChannelType.Selection)} session has a {nameof(TransactionSessionId)} of: [{_SelectionSessionLock.Session.GetTransactionSessionId()}]");
            }

            _CombinationSelector.ProcessPpseResponse(response.GetTransactionSessionId(), _CandidateList, _PreProcessingIndicators,
                _SelectionSessionLock.Session.GetOutcome(), _SelectionSessionLock.Session.GetTransactionType(), response);
        }
    }

    public void Handle(SendPoiInformationResponse response)
    {
        lock (_SelectionSessionLock)
        {
            if (_SelectionSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(SelectionSession)} no longer exists");
            }

            if (_SelectionSessionLock.Session.GetTransactionSessionId() != response.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(ChannelType.Selection)} session has a {nameof(TransactionSessionId)} of: [{_SelectionSessionLock.Session.GetTransactionSessionId()}]");
            }

            _CombinationSelector.ProcessPointOfInteractionResponse(response.GetTransactionSessionId(), _CandidateList,
                _PreProcessingIndicators, _SelectionSessionLock.Session.GetOutcome(), _SelectionSessionLock.Session.GetTransactionType(),
                response);
        }
    }

    public void Handle(SelectApplicationDefinitionFileInfoResponse response)
    {
        lock (_SelectionSessionLock)
        {
            if (_SelectionSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(SelectionSession)} no longer exists");
            }

            if (_SelectionSessionLock.Session.GetTransactionSessionId() != response.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(ChannelType.Selection)} session has a {nameof(TransactionSessionId)} of: [{_SelectionSessionLock.Session.GetTransactionSessionId()}]");
            }

            if (!_CombinationSelector.TrySelectApplet(response.GetTransactionSessionId(), _CandidateList,
                _SelectionSessionLock.Session.GetOutcome(), _CandidateList.ElementAt(0), response,
                out CombinationOutcome? combinationOutcome))
            {
                _CombinationSelector.ProcessInvalidAppletResponse(response.GetTransactionSessionId(), _CandidateList,
                    _SelectionSessionLock.Session.GetOutcome());

                return;
            }

            _CombinationSelector.ProcessValidApplet(response.GetTransactionSessionId(), response.GetCorrelationId(),
                _SelectionSessionLock.Session.GetTransaction(), combinationOutcome!,
                _PreProcessingIndicators[combinationOutcome!.Combination.GetCombinationCompositeKey()].AsPreProcessingIndicatorResult(),
                response, _EndpointClient.Send);
        }
    }

    #endregion

    #region Activate

    public void Handle(ActivateSelectionRequest request)
    {
        lock (_SelectionSessionLock)
        {
            if (_SelectionSessionLock.Session != null)
                ReprocessActivationSelectionRequest(_SelectionSessionLock, request);
            else
                ProcessActivationSelectionRequest(_SelectionSessionLock, request);
        }
    }

    public void ProcessActivationSelectionRequest(SelectionSessionLock sessionLock, ActivateSelectionRequest request)
    {
        sessionLock.Session = new SelectionSession(request.GetTransaction());
        ProcessEntryPoint(sessionLock, request);
    }

    public void ReprocessActivationSelectionRequest(SelectionSessionLock sessionLock, ActivateSelectionRequest request)
    {
        if (sessionLock.Session!.GetTransactionSessionId() != request.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(ActivateSelectionRequest)} can't be processed because the {nameof(TransactionSessionId)}: [{sessionLock.Session!.GetTransactionSessionId()}] is currently processing");
        }

        ProcessEntryPoint(sessionLock, request);
    }

    private void ProcessEntryPoint(SelectionSessionLock sessionLock, ActivateSelectionRequest request)
    {
        if (request.GetStartOutcome() == StartOutcome.A)
            ProcessAtA(request.GetTransaction());
        else if (request.GetStartOutcome() == StartOutcome.B)
            ProcessAtB(request.GetTransaction());
        else if (request.GetStartOutcome() == StartOutcome.C)
            ProcessAtC(request.GetTransaction());
        else
        {
            throw new InvalidSignalRequest(
                $"A {nameof(StartOutcome)} of type {nameof(StartOutcome.A)}, {nameof(StartOutcome.A)}, or {nameof(StartOutcome.A)} was expected but the value {request.GetStartOutcome()} was found");
        }
    }

    #endregion

    public class SelectionSessionLock
    {
        #region Instance Values

        public SelectionSession? Session;

        #endregion
    }

    #region Helper Logic

    private void ProcessAtA(Transaction transaction)
    {
        _Preprocessor.SetPreprocessingIndicators(transaction.GetOutcome(), _PreProcessingIndicators,
            transaction.GetAmountAuthorizedNumeric(), transaction.GetCultureProfile());

        ProcessAtB(transaction);
    }

    private void ProcessAtB(Transaction transaction)
    {
        _ProtocolActivator.ActivateProtocol(transaction.GetTransactionSessionId(), transaction.GetOutcome(), _PreProcessingIndicators,
            _CandidateList);
        _PcdClient.Request(new ActivatePcdRequest(transaction.GetTransactionSessionId()));
    }

    private void ProcessAtC(Transaction transaction)
    {
        _CombinationSelector.Start(transaction.GetTransactionSessionId(), _CandidateList, _PreProcessingIndicators,
            transaction.GetOutcome(), transaction.GetTransactionType());
    }

    private void HandleCommunicationsError(SelectionSession session, TransactionSessionId transactionSessionId)
    {
        OutcomeParameterSet.Builder? outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
        outcomeParameterSetBuilder.Set(StartOutcome.B);
        session.GetOutcome().Update(outcomeParameterSetBuilder);

        _SelectionProcess.Enqueue(new ActivateSelectionRequest(session.GetTransaction()));
    }

    #endregion
}