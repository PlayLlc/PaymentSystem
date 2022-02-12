using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Terminal;

public interface IPerformTerminalActionAnalysis
{
    #region Instance Members

    public void Process(TerminalActionAnalysisCommand command);

    #endregion
}