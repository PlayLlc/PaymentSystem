using Play.Ber.Identifiers;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

/// <summary>
///     The GET DATA command is used to retrieve a primitive data object not encapsulated in a record within the current
///     application. The usage of the GET DATA command in this specification is limited to the retrieval of the following
///     primitive data objects that are defined in Annex A and  interpreted by the application in the ICC:
///     • ATC(tag '9F36')
///     • Last Online ATC Register(tag '9F13')
///     • PIN Try Counter(tag '9F17')
///     • Log Format(tag '9F4F')
/// </summary>
public record GetDataRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GetDataRequest));

    #endregion

    #region Constructor

    private GetDataRequest(CApduSignal cApduSignal, MessageTypeId messageTypeId, TransactionSessionId transactionSessionId) :
        base(cApduSignal, messageTypeId, transactionSessionId)
    { }

    private GetDataRequest(TransactionSessionId transactionSessionId, CApduSignal cApduSignal) : base(cApduSignal, MessageTypeId,
     transactionSessionId)
    { }

    #endregion

    #region Instance Members

    public static GetDataRequest Create(Tag tag, TransactionSessionId sessionId) =>
        new(GetDataCApduSignal.Create(tag), MessageTypeId, sessionId);

    #endregion
}