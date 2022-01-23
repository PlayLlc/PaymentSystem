using System;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Contracts.SignalOut;
using Play.Emv.Messaging;
using Play.Emv.Reader.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Configuration;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Emv.Transactions;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.Services;

internal class TerminalStateMachine
{
    #region Instance Values

    private readonly SelectionSessionLock _TerminalSessionLock = new();
    private readonly IHandleDisplayRequests _DisplayEndpoint;
    private readonly IHandleKernelRequests _KernelEndpoint;
    private readonly IHandleReaderRequests _ReaderEndpoint;
    private readonly IPerformTerminalActionAnalysis _TerminalActionAnalysisService;
    private readonly IManageTerminalRisk _TerminalRiskManager;
    private readonly ITerminalConfigurationRepository _TerminalConfigurationRepository;
    private readonly ISendTerminalResponses _TerminalEndpoint;

    #endregion

    #region Constructor

    public TerminalStateMachine(
        IHandleDisplayRequests displayEndpoint,
        IHandleKernelRequests kernelEndpoint,
        IHandleReaderRequests readerEndpoint,
        IPerformTerminalActionAnalysis terminalActionAnalysisService,
        IManageTerminalRisk terminalRiskManager,
        ITerminalConfigurationRepository terminalConfigurationRepository,
        ISendTerminalResponses terminalEndpoint)
    {
        _DisplayEndpoint = displayEndpoint;
        _KernelEndpoint = kernelEndpoint;
        _ReaderEndpoint = readerEndpoint;
        _TerminalActionAnalysisService = terminalActionAnalysisService;
        _TerminalRiskManager = terminalRiskManager;
        _TerminalConfigurationRepository = terminalConfigurationRepository;
        _TerminalEndpoint = terminalEndpoint;
    }

    #endregion

    #region Instance Members

    // TODO: I'm not sure if this is the correct way to implement this. I'm not sure what form of communication the point of sale will have with the Terminal. Keeping this here for now
    public void Handle(ActivateTerminalRequest request)
    {
        lock (_TerminalSessionLock)
        {
            if (_TerminalSessionLock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivateTerminalRequest)} can't be processed because the {nameof(ChannelType.Terminal)} already has an active session in progress");
            }

            // HACK: This won't change unless it's a cloud kernel implementation. Find a better strategy
            TerminalConfiguration systemConfiguration =
                _TerminalConfigurationRepository.GeTerminalConfiguration(request.GetTerminalIdentification(),
                    request.GetAcquirerIdentifier(), request.GetMerchantIdentifier());

            Transaction transaction = new(new TransactionSessionId(request.GetTransactionType()), request.GetAmountAuthorizedNumeric(),
                request.GetAmountOtherNumeric(), request.GetTransactionType(), systemConfiguration.GetLanguagePreference(),
                systemConfiguration.GetTerminalCountryCode(), new TransactionDate(DateTimeUtc.Now()));

            _TerminalSessionLock.Session = new TerminalSession(new TerminalSessionId(), transaction, new TerminalVerificationResults(0));

            // HACK: Develop logic for passing TagsToRead and DataToSend along with the ACT signal below

            _ReaderEndpoint.Request(new ActivateReaderRequest(transaction));
        }
    }

    public void Handle(QueryTerminalRequest request)
    {
        lock (_TerminalSessionLock)
        {
            if (_TerminalSessionLock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryTerminalRequest)} can't be processed because the {nameof(ChannelType.Terminal)} doesn't currently have an active session");
            }

            if (_TerminalSessionLock.Session.GetTransactionSessionId() != request.GetTransactionSessionId())
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryTerminalRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{request.GetTransactionSessionId()}] but the current {nameof(ChannelType.Terminal)} session has a {nameof(TransactionSessionId)} of: [{_TerminalSessionLock.Session.GetTransactionSessionId()}]");
            }

            // TODO: I'm not sure if this is correct. We might need to request data from the reader database first. That would mean we would have to send a QueryReaderRequest and wait for a response callback before sending back the data the kernel needs. If that's the case we would need to store the correlation information in the session. Hammer out the finer details here 

            TagLengthValue[] requestedData = RetrieveNeededData(request.GetDataNeeded().AsTagArray(), _TerminalSessionLock);

            _TerminalEndpoint.Send(new QueryTerminalResponse(default, new DataToSend(requestedData), request.GetDataExchangeKernelId()));
        }
    }

    public void Handle(OutReaderResponse hello)
    {
        throw new NotImplementedException();
    }

    public void Handle(QueryKernelResponse hello)
    {
        throw new NotImplementedException();
    }

    public void Handle(StopReaderAcknowledgedResponse hello)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Private Helper Methods

    private TagLengthValue[] RetrieveNeededData(Tag[] dataNeeded, SelectionSessionLock sessionLock)
    {
        TagLengthValue[] buffer = new TagLengthValue[dataNeeded.Length];

        for (int i = 0; i < dataNeeded.Length; i++)
        {
            if (TryProcessCommonTerminalService(dataNeeded[i], sessionLock, out TagLengthValue? result))
            {
                buffer[i] = result!;

                continue;
            }

            // HACK: Continue looking for the requested data. Do we retrieve the data from the Reader Database's Persistent values? Probably so. How do we do that concurrently when the signals are enqueue and the data is sent with a callback? Do we query the reader with a correlation value, the KernelSessionId would probably be good enough for this. Confirm this is correct

            buffer[i] = new TagLengthValue(dataNeeded[i], Array.Empty<byte>());
        }

        return buffer;
    }

    private bool TryProcessCommonTerminalService(Tag tag, SelectionSessionLock sessionLock, out TagLengthValue? result)
    {
        if (tag == TerminalVerificationResults.Tag)
        {
            result = PerformTerminalRiskManagement(sessionLock);

            return true;
        }

        // TODO: Find out where Terminal Action Analysis needs to take place. Is it requested by a tag in DEK with DataNeeded?
        // ...etc

        result = null;

        return true;
    }

    private TagLengthValue PerformTerminalRiskManagement(SelectionSessionLock sessionLock) =>
        throw

            // BUG: Validate and fix the code commented out below
            //TerminalVerificationResult terminalVerificationResult = _TerminalRiskManager.Process(new TerminalRiskManagementCommand());
            //TerminalVerificationResults update = new TerminalVerificationResults(terminalVerificationResult);
            //sessionLock.Session = sessionLock!.Session with {TerminalVerificationResults = update};
            //return update.AsTagLengthValue();
            new NotImplementedException();

    private void PerformTerminalActionAnalysis(TerminalSession session)
    {
        // BUG: Flesh out the entire flow of Authentication. Use Play.Emv.Security to inject the needed services

        TerminalActionAnalysisResponse terminalActionAnalysisResponse =
            _TerminalActionAnalysisService.Process(new TerminalActionAnalysisCommand(session.TerminalVerificationResults, default, default,
                default));

        // IGenerateApplicationCryptogramResponse.Generate(
        //          CryptogramType cryptogramType,
        //          bool isCdaRequested,
        //          DataObjectListResult cardRiskManagementDataObjectListResult,
        //          DataObjectListResult dataStorageDataObjectListResult);

        // etc..

        throw new NotImplementedException();
    }

    #endregion

    public class SelectionSessionLock
    {
        #region Instance Values

        public TerminalSession? Session;

        #endregion
    }
}