﻿using System;

using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal) => throw new NotImplementedException();

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
}