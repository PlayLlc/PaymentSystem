using Play.Ber.DataObjects;
using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts.SignalIn;

public abstract record AcquirerRequestSignal : RequestMessage
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = AcquirerChannel.Id;

    #endregion

    #region Instance Values

    public readonly TagLengthValue[] TagLengthValues;

    #endregion

    #region Constructor

    protected AcquirerRequestSignal(MessageTypeId messageTypeId, TagLengthValue[] tagLengthValues) : base(ChannelTypeId, messageTypeId)
    {
        TagLengthValues = tagLengthValues;
    }

    #endregion

    #region Instance Members

    public abstract MessageTypeIndicator GetMessageTypeIndicator();

    #endregion
}