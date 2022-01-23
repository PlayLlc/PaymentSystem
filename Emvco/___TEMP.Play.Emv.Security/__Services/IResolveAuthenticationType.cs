using ___TEMP.Play.Emv.Security.Authentications;

using Play.Emv.DataElements;

namespace ___TEMP.Play.Emv.Security.__Services;

public interface IResolveAuthenticationType
{
    AuthenticationType GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities,
        ApplicationInterchangeProfile applicationInterchangeProfile);
}