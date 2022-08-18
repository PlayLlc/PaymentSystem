using Play.Ber.DataObjects;
using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts.SignalIn;

public record SettlementRequestSignal : AcquirerRequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(SettlementRequestSignal));
    public static readonly MessageTypeIndicator MessageTypeIndicator = MessageTypeIndicatorTypes.Reconciliation.ReconciliationRequest;

    #endregion

    #region Constructor

    public SettlementRequestSignal(TagLengthValue[] tagLengthValues) : base(MessageTypeId, tagLengthValues)
    { }

    #endregion

    #region Instance Members

    public override MessageTypeIndicator GetMessageTypeIndicator() => MessageTypeIndicator;

    #endregion
}