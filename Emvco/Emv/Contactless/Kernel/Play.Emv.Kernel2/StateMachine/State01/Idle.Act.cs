using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Globalization.Time.Seconds;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class Idle : KernelState
{
    #region ACT

    /// <remarks>Book C-2 Section 6.3.3</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal)
    {
        HandleRequestOutOfSync(session, signal);

        Kernel2Session kernel2Session = (Kernel2Session) session;

        if (!TryInitialize(signal.GetCorrelationId(), signal.GetKernelSessionId(), signal.GetTransaction()))
            return _KernelStateResolver.GetKernelState(StateId);

        if (!TryParseTemplateAndAddTransactionDataToDatabase(signal, out FileControlInformationAdf? fci))
            return _KernelStateResolver.GetKernelState(StateId);

        UpdateLanguagePreferences(kernel2Session, fci!);
        HandleSupportForFieldOffDetection(kernel2Session, fci!);
        InitializeEmvDataObjects();

        // HACK: Should we be pulling 'TagsToRead' directly from the ACT signal? What if the process changes the value before we get here? We should probably flatten the ActivateKernelRequest, or just only expose the ToTagLengthValue() method. That would allow us to strongly type the required objects for the Signal and force us to use the KernelDatabase to retrieve stateful values
        signal.TryGetTagsToRead(out TagsToRead? tagsToRead);
        InitializeDataExchangeObjects(tagsToRead);

        // HACK: Same as above
        HandleProcessingOptionsDataObjectList(kernel2Session, signal.GetFileControlInformationCardResponse().GetFileControlInformation());

        InitializeAcPutData();
        UpdateIntegratedDataStorage();
        HandleDataStorageVersionNumberTerm(kernel2Session);

        return RouteStateTransition(kernel2Session);
    }

    #region S1.7

    /// <remarks>Book C-2 Section 6.3.3 S1.7</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdateLanguagePreferences(Kernel2Session session, FileControlInformationAdf fci)
    {
        if (fci.TryGetLanguagePreference(out LanguagePreference? languagePreference))
            _Database.Update(languagePreference!);
    }

    /// <remarks>Book C-2 Section 6.3.3 - S1.7</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleBerEncodingException(CorrelationId correlationId, KernelSessionId kernelSessionId)
    {
        _Database.Update(StatusOutcome.SelectNext);
        _Database.Update(StartOutcomes.C);

        _KernelEndpoint.Send(new OutKernelResponse(correlationId, kernelSessionId, _Database.GetOutcome()));
    }

    /// <summary>
    ///     HandleSupportForFieldOffDetection
    /// </summary>
    /// <param name="session"></param>
    /// <param name="fci"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleSupportForFieldOffDetection(Kernel2Session session, FileControlInformationAdf fci)
    {
        if (fci!.TryGetApplicationCapabilitiesInformation(out ApplicationCapabilitiesInformation? result))
        {
            if (result!.IsSupportForFieldOffDetectionSet())
            {
                byte holdTime = ((MessageHoldTime) _Database.Get(MessageHoldTime.Tag)).EncodeValue()[0];
                _Database.Update(new FieldOffRequestOutcome(holdTime));
            }
        }
    }

    #endregion

    #region S1.7 & S1.8

    /// <remarks>Book C-2 Section 6.3.3 - S1.7 & S1.8</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private bool TryParseTemplateAndAddTransactionDataToDatabase(ActivateKernelRequest signal, out FileControlInformationAdf? result)
    {
        try
        {
            _Database.Update(signal.AsPrimitiveValues());
            result = signal.GetFileControlInformationCardResponse().GetFileControlInformation();

            return true;
        }
        catch (BerParsingException)
        {
            /* logging */
            _Database.Update(Level2Error.ParsingError);
            HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId());
        }

        catch (CardDataMissingException)
        {
            /* logging */
            _Database.Update(Level2Error.CardDataError);
            HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId());
        }

        result = null;

        return false;
    }

    /// <remarks>Book C-2 Section 6.2.3; Book C-2 Section 6.3.3 - S1.7 & S1.8</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryInitialize(CorrelationId correlationId, KernelSessionId kernelSessionId, Transaction transaction)
    {
        try
        {
            _Database.Activate(kernelSessionId);
            _Database.Update(transaction.AsPrimitiveValueArray());

            OutcomeParameterSet.Builder outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
            UserInterfaceRequestData.Builder userInterfaceBuilder = UserInterfaceRequestData.GetBuilder();

            userInterfaceBuilder.Set((MessageHoldTime) _Database.Get(MessageHoldTime.Tag));
            _Database.Reset(outcomeParameterSetBuilder.Complete());
            _Database.Reset(userInterfaceBuilder.Complete());
            _Database.Reset(new ErrorIndication());

            return true;
        }
        catch (BerParsingException)
        {
            _Database.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId);
        }

        catch (CardDataMissingException)
        {
            /* logging */
            _Database.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId);
        }
        catch (Exception)
        {
            /* logging */
            _Database.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId);
        }

        return false;
    }

    #endregion

    #region S1.9

    /// <remarks>Book C-2 Section 6.3.3 - S1.9</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void InitializeEmvDataObjects()
    {
        CardDataInputCapability cardDataInputCapability = _Database.Get<CardDataInputCapability>(CardDataInputCapability.Tag);
        SecurityCapability securityCapability = _Database.Get<SecurityCapability>(SecurityCapability.Tag);

        _Database.Initialize(StaticDataAuthenticationTagList.Tag);
        _Database.Update(new TerminalCapabilities(cardDataInputCapability, securityCapability));
        _Database.Update(new CvmResults(0));

        _Database.Update(_UnpredictableNumberGenerator.GenerateUnpredictableNumber());
    }

    #endregion

    #region S1.10

    /// <remarks>Book C-2 Section 6.3.3 - S1.10</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void InitializeDataExchangeObjects(TagsToRead? tagsToRead)
    {
        // TODO: DataExchangeKernelService
        _DataExchangeKernelService.Initialize(new DataNeeded());
        _DataExchangeKernelService.Initialize(new DataToSend());
        _DataExchangeKernelService.Initialize(new TagsToRead());

        if (tagsToRead != null)
        {
            _DataExchangeKernelService.Enqueue(DekRequestType.TagsToRead, tagsToRead.GetTagsToReadYet());

            // TODO: TagsToReadYet.Update(tagsToRead);
        }
        else
            _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, TagsToRead.Tag);
    }

    #endregion

    #region S1.11

    // Section S1.11 is handled in S1.12

    #endregion

    #region S1.12

    /// <remarks>Book C-2 Section 6.3.3 - S1.12</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private void HandleProcessingOptionsDataObjectList(Kernel2Session session, FileControlInformationAdf fci)
    {
        if (fci.TryGetProcessingOptionsDataObjectList(out ProcessingOptionsDataObjectList? pdol))
            AddKnownObjectsToDataToSend();

        if (pdol!.IsRequestedDataAvailable(_Database))
        {
            session.SetIsPdolDataMissing(false);
            HandlePdolDataIsReady(session.GetTransactionSessionId(), pdol);

            return;
        }

        session.SetIsPdolDataMissing(true);
        HandlePdolDataIsEmpty(session.GetTransactionSessionId());
    }

    #endregion

    #region S1.13

    // S1.13.1 Is handled by HandleProcessingOptionsDataObjectList and the below methods

    /// <remarks>Book C-2 Section 6.3.3  S1.13.2 - S1.13.3</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private void HandlePdolDataIsReady(TransactionSessionId transactionSessionId, ProcessingOptionsDataObjectList pdol)
    {
        SendGetProcessingOptions(GetProcessingOptionsRequest.Create(pdol.AsCommandTemplate(_Database), transactionSessionId));
    }

    /// <remarks>Book C-2 Section 6.3.3 S1.13.4 - S1.13.5</remarks>
    private void HandlePdolDataIsEmpty(TransactionSessionId transactionSessionId)
    {
        SendGetProcessingOptions(GetProcessingOptionsRequest.Create(transactionSessionId));
    }

    #endregion

    #region S1.14

    /// <remarks>Book C-2 Section 6.3.3 S1.14</remarks>
    private void SendGetProcessingOptions(GetProcessingOptionsRequest command)
    {
        _PcdEndpoint.Request(command);
    }

    #endregion

    #region S1.15

    /// <remarks>Book C-2 Section 6.3.3  S1.15</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void AddKnownObjectsToDataToSend()
    {
        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);
    }

    #endregion

    #region S1.16

    /// <remarks>Book C-2 Section 6.3.3  S1.16</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void InitializeAcPutData()
    {
        _Database.Update(new PostGenAcPutDataStatus(0));
        _Database.Update(new PreGenAcPutDataStatus(0));
        _Database.Initialize(DekResponseType.TagsToWriteBeforeGenAc);
        _Database.Initialize(DekResponseType.TagsToWriteAfterGenAc);

        if (_Database.TryGet(TagsToWriteBeforeGenAc.Tag, out PrimitiveValue? tagsToWriteBeforeGenAc))
            _DataExchangeKernelService.Enqueue(DekResponseType.TagsToWriteBeforeGenAc, tagsToWriteBeforeGenAc!);
        else
            _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, TagsToWriteBeforeGenAc.Tag);

        if (!_Database.TryGet(TagsToWriteAfterGenAc.Tag, out PrimitiveValue? tagsToWriteAfterGenAc))
            _DataExchangeKernelService.Enqueue(DekResponseType.TagsToWriteAfterGenAc, tagsToWriteAfterGenAc!);
        else
            _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, TagsToWriteAfterGenAc.Tag);

        UpdateIntegratedDataStorage();
    }

    #endregion

    #region S1.16.1

    /// <remarks>Book C-2 Section 6.3.3  S1.16.1</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdateIntegratedDataStorage()
    {
        _Database.Update(new IntegratedDataStorageStatus(0));
        _Database.Update(new DataStorageSummaryStatus(0));
        _Database.Update(new DataStorageDigestHash(0));
    }

    #endregion

    #region S1.17

    /// <remarks>Book C-2 Section 6.3.3  S1.17</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void HandleDataStorageVersionNumberTerm(Kernel2Session session)
    {
        if (!_Database.IsPresentAndNotEmpty(DataStorageVersionNumberTerminal.Tag))
        {
            EnqueueDataStorageId();
            EnqueueApplicationCapabilitiesInformation();

            return;
        }

        if (!_Database.IsPresent(DataStorageRequestedOperatorId.Tag))
        {
            EnqueueDataStorageId();
            EnqueueApplicationCapabilitiesInformation();

            return;
        }

        HandlePdolData(session);
    }

    #endregion

    #region S1.18

    /// <summary>
    ///     EnqueueDataStorageId
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks> EMV Book C-2 Section S1.18 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void EnqueueDataStorageId()
    {
        if (_Database.TryGet(DataStorageId.Tag, out PrimitiveValue? dataStorageId))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageId!);
        else
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, new DataStorageId(0));
    }

    /// <summary>
    ///     EnqueueApplicationCapabilitiesInformation
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void EnqueueApplicationCapabilitiesInformation()
    {
        if (_Database.TryGet(ApplicationCapabilitiesInformation.Tag, out PrimitiveValue? applicationCapabilitiesInformation))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, applicationCapabilitiesInformation!);
        else
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, new ApplicationCapabilitiesInformation(0));
    }

    #endregion

    #region S1.19

    /// <summary>
    ///     RouteStateTransition
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <remarks> EMV Book C-2 Section S1.19 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private KernelState RouteStateTransition(Kernel2Session session)
    {
        if (!_Database.TryGet(ApplicationCapabilitiesInformation.Tag, out PrimitiveValue? applicationCapabilitiesInformationTlv))
            return HandlePdolData(session);

        if (!_Database.IsPresentAndNotEmpty(DataStorageId.Tag))
            return HandlePdolData(session);

        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = (ApplicationCapabilitiesInformation) applicationCapabilitiesInformationTlv!;

        if (applicationCapabilitiesInformation.GetDataStorageVersionNumber() == DataStorageVersionNumbers.Version1)
            SetIntegratedDataStorageReadStatus();

        if (applicationCapabilitiesInformation.GetDataStorageVersionNumber() == DataStorageVersionNumbers.Version2)
            SetIntegratedDataStorageReadStatus();

        return HandlePdolData(session);
    }

    #endregion

    #region S1.20

    /// <summary>
    ///     SetIntegratedDataStorageReadStatus
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks> EMV Book C-2 Section S1.20 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void SetIntegratedDataStorageReadStatus()
    {
        if (_Database.TryGet(IntegratedDataStorageStatus.Tag, out PrimitiveValue? integratedDataStorageStatusTlv))
        {
            IntegratedDataStorageStatus integratedDataStorageStatus = (IntegratedDataStorageStatus) integratedDataStorageStatusTlv!;
            _Database.Update(integratedDataStorageStatus.SetRead(true));
        }
    }

    #endregion

    #region S1.21

    // HACK
    /// <summary>
    ///     HandlePdolData
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <remarks> EMV Book C-2 Section S1.21 </remarks>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private KernelState HandlePdolData(Kernel2Session session)
    {
        if (session.IsPdolDataMissing())
        {
            DispatchDataExchangeMessages(session.GetKernelSessionId());
            SetTimeout(session);

            return _KernelStateResolver.GetKernelState(WaitingForPdolData.StateId);
        }

        return _KernelStateResolver.GetKernelState(WaitingForGpoResponse.StateId);
    }

    #endregion

    #region S1.22

    /// <summary>
    ///     DispatchDataExchangeMessages
    /// </summary>
    /// <param name="kernelSessionId"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks> EMV Book C-2 Section S1.22 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void DispatchDataExchangeMessages(KernelSessionId kernelSessionId)
    {
        // HACK: The correlationId cannot be null where. We need to revisit the pattern we're using the resolve requests and responses and implement that pattern here
        _DataExchangeKernelService.SendResponse(kernelSessionId, null);
        _DataExchangeKernelService.SendRequest(kernelSessionId);
        _DataExchangeKernelService.Initialize(DekResponseType.DataToSend);
        _DataExchangeKernelService.Initialize(new DataNeeded());
    }

    #endregion

    #region S1.23

    /// <summary>
    ///     SetTimeout
    /// </summary>
    /// <param name="session"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <remarks> EMV Book C-2 Section S1.23 </remarks>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void SetTimeout(Kernel2Session session)
    {
        TimeoutValue timeout = _Database.Get<TimeoutValue>(TimeoutValue.Tag);

        session.Timer.Start((Milliseconds) timeout, () => _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId())));
    }

    #endregion

    #endregion
}