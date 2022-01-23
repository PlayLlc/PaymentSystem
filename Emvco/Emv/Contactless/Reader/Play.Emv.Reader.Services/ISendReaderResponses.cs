using Play.Emv.Reader.Contracts.SignalOut;

namespace Play.Emv.Reader.Services;

internal interface ISendReaderResponses : ISendReaderOutSignal
{
    public void Send(QueryReaderResponse message);
    public void Send(StopReaderAcknowledgedResponse message);
}