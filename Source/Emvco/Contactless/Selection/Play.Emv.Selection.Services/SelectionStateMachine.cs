using System.Runtime.Remoting;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Contracts;
using Play.Emv.Selection.Start;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Selection.Services;

internal class SelectionStateMachine
{
    #region Instance Values

    private readonly IEndpointClient _EndpointClient;
    private readonly CandidateList _CandidateList;
    private readonly CombinationSelector _CombinationSelector;
    private readonly PreProcessingIndicators _PreProcessingIndicators;
    private readonly Preprocessor _Preprocessor;
    private readonly ProtocolActivator _ProtocolActivator;
    private readonly CardCollisionHandler _CardCollisionHandler;
    private readonly SelectionSessionLock _Lock = new();

    #endregion

    #region Constructor

    public SelectionStateMachine(IEndpointClient endpointClient, TransactionProfile[] transactionProfiles, PoiInformation poiInformation)
    {
        _EndpointClient = endpointClient;

        _PreProcessingIndicators = new PreProcessingIndicators(transactionProfiles);
        _CandidateList = new CandidateList();
        _Preprocessor = new Preprocessor();
        _ProtocolActivator = new ProtocolActivator(endpointClient);

        _CombinationSelector = new CombinationSelector(poiInformation, endpointClient);
        _CardCollisionHandler = new CardCollisionHandler(endpointClient);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
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
                    $"The {nameof(StopSelectionRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{request.GetTransactionSessionId()}] but the current {nameof(SelectionChannel.Id)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _Lock.Session = null;
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="response"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
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
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(SelectionChannel.Id)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _CombinationSelector.ProcessPpseResponse(_Lock.Session.GetTransaction(), _CandidateList, _PreProcessingIndicators,
                _Lock.Session.GetTransactionType(), response);
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="response"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
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
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(SelectionChannel.Id)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _CombinationSelector.ProcessPointOfInteractionResponse(_Lock.Session.GetTransaction(), _CandidateList, _PreProcessingIndicators,
                _Lock.Session.GetOutcome(), _Lock.Session.GetTransactionType(), response);
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="response"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
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
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{response.GetTransactionSessionId()}] but the current {nameof(SelectionChannel.Id)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            if (!_CombinationSelector.TrySelectApplet(_CandidateList.ElementAt(0), response, out CombinationOutcome? combinationOutcome))
            {
                ProcessEmptyCandidateList(_Lock.Session.GetCorrelationId(), _Lock.Session.GetTransaction());

                return;
            }

            OutSelectionResponse outSelectionResponse = new(response.GetCorrelationId(), _Lock.Session.GetTransaction(),
                combinationOutcome!.Combination.GetCombinationCompositeKey(), response);

            _EndpointClient.Send(outSelectionResponse);
        }
    }

    public void Handle(EmptyCombinationSelectionRequest request)
    {
        lock (_Lock)
        {
            if (!_Lock.IsActive())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(SelectionSession)} no longer exists");
            }

            if (_Lock.Session!.GetTransactionSessionId() != request.TransactionSessionId)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(SelectApplicationDefinitionFileInfoResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{request.TransactionSessionId}] but the current {nameof(SelectionChannel.Id)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            ProcessEmptyCandidateList(_Lock.Session.GetCorrelationId(), _Lock.Session.GetTransaction());
        }
    }

    #endregion

    #region Activate

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidSignalRequest"></exception>
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

    /// <summary>
    ///     ProcessActivationSelectionRequest
    /// </summary>
    /// <param name="sessionLock"></param>
    /// <param name="request"></param>
    /// <exception cref="InvalidSignalRequest"></exception>
    public void ProcessActivationSelectionRequest(SelectionSessionLock sessionLock, ActivateSelectionRequest request)
    {
        sessionLock.Session = new SelectionSession(request.GetTransaction(), request.GetCorrelationId());

        ProcessEntryPoint(request);
    }

    /// <summary>
    ///     ReprocessActivationSelectionRequest
    /// </summary>
    /// <param name="sessionLock"></param>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidSignalRequest"></exception>
    public void ReprocessActivationSelectionRequest(SelectionSessionLock sessionLock, ActivateSelectionRequest request)
    {
        if (sessionLock.Session!.GetTransactionSessionId() != request.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(ActivateSelectionRequest)} can't be processed because the {nameof(TransactionSessionId)}: [{sessionLock.Session!.GetTransactionSessionId()}] is currently processing");
        }

        ProcessEntryPoint(request);
    }

    /// <summary>
    ///     ProcessEntryPoint
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="InvalidSignalRequest"></exception>
    private void ProcessEntryPoint(ActivateSelectionRequest request)
    {
        if (request.GetStartOutcome() == StartOutcomes.A)
            ProcessAtA(request.GetTransaction());
        else if (request.GetStartOutcome() == StartOutcomes.B)
            ProcessAtB(request.GetTransaction());
        else if (request.GetStartOutcome() == StartOutcomes.C)
            ProcessAtC(request.GetTransaction());
        else
        {
            throw new InvalidSignalRequest(
                $"A {nameof(StartOutcomes)} of type {nameof(StartOutcomes.A)}, {nameof(StartOutcomes.A)}, or {nameof(StartOutcomes.A)} was expected but the value {request.GetStartOutcome()} was found");
        }
    }

    #endregion

    #region Helper Logic

    private void ProcessAtA(Transaction transaction)
    {
        _Preprocessor.SetPreprocessingIndicators(transaction.GetOutcome(), _PreProcessingIndicators, transaction.GetAmountAuthorizedNumeric(),
            transaction.GetCultureProfile());

        ProcessAtB(transaction);
    }

    private void ProcessAtB(Transaction transaction)
    {
        _ProtocolActivator.ActivateProtocol(transaction.GetTransactionSessionId(), transaction.GetOutcome(), _PreProcessingIndicators, _CandidateList);
        _EndpointClient.Send(new ActivatePcdRequest(transaction.GetTransactionSessionId()));
    }

    private void ProcessAtC(Transaction transaction)
    {
        _CombinationSelector.Start(transaction, _CandidateList);
    }

    private void ProcessEmptyCandidateList(CorrelationId correlationId, Transaction transaction)
    {
        UserInterfaceRequestData.Builder? userInterfaceRequestDataBuilder = UserInterfaceRequestData.GetBuilder();
        userInterfaceRequestDataBuilder.Set(DisplayMessageIdentifiers.ErrorUseAnotherCard);
        userInterfaceRequestDataBuilder.Set(DisplayStatuses.ReadyToRead);

        OutcomeParameterSet.Builder? outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
        outcomeParameterSetBuilder.SetIsUiRequestOnOutcomePresent(true);
        outcomeParameterSetBuilder.Set(new Milliseconds(0));

        ErrorIndication.Builder errorIndicationBuilder = ErrorIndication.GetBuilder();
        errorIndicationBuilder.Set(Level2Error.EmptyCandidateList);

        transaction.Update(new Outcome(errorIndicationBuilder.Complete(), outcomeParameterSetBuilder.Complete()));

        _EndpointClient.Send(new OutSelectionResponse(correlationId, transaction));
    }

    #endregion

    public class SelectionSessionLock
    {
        #region Instance Values

        public SelectionSession? Session;

        #endregion

        #region Instance Members

        public bool IsActive()
        {
            Type d1 = typeof(DataElement<>);

            ObjectHandle? a = Activator.CreateInstance(typeof(AmountAuthorizedNumeric).AssemblyQualifiedName, nameof(AmountAuthorizedNumeric));

            return Session != null;
        }

        #endregion
    }
}