using System;
using System.Threading;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForCccResponse2
{
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        // S13.1 - S13.5
        if (TryHandlingL1Error(session.GetKernelSessionId(), signal))
            return _KernelStateResolver.GetKernelState(StateId);

        throw new NotImplementedException();
    }

    #region S13.1 - S13.5

    /// <remarks>Book C-2 Section S14.1 - S14.5</remarks>
    private bool TryHandlingL1Error(KernelSessionId sessionId, QueryPcdResponse rapdu)
    {
        if (@rapdu.IsLevel1ErrorPresent())
            return false;

        Sleep();

        // S13.3
        _Database.FailedMagstripeCounter.Increment();

        HandleL1Error(sessionId, rapdu);

        return true;
    }

    #endregion

    #region S14.2

    /// <remarks>Book C-2 Section S14.2</remarks>
    private void Sleep()
    {
        int exponent = _Database.FailedMagstripeCounter.GetSequenceValue();
        Milliseconds waitTime = new((uint) (Math.Pow(2, exponent) * 300));

        // HACK: This seems pretty hack-ish
        Thread.Sleep(waitTime);
    }

    #endregion

    #region S14.4 - S14.5

    /// <remarks>Book C-2 Section S14.4 - S14.5</remarks>
    private void HandleL1Error(KernelSessionId sessionId, QueryPcdResponse rapdu)
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
            _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);
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
}