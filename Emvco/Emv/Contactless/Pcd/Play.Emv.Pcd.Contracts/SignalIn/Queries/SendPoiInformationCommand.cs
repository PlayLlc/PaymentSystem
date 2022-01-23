using Play.Ber.Emv.DataObjects;
using Play.Emv.Sessions;
using Play.Icc.Emv;
using Play.Icc.Emv.SendPoiInformation;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SendPoiInformationCommand : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(SendPoiInformationCommand));

    #endregion

    #region Instance Values

    private readonly CommandTemplate _CommandTemplate;

    #endregion

    #region Constructor

    private SendPoiInformationCommand(CApduSignal cApduSignal, TransactionSessionId transactionSessionId, CommandTemplate commandTemplate) :
        base(cApduSignal, MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = commandTemplate;
    }

    #endregion

    #region Instance Members

    public static SendPoiInformationCommand Create(TransactionSessionId transactionSessionId, CommandTemplate commandTemplate) =>
        new(SendPoiInformationCApduSignal.Create(commandTemplate), transactionSessionId, commandTemplate);

    public static SendPoiInformationCommand Create(TransactionSessionId transactionSessionId, DataObjectListResult dataObjectListResult) =>
        new(SendPoiInformationCApduSignal.Create(dataObjectListResult), transactionSessionId, dataObjectListResult.AsCommandTemplate());

    #endregion
}