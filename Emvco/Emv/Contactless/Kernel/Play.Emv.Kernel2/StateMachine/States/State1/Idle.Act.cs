﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.StateMachine.State3;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Templates.Exceptions;
using Play.Emv.Templates.FileControlInformation;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class Idle : KernelState
{
    #region ACT

    /// <remarks>Book C-2 Section 6.3.3</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal)
    {
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
    private void UpdateLanguagePreferences(Kernel2Session session, FileControlInformationAdf fci)
    {
        if (fci.TryGetLanguagePreference(out LanguagePreference? languagePreference))
        {
            UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();
            builder.Set(languagePreference!);
            ((Kernel2Database) _KernelDatabase).Update(builder);
        }
    }

    /// <remarks>Book C-2 Section 6.3.3 - S1.7</remarks>
    private void HandleBerEncodingException(CorrelationId correlationId, KernelSessionId kernelSessionId)
    {
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        builder.Set(StatusOutcome.SelectNext);
        builder.Set(StartOutcome.C);
        _KernelDatabase.Update(builder);

        _KernelEndpoint.Send(new OutKernelResponse(correlationId, kernelSessionId, _KernelDatabase.GetOutcome()));
    }

    private void HandleSupportForFieldOffDetection(Kernel2Session session, FileControlInformationAdf fci)
    {
        if (fci!.TryGetApplicationCapabilitiesInformation(out ApplicationCapabilitiesInformation? result))
        {
            if (result!.IsSupportForFieldOffDetectionSet())
            {
                byte holdTime = _KernelDatabase.Get(MessageHoldTime.Tag).EncodeValue()[0];
                OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
                builder.Set(new FieldOffRequestOutcome(holdTime));
                ((Kernel2Database) _KernelDatabase).Update(builder);
                _KernelDatabase.Update(builder);
            }
        }
    }

    #endregion

    #region S1.7 & S1.8

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
            HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId());
        }
        catch (BerException)
        {
            /* logging */
            signal.GetTransaction().Update(Level2Error.ParsingError);
            HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId());
        }
        catch (CardDataMissingException)
        {
            /* logging */
            signal.GetTransaction().Update(Level2Error.CardDataError);
            HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId());
        }

        result = null;

        return false;
    }

    /// <remarks>Book C-2 Section 6.2.3; Book C-2 Section 6.3.3 - S1.7 & S1.8</remarks>
    private bool TryInitialize(CorrelationId correlationId, KernelSessionId kernelSessionId, Transaction transaction)
    {
        try
        {
            _KernelDatabase.Activate(kernelSessionId, transaction);
            OutcomeParameterSet.Builder outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
            UserInterfaceRequestData.Builder userInterfaceBuilder = UserInterfaceRequestData.GetBuilder();

            userInterfaceBuilder.Set(MessageHoldTime.Decode(_KernelDatabase.Get(KnownObjects.MessageHoldTime).EncodeValue()));
            _KernelDatabase.Reset(outcomeParameterSetBuilder.Complete());
            _KernelDatabase.Reset(userInterfaceBuilder.Complete());
            _KernelDatabase.Reset(new ErrorIndication(0));

            return true;
        }
        catch (BerInternalException)
        {
            transaction.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId);
        }
        catch (BerException)
        {
            /* logging */
            transaction.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId);
        }
        catch (CardDataMissingException)
        {
            /* logging */
            transaction.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId);
        }
        catch (Exception)
        {
            /* logging */
            transaction.Update(Level2Error.ParsingError);
            HandleBerEncodingException(correlationId, kernelSessionId);
        }

        return false;
    }

    #endregion

    #region S1.9

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

    #endregion

    #region S1.10

    /// <remarks>Book C-2 Section 6.3.3 - S1.10</remarks>
    private void InitializeDataExchangeObjects(TagsToRead? tagsToRead)
    {
        // TODO: DataExchangeKernelService
        _DataExchangeKernelService.Initialize(new DataNeeded());
        _DataExchangeKernelService.Initialize(new DataToSend());
        _DataExchangeKernelService.Initialize(new TagsToRead());

        if (tagsToRead != null)
        {
            _DataExchangeKernelService.Enqueue(DekRequestType.TagsToRead, tagsToRead);

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
    public void HandleProcessingOptionsDataObjectList(Kernel2Session session, FileControlInformationAdf fci)
    {
        if (fci.TryGetProcessingOptionsDataObjectList(out ProcessingOptionsDataObjectList? pdol))
            AddKnownObjectsToDataToSend();

        if (pdol!.IsRequestedDataAvailable(_KernelDatabase))
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
    private void HandlePdolDataIsReady(TransactionSessionId transactionSessionId, ProcessingOptionsDataObjectList pdol)
    {
        SendGetProcessingOptions(GetProcessingOptionsRequest.Create(pdol.AsCommandTemplate(_KernelDatabase), transactionSessionId));
    }

    /// <remarks>Book C-2 Section 6.3.3 S1.13.4 - S1.13.5</remarks>
    private void HandlePdolDataIsEmpty(TransactionSessionId transactionSessionId)
    {
        SendGetProcessingOptions(GetProcessingOptionsRequest.Create(transactionSessionId));
    }

    #endregion

    #region S1.14

    /// <remarks>Book C-2 Section 6.3.3 S1.14</remarks>
    public void SendGetProcessingOptions(GetProcessingOptionsRequest command)
    {
        _PcdEndpoint.Request(command);
    }

    #endregion

    #region S1.15

    /// <remarks>Book C-2 Section 6.3.3  S1.15</remarks>
    private void AddKnownObjectsToDataToSend()
    {
        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);
    }

    #endregion

    #region S1.16

    /// <remarks>Book C-2 Section 6.3.3  S1.16</remarks>
    private void InitializeAcPutData()
    {
        _KernelDatabase.Update(new PostGenAcPutDataStatus(0));
        _KernelDatabase.Update(new PreGenAcPutDataStatus(0));
        _KernelDatabase.Initialize(DekResponseType.TagsToWriteBeforeGenAc);
        _KernelDatabase.Initialize(DekResponseType.TagsToWriteAfterGenAc);

        if (_KernelDatabase.TryGet(TagsToWriteBeforeGenAc.Tag, out TagLengthValue? tagsToWriteBeforeGenAc))
            _DataExchangeKernelService.Enqueue(DekResponseType.TagsToWriteBeforeGenAc, tagsToWriteBeforeGenAc!);
        else
            _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, TagsToWriteBeforeGenAc.Tag);

        if (!_KernelDatabase.TryGet(TagsToWriteAfterGenAc.Tag, out TagLengthValue? tagsToWriteAfterGenAc))
            _DataExchangeKernelService.Enqueue(DekResponseType.TagsToWriteAfterGenAc, tagsToWriteAfterGenAc!);
        else
            _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, TagsToWriteAfterGenAc.Tag);

        UpdateIntegratedDataStorage();
    }

    #endregion

    #region S1.16.1

    /// <remarks>Book C-2 Section 6.3.3  S1.16.1</remarks>
    private void UpdateIntegratedDataStorage()
    {
        _KernelDatabase.Update(new IntegratedDataStorageStatus(0));
        _KernelDatabase.Update(new DataStorageSummaryStatus(0));
        _KernelDatabase.Update(new DataStorageDigestHash(0));
    }

    #endregion

    #region S1.17

    /// <remarks>Book C-2 Section 6.3.3  S1.17</remarks>
    public void HandleDataStorageVersionNumberTerm(Kernel2Session session)
    {
        if (!_KernelDatabase.IsPresentAndNotEmpty(DataStorageVersionNumberTerm.Tag))
        {
            EnqueueDataStorageId();
            EnqueueApplicationCapabilitiesInformation();

            return;
        }

        if (!_KernelDatabase.IsPresent(DataStorageRequestedOperatorId.Tag))
        {
            EnqueueDataStorageId();
            EnqueueApplicationCapabilitiesInformation();

            return;
        }

        HandlePdolData(session);
    }

    #endregion

    #region S1.18

    private void EnqueueDataStorageId()
    {
        if (_KernelDatabase.TryGet(DataStorageId.Tag, out TagLengthValue? dataStorageId))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageId!);
        else
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, new DataStorageId(0));
    }

    private void EnqueueApplicationCapabilitiesInformation()
    {
        if (_KernelDatabase.TryGet(ApplicationCapabilitiesInformation.Tag, out TagLengthValue? applicationCapabilitiesInformation))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, applicationCapabilitiesInformation!);
        else
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, new ApplicationCapabilitiesInformation(0));
    }

    #endregion

    #region S1.19

    public KernelState RouteStateTransition(Kernel2Session session)
    {
        if (!_KernelDatabase.TryGet(ApplicationCapabilitiesInformation.Tag, out TagLengthValue? applicationCapabilitiesInformationTlv))
            return HandlePdolData(session);

        if (!_KernelDatabase.IsPresentAndNotEmpty(DataStorageId.Tag))
            return HandlePdolData(session);

        ApplicationCapabilitiesInformation applicationCapabilitiesInformation =
            ApplicationCapabilitiesInformation.Decode(applicationCapabilitiesInformationTlv!.GetValue().AsSpan());

        if ((byte) applicationCapabilitiesInformation.GetDataStorageVersionNumber() == DataStorageVersionNumberTypes.Version1)
            SetIntegratedDataStorageReadStatus();

        if ((byte) applicationCapabilitiesInformation.GetDataStorageVersionNumber() == DataStorageVersionNumberTypes.Version2)
            SetIntegratedDataStorageReadStatus();

        return HandlePdolData(session);
    }

    #endregion

    #region S1.20

    private void SetIntegratedDataStorageReadStatus()
    {
        if (_KernelDatabase.TryGet(IntegratedDataStorageStatus.Tag, out TagLengthValue? integratedDataStorageStatusTlv))
        {
            IntegratedDataStorageStatus integratedDataStorageStatus =
                IntegratedDataStorageStatus.Decode(integratedDataStorageStatusTlv!.GetValue().AsSpan());
            _KernelDatabase.Update(integratedDataStorageStatus.SetRead());
        }
    }

    #endregion

    #region S1.21

    // HACK
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

    public void DispatchDataExchangeMessages(KernelSessionId kernelSessionId)
    {
        _DataExchangeKernelService.SendResponse(kernelSessionId);
        _DataExchangeKernelService.SendRequest(kernelSessionId);
        _DataExchangeKernelService.Initialize(DekResponseType.DataToSend);
        _DataExchangeKernelService.Initialize(new DataNeeded());
    }

    #endregion

    #region S1.23

    public void SetTimeout(Kernel2Session session)
    {
        TimeoutValue timeout = TimeoutValue.Decode(_KernelDatabase.Get(TimeoutValue.Tag).GetValue().AsSpan());

        session.StartTimeout((Milliseconds) timeout, () => _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId())));
    }

    #endregion

    #endregion
}