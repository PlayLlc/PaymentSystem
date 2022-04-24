using Play.Ber.DataObjects;
using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts.SignalOut;

public record ReconciliationResponse : AcquirerResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeIndicator MessageTypeIndicator = MessageTypeIndicatorTypes.Reconciliation.ReconciliationResponse;

    #endregion

    #region Constructor

    public ReconciliationResponse(CorrelationId correlationId, MessageTypeId messageTypeId, TagLengthValue[] tagLengthValues) : base(correlationId,
        messageTypeId, tagLengthValues)
    { }

    #endregion

    #region Instance Members

    public override MessageTypeIndicator GetMessageTypeIndicator() => MessageTypeIndicator;

    #endregion
}