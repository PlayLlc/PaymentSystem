using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

/*
 * =================================================================================
 * Control Flow Parts
 * =================================================================================
 *  Section A		With CDA                        Auth.WithCda.cs
 *  Section B 		Without CDA                     Auth.WithoutCda.cs
 *  Section C 		Invalid Response 1              Response.Invalid.cs
 *  Section D 		Invalid Response 2              Response.Invalid.cs
 *  Section E 		Valid Response                  Response.Valid.cs
 *  Section F		Invalid Response 1              Response.Invalid.cs
 * =================================================================================
 * Operations
 * =================================================================================
 *  Part 1 		    L1RSP                           WaitingForGenerateAcResponse2.cs
 *  Part 2		    - N/A
 *  Part 3 		    Balance Reading & Card Write    WaitingForGenerateAcResponse2.cs
 *  Part 4		    - N/A
 *  Part 5		    - N/A
 *  Part 6 		    Replay Attack                   Auth.WithCda.cs
 *  Part 7		    Route Response                  Auth.WithCda.cs
 *  Part 8 		    Man in the Middle               Auth.WithCda.cs
 *  Part 9		    Card State Validation           Auth.WithCda.cs
 *  Part 10		    Set RRP Results                 Auth.WithoutCda.cs
 *  Part 11		    Double Tap That Shit            Response.Valid.cs
 */

public partial class WaitingForGenerateAcResponse2
{
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        RecoverAcResponse rapdu = (RecoverAcResponse) signal;
        Kernel2Session kernel2Session = (Kernel2Session) session;

        if (TryHandlingL1Error(kernel2Session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        if (!TryHandlingL2Error(kernel2Session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        throw new NotImplementedException();
    }

    #region Part 1 - L1 Error

    #region S11.1, S11.11 - 13, S11.15 - S11.17

    /// <remarks>Book C-2 Section S11.1, S11.11 - 13, S11.15 - S11.17</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private bool TryHandlingL1Error(Kernel2Session session, RecoverAcResponse rapdu)
    {
        if (!rapdu.IsLevel1ErrorPresent())
            return false;

        if (!session.TryGetTornEntry(out TornEntry? tornEntry))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornEntry)} could not be retrieved from the {nameof(Kernel2Session)}");
        }

        HandleIdsReadFlagSet(tornEntry!);

        PrepareNewTornRecord();
        HandleLevel1Response(session.GetKernelSessionId(), rapdu);

        return true;
    }

    #endregion

    #region S11.11 - S11.12

    /// <remarks>Book C-2 Section S11.11 - S11.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleIdsReadFlagSet(TornEntry tornEntry)
    {
        if (!_TornTransactionLog.TryGet(tornEntry!, out TornRecord? tornTempRecord))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornRecord)} could not be retrieved from the {nameof(TornTransactionLog)}");
        }

        if (tornTempRecord!.TryGetRecordItem(IntegratedDataStorageStatus.Tag, out PrimitiveValue? idsStatus))
            _TornTransactionLog.Remove(_DataExchangeKernelService, tornEntry); // S11.12 

        if (!((IntegratedDataStorageStatus) idsStatus!).IsWriteSet())
            _TornTransactionLog.Remove(_DataExchangeKernelService, tornEntry); // S11.12
    }

    #endregion

    #region S11.13, S11.15

    /// <remarks>Book C-2 Section S11.13, S11.15</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private void PrepareNewTornRecord()
    {
        if (!_Database.TryGet(DataRecoveryDataObjectList.Tag, out DataRecoveryDataObjectList? drDol))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not  create a new {nameof(TornRecord)} because the  {nameof(DataRecoveryDataObjectList)} could not be retrieved from the {nameof(KernelDatabase)}");
        }

        DataRecoveryDataObjectListRelatedData drDolRelatedData = drDol!.AsRelatedData(_Database);

        _Database.Update(drDolRelatedData);

        _TornTransactionLog.Add(TornRecord.Create(_Database), _Database);
    }

    #endregion

    #region S11.16 - S11.17

    /// <remarks>Book C-2 Section S11.16 - S11.17</remarks>
    private void HandleLevel1Response(KernelSessionId sessionId, RecoverAcResponse rapdu)
    {
        try
        {
            _Database.Update(MessageIdentifiers.TryAgain);
            _Database.Update(Status.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(StartOutcome.B);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.Update(rapdu.GetLevel1Error());
            _Database.Update(MessageOnErrorIdentifiers.TryAgain);
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        finally
        {
            _KernelEndpoint.Request(new StopKernelRequest(sessionId));
        }
    }

    #endregion

    #endregion

    #region L2 Error

    #region S11.6 - S11.10

    private bool TryHandlingL2Error(Kernel2Session session, RecoverAcResponse rapdu)
    {
        RemoveTornEntryFrom(session); // S11.5

        // S11.6
        if (rapdu.IsLevel2ErrorPresent())
        {
            // S11.7
            HandleLStatusBytesError();

            return true;
        }

        if (TryHandlingBerParsingError(rapdu))
            return true;

        return false;
    }

    #endregion

    #region S11.5

    private void RemoveTornEntryFrom(Kernel2Session session)
    {
        if (!session.TryGetTornEntry(out TornEntry? tornEntry))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornEntry)} could not be retrieved from the {nameof(Kernel2Session)}");
        }

        _TornTransactionLog.Remove(_DataExchangeKernelService, tornEntry);
    }

    #endregion

    #region S11.6 - S11.7

    private void HandleLStatusBytesError()
    { }

    #endregion

    #region S8 - S10

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private bool TryHandlingBerParsingError(RecoverAcResponse rapdu)
    {
        try
        {
            _Database.Update(rapdu.GetPrimitiveDataObjects());
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        catch (BerParsingException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        catch (CodecParsingException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }

        throw new NotImplementedException();
    }

    #endregion

    #endregion

    #region S11.5

    #endregion
}