using Play.Emv.DataElements;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Transactions;
using Play.Messaging;

namespace Play.Emv.Selection.Contracts.SignalOut;

public record OutSelectionResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(OutSelectionResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Selection;

    #endregion

    #region Instance Values

    private readonly CombinationCompositeKey _CombinationCompositeKey;
    private readonly SelectApplicationDefinitionFileInfoResponse _ApplicationFileInformationResponse;
    private readonly TerminalTransactionQualifiers _TerminalTransactionQualifiers;
    private readonly Transaction _Transaction;

    #endregion

    #region Constructor

    public OutSelectionResponse(
        CorrelationId correlationId,
        CombinationCompositeKey combinationCompositeKey,
        Transaction transaction,
        TerminalTransactionQualifiers terminalTransactionQualifiers,
        SelectApplicationDefinitionFileInfoResponse applicationFileInformationResponse) : base(correlationId, MessageTypeId, ChannelTypeId)
    {
        _Transaction = transaction;
        _TerminalTransactionQualifiers = terminalTransactionQualifiers;
        _ApplicationFileInformationResponse = applicationFileInformationResponse;
        _CombinationCompositeKey = combinationCompositeKey;
    }

    #endregion

    #region Instance Members

    //public CombinationCompositeKey GetCombinationCompositeKey() => _CombinationCompositeKey;
    //public DedicatedFileName GetApplicationId() => _ApplicationId;
    public SelectApplicationDefinitionFileInfoResponse GetApplicationFileInformationResponse() => _ApplicationFileInformationResponse;
    public KernelId GetKernelId() => _CombinationCompositeKey.GetKernelId();
    public Transaction GetTransaction() => _Transaction;
    public CombinationCompositeKey GetCombinationCompositeKey() => _CombinationCompositeKey;
    public TerminalTransactionQualifiers GetTerminalTransactionQualifiers() => _TerminalTransactionQualifiers;
    public TransactionSessionId GetTransactionSessionId() => _Transaction.GetTransactionSessionId();

    #endregion
}