using Play.Emv.Outcomes;

namespace Play.Emv.Terminal;

public interface IProcessFinalOutcome
{
    #region Instance Members

    public void Process(FinalOutcome finalOutcome);

    #endregion
}