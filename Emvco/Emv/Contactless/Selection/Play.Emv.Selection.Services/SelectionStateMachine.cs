using Play.Emv.DataElements.Emv;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;
using Play.Emv.Selection.Start;
using Play.Emv.Sessions;

namespace Play.Emv.Selection.Services;

internal class SelectionStateMachine
{
    #region Instance Values

    private readonly IHandlePcdRequests _PcdClient;
    private readonly ISendSelectionResponses _EndpointClient;
    private readonly CandidateList _CandidateList;
    private readonly CombinationSelector _CombinationSelector;
    private readonly PreProcessingIndicators _PreProcessingIndicators;
    private readonly Preprocessor _Preprocessor;
    private readonly ProtocolActivator _ProtocolActivator;
    private readonly CardCollisionHandler _CardCollisionHandler;
    private readonly SelectionSessionLock _Lock = new();

    #endregion

    #region Constructor

    public SelectionStateMachine(
        IHandlePcdRequests pcdClient,
        IHandleDisplayRequests displayEndpoint,
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
        _ProtocolActivator = new ProtocolActivator(pcdClient, displayEndpoint);

        _CombinationSelector = new CombinationSelector(poiInformation, pcdClient);
        _CardCollisionHandler = new CardCollisionHandler(displayEndpoint);
    }

    #endregion

    #region Instance Members

    public void Handle(StopSelectionRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(StopSelectionRequest)} can't be processed because the {nameof(TransactionSessionId)}: [{_Lock.Session!.GetTransactionSessionId()}] is currently processing");
            }

            if (_Lock.Session.GetTransactionSessionId() != request.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(StopSelectionRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{request.GetTransactionSessionId()}] but the current {nameof(ChannelType.Selection)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _Lock.Session = null;
        }
    }

    public void Handle(ActivatePcdResponse request)
    {
        lock (_Lock)
        {
            if (!_Lock.IsActive())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivatePcdResponse)} can't be processed because the {nameof(SelectionSession)} no longer exists");
            }

            if (request.GetLevel1Error() == Level1Error.Ok)
            {
                ProcessAtC(_Lock.Session!.GetTransaction());

                return;
            }

            _CardCollisionHandler.HandleCardCollisions(request, _Lock.Session.GetOutcome());
            ProcessAtB(_Lock.Session.GetTransaction());
        }
    }

    public void Handle(SelectProximityPaymentSystemEnvironmentResponse response)
    {
        lock (_Lock)
        {
            if (!_Lock.IsActive())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(SelectionSession)} no longer exists");
            }

            if (_Lock.Session.GetTransactionSessionId() != response.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(ChannelType.Selection)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _CombinationSelector.ProcessPpseResponse(response.GetTransactionSessionId(), _CandidateList, _PreProcessingIndicators,
                _Lock.Session.GetOutcome(), _Lock.Session.GetTransactionType(), response);
        }
    }

    public void Handle(SendPoiInformationResponse response)
    {
        lock (_Lock)
        {
            if (!_Lock.IsActive())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(SelectionSession)} no longer exists");
            }

            if (_Lock.Session!.GetTransactionSessionId() != response.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(ChannelType.Selection)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _CombinationSelector.ProcessPointOfInteractionResponse(response.GetTransactionSessionId(), _CandidateList,
                _PreProcessingIndicators, _Lock.Session.GetOutcome(), _Lock.Session.GetTransactionType(), response);
        }
    }

    public void Handle(SelectApplicationDefinitionFileInfoResponse response)
    {
        lock (_Lock)
        {
            if (!_Lock.IsActive())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(SelectionSession)} no longer exists");
            }

            if (_Lock.Session!.GetTransactionSessionId() != response.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(ChannelType.Selection)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            if (!_CombinationSelector.TrySelectApplet(response.GetTransactionSessionId(), _CandidateList, _Lock.Session.GetOutcome(),
                _CandidateList.ElementAt(0), response, out CombinationOutcome? combinationOutcome))
            {
                _CombinationSelector.ProcessInvalidAppletResponse(response.GetTransactionSessionId(), _CandidateList,
                    _Lock.Session.GetOutcome());

                return;
            }

            _CombinationSelector.ProcessValidApplet(response.GetTransactionSessionId(), response.GetCorrelationId(),
                _Lock.Session.GetTransaction(), combinationOutcome!,
                _PreProcessingIndicators[combinationOutcome!.Combination.GetCombinationCompositeKey()].AsPreProcessingIndicatorResult(),
                response, _EndpointClient.Send);
        }
    }

    #endregion

    #region Activate

    public void Handle(ActivateSelectionRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.IsActive())
                ReprocessActivationSelectionRequest(_Lock, request);
            else
                ProcessActivationSelectionRequest(_Lock, request);
        }
    }

    public void ProcessActivationSelectionRequest(SelectionSessionLock sessionLock, ActivateSelectionRequest request)
    {
        sessionLock.Session = new SelectionSession(request.GetTransaction(), request.GetCorrelationId());

        ProcessEntryPoint(request);
    }

    public void ReprocessActivationSelectionRequest(SelectionSessionLock sessionLock, ActivateSelectionRequest request)
    {
        if (sessionLock.Session!.GetTransactionSessionId() != request.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(ActivateSelectionRequest)} can't be processed because the {nameof(TransactionSessionId)}: [{sessionLock.Session!.GetTransactionSessionId()}] is currently processing");
        }

        ProcessEntryPoint(request);
    }

    private void ProcessEntryPoint(ActivateSelectionRequest request)
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

    #endregion

    public class SelectionSessionLock
    {
        #region Instance Values

        public SelectionSession? Session;

        #endregion

        #region Instance Members

        public bool IsActive() => Session != null;

        #endregion
    }
}