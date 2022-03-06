using System;
using System.Threading.Tasks;

using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
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

    public void Handle(ActivatePcdRequest request)
    {
        lock (_PcdSessionLock)
        {
            if (_PcdSessionLock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivatePcdRequest)} can't be processed because a {nameof(ChannelType.ProximityCouplingDevice)} already exists for {nameof(TransactionSessionId)}: [{_PcdSessionLock.Session!.TransactionSessionId}]");
            }

            _PcdSessionLock.Session = new PcdSession(request.GetTransactionSessionId());
            _CardClient.Activate();
        }
    }

    public void Handle(StopPcdRequest request, Action abortHandler)
    {
        lock (_PcdSessionLock)
        {
            if (_PcdSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(StopPcdRequest)} {nameof(Abort)} operation can't be processed because a session doesn't currently exist.");
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

    private void CloseSessionCardCheck(CorrelationId correlationId, TransactionSessionId transactionSessionId)
    {
        _CardClient.CloseSessionCardCheck();
        _PcdEndpoint.Send(new StopPcdAcknowledgedResponse(correlationId, transactionSessionId, Level1Error.Ok));
    }

    private void CloseSession(CorrelationId correlationId, TransactionSessionId transactionSessionId)
    {
        _CardClient.CloseSession();
        _PcdEndpoint.Send(new StopPcdAcknowledgedResponse(correlationId, transactionSessionId, Level1Error.Ok));
    }

    private void Abort(CorrelationId correlationId, TransactionSessionId transactionSessionId, Action abortHandler)
    {
        abortHandler.Invoke();
        _CardClient.Abort();

        // BUG: C-2 specification says to return 'Card Removed' for L1RSP. I can't find a 'Card Removed' anywhere in the specs
        _PcdEndpoint.Send(new StopPcdAcknowledgedResponse(correlationId, transactionSessionId, Level1Error.ProtocolError));

        throw new NotImplementedException();
    }

    #endregion

    #region QUERY

    public void Handle(CardClient cardClient, dynamic request)
    {
        Task<dynamic> response = cardClient.Transceive(request);
        Task.WaitAny(response);
        _PcdEndpoint.Send(response.Result);
    }

    public void Handle(QueryPcdRequest request)
    {
        lock (_PcdSessionLock)
        {
            if (_PcdSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryPcdRequest)} can't be processed because a {nameof(ChannelType.ProximityCouplingDevice)} session does not exist");
            }

            if (request.GetTransactionSessionId() != _PcdSessionLock.Session.TransactionSessionId)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryPcdRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{request.GetTransactionSessionId()}] but the current {nameof(ChannelType.ProximityCouplingDevice)} session has a {nameof(TransactionSessionId)} of: [{_PcdSessionLock.Session.TransactionSessionId}]");
            }

            switch (request)
            {
                case GenerateApplicationCryptogramRequest generateApplicationCryptogramRequest:
                    Handle(_CardClient, generateApplicationCryptogramRequest);

                    return;
                case GetProcessingOptionsRequest getProcessingOptionsCommand:
                    Handle(_CardClient, getProcessingOptionsCommand);

                    return;
                case ReadApplicationDataRequest readApplicationDataCommand:
                    Handle(_CardClient, readApplicationDataCommand);

                    return;
                case ReadElementaryFileRecordRequest readElementaryFileRecordCommand:
                    Handle(_CardClient, readElementaryFileRecordCommand);

                    return;
                case SelectApplicationDefinitionFileInfoRequest selectApplicationDefinitionFileInfoCommand:
                    Handle(_CardClient, selectApplicationDefinitionFileInfoCommand);

                    return;
                case SelectDirectoryDefinitionFileRequest selectDirectoryDefinitionFileCommand:
                    Handle(_CardClient, selectDirectoryDefinitionFileCommand);

                    return;
                case SelectProximityPaymentSystemEnvironmentRequest ppseRequest:
                    Handle(_CardClient, ppseRequest);

                    return;
                case SendPoiInformationRequest sendPoiInformationCommand:
                    Handle(_CardClient, sendPoiInformationCommand);

                    return;
                default:
                    throw new InvalidMessageRoutingException(
                        $"The {nameof(QueryPcdRequest)} couldn't be be processed because there isn't a handler for the request type {request.GetType().FullName}");
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