using System;

using Play.Ber.DataObjects;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPreGenAcBalance
{
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        GetDataResponse rapdu = (GetDataResponse) signal;

        if (TryHandlingL1Error(session.GetKernelSessionId(), rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandlingInvalidStatusBytes(rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        UpdateBalance(rapdu);

        // BUG: State 16 doesn't specify the next state that we need to transition into. Track this down and implement it here
        throw new NotImplementedException();
    }

    #region S16.1 - S16.3

    /// <remarks>Book C-2 Section S16.1 - S16.3</remarks>
    private bool TryHandlingL1Error(KernelSessionId sessionId, GetDataResponse rapdu)
    {
        if (!rapdu.IsLevel1ErrorPresent())
            return false;

        HandleL1Error(sessionId, rapdu);

        return true;
    }

    #endregion

    #region S16.3

    /// <remarks>Book C-2 Section S16.3</remarks>
    private void HandleL1Error(KernelSessionId sessionId, GetDataResponse rapdu)
    {
        try
        {
            _Database.Update(MessageIdentifiers.TryAgain);
            _Database.Update(Statuses.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);
            _Database.Update(StatusOutcomes.EndApplication);
            _Database.Update(StartOutcomes.B);
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

    #region S16.8

    /// <remarks>Book C-2 Section S16.8</remarks>
    /// <exception cref="NotImplementedException"></exception>
    private bool TryHandlingInvalidStatusBytes(GetDataResponse rapdu)
    {
        if (!rapdu.IsLevel1ErrorPresent())
            return false;

        // BUG: State 16 doesn't specify the next state that we need to transition into. Track this down and implement it here
        throw new NotImplementedException();
    }

    #endregion

    #region S16.9

    /// <remarks>Book C-2 Section S16.9</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdateBalance(GetDataResponse rapdu)
    {
        if (!rapdu.TryGetPrimitiveValue(out PrimitiveValue? balanceReadBeforeGenAcPrimitive))
            return;

        _Database.Update((BalanceReadBeforeGenAc) balanceReadBeforeGenAcPrimitive!);
    }

    #endregion
}