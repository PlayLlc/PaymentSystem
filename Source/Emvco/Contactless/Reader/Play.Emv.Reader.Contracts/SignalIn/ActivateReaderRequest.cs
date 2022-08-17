using Play.Emv.Ber.DataElements;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Reader.Contracts.SignalIn;

public record ActivateReaderRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ActivateReaderRequest));
    public static readonly ChannelTypeId ChannelTypeId = ReaderChannel.Id;

    #endregion

    #region Instance Values

    public readonly Transaction Transaction;

    #endregion

    #region Constructor

    public ActivateReaderRequest(Transaction transaction) : base(MessageTypeId, ChannelTypeId)
    {
        Transaction = transaction;
    }

    #endregion
}