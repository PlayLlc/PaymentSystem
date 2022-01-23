using ___TEMP.Play.Emv.Security.Authentications;

using Play.Emv.DataElements;

namespace Play.Emv.Security;

public interface IResolveAuthenticationType
{
    AuthenticationType GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities,
        ApplicationInterchangeProfile applicationInterchangeProfile);
}