using System;

using Play.Ber.DataObjects;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPostGenAcBalance
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

        UpdateBalance(rapdu);

        throw new NotImplementedException();
    }

    /// <exception cref="NotImplementedException"></exception>
    private bool TryHandlingL1Error(KernelSessionId sessionId, GetDataResponse rapdu)
    {
        if (!rapdu.IsLevel1ErrorPresent())
            return false;

        // BUG: State 17 doesn't specify the next state that we need to transition into. Track this down and implement it here
        throw new NotImplementedException();
    }

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdateBalance(GetDataResponse rapdu)
    {
        if (!rapdu.TryGetPrimitiveValue(out PrimitiveValue? balanceReadAfterGenAcPrimitive))
            return;

        _Database.Update((BalanceReadAfterGenAc) balanceReadAfterGenAcPrimitive!);
    }
}