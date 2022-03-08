using Play.Emv.DataElements.Emv.Enums;
using Play.Emv.DataElements.Emv.Primitives.Card.Icc;
using Play.Emv.DataElements.Emv.Primitives.Terminal;

namespace Play.Emv.Security;

public interface IResolveAuthenticationType
{
    AuthenticationTypes GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities,
        ApplicationInterchangeProfile applicationInterchangeProfile);
}