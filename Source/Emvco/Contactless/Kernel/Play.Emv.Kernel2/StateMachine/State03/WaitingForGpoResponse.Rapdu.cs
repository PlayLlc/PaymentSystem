﻿using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (signal is not GetProcessingOptionsResponse rapdu)
            throw new RequestOutOfSyncException($"The request is invalid for the current state of the [{nameof(KernelChannel)}] channel");

        if (TryHandleL1Error(session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandleInvalidResultCode(session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryPersistingRapdu(session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandleMissingCardData(session))
            return _KernelStateResolver.GetKernelState(StateId);

        Kernel2Session kernel2Session = (Kernel2Session) session;
        ApplicationInterchangeProfile applicationInterchangeProfile = _Database.Get<ApplicationInterchangeProfile>(ApplicationInterchangeProfile.Tag);
        ApplicationFileLocator applicationFileLocator = _Database.Get<ApplicationFileLocator>(ApplicationFileLocator.Tag);
        KernelConfiguration kernelConfiguration = _Database.Get<KernelConfiguration>(KernelConfiguration.Tag);

        if (IsEmvModeSupported(applicationInterchangeProfile))
            return HandleEmvMode(kernel2Session, signal, applicationFileLocator, applicationInterchangeProfile, kernelConfiguration);

        if (TryHandlingMagstripeNotSupported(kernel2Session))
            return _KernelStateResolver.GetKernelState(StateId);

        return HandleMagstripeMode(kernel2Session, applicationFileLocator);
    }

    #region S3.4 - S3.5

    /// <remarks>Book C-2 Section S3.4 - S3.5 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsLevel1ErrorPresent())
            return false;

        _Database.Update(MessageIdentifiers.TryAgain);
        _Database.Update(Statuses.ReadyToRead);
        _Database.Update(new MessageHoldTime(0));
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(StartOutcomes.B);
        _Database.SetUiRequestOnRestartPresent(true);
        _Database.Update(signal.GetLevel1Error());
        _Database.Update(MessageOnErrorIdentifiers.TryAgain);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _EndpointClient.Send(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region S3.8 - S3.9.2

    /// <remarks>Book C-2 Section S3.8 - S3.9.2 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleInvalidResultCode(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Statuses.NotReady);
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Level2Error.StatusBytes);
        _Database.Update(signal.GetStatusWords());
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _Database.SetUiRequestOnRestartPresent(true);

        _EndpointClient.Send(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region S3.13 - S3.14

    /// <remarks>Emv Book C-2 Section  S3.13 - S3.14 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private bool TryHandleMissingCardData(KernelSession session)
    {
        if (!_Database.IsPresentAndNotEmpty(ApplicationFileLocator.Tag))
        {
            HandleInvalidResponse(session, Level2Error.CardDataMissing);

            return true;
        }

        if (!_Database.IsPresentAndNotEmpty(ApplicationInterchangeProfile.Tag))
        {
            HandleInvalidResponse(session, Level2Error.CardDataMissing);

            return true;
        }

        return false;
    }

    #endregion

    #region S3.15 - S3.16

    /// <remarks>EMV Book C-2 Section S3.15 - S3.16</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsEmvModeSupported(ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (!applicationInterchangeProfile.IsEmvModeSupported())
            return false;

        if (_Database.IsEmvModeSupported())
            return false;

        return true;
    }

    #endregion

    #region S3.17 - S3.18

    /// <remarks>EMV Book C-2 Section S3.17 - S3.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private bool TryHandlingMagstripeNotSupported(Kernel2Session session)
    {
        if (_Database.IsMagstripeModeSupported())
            return false;

        HandleInvalidResponse(session, Level2Error.MagstripeNotSupported);

        return true;
    }

    #endregion

    #region S3.90.1 - S3.90.2

    /// <remarks>Emv Book C-2 Section S3.90.1 - S3.90.2 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void HandleInvalidResponse(KernelSession session, Level2Error level2Error)
    {
        _Database.Update(level2Error);
        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Statuses.NotReady);
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _DataExchangeKernelService.Enqueue(DekResponseType.DiscretionaryData, _Database.GetErrorIndication());
        _Database.SetUiRequestOnOutcomePresent(true);

        _EndpointClient.Send(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    #region S3.10 - S3.12

    /// <remarks>Book C-2 SectionS3.10 - S3.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryPersistingRapdu(KernelSession session, GetProcessingOptionsResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        try
        {
            ProcessingOptions processingOptions = signal.AsProcessingOptions();
            PrimitiveValue[] dataToGet = signal.AsProcessingOptions().GetPrimitiveDescendants();

            _Database.Update(dataToGet);
            _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Logging

            HandleBerParsingException(session);

            return false;
        }
        catch (Exception)
        {
            // TODO: Logging

            HandleBerParsingException(session);

            return false;
        }
    }

    #endregion

    #region S3.10 - S3.12 continued

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleBerParsingException(KernelSession session)
    {
        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Statuses.NotReady);
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Level2Error.ParsingError);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _Database.SetUiRequestOnRestartPresent(true);

        _EndpointClient.Send(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    #endregion

    #region Emv Mode

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private KernelState HandleEmvMode(
        Kernel2Session session, Message message, ApplicationFileLocator applicationFileLocator, ApplicationInterchangeProfile applicationInterchangeProfile,
        KernelConfiguration kernelConfiguration)
    {
        SetActiveAflForEmvMode(session, applicationFileLocator, kernelConfiguration);

        // S3.33 - S3.35 - Setting the Reader Contactless Transaction Limit is done automatically on retrieval by the KernelDatabase

        if (IsRelayResistanceProtocolSupported(applicationInterchangeProfile))
            return InitializeRelayResistanceProtocol(session);

        return HandleRelayResistanceProtocolNotSupported(session, message);
    }

    #region S3.30 - S3.32

    /// <remarks>EMV Book C-2 Section S3.30 - S3.32</remarks>
    private static void SetActiveAflForEmvMode(Kernel2Session session, ApplicationFileLocator applicationFileLocator, KernelConfiguration kernelConfiguration)
    {
        // S3.30
        if (applicationFileLocator.IsOptimizedForEmv(kernelConfiguration))
            session.EnqueueActiveTag(applicationFileLocator.GetRecordRanges()[1..]);
        else
            session.EnqueueActiveTag(applicationFileLocator.GetRecordRanges());
    }

    #endregion

    #region S3.33 - S3.35

    /// <summary>
    ///     IsOnDeviceCardholderVerificationSupported
    /// </summary>
    /// <param name="applicationInterchangeProfile"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsOnDeviceCardholderVerificationSupported(ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (!applicationInterchangeProfile.IsOnDeviceCardholderVerificationSupported())
            return false;

        if (!_Database.IsOnDeviceCardholderVerificationSupported())
            return false;

        return true;
    }

    #endregion

    #region S3.60

    /// <remarks>EMV Book C-2 Section S3.60</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsRelayResistanceProtocolSupported(ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (!applicationInterchangeProfile.IsRelayResistanceProtocolSupported())
            return false;

        if (!_Database.IsRelayResistanceProtocolSupported())
            return false;

        return true;
    }

    #endregion

    #region S3.65

    /// <remarks>EMV Book C-2 Section S3.65</remarks>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private KernelState HandleRelayResistanceProtocolNotSupported(Kernel2Session session, Message message)
    {
        _Database.Set(TerminalVerificationResultCodes.RelayResistanceNotPerformed);

        return _KernelStateResolver.GetKernelState(_S3R1.Process(this, session, message));
    }

    #endregion

    #region S3.60 - S3.64

    /// <remarks>EMV Book C-2 Section S3.60 - S3.64</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private KernelState InitializeRelayResistanceProtocol(Kernel2Session session)
    {
        TerminalRelayResistanceEntropy terminalEntropy = CreateTerminalEntropy();

        // S3.62
        ExchangeRelayResistanceDataRequest capdu = ExchangeRelayResistanceDataRequest.Create(session.GetTransactionSessionId(), terminalEntropy);

        // HACK: We need to create another object that is just a stopwatch and not a timeout manager
        // S3.63
        session.Stopwatch.Start();

        // S3.64
        _EndpointClient.Send(capdu);

        return _KernelStateResolver.GetKernelState(WaitingForExchangeRelayResistanceDataResponse.StateId);
    }

    #endregion

    #region S3.61

    /// <remarks>Book C-2 Section S3.61</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private TerminalRelayResistanceEntropy CreateTerminalEntropy()
    {
        UnpredictableNumber unpredictableNumber = _UnpredictableNumberGenerator.GenerateUnpredictableNumber();
        _Database.Update(unpredictableNumber);

        TerminalRelayResistanceEntropy entropy = new(unpredictableNumber);
        _Database.Update(entropy);

        return entropy;
    }

    #endregion

    #endregion

    #region Magstripe Mode

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private KernelState HandleMagstripeMode(Kernel2Session session, ApplicationFileLocator applicationFileLocator)
    {
        SetActiveAflForMagstripeMode(session, applicationFileLocator);

        // S3.73 - S3.75 - Setting the Reader Contactless Transaction Limit is done automatically on retrieval by the KernelDatabase
        UpdateDataToSend();
        HandleDekRequest(session);
        SendReadRecordCommand(session);

        return _KernelStateResolver.GetKernelState(WaitingForMagStripeReadRecordResponse.StateId);
    }

    #region S3.70 - S3.72

    /// <remarks>Emv Book C-2 Section S3.70 - S3.72 </remarks>
    private static void SetActiveAflForMagstripeMode(Kernel2Session session, ApplicationFileLocator applicationFileLocator)
    {
        // S3.30
        if (applicationFileLocator.IsOptimizedForMagstripe())
            session.EnqueueActiveTag(applicationFileLocator.GetRecordRanges()[..1]);
        else
            session.EnqueueActiveTag(applicationFileLocator.GetRecordRanges());
    }

    #endregion

    #region S3.73 - S3.75

    #endregion

    #region S3.76

    /// <remarks>Emv Book C-2 Section S3.76 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdateDataToSend()
    {
        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);
    }

    #endregion

    #region S3.77 - S3.78

    /// <remarks>Emv Book C-2 Section S3.77 - S3.78 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleDekRequest(Kernel2Session session)
    {
        if (_DataExchangeKernelService.IsEmpty(DekRequestType.DataNeeded))
            _DataExchangeKernelService.SendRequest(session.GetKernelSessionId());

        if (!_DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead))
            return;

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend))
            return;

        // HACK: 
        _DataExchangeKernelService.SendResponse(session.GetKernelSessionId(), null);
    }

    #endregion

    #region S3.80 - S3.81

    /// <remarks>Emv Book C-2 Section S3.80 - S3.81 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SendReadRecordCommand(Kernel2Session session)
    {
        if (!session.TryPeekActiveTag(out RecordRange recordRange))
            throw new TerminalDataException($"The {nameof(ApplicationFileLocator)} was correctly parsed but was unable to be retrieved");

        _EndpointClient.Send(ReadRecordRequest.Create(session.GetTransactionSessionId(), recordRange.GetShortFileIdentifier()));
    }

    #endregion

    #endregion
}