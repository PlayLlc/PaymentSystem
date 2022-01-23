namespace Play.Emv.Terminal;

public interface ITerminalServices
{
    public void Activate(ActivateTerminalCommand command);
}