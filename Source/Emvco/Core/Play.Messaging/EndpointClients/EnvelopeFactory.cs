namespace Play.Messaging;

internal class EnvelopeFactory
{
    #region Event Envelope

    #endregion

    #region Request Envelope

    public static RequestMessageEnvelope CreateRequestEnvelope(RequestMessage message, MessagingConfiguration messagingConfiguration) =>
        new(CreateRequestMessageHeader(message.GetCorrelationId(), messagingConfiguration), message);

    private static RequestMessageHeader CreateRequestMessageHeader(CorrelationId correlationId, MessagingConfiguration messagingConfiguration) =>
        new(correlationId, messagingConfiguration);

    #endregion

    #region Response Envelope

    public static ResponseMessageEnvelope CreateResponseEnvelope(ResponseMessage message, MessagingConfiguration configuration) =>
        new(CreateResponseMessageHeader(message.GetCorrelationId(), configuration), message);

    private static ResponseMessageHeader CreateResponseMessageHeader(CorrelationId correlationId, MessagingConfiguration configuration) =>
        new(correlationId, configuration);

    #endregion
}