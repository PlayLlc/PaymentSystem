using Play.Emv.DataElements;

namespace Play.Emv.Configuration;

public class PcdProtocolConfiguration
{
    #region Instance Values

    private readonly MerchantIdentifier _MerchantIdentifier;
    private readonly TerminalIdentification _TerminalIdentification;
    private readonly InterfaceDeviceSerialNumber _InterfaceDeviceSerialNumber;

    #endregion

    #region Constructor

    // Modulation Type - Type A, Type B (ICC)
    public PcdProtocolConfiguration(
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification,
        InterfaceDeviceSerialNumber interfaceDeviceSerialNumber)
    {
        _MerchantIdentifier = merchantIdentifier;
        _TerminalIdentification = terminalIdentification;
        _InterfaceDeviceSerialNumber = interfaceDeviceSerialNumber;
    }

    #endregion
}