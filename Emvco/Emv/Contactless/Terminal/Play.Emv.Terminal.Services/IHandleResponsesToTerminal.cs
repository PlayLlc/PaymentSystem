﻿using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Reader.Contracts.SignalOut;

namespace Play.Emv.Terminal.Services;

internal interface IHandleResponsesToTerminal
{
    internal void Handle(OutReaderResponse message);
    internal void Handle(QueryKernelResponse message);
    internal void Handle(StopReaderAcknowledgedResponse message);
    internal void Handle(AcquirerResponseSignal message);
}