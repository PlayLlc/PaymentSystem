using Play.Emv.Reader.Contracts;

namespace Play.Emv.Reader;

public interface IReaderEndpoint : ISendReaderOutSignal, IHandleReaderRequests
{ }