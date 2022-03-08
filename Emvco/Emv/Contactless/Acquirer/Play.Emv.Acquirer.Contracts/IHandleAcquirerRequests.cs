using Play.Emv.Acquirer.Contracts.SignalIn;
using Play.Emv.DataElements.Interchange.ValueTypes;

namespace Play.Emv.Acquirer.Contracts;

public interface IHandleAcquirerRequests
{
    public AcquirerMessageFactory GetMessageFactory(MessageTypeIndicator messageTypeIndicator);
    public void Request(AcquirerRequestSignal message);
}