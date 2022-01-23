﻿using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts.SignalIn;

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

    public DataExchangeTerminalId GetDataExchangeTerminalId()
    {
        return _DataExchangeTerminalId;
    }

    public TagsToRead GetTagsToRead()
    {
        return _TagsToRead;
    }

    #endregion
}