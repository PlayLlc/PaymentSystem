using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record RecoverAcRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(PutDataRequest));

    #endregion

    #region Constructor

    private RecoverAcRequest(TransactionSessionId transactionSessionId, RecoverApplicationCryptogramCApduSignal cApduSignal) : base(
        cApduSignal, MessageTypeId, transactionSessionId)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="BerParsingException"></exception>
    public static RecoverAcRequest Create(TransactionSessionId sessionId, DataRecoveryDataObjectListRelatedData primitiveValue) =>
        new(sessionId, RecoverApplicationCryptogramCApduSignal.Create(primitiveValue.EncodeTagLengthValue()));

    #endregion
}