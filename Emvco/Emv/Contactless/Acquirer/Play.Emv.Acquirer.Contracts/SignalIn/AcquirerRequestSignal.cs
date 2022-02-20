using Play.Ber.DataObjects;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts.SignalIn;

public record AcquirerRequestSignal : RequestSignal
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Acquirer;

    #endregion

    #region Instance Values

    public readonly TagLengthValue[] TagLengthValues;

    #endregion

    #region Constructor

    public AcquirerRequestSignal(MessageTypeId messageTypeId, TagLengthValue[] tagLengthValues) : base(messageTypeId, ChannelTypeId)
    {
        TagLengthValues = tagLengthValues;
    }

    #endregion
}