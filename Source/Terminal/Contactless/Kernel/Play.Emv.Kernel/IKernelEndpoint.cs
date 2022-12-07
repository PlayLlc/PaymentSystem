namespace Play.Emv.Kernel;

public interface IKernelEndpoint : IHandleKernelStopRequests, ISendKernelOutSignal, ISendTerminalQueryResponse
{ }