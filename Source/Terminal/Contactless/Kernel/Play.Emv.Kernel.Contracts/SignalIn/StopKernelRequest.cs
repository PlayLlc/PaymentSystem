﻿using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts;

public record StopKernelRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(StopKernelRequest));
    public static readonly ChannelTypeId ChannelTypeId = KernelChannel.Id;

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

    public KernelId GetKernelId() => _KernelSessionId.GetKernelId();
    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();

    #endregion
}