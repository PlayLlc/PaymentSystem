using Play.Ber.DataObjects;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts.SignalIn;

public record AcquirerRequestSignal : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(AcquirerRequestSignal));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Acquirer;

    #endregion

    #region Instance Values

    public readonly MessageTypeIndicator MessageTypeIndicator;
    public readonly TagLengthValue[] TagLengthValues;

    #endregion

    #region Constructor

    public AcquirerRequestSignal(MessageTypeIndicator messageTypeIndicator, TagLengthValue[] tagLengthValues) : base(MessageTypeId,
        ChannelTypeId)
    {
        MessageTypeIndicator = messageTypeIndicator;
        TagLengthValues = tagLengthValues;
    }

    #endregion
}