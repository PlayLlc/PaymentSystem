using Play.Emv.Acquirer.Contracts.SignalIn;

namespace Play.Emv.Acquirer.Contracts;

public interface IHandleAcquirerRequests
{
    #region Instance Members

    public AcquirerMessageFactory GetMessageFactory(MessageTypeIndicator messageTypeIndicator);
    public void Request(AcquirerRequestSignal message);

    #endregion
}