using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;

namespace Play.Emv.Security;

public interface IResolveAuthenticationType
{
    AuthenticationTypes GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities,
        ApplicationInterchangeProfile applicationInterchangeProfile);
}