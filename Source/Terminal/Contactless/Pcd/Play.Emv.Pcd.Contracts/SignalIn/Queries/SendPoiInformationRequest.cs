using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SendPoiInformationRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(SendPoiInformationRequest));

    #endregion

    #region Instance Values

    private readonly CommandTemplate _CommandTemplate;

    #endregion

    #region Constructor

    private SendPoiInformationRequest(CApduSignal cApduSignal, TransactionSessionId transactionSessionId, CommandTemplate commandTemplate) : base(cApduSignal,
        MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = commandTemplate;
    }

    #endregion

    #region Instance Members

    public static SendPoiInformationRequest Create(TransactionSessionId transactionSessionId, CommandTemplate commandTemplate) =>
        new(SendPoiInformationCApduSignal.Create(commandTemplate), transactionSessionId, commandTemplate);

    public static SendPoiInformationRequest Create(TransactionSessionId transactionSessionId, DataObjectListResult dataObjectListResult) =>
        new(SendPoiInformationCApduSignal.Create(dataObjectListResult), transactionSessionId, dataObjectListResult.AsCommandTemplate());

    #endregion
}