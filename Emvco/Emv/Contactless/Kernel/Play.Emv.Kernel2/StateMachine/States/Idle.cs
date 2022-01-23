using System;

using Play.Ber.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts.SignalIn;
using Play.Emv.Kernel.Contracts.SignalOut;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Templates.Exceptions;
using Play.Emv.Templates.FileControlInformation;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Emv.Transactions;
using Play.Icc.Emv;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

/// <remarks>
///     The first state entered after wakeup
/// </remarks>
public class Idle : KernelState
{
    #region Static Metadata

    public static readonly KernelStateId KernelStateId = new(1);

    #endregion

    #region Instance Values

    private readonly IKernelEndpoint _KernelEndpoint;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly ICleanTornTransactions _KernelCleaner;
    private readonly KernelDatabase _KernelDatabase;

    #endregion

    #region Constructor

    public Idle(
        ICleanTornTransactions kernelCleaner,
        KernelDatabase kernelDatabase,
        IGetKernelState kernelStateResolver,
        IKernelEndpoint kernelEndpoint,
        IHandleTerminalRequests terminalEndpoint,
        IHandlePcdRequests pcdEndpoint)
    {
        _KernelCleaner = kernelCleaner;
        _KernelDatabase = kernelDatabase;
        _KernelStateResolver = kernelStateResolver;
        _KernelEndpoint = kernelEndpoint;
        _TerminalEndpoint = terminalEndpoint;
        _PcdEndpoint = pcdEndpoint;
    }

    #endregion

    public override KernelStateId GetKernelStateId() => KernelStateId;

    public override KernelState Handle(StopKernelRequest signal)
    {
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        builder.Set(StatusOutcome.EndApplication);

        _KernelDatabase.GetKernelSession().Update(builder);
        _KernelDatabase.GetKernelSession().Update(Level3Error.Stop);

        _KernelEndpoint.Send(new OutKernelResponse(signal.GetCorrelationId(), signal.GetKernelSessionId(),
                                                   _KernelDatabase.GetKernelSession().GetOutcome()));

        return _KernelStateResolver.GetKernelState(KernelStateId);
    }

    public override KernelState Handle(CleanKernelRequest signal)
    {
        _KernelCleaner.Clean();

        return _KernelStateResolver.GetKernelState(KernelStateId);
    }

    /// <remarks>Book C-2 Section 6.3.3</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public override KernelState Handle(ActivateKernelRequest signal)
    {
        if (!TryInitialize(signal.GetCorrelationId(), signal.GetKernelSessionId(), signal.GetTransaction()))
            return _KernelStateResolver.GetKernelState(KernelStateId);

        if (!TryParseTemplateAndAddTransactionDataToDatabase(signal, out FileControlInformationAdf? fci))
            return _KernelStateResolver.GetKernelState(KernelStateId);

        UpdateLanguagePreferences(fci!);
        HandleSupportForFieldOffDetection(fci!);
        InitializeEmvDataObjects();

        return _KernelStateResolver.GetKernelState(KernelStateId);
    }

    /// <remarks>Book C-2 Section 6.3.3 S1.7</remarks>
    private void UpdateLanguagePreferences(FileControlInformationAdf fci)
    {
        if (fci.TryGetLanguagePreference(out LanguagePreference? languagePreference))
        {
            UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();
            builder.Set(languagePreference!);
            _KernelDatabase.GetKernelSession().Update(builder);
        }
    }

    /// <remarks>Book C-2 Section 6.3.3 - S1.7 & S1.8</remarks>
    private bool TryParseTemplateAndAddTransactionDataToDatabase(ActivateKernelRequest signal, out FileControlInformationAdf? result)
    {
        try
        {
            _KernelDatabase.UpdateRange(signal.AsTagLengthValueArray());
            result = signal.GetFileControlInformationCardResponse().GetFileControlInformation();

            return true;
        }
        catch (BerInternalException)
        {
            /* logging */
            signal.GetTransaction().Update(Level2Error.ParsingError);
            HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId(), signal.GetOutcome());
        }
        catch (BerException)
        {
            /* logging */
            signal.GetTransaction().Update(Level2Error.ParsingError);
            HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId(), signal.GetOutcome());
        }
        catch (CardDataMissingException)
        {
            /* logging */
            signal.GetTransaction().Update(Level2Error.CardDataError);
            HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId(), signal.GetOutcome());
        }

        result = null;

        return false;
    }

    /// <remarks>Book C-2 Section 6.2.3; Book C-2 Section 6.3.3 - S1.7 & S1.8</remarks>
    private bool TryInitialize(CorrelationId correlationId, KernelSessionId kernelSessionId, Transaction transaction)
    {
        try
        {
            _KernelDatabase.Activate(kernelSessionId, _TerminalEndpoint, _KernelEndpoint, transaction);
            OutcomeParameterSet.Builder outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
            UserInterfaceRequestData.Builder userInterfaceBuilder = UserInterfaceRequestData.GetBuilder();

            userInterfaceBuilder.Set(MessageHoldTime.Decode(_KernelDatabase.Get(KnownObjects.MessageHoldTime).EncodeValue()));
            _KernelDatabase.GetKernelSession().Reset(outcomeParameterSetBuilder.Complete());
            _KernelDatabase.GetKernelSession().Reset(userInterfaceBuilder.Complete());
            _KernelDatabase.GetKernelSession().Reset(new ErrorIndication(0));

            return true;
        }
        catch (BerInternalException)
        {
            transaction.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId, transaction.GetOutcome());
        }
        catch (BerException)
        {
            /* logging */
            transaction.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId, transaction.GetOutcome());
        }
        catch (CardDataMissingException)
        {
            /* logging */
            transaction.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId, transaction.GetOutcome());
        }
        catch (Exception)
        {
            /* logging */
            transaction.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId, transaction.GetOutcome());
        }

        return false;
    }

    private void HandleBerEncodingException(CorrelationId correlationId, KernelSessionId kernelSessionId, Outcome outcome)
    {
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        builder.Set(StatusOutcome.SelectNext);
        builder.Set(StartOutcome.C);
        outcome.Update(builder);

        _KernelEndpoint.Send(new OutKernelResponse(correlationId, kernelSessionId, outcome));
    }

    private void HandleSupportForFieldOffDetection(FileControlInformationAdf fci)
    {
        if (fci!.TryGetApplicationCapabilitiesInformation(out ApplicationCapabilitiesInformation? result))
        {
            if (result!.IsSupportForFieldOffDetectionSet())
            {
                byte holdTime = _KernelDatabase.Get(MessageHoldTime.Tag).EncodeValue()[0];
                OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
                builder.Set(new FieldOffRequestOutcome(holdTime));
                _KernelDatabase.Update(builder);
            }
        }
    }

    /// <remarks>Book C-2 Section 6.3.3 - S1.9</remarks>
    private void InitializeEmvDataObjects()
    {
        CardDataInputCapability cardDataInputCapability =
            CardDataInputCapability.Decode(_KernelDatabase.Get(CardDataInputCapability.Tag).EncodeValue().AsSpan());
        SecurityCapability securityCapability =
            SecurityCapability.Decode(_KernelDatabase.Get(SecurityCapability.Tag).EncodeValue().AsSpan());

        _KernelDatabase.Initialize(StaticDataAuthenticationTagList.Tag);
        _KernelDatabase.Update(new TerminalCapabilities(cardDataInputCapability, securityCapability));
        _KernelDatabase.Update(new CvmResults(0));

        // WARNING - This should be generated by secure hardware like DUKPT
        _KernelDatabase.Update(new UnpredictableNumber());
    }

    /// <remarks>Book C-2 Section 6.3.3 - S1.10</remarks>
    private void InitializeDataExchangeObjects(TagsToRead? tagsToRead)
    {
        // TODO: DataExchangeKernelService

        DataExchangeKernelService dataExchangeService = _KernelDatabase.GetDataExchanger();

        dataExchangeService.Initialize(new DataNeeded());
        dataExchangeService.Initialize(new DataToSend());
        dataExchangeService.Initialize(new TagsToRead());

        if (tagsToRead != null)
        {
            dataExchangeService.Enqueue(DekRequestType.TagsToRead, tagsToRead);

            // TODO: TagsToReadYet.Update(tagsToRead);
        }
        else
            dataExchangeService.Enqueue(DekRequestType.DataNeeded, TagsToRead.Tag);
    }

    /// <remarks>Book C-2 Section 6.3.3 - S1.12</remarks>
    public void HandleProcessingOptionsDataObjectList(TransactionSessionId transactionSessionId, FileControlInformationAdf fci)
    {
        if (!fci.TryGetProcessingOptionsDataObjectList(out ProcessingOptionsDataObjectList? pdol))
            AddKnownObjectsToDataToSend();

        else if (pdol!.IsRequestedDataAvailable(_KernelDatabase))
            HandlePdolDataIsReady(transactionSessionId, pdol);
        else

            HandlePdolDataIsEmpty(transactionSessionId);
    }

    /// <remarks>Book C-2 Section 6.3.3  S1.13.2 - S1.13.3</remarks>
    private void HandlePdolDataIsReady(TransactionSessionId transactionSessionId, ProcessingOptionsDataObjectList pdol)
    {
        GetProcessingOptionsCommand gpoRequest =
            GetProcessingOptionsCommand.Create(pdol.AsCommandTemplate(_KernelDatabase), transactionSessionId);

        _PcdEndpoint.Request(gpoRequest);
    }

    /// <remarks>Book C-2 Section 6.3.3 S1.13.4 - S1.13.5</remarks>
    private void HandlePdolDataIsEmpty(TransactionSessionId transactionSessionId)
    {
        GetProcessingOptionsCommand gpoRequest = GetProcessingOptionsCommand.Create(transactionSessionId);

        _PcdEndpoint.Request(gpoRequest);
    }

    /// <remarks>Book C-2 Section 6.3.3  S1.15</remarks>
    private void AddKnownObjectsToDataToSend()
    {
        _KernelDatabase.GetDataExchanger().Resolve(DekRequestType.TagsToRead);
    }

    public void InitializeEmvAndDataExchangeObjects()
    {
        _KernelDatabase.Update(new PostGenAcPutDataStatus(0));
        _KernelDatabase.Update(new PostGenAcPutDataStatus(0));
        _KernelDatabase.GetDataExchanger().Initialize(DekResponseType.TagsToWriteAfterGenAc);
        _KernelDatabase.GetDataExchanger().Initialize(DekResponseType.TagsToWriteBeforeGenAc);

        if (_KernelDatabase.IsPresentAndNotEmpty(TagsToWriteBeforeGenAc.Tag))
            _KernelDatabase.GetDataExchanger().Resolve(DekResponseType.TagsToWriteBeforeGenAc);
        else
            _KernelDatabase.GetDataExchanger().Enqueue(DekRequestType.DataNeeded, TagsToWriteBeforeGenAc.Tag);

        if (_KernelDatabase.IsPresentAndNotEmpty(TagsToWriteAfterGenAc.Tag))
            _KernelDatabase.GetDataExchanger().Resolve(DekResponseType.TagsToWriteAfterGenAc);
        else
            _KernelDatabase.GetDataExchanger().Enqueue(DekRequestType.DataNeeded, TagsToWriteAfterGenAc.Tag);
    }

    public override KernelState Handle(QueryKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);
    public override KernelState Handle(UpdateKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);
    public override KernelState Handle(QueryPcdResponse signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);
    public override KernelState Handle(QueryTerminalResponse signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);
}