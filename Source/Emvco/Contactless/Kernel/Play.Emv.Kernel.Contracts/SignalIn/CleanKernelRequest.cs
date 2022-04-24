﻿using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts;

/// <summary>
///     This message clears the log of <see cref="TornRecord" /> from previous transactions
/// </summary>
public record CleanKernelRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(CleanKernelRequest));
    public static readonly ChannelTypeId ChannelTypeId = KernelChannel.Id;

    #endregion

    #region Instance Values

    private readonly KernelSessionId _KernelSessionId;

    #endregion

    #region Constructor

    public CleanKernelRequest(KernelSessionId kernelSessionId) : base(MessageTypeId, ChannelTypeId)
    {
        _KernelSessionId = kernelSessionId;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();
    public ShortKernelIdTypes GetShortKernelId() => _KernelSessionId.GetKernelId();

    #endregion
}