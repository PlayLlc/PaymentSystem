using Play.Icc.Messaging.Apdu;

namespace Play.Icc;

public interface IIntegratedCircuitCardReader
{
    #region Instance Members

    /// <summary>
    ///     CA(C-APDU) DataExchangeSignal. Send a C-APDU to the Card and return either an R-APDU or an error indication. The
    ///     parameter to the DataExchangeSignal
    ///     is the command to be sent to the Card
    /// </summary>
    public ApduResponse Transcieve(ApduCommand command);

    #endregion
}