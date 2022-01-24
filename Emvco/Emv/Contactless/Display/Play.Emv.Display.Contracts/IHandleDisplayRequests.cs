namespace Play.Emv.Display.Contracts;

public interface IHandleDisplayRequests
{
    /// <summary>
    ///     a MSG DataExchangeSignal is used as a carrier of the User Interface Request Data.Process D may receive MSG Signals
    ///     from any other Process
    /// </summary>
    public void Request(DisplayMessageRequest message);

    /// <summary>
    ///     The STOP Signal clears the display immediately and flushes all pending messages
    /// </summary>
    public void Request(StopDisplayRequest message);
}