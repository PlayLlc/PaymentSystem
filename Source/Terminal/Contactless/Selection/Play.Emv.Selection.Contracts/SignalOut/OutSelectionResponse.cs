using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

namespace Play.Emv.Selection.Contracts;

public record OutSelectionResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(OutSelectionResponse));
    public static readonly ChannelTypeId ChannelTypeId = SelectionChannel.Id;

    #endregion

    #region Instance Values

    private readonly CombinationCompositeKey? _CombinationCompositeKey;
    private readonly SelectApplicationDefinitionFileInfoResponse? _ApplicationFileInformationResponse;
    private readonly Transaction _Transaction;

    #endregion

    #region Constructor

    public OutSelectionResponse(CorrelationId correlationId, Transaction transaction) : base(correlationId, MessageTypeId, ChannelTypeId)
    {
        _Transaction = transaction;
    }

    public OutSelectionResponse(
        CorrelationId correlationId, Transaction transaction, CombinationCompositeKey combinationCompositeKey,
        SelectApplicationDefinitionFileInfoResponse applicationFileInformationResponse) : base(correlationId, MessageTypeId, ChannelTypeId)
    {
        _Transaction = transaction;
        _ApplicationFileInformationResponse = applicationFileInformationResponse;
        _CombinationCompositeKey = combinationCompositeKey;
    }

    #endregion

    #region Instance Members

    public ErrorIndication GetErrorIndication() => _Transaction.GetOutcome().GetErrorIndication();
    public SelectApplicationDefinitionFileInfoResponse? GetApplicationFileInformationResponse() => _ApplicationFileInformationResponse;
    public KernelId? GetKernelId() => _CombinationCompositeKey?.GetKernelId() ?? ShortKernelIdTypes.Empty;
    public Transaction GetTransaction() => _Transaction;
    public CombinationCompositeKey? GetCombinationCompositeKey() => _CombinationCompositeKey;

    #endregion
}