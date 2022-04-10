namespace Play.Emv.Terminal;

public interface ITerminalServices
{
    #region Instance Members

    public void Activate(ActivateTerminalCommand command);

    #endregion
}