using Play.Emv.Reader.Contracts.SignalOut;

namespace Play.Emv.Reader;

public interface ISendReaderOutSignal
{
    #region Instance Members

    public void Send(OutReaderResponse message);

    #endregion
}