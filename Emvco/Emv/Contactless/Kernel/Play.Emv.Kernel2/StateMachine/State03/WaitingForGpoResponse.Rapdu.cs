﻿using System;

using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (TryHandleL1Error(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandleInvalidResultCode(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryPersistingRapdu(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandleMissingCardData(session))
            return _KernelStateResolver.GetKernelState(StateId);

        Kernel2Session kernel2Session = (Kernel2Session) session;
        ApplicationInterchangeProfile applicationInterchangeProfile =
            ApplicationInterchangeProfile.Decode(_KernelDatabase.Get(ApplicationInterchangeProfile.Tag).EncodeValue().AsSpan());
        ApplicationFileLocator applicationFileLocator =
            ApplicationFileLocator.Decode(_KernelDatabase.Get(ApplicationFileLocator.Tag).EncodeValue().AsSpan());
        KernelConfiguration kernelConfiguration =
            KernelConfiguration.Decode(_KernelDatabase.Get(KernelConfiguration.Tag).EncodeValue().AsSpan());

        if (IsEmvModeSupported(applicationInterchangeProfile))
            return HandleEmvMode(kernel2Session, applicationFileLocator, applicationInterchangeProfile, kernelConfiguration);

        if (TryHandlingMagstripeNotSupported(kernel2Session))
            return _KernelStateResolver.GetKernelState(StateId);

        return HandleMagstripeMode(kernel2Session, applicationFileLocator, applicationInterchangeProfile, kernelConfiguration);
    }

    #region S3.4 - S3.5

    /// <remarks>Book C-2 Section S3.4 - S3.5 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsSuccessful())
            return false;

        session.StopTimeout();

        _KernelDatabase.Update(MessageIdentifier.TryAgain);
        _KernelDatabase.Update(Status.ReadyToRead);
        _KernelDatabase.Update(new MessageHoldTime(0));
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(StartOutcome.B);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);
        _KernelDatabase.Update(signal.GetLevel1Error());
        _KernelDatabase.Update(MessageOnErrorIdentifier.TryAgain);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

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

        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.StatusBytes);
        _KernelDatabase.Update(signal.GetStatusWords());
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region S3.10 - S3.12

    /// <remarks>Book C-2 SectionS3.10 - S3.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryPersistingRapdu(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        try
        {
            _KernelDatabase.Update(((GetDataResponse) signal).GetTagLengthValueResult());
            _DataExchangeKernelService.Resolve((GetDataResponse) signal);

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Logging

            HandleBerParsingException(session, signal);

            return false;
        }
        catch (Exception)
        {
            // TODO: Logging

            HandleBerParsingException(session, signal);

            return false;
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleBerParsingException(KernelSession session, QueryPcdResponse signal)
    {
        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.ParsingError);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    #region S3.13 - S3.14

    /// <remarks>Emv Book C-2 Section  S3.13 - S3.14 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryHandleMissingCardData(KernelSession session)
    {
        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationFileLocator.Tag))
        {
            HandleInvalidResponse(session, Level2Error.CardDataMissing);

            return true;
        }

        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationInterchangeProfile.Tag))
        {
            HandleInvalidResponse(session, Level2Error.CardDataMissing);

            return true;
        }

        return false;
    }

    #endregion

    #region S3.15 - S3.16

    /// <remarks>EMV Book C-2 Section S3.15 - S3.16</remarks>
    public bool IsEmvModeSupported(ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (!applicationInterchangeProfile.IsEmvModeSupported())
            return false;

        if (_KernelDatabase.IsEmvModeSupported())
            return false;

        return true;
    }

    #endregion

    #region S3.17 - S3.18

    /// <remarks>EMV Book C-2 Section S3.17 - S3.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public bool TryHandlingMagstripeNotSupported(Kernel2Session session)
    {
        if (_KernelDatabase.IsMagstripeModeSupported())
            return false;

        HandleInvalidResponse(session, Level2Error.MagstripeNotSupported);

        return true;
    }

    #endregion

    #region Emv Mode

    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public KernelState HandleEmvMode(
        Kernel2Session session,
        ApplicationFileLocator applicationFileLocator,
        ApplicationInterchangeProfile applicationInterchangeProfile,
        KernelConfiguration kernelConfiguration)
    {
        SetActiveAflForEmvMode(session, applicationFileLocator, kernelConfiguration);
        SetContactlessTransactionLimitForEmvMode(session, applicationInterchangeProfile);

        if (IsRelayResistanceProtocolSupported(applicationInterchangeProfile))
            return InitializeRelayResistanceProtocol(session);

        return HandleRelayResistanceProtocolNotSupported(session);
    }

    #region S3.30 - S3.32

    /// <remarks>EMV Book C-2 Section S3.30 - S3.32</remarks>
    public void SetActiveAflForEmvMode(
        Kernel2Session session,
        ApplicationFileLocator applicationFileLocator,
        KernelConfiguration kernelConfiguration)
    {
        // S3.30
        if (applicationFileLocator.IsOptimizedForEmv(kernelConfiguration))
            session.EnqueueActiveTag(applicationFileLocator.GetRecordRanges()[1..]);
        else
            session.EnqueueActiveTag(applicationFileLocator.GetRecordRanges());
    }

    #endregion

    #region S3.33 - S3.35

    /// <remarks>EMV Book C-2 Section S3.33 - S3.35</remarks>
    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void SetContactlessTransactionLimitForEmvMode(
        Kernel2Session session,
        ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (IsOnDeviceCardholderVerificationSupported(applicationInterchangeProfile))
        {
            ReaderContactlessTransactionLimitWhenCvmIsOnDevice onDevice =
                ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Decode(_KernelDatabase
                                                                              .Get(ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag)
                                                                              .EncodeValue().AsSpan());
            session.Update(onDevice);
        }
        else
        {
            ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice onDevice =
                ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Decode(_KernelDatabase
                                                                                 .Get(ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice
                                                                                          .Tag).EncodeValue().AsSpan());
            session.Update(onDevice);
        }
    }

    public bool IsOnDeviceCardholderVerificationSupported(ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (!applicationInterchangeProfile.IsOnDeviceCardholderVerificationSupported())
            return false;

        if (!_KernelDatabase.IsOnDeviceCardholderVerificationSupported())
            return false;

        return true;
    }

    #endregion

    #region S3.60

    /// <remarks>EMV Book C-2 Section S3.60</remarks>
    public bool IsRelayResistanceProtocolSupported(ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (!applicationInterchangeProfile.IsRelayResistanceProtocolSupported())
            return false;

        if (!_KernelDatabase.IsRelayResistanceProtocolSupported())
            return false;

        return true;
    }

    #endregion

    #region S3.65

    /// <remarks>EMV Book C-2 Section S3.65</remarks>
    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public KernelState HandleRelayResistanceProtocolNotSupported(Kernel2Session session)
    {
        _KernelDatabase.Set(TerminalVerificationResultCodes.RelayResistanceNotPerformed);

        return _S3R1.Process(this, session);
    }

    #endregion

    #region S3.60 - S3.64

    /// <remarks>EMV Book C-2 Section S3.60 - S3.64</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public KernelState InitializeRelayResistanceProtocol(Kernel2Session session)
    {
        TerminalRelayResistanceEntropy terminalEntropy = CreateTerminalEntropy();

        // S3.62
        ExchangeRelayResistanceDataRequest capdu =
            ExchangeRelayResistanceDataRequest.Create(session.GetTransactionSessionId(), terminalEntropy);

        // HACK: We need to create another object that is just a stopwatch and not a timeout manager
        // S3.63
        session.Stopwatch.Start();

        // S3.64
        _PcdEndpoint.Request(capdu);

        return _KernelStateResolver.GetKernelState(WaitingForExchangeRelayResistanceDataResponse.StateId);
    }

    #endregion

    #region S3.61

    /// <remarks>Book C-2 Section S3.61</remarks>
    /// <exception cref="TerminalDataException"></exception>
    public TerminalRelayResistanceEntropy CreateTerminalEntropy()
    {
        UnpredictableNumber unpredictableNumber = _UnpredictableNumberGenerator.GenerateUnpredictableNumber();
        _KernelDatabase.Update(unpredictableNumber);

        TerminalRelayResistanceEntropy entropy = new(unpredictableNumber);
        _KernelDatabase.Update(entropy);

        return entropy;
    }

    #endregion

    #endregion

    #region Magstripe Mode

    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public KernelState HandleMagstripeMode(
        Kernel2Session session,
        ApplicationFileLocator applicationFileLocator,
        ApplicationInterchangeProfile applicationInterchangeProfile,
        KernelConfiguration kernelConfiguration)
    {
        SetActiveAflForMagstripeMode(session, applicationFileLocator);
        SetContactlessTransactionLimitForMagstripeMode(session, applicationInterchangeProfile);
        UpdateDataToSend();
        HandleDekRequest(session);
        SendReadRecordCommand(session);

        return _KernelStateResolver.GetKernelState(WaitingForMagStripeReadRecordResponse.StateId);
    }

    #region S3.70 - S3.72

    /// <remarks>Emv Book C-2 Section S3.70 - S3.72 </remarks>
    public void SetActiveAflForMagstripeMode(Kernel2Session session, ApplicationFileLocator applicationFileLocator)
    {
        // S3.30
        if (applicationFileLocator.IsOptimizedForMagstripe())
            session.EnqueueActiveTag(applicationFileLocator.GetRecordRanges()[..1]);
        else
            session.EnqueueActiveTag(applicationFileLocator.GetRecordRanges());
    }

    #endregion

    #region S3.73 - S3.75

    /// <remarks>Emv Book C-2 Section S3.73 - S3.75 </remarks>
    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void SetContactlessTransactionLimitForMagstripeMode(
        Kernel2Session session,
        ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (IsOnDeviceCardholderVerificationSupported(applicationInterchangeProfile))
        {
            ReaderContactlessTransactionLimitWhenCvmIsOnDevice onDevice =
                ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Decode(_KernelDatabase
                                                                              .Get(ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag)
                                                                              .EncodeValue().AsSpan());
            session.Update(onDevice);
        }
        else
        {
            ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice onDevice =
                ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Decode(_KernelDatabase
                                                                                 .Get(ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice
                                                                                          .Tag).EncodeValue().AsSpan());
            session.Update(onDevice);
        }
    }

    #endregion

    #region S3.76

    /// <remarks>Emv Book C-2 Section S3.76 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public void UpdateDataToSend()
    {
        _DataExchangeKernelService.Resolve(_KernelDatabase);
    }

    #endregion

    #region S3.77 - S3.78

    /// <remarks>Emv Book C-2 Section S3.77 - S3.78 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public void HandleDekRequest(Kernel2Session session)
    {
        if (_DataExchangeKernelService.IsEmpty(DekRequestType.DataNeeded))
            _DataExchangeKernelService.SendRequest(session.GetKernelSessionId());

        if (!_DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead))
            return;

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend))
            return;

        _DataExchangeKernelService.SendResponse(session.GetKernelSessionId());
    }

    #endregion

    #region S3.80 - S3.81

    /// <remarks>Emv Book C-2 Section S3.80 - S3.81 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    public void SendReadRecordCommand(Kernel2Session session)
    {
        if (!session.TryPeekActiveTag(out RecordRange recordRange))
            throw new TerminalDataException($"The {nameof(ApplicationFileLocator)} was correctly parsed but was unable to be retrieved");

        _PcdEndpoint.Request(ReadRecordRequest.Create(session.GetTransactionSessionId(), recordRange.GetShortFileIdentifier()));
    }

    #endregion

    #endregion

    #region S3.90.1 - S3.90.2

    /// <remarks>Emv Book C-2 Section S3.90.1 - S3.90.2 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private void HandleInvalidResponse(KernelSession session, Level2Error level2Error)
    {
        _KernelDatabase.Update(level2Error);
        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _DataExchangeKernelService.Enqueue(DekResponseType.DiscretionaryData, _KernelDatabase.GetErrorIndication());
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion
}