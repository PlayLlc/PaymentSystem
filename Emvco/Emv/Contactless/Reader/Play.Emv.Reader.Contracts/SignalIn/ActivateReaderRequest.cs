using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Reader.Contracts.SignalIn;

public record ActivateReaderRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ActivateReaderRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Reader;

    #endregion

    #region Instance Values

    private readonly TagsToRead? _TagsToRead;
    private readonly DataToSend? _DataToSend;

    // Needs DataToSend to populate Kernel TLV DB - wait values from terminal
    private Transaction _Transaction;

    #endregion

    #region Constructor

    public ActivateReaderRequest(Transaction transaction) : base(MessageTypeId, ChannelTypeId)
    {
        _Transaction = transaction;
    }

    public ActivateReaderRequest(Transaction transaction, DataToSend? dataToSend) : base(MessageTypeId, ChannelTypeId)
    {
        _DataToSend = dataToSend;
        _Transaction = transaction;
    }

    public ActivateReaderRequest(Transaction transaction, TagsToRead? tagsToRead) : base(MessageTypeId, ChannelTypeId)
    {
        _Transaction = transaction;
        _TagsToRead = tagsToRead;
    }

    public ActivateReaderRequest(Transaction transaction, DataToSend? dataToSend, TagsToRead? tagsToRead) : base(MessageTypeId,
     ChannelTypeId)
    {
        _DataToSend = dataToSend;
        _Transaction = transaction;
        _TagsToRead = tagsToRead;
    }

    #endregion

    #region Instance Members

    public TagsToRead? GetTagsToRead() => _TagsToRead;
    public ref Transaction GetTransaction() => ref _Transaction;

    #endregion
}