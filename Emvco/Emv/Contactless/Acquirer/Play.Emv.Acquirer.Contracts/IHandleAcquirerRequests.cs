using Play.Emv.Acquirer.Contracts.SignalIn;
using Play.Emv.DataElements.Interchange;

namespace Play.Emv.Acquirer.Contracts;

public interface IHandleAcquirerRequests
{
    public AcquirerMessageFactory GetMessageFactory(MessageTypeIndicator messageTypeIndicator);
    public void Request(AcquirerRequestSignal message);
}