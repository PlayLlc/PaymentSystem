﻿using Play.Emv.Messaging;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts.SignalIn;

public record StopKernelRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(StopKernelRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Kernel;

    #endregion

    #region Instance Values

    private readonly KernelSessionId _KernelSessionId;

    #endregion

    #region Constructor

    public StopKernelRequest(KernelSessionId kernelSessionId) : base(MessageTypeId, ChannelTypeId)
    {
        _KernelSessionId = kernelSessionId;
    }

    #endregion

    #region Instance Members

    public KernelSessionId GetKernelSessionId() => _KernelSessionId;

    #endregion
}