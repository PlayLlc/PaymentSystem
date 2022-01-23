using Play.Emv.DataExchange;
using Play.Messaging;

namespace Play.Emv.Terminal.Services._Temp_DataExchange;

public record TerminalQuerySession(CorrelationId CorrelationId, DataExchangeKernelId DataExchangeKernelId);