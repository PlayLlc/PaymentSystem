using Play.Emv.Reader.Contracts.SignalOut;

namespace Play.Emv.Reader.Services;

internal interface ISendReaderResponses : ISendReaderOutSignal
{
    internal void Send(QueryReaderResponse message);
    internal void Send(StopReaderAcknowledgedResponse message);
}