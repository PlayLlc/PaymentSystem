using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Security;

public interface IResolveAuthenticationType
{
    AuthenticationTypes GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities,
        ApplicationInterchangeProfile applicationInterchangeProfile);
}