using Play.Emv.DataElements.Emv;

namespace Play.Emv.Security;

public interface IResolveAuthenticationType
{
    AuthenticationTypes GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities,
        ApplicationInterchangeProfile applicationInterchangeProfile);
}