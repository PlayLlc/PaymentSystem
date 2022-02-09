using System;

using Play.Emv.Exceptions;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public class WaitingForRecoverAcResponse : KernelState
{
    #region Static Metadata

    public static readonly KernelStateId KernelStateId = new(nameof(WaitingForRecoverAcResponse));

    #endregion

    #region Constructor

    public WaitingForRecoverAcResponse(KernelDatabase kernelDatabase, DataExchangeKernelService dataExchange) :
        base(kernelDatabase, dataExchange)
    { }

    #endregion

    public override KernelStateId GetKernelStateId() => KernelStateId;
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(CleanKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, StopKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal) => throw new NotImplementedException();
}