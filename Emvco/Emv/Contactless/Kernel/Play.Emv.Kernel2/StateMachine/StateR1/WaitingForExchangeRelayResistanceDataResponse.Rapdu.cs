using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Icc.GetData;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Contracts.SignalOut.Queddries;
using Play.Globalization.Time;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForExchangeRelayResistanceDataResponse : KernelState
{
    #region RAPDU

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (TryHandleL1Error(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        // SR1.10
        Milliseconds timeElapsed = session.StopTimeout();

        if (TryHandleInvalidResultCode(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryPersistingRapdu(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        MeasuredRelayResistanceProcessingTime processingTime = CalculateMeasuredRrpTime(timeElapsed);

        if (IsRelayResistanceWithinMinimumRange(processingTime))
        {
            HandleRelayResistanceProtocolFailed(session, signal);

            return _KernelStateResolver.GetKernelState(StateId);
        }

        throw new NotImplementedException();
    }

    #endregion

    #region SR1.3 - SR1.4, SR1.5.1 - SR1.5.2

    /// <remarks>Book C-2 Section SR1.3 - SR1.4, SR1.5.1 - SR1.5.2</remarks>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
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

    #region SR1.11 - SR1.13

    /// <remarks>Book C-2 Section SR1.11 - SR1.13 </remarks>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
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

    #region SR1.14 - SR1.17

    /// <remarks>Book C-2 Section SR1.14 - SR1.17 </remarks>
    /// <exception cref="TerminalDataException"></exception>
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

    #region SR1.18

    /// <remarks>Book C-2 Section SR1.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private MeasuredRelayResistanceProcessingTime CalculateMeasuredRrpTime(Seconds timeElapsed)
    {
        TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedCapduTransmissionTime =
            TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Decode(_KernelDatabase
                                                                               .Get(TerminalExpectedTransmissionTimeForRelayResistanceCapdu
                                                                                        .Tag).EncodeValue().AsSpan());
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedRapduTransmissionTime =
            TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Decode(_KernelDatabase
                                                                               .Get(TerminalExpectedTransmissionTimeForRelayResistanceRapdu
                                                                                        .Tag).EncodeValue().AsSpan());
        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceExpectedRapduTransmissionTime =
            DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode(_KernelDatabase
                                                                              .Get(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu
                                                                                       .Tag).EncodeValue().AsSpan());

        MeasuredRelayResistanceProcessingTime processingTime =
            MeasuredRelayResistanceProcessingTime.Create(timeElapsed, terminalExpectedCapduTransmissionTime,
                                                         terminalExpectedRapduTransmissionTime, deviceExpectedRapduTransmissionTime);

        _KernelDatabase.Update(processingTime);

        return processingTime;
    }

    #endregion

    #region SR1.19

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public bool IsRelayResistanceWithinMinimumRange(MeasuredRelayResistanceProcessingTime processingTime)
    {
        MinTimeForProcessingRelayResistanceApdu minTimeForProcessingRelayResistanceApdu =
            MinTimeForProcessingRelayResistanceApdu.Decode(_KernelDatabase.Get(MinTimeForProcessingRelayResistanceApdu.Tag).EncodeValue()
                                                               .AsSpan());
        MinimumRelayResistanceGracePeriod minGracePeriod =
            MinimumRelayResistanceGracePeriod.Decode(_KernelDatabase.Get(MinimumRelayResistanceGracePeriod.Tag).EncodeValue().AsSpan());

        int expectedProcessingTime = (ushort) minTimeForProcessingRelayResistanceApdu - (ushort) minGracePeriod;

        if ((ushort) processingTime < (expectedProcessingTime < 0 ? 0 : expectedProcessingTime))
            return false;

        return true;
    }

    #endregion

    #region SR1.20 - SR1.21

    /// <exception cref="TerminalDataException"></exception>
    private void HandleRelayResistanceProtocolFailed(KernelSession session, QueryPcdResponse signal)
    {
        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.CardDataError);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    private void TryRepeatExchangeRelayResistanceData(Kernel2Session session)
    {
        if (session.GetRelayResistanceProtocolCount() > 2)
            return;

        UnpredictableNumber unpredictableNumber = _UnpredictableNumberGenerator.GenerateUnpredictableNumber();
    }

    private void RangeCheck(MeasuredRelayResistanceProcessingTime processingTime)
    {
        MaxTimeForProcessingRelayResistanceApdu maxProcessingTime =
            MaxTimeForProcessingRelayResistanceApdu.Decode(_KernelDatabase.Get(MaxTimeForProcessingRelayResistanceApdu.Tag).EncodeValue()
                                                               .AsSpan());
        MaximumRelayResistanceGracePeriod maxGraceTime =
            MaximumRelayResistanceGracePeriod.Decode(_KernelDatabase.Get(MaximumRelayResistanceGracePeriod.Tag).EncodeValue().AsSpan());
    }
}