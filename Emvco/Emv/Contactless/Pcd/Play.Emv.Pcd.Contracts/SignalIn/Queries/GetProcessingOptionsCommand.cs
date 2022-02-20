using Play.Emv.Ber.DataObjects;
using Play.Emv.Icc;
using Play.Emv.Icc.GetProcessingOptions;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GetProcessingOptionsCommand : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GetProcessingOptionsCommand));

    #endregion

    #region Instance Values

    private readonly CommandTemplate? _CommandTemplate;

    #endregion

    #region Constructor

    private GetProcessingOptionsCommand(CommandTemplate commandTemplate, CApduSignal cApduSignal, TransactionSessionId transactionSessionId)
        : base(cApduSignal, MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = commandTemplate;
    }

    private GetProcessingOptionsCommand(
        DataObjectListResult dataObjectListResult,
        CApduSignal cApduSignal,
        TransactionSessionId transactionSessionId) : base(cApduSignal, MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = dataObjectListResult.AsCommandTemplate();
    }

    private GetProcessingOptionsCommand(CApduSignal cApduSignal, TransactionSessionId transactionSessionId) : base(cApduSignal,
        MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = null;
    }

    #endregion

    #region Instance Members

    public static GetProcessingOptionsCommand Create(TransactionSessionId transactionSessionId) =>
        new(GetProcessingOptionsCApduSignal.Create(), transactionSessionId);

    public static GetProcessingOptionsCommand Create(DataObjectListResult dataObjectListResult, TransactionSessionId transactionSessionId)
    {
        CommandTemplate commandTemplate = dataObjectListResult.AsCommandTemplate();

        return new GetProcessingOptionsCommand(commandTemplate, GetProcessingOptionsCApduSignal.Create(commandTemplate),
            transactionSessionId);
    }

    public static GetProcessingOptionsCommand Create(CommandTemplate commandTemplate, TransactionSessionId transactionSessionId) =>
        new(commandTemplate, GetProcessingOptionsCApduSignal.Create(commandTemplate), transactionSessionId);

    #endregion
}