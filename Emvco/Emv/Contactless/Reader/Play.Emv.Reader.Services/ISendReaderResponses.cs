using Play.Emv.Reader.Contracts.SignalOut;

namespace Play.Emv.Reader.Services;

internal interface ISendReaderResponses : ISendReaderOutSignal
{
    #region Instance Members

    internal void Send(QueryReaderResponse message);
    internal void Send(StopReaderAcknowledgedResponse message);

    #endregion
}