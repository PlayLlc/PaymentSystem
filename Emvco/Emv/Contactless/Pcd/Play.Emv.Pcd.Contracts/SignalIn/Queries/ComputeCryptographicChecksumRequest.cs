using Play.Emv.Ber.DataElements;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ComputeCryptographicChecksumRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ComputeCryptographicChecksumRequest));

    #endregion

    #region Constructor

    private ComputeCryptographicChecksumRequest(
        TransactionSessionId transactionSessionId, ComputeCryptographicChecksumCApduSignal cApduSignal) : base(cApduSignal, MessageTypeId,
        transactionSessionId)
    { }

    #endregion

    #region Instance Members

    public static ComputeCryptographicChecksumRequest Create(TransactionSessionId sessionId, UnpredictableNumberDataObjectList udol) =>
        new(sessionId, ComputeCryptographicChecksumCApduSignal.Create(udol));

    #endregion
}