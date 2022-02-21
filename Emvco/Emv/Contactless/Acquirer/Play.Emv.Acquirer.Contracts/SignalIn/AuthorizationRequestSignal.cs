using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Acquirer.Contracts;
using Play.Ber.DataObjects;
using Play.Emv.Acquirer.Contracts;
using Play.Emv.Acquirer.Contracts.SignalIn;
using Play.Emv.Messaging;
using Play.Interchange.Messages.Header;
using Play.Messaging;

namespace Play.Emv.Acquirer.Messages;

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