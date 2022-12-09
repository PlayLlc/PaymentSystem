﻿using System;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForMagstripeFirstWriteFlag
{
    #region DET

    // BUG: Need to make sure you're properly implementing each DEK handler for each state
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        if (TryHandleTimeout(session))
            return _KernelStateResolver.GetKernelState(StateId);

        // S8.6
        UpdateDatabase(signal);

        // S8.7
        StopTimer(session);

        return _KernelStateResolver.GetKernelState(_S78.Process(this, (Kernel2Session) session, signal));
    }

    #region S8.1 - S8.2

    /// <remarks>Book C-2 Section S8.1 - S8.2</remarks>
    private bool TryHandleTimeout(KernelSession session)
    {
        try
        {
            // HACK: Move exception handling to a single exception handler
            if (!session.Timer.IsTimedOut())
                return false;

            _Database.Update(StatusOutcomes.EndApplication);
            _Database.Update(Level3Error.TimeOut);
            _Database.Initialize(DiscretionaryData.Tag);
            _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);
            _DataExchangeKernelService.Initialize(DekResponseType.DiscretionaryData);
            _DataExchangeKernelService.Enqueue(DekResponseType.DiscretionaryData, _Database.GetErrorIndication());

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        catch (OverflowException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        finally
        {
            _EndpointClient.Send(new StopKernelRequest(session.GetKernelSessionId()));
        }

        return false;
    }

    #endregion

    #region S8.6

    /// <exception cref="TerminalDataException"></exception>
    private void UpdateDatabase(QueryTerminalResponse signal)
    {
        _Database.Update(signal.GetDataToSend().AsPrimitiveValues());
    }

    #endregion

    #region S8.7

    /// <remarks>Book C-2 Section S8.7</remarks>
    private void StopTimer(KernelSession session) => session.Timer.Stop();

    #endregion

    #endregion
}