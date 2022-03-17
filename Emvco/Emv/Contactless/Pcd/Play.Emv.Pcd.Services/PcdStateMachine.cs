using System;
using System.Threading.Tasks;

using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Exceptions;
using Play.Emv.Sessions;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Pcd.Services;

internal class PcdStateMachine
{
    #region Instance Values

    private readonly PcdSessionLock _PcdSessionLock = new();
    private readonly CardClient _CardClient;
    private readonly ISendPcdResponses _PcdEndpoint;

    // HACK: Figure out what you need to do with the PCD Protocol Configuration
    private readonly PcdProtocolConfiguration _Configuration;

    #endregion

    #region Constructor

    public PcdStateMachine(CardClient cardClient, PcdProtocolConfiguration configuration, ISendPcdResponses pcdEndpoint)
    {
        _CardClient = cardClient;
        _Configuration = configuration;
        _PcdEndpoint = pcdEndpoint;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    internal void Handle(ActivatePcdRequest request)
    {
        lock (_PcdSessionLock)
        {
            if (_PcdSessionLock.Session != null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(ActivatePcdRequest)} can't be processed because a {nameof(ChannelType.ProximityCouplingDevice)} already exists for {nameof(TransactionSessionId)}: [{_PcdSessionLock.Session!.TransactionSessionId}]");
            }

            _PcdSessionLock.Session = new PcdSession(request.GetTransactionSessionId());
            _CardClient.Activate();
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <param name="abortHandler"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TransmissionError"></exception>
    /// <exception cref="InvalidSignalRequest"></exception>
    public void Handle(StopPcdRequest request, Action abortHandler)
    {
        lock (_PcdSessionLock)
        {
            if (_PcdSessionLock.Session == null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(StopPcdRequest)} {nameof(Abort)} operation can't be processed because a session doesn't currently exist.");
            }

            if (request.GetStopType() == StopPcdRequest.StopType.Abort)
                Abort(request.GetCorrelationId(), request.GetTransactionSessionId(), abortHandler);

            else if (request.GetStopType() == StopPcdRequest.StopType.CloseSession)
                CloseSession(request.GetCorrelationId(), request.GetTransactionSessionId());

            else if (request.GetStopType() == StopPcdRequest.StopType.CloseSessionCardCheck)
                CloseSessionCardCheck(request.GetCorrelationId(), request.GetTransactionSessionId());
            else
                throw new InvalidSignalRequest("The Stop type could not be determined");
        }
    }

    /// <summary>
    ///     CloseSessionCardCheck
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="transactionSessionId"></param>
    /// <exception cref="TransmissionError"></exception>
    private void CloseSessionCardCheck(CorrelationId correlationId, TransactionSessionId transactionSessionId)
    {
        _CardClient.CloseSessionCardCheck();
        _PcdEndpoint.Send(new StopPcdAcknowledgedResponse(correlationId, transactionSessionId, Level1Error.Ok));
    }

    /// <summary>
    ///     CloseSession
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="transactionSessionId"></param>
    /// <exception cref="TransmissionError"></exception>
    private void CloseSession(CorrelationId correlationId, TransactionSessionId transactionSessionId)
    {
        _CardClient.CloseSession();
        _PcdEndpoint.Send(new StopPcdAcknowledgedResponse(correlationId, transactionSessionId, Level1Error.Ok));
    }

    /// <summary>
    ///     Abort
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="transactionSessionId"></param>
    /// <param name="abortHandler"></param>
    /// <exception cref="TransmissionError"></exception>
    private void Abort(CorrelationId correlationId, TransactionSessionId transactionSessionId, Action abortHandler)
    {
        abortHandler.Invoke();
        _CardClient.Abort();

        // BUG: C-2 specification says to return 'Card Removed' for L1RSP. I can't find a 'Card Removed' anywhere in the specs
        _PcdEndpoint.Send(new StopPcdAcknowledgedResponse(correlationId, transactionSessionId, Level1Error.ProtocolError));

        throw new NotImplementedException();
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="cardClient"></param>
    /// <param name="request"></param>
    /// <exception cref="TransmissionError"></exception>
    public void Handle(CardClient cardClient, dynamic request)
    {
        Task<dynamic> response = cardClient.Transceive(request);
        Task.WaitAny(response);
        _PcdEndpoint.Send(response.Result);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidMessageRoutingException"></exception>
    /// <exception cref="TransmissionError"></exception>
    public void Handle(QueryPcdRequest request)
    {
        lock (_PcdSessionLock)
        {
            if (_PcdSessionLock.Session == null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(QueryPcdRequest)} can't be processed because a {nameof(ChannelType.ProximityCouplingDevice)} session does not exist");
            }

            if (request.GetTransactionSessionId() != _PcdSessionLock.Session.TransactionSessionId)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(QueryPcdRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{request.GetTransactionSessionId()}] but the current {nameof(ChannelType.ProximityCouplingDevice)} session has a {nameof(TransactionSessionId)} of: [{_PcdSessionLock.Session.TransactionSessionId}]");
            }

            switch (request)
            {
                case SelectApplicationDefinitionFileInfoRequest selectApplicationDefinitionFileInfoCommand:
                    Handle(_CardClient, selectApplicationDefinitionFileInfoCommand);

                    return;
                case SelectDirectoryDefinitionFileRequest selectDirectoryDefinitionFileCommand:
                    Handle(_CardClient, selectDirectoryDefinitionFileCommand);

                    return;
                case SelectProximityPaymentSystemEnvironmentRequest ppseRequest:
                    Handle(_CardClient, ppseRequest);

                    return;
                case GetProcessingOptionsRequest getProcessingOptionsCommand:
                    Handle(_CardClient, getProcessingOptionsCommand);

                    return;
                case GetDataRequest getDataRequest:
                    Handle(_CardClient, getDataRequest);

                    return;
                case ReadRecordRequest readRecordRequest:
                    Handle(_CardClient, readRecordRequest);

                    return;
                case ExchangeRelayResistanceDataRequest exchangeRelayResistanceDataRequest:
                    Handle(_CardClient, exchangeRelayResistanceDataRequest);

                    return;
                case GenerateApplicationCryptogramRequest generateApplicationCryptogramRequest:
                    Handle(_CardClient, generateApplicationCryptogramRequest);

                    return;

                case SendPoiInformationRequest sendPoiInformationCommand:
                    Handle(_CardClient, sendPoiInformationCommand);

                    return;
                default:
                    throw new
                        InvalidMessageRoutingException($"The {nameof(QueryPcdRequest)} couldn't be be processed because there isn't a handler for the request type {request.GetType().FullName}");
            }
        }
    }

    #endregion

    public class PcdSessionLock
    {
        #region Instance Values

        public PcdSession? Session;

        #endregion
    }
}