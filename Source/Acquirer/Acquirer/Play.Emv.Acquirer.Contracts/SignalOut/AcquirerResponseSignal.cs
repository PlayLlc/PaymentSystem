using Play.Ber.DataObjects;
using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts.SignalOut;

public abstract record AcquirerResponseSignal : ResponseMessage
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = AcquirerChannel.Id;

    #endregion

    #region Instance Values

    public readonly TagLengthValue[] TagLengthValues;

    #endregion

    #region Constructor

    protected AcquirerResponseSignal(CorrelationId correlationId, MessageTypeId messageTypeId, TagLengthValue[] tagLengthValues) : base(correlationId,
        messageTypeId, ChannelTypeId)
    {
        TagLengthValues = tagLengthValues;
    }

    #endregion

    #region Instance Members

    public abstract MessageTypeIndicator GetMessageTypeIndicator();

    #endregion
}