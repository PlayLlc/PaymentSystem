using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.Enums.Interchange;
using Play.Messaging;

namespace Play.Emv.Acquirer.Contracts.SignalIn;

public record AuthorizationRequestSignal : AcquirerRequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(AuthorizationRequestSignal));

    public static readonly MessageTypeIndicator
        MessageTypeIndicator = (ushort) MessageTypeIndicatorTypes.Authorization.AuthorizationRequest;

    #endregion

    #region Constructor

    public AuthorizationRequestSignal(TagLengthValue[] tagLengthValues) : base(MessageTypeId, tagLengthValues)
    { }

    #endregion

    #region Instance Members

    public override MessageTypeIndicator GetMessageTypeIndicator() => MessageTypeIndicator;

    #endregion
}