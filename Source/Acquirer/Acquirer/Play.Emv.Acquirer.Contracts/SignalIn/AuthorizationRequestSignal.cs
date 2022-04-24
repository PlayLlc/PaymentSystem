using Play.Ber.DataObjects;
using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts.SignalIn;

public record AuthorizationRequestSignal : AcquirerRequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(AuthorizationRequestSignal));

    #endregion

    #region Constructor

    public AuthorizationRequestSignal(TagLengthValue[] tagLengthValues) : base(MessageTypeId, tagLengthValues)
    { }

    #endregion

    #region Instance Members

    public override MessageTypeIndicator GetMessageTypeIndicator() => MessageTypeIndicatorTypes.Authorization.AuthorizationRequest;

    #endregion
}