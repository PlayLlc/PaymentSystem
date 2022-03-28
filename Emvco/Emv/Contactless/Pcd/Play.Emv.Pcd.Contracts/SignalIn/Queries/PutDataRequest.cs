using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Icc.Exceptions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record PutDataRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(PutDataRequest));

    #endregion

    #region Constructor

    private PutDataRequest(TransactionSessionId transactionSessionId, PutDataCApduSignal cApduSignal) : base(cApduSignal, MessageTypeId,
     transactionSessionId)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     The only valid <see cref="Tag" /> values that are valid for this method are UnprotectedDataEnvelope1 -
    ///     UnprotectedDataEnvelope5. In other words, Tags: 0x9F75, 0x9F76, 0x9F77, 0x9F78, 0x9F79
    /// </summary>
    /// <returns></returns>
    /// <exception cref="IccProtocolException"></exception>
    public static PutDataRequest Create(TransactionSessionId sessionId, PrimitiveValue primitiveValue) =>
        new(sessionId, PutDataCApduSignal.Create(primitiveValue));

    #endregion
}