using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
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

    private GetProcessingOptionsRequest(CommandTemplate commandTemplate, CApduSignal cApduSignal, TransactionSessionId transactionSessionId) : base(cApduSignal,
        MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = commandTemplate;
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="dataObjectListResult"></param>
    /// <param name="cApduSignal"></param>
    /// <param name="transactionSessionId"></param>
    /// <exception cref="BerParsingException"></exception>
    private GetProcessingOptionsRequest(DataObjectListResult dataObjectListResult, CApduSignal cApduSignal, TransactionSessionId transactionSessionId) : base(
        cApduSignal, MessageTypeId, transactionSessionId)
    {
        _CommandTemplate = dataObjectListResult.AsCommandTemplate();
    }

    private GetProcessingOptionsRequest(CApduSignal cApduSignal, TransactionSessionId transactionSessionId) : base(cApduSignal, MessageTypeId,
        transactionSessionId)
    {
        _CommandTemplate = null;
    }

    #endregion

    #region Instance Members

    public static GetProcessingOptionsRequest Create(TransactionSessionId transactionSessionId) =>
        new(GetProcessingOptionsCApduSignal.Create(), transactionSessionId);

    /// <summary>
    ///     Create
    /// </summary>
    /// <param name="dataObjectListResult"></param>
    /// <param name="transactionSessionId"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public static GetProcessingOptionsRequest Create(DataObjectListResult dataObjectListResult, TransactionSessionId transactionSessionId)
    {
        CommandTemplate commandTemplate = dataObjectListResult.AsCommandTemplate();

        return new GetProcessingOptionsRequest(commandTemplate, GetProcessingOptionsCApduSignal.Create(commandTemplate), transactionSessionId);
    }

    public static GetProcessingOptionsRequest Create(CommandTemplate commandTemplate, TransactionSessionId transactionSessionId) =>
        new(commandTemplate, GetProcessingOptionsCApduSignal.Create(commandTemplate), transactionSessionId);

    #endregion
}