using Play.Emv.DataElements.Interchange;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts.SignalOut;

public record AcquirerResponseSignal : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(AcquirerResponseSignal));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Terminal;

    #endregion

    #region Instance Values

    public readonly MessageTypeIndicator MessageTypeIndicator;
    public readonly TagLengthValue[] TagLengthValues;

    #endregion

    #region Constructor

    protected AcquirerResponseSignal(
        CorrelationId correlationId,
        MessageTypeIndicator messageTypeIndicator,
        TagLengthValue[] tagLengthValues) : base(correlationId, MessageTypeId, ChannelTypeId)
    {
        MessageTypeIndicator = messageTypeIndicator;
        TagLengthValues = tagLengthValues;
    }

    #endregion
}