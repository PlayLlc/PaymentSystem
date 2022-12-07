namespace Play.Messaging;

internal class EnvelopeFactory
{
    #region Event Envelope

    #endregion

    #region Request Envelope

    public static RequestMessageEnvelope CreateRequestEnvelope(RequestMessage message) => new(CreateRequestMessageHeader(message.GetCorrelationId()), message);
    private static RequestMessageHeader CreateRequestMessageHeader(CorrelationId correlationId) => new(correlationId);

    #endregion

    #region Response Envelope

    public static ResponseMessageEnvelope CreateResponseEnvelope(ResponseMessage message) =>
        new(CreateResponseMessageHeader(message.GetCorrelationId()), message);

    private static ResponseMessageHeader CreateResponseMessageHeader(CorrelationId correlationId) => new(correlationId);

    #endregion
}