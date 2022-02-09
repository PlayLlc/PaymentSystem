﻿using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts;

/// <summary>
///     ACT DataExchangeSignal. Generate an Answer to Reset, start polling for an PICC or HCE
/// </summary>
public record QueryKernelRequest : RequestSignal, IExchangeDataWithTheTerminal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(QueryKernelRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Kernel;

    #endregion

    #region Instance Values

    private readonly TagsToRead _TagsToRead;
    private readonly DataExchangeTerminalId _DataExchangeTerminalId;

    #endregion

    #region Constructor

    public QueryKernelRequest(DataExchangeTerminalId dataExchangeTerminalId, TagsToRead tagsToRead) : base(MessageTypeId, ChannelTypeId)
    {
        _TagsToRead = tagsToRead;
        _DataExchangeTerminalId = dataExchangeTerminalId;
    }

    #endregion

    #region Instance Members

    public DataExchangeTerminalId GetDataExchangeTerminalId() => _DataExchangeTerminalId;
    public TagsToRead GetTagsToRead() => _TagsToRead;
    public TransactionSessionId GetTransactionSessionId() => _DataExchangeTerminalId.GetTransactionSessionId();

    #endregion
}