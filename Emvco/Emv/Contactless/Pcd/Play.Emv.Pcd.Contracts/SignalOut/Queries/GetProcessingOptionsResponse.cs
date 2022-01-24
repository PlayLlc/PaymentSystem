using Play.Emv.Icc.GetProcessingOptions;
using Play.Emv.Sessions;
using Play.Emv.Templates.ResponseMessages;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GetProcessingOptionsResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(GetProcessingOptionsResponse));

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