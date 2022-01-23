using Play.Emv.Outcomes;

namespace Play.Emv.Terminal;

public interface IProcessFinalOutcome
{
    public void Process(FinalOutcome finalOutcome);
}