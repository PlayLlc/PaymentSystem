using Play.Emv.DataElements;

namespace Play.Emv.Security;

public interface IResolveAuthenticationType
{
    AuthenticationType GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities,
        ApplicationInterchangeProfile applicationInterchangeProfile);
}