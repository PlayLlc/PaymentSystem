using Play.Emv.Reader.Contracts.SignalIn;

namespace Play.Emv.Reader.Contracts;

public interface IHandleReaderRequests
{
    #region Instance Members

    public void Request(AbortReaderRequest message);
    public void Request(ActivateReaderRequest message);
    public void Request(QueryReaderRequest message);
    public void Request(StopReaderRequest message);
    public void Request(UpdateReaderRequest message);

    #endregion
}