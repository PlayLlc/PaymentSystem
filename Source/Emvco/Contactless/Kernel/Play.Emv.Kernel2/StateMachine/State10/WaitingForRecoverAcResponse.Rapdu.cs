using System;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Core.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes.DataStorage;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForRecoverAcResponse
{
    #region Instance Members

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CardDataException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        RecoverAcResponse rapdu = (RecoverAcResponse) signal;
        Kernel2Session kernel2Session = (Kernel2Session) session;

        if (TryHandlingL1Error(session.GetKernelSessionId(), signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandlingL2Error(kernel2Session, signal))
            return _KernelStateResolver.GetKernelState(WaitingForGenerateAcResponse2.StateId);

        // S10.10 - S10.14
        if (TryHandlingBerParsingException(kernel2Session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        //  S10.15 - S10.16
        if (TryHandlingMissingMandatoryData(kernel2Session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        // S10.17 - S10.18
        if (TryHandlingInvalidCryptogramInformationData(kernel2Session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        // S10.19 - S10.22
        return _KernelStateResolver.GetKernelState(HandleCda(kernel2Session, rapdu));
    }

    #region S10.1, S10.5 - S10.6

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingL1Error(KernelSessionId sessionId, QueryPcdResponse signal)
    {
        if (!signal.IsLevel1ErrorPresent())
            return false;

        try
        {
            _Database.Update(DisplayMessageIdentifiers.TryAgain);
            _Database.Update(DisplayStatuses.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);
            _Database.Update(signal.GetLevel1Error());
            _Database.Update(StatusOutcomes.EndApplication);
            _Database.Update(StartOutcomes.B);
            _Database.Update(DisplayMessageOnErrorIdentifiers.TryAgain);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            return false;
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            return false;
        }
        finally
        {
            _EndpointClient.Send(new StopKernelRequest(sessionId));
        }
    }

    #endregion

    #region S10.7 - S10.9

    /// <remarks>Book C-2 Section S10.7 - S10.9</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CardDataException"></exception>
    private bool TryHandlingL2Error(Kernel2Session session, QueryPcdResponse signal)
    {
        if (!signal.IsLevel2ErrorPresent())
            return false;

        _ = _PrepareApplicationCryptogramService.Process(this, session, signal);

        return true;
    }

    #endregion

    #region S10.10 - S10.14

    /// <remarks>Book C-2 Section S10.10 - S10.14</remarks>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandlingBerParsingException(Kernel2Session session, RecoverAcResponse rapdu)
    {
        try
        {
            RemoveTornEntry(session);
            UpdateDatabaseWithRapdu(rapdu);

            return false;
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception

            HandleBerParsingError(session, rapdu);

            return true;
        }
        catch (IccProtocolException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingError(session, rapdu);

            return true;
        }
        catch (BerParsingException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingError(session, rapdu);

            return true;
        }
        catch (CodecParsingException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingError(session, rapdu);

            return true;
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingError(session, rapdu);

            return true;
        }
    }

    #endregion

    #region S10.10 - S10.12

    /// <remarks>Book C-2 Section S10.10 - S10.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void RemoveTornEntry(Kernel2Session session)
    {
        if (!session.TryGetTornEntry(out TornEntry? tornEntry))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForRecoverAcResponse)} could not retrieve the {nameof(TornEntry)} from the {nameof(Kernel2Session)}");
        }

        if (!_Database.TryGet(tornEntry, out TornRecord? tornRecord))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForRecoverAcResponse)} could not {nameof(RemoveTornEntry)} from the {nameof(TornTransactionLog)}");
        }

        _Database.Update(tornRecord!.GetDataObjects());
    }

    #endregion

    #region S10.13

    /// <remarks>Book C-2 Section S10.13</remarks>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void UpdateDatabaseWithRapdu(RecoverAcResponse rapdu)
    {
        _Database.Update(rapdu.GetPrimitiveDataObjects());
    }

    #endregion

    #region S10.14

    /// <remarks>Book C-2 Section S10.14</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleBerParsingError(Kernel2Session session, RecoverAcResponse rapdu)
    {
        _Database.Update(Level2Error.ParsingError);
        _S910.Process(this, session, rapdu);
    }

    #endregion

    #region S10.15 - S10.16

    /// <remarks>Book C-2 Section S10.15 - S10.16</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandlingMissingMandatoryData(Kernel2Session session, RecoverAcResponse rapdu)
    {
        if (_Database.IsPresentAndNotEmpty(ApplicationTransactionCounter.Tag))
        {
            HandleMissingMandatoryData(session, rapdu);

            return true;
        }

        if (_Database.IsPresentAndNotEmpty(CryptogramInformationData.Tag))
        {
            HandleMissingMandatoryData(session, rapdu);

            return true;
        }

        return false;
    }

    #endregion

    #region S10.16

    /// <remarks>Book C-2 Section S10.16</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleMissingMandatoryData(Kernel2Session session, RecoverAcResponse rapdu)
    {
        _Database.Update(Level2Error.CardDataMissing);
        _S910.Process(this, session, rapdu);
    }

    #endregion

    #region S10.17 - S10.18

    /// <remarks>Book C-2 Section S10.17 - S10.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandlingInvalidCryptogramInformationData(Kernel2Session session, RecoverAcResponse rapdu)
    {
        CryptogramInformationData cid = _Database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);

        if (cid.IsValid(_Database))
        {
            HandleInvalidCryptogramInformationData(session, rapdu);

            return true;
        }

        return false;
    }

    #endregion

    #region S10.18

    /// <remarks>Book C-2 Section S10.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleInvalidCryptogramInformationData(Kernel2Session session, RecoverAcResponse rapdu)
    {
        _Database.Update(Level2Error.CardDataError);
        _S910.Process(this, session, rapdu);
    }

    #endregion

    #region S10.19 - S10.22

    /// <remarks>Book C-2 Section S10.19 - S10.22</remarks>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private StateId HandleCda(Kernel2Session session, RecoverAcResponse rapdu)
    {
        _OfflineBalanceReader.Process(this, session, rapdu);

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.TagsToWriteAfterGenAc))
        {
            _Database.Update(DisplayMessageIdentifiers.ClearDisplay);
            _Database.Update(DisplayStatuses.CardReadSuccessful);
            _Database.Update(MessageHoldTime.MinimumValue);
            _EndpointClient.Send(new DisplayMessageRequest(_Database.GetUserInterfaceRequestData()));
        }

        return _S910.Process(this, session, rapdu);
    }

    #endregion

    #endregion
}