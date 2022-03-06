using Play.Emv.Ber.DataObjects;
using Play.Emv.Icc;
using Play.Emv.Icc.GetProcessingOptions;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GetProcessingOptionsRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GetProcessingOptionsRequest));

    #endregion

    #region Instance Values

    private readonly CommandTemplate? _CommandTemplate;

    #endregion

    #region Constructor

    private GetProcessingOptionsRequest(CommandTemplate commandTemplate, CApduSignal cApduSignal, TransactionSessionId transactionSessionId)
        : base(cApduSignal, MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = commandTemplate;
    }

    private GetProcessingOptionsRequest(
        DataObjectListResult dataObjectListResult,
        CApduSignal cApduSignal,
        TransactionSessionId transactionSessionId) : base(cApduSignal, MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = dataObjectListResult.AsCommandTemplate();
    }

    private GetProcessingOptionsRequest(CApduSignal cApduSignal, TransactionSessionId transactionSessionId) : base(cApduSignal,
        MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = null;
    }

    #endregion

    #region Instance Members

    public static GetProcessingOptionsRequest Create(TransactionSessionId transactionSessionId) =>
        new(GetProcessingOptionsCApduSignal.Create(), transactionSessionId);

    public static GetProcessingOptionsRequest Create(DataObjectListResult dataObjectListResult, TransactionSessionId transactionSessionId)
    {
        CommandTemplate commandTemplate = dataObjectListResult.AsCommandTemplate();

        return new GetProcessingOptionsRequest(commandTemplate, GetProcessingOptionsCApduSignal.Create(commandTemplate),
            transactionSessionId);
    }

    public static GetProcessingOptionsRequest Create(CommandTemplate commandTemplate, TransactionSessionId transactionSessionId) =>
        new(commandTemplate, GetProcessingOptionsCApduSignal.Create(commandTemplate), transactionSessionId);

    #endregion
}