using Play.Emv.Display.Contracts.SignalIn;

namespace Play.Emv.Display;

/// <summary>
///     a MSG DataExchangeSignal is used as a carrier of the User Interface Request Data.Process D may receive MSG Signals
///     from
///     any other Process.
///     • default language
///     • the currency symbol to display for each currency code and the number of minor units for that currency code
///     • a number of message strings in the default language and potentially other languages
///     • a number of status identifiers(and the corresponding audio and LED Signals)
/// </summary>
public interface IDisplayProcess
{
    #region Instance Members

    /// <summary>
    ///     a MSG DataExchangeSignal is used as a carrier of the User Interface Request Data.Process D may receive MSG Signals
    ///     from any other Process
    /// </summary>
    /// <param name="request"></param>
    public void Display(DisplayMessageRequest request);

    /// <summary>
    ///     The STOP Signal clears the display immediately and flushes all pending messages
    /// </summary>
    public void Stop(StopDisplayRequest request);

    #endregion
}