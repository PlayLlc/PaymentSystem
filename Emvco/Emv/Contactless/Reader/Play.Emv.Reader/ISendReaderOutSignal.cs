using Play.Emv.Reader.Contracts.SignalOut;

namespace Play.Emv.Reader;

public interface ISendReaderOutSignal
{
    public void Send(OutReaderResponse message);
}