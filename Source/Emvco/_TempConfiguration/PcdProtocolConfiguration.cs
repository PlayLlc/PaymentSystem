using Play.Emv.Ber.DataElements;

namespace _TempConfiguration;

public class PcdProtocolConfiguration
{
    #region Instance Values

    private readonly MerchantIdentifier _MerchantIdentifier;
    private readonly TerminalIdentification _TerminalIdentification;
    private readonly InterfaceDeviceSerialNumber _InterfaceDeviceSerialNumber;
    private readonly HoldTimeValue _HoldTimeValue;

    #endregion

    #region Constructor

    // Modulation Type - Type A, Type B (ICC)
    public PcdProtocolConfiguration(
        MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification, InterfaceDeviceSerialNumber interfaceDeviceSerialNumber,
        HoldTimeValue holdTimeValue)
    {
        _MerchantIdentifier = merchantIdentifier;
        _TerminalIdentification = terminalIdentification;
        _InterfaceDeviceSerialNumber = interfaceDeviceSerialNumber;
        _HoldTimeValue = holdTimeValue;
    }

    #endregion
}