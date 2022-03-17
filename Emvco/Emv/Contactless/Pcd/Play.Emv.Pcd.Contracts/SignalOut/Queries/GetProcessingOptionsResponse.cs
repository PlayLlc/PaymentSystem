using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Templates;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GetProcessingOptionsResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GetProcessingOptionsResponse));

    #endregion

    #region Constructor

    public GetProcessingOptionsResponse(
        CorrelationId correlation,
        TransactionSessionId transactionSessionId,
        GetProcessingOptionsRApduSignal response) : base(correlation, MessageTypeId, transactionSessionId, response)
    { }

    #endregion

    #region Instance Members

    public ProcessingOptions AsProcessingOptions() => ProcessingOptions.Decode(GetData());

    #endregion
}