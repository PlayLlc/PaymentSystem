using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;

namespace Play.Emv.Terminal.Common.Services.CardholderVerificationMethods.Pin;

// TODO: Book 3 Section 10.5.1 Offline PIN Processing
internal class OfflinePinProcessor
{
    #region Instance Values

    private readonly TerminalCapabilities _TerminalCapabilities;

    #endregion

    #region Constructor

    public OfflinePinProcessor(TerminalCapabilities terminalCapabilities)
    {
        _TerminalCapabilities = terminalCapabilities;
    }

    #endregion
}