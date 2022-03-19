using Play.Emv.Ber.DataElements;
using Play.Emv.Configuration;

namespace Play.Emv.Terminal.Configuration.ApplicationDependent;

public class TerminalActionAnalysisConfiguration
{
    #region Instance Values

    private readonly TerminalActionCodeDefault _TerminalActionCodeDefault;
    private readonly TerminalActionCodeDenial _TerminalActionCodeDenial;
    private readonly TerminalActionCodeOnline _TerminalActionCodeOnline;

    #endregion

    #region Constructor

    public TerminalActionAnalysisConfiguration(
        TerminalActionCodeDefault terminalActionCodeDefault,
        TerminalActionCodeDenial terminalActionCodeDenial,
        TerminalActionCodeOnline terminalActionCodeOnline)
    {
        _TerminalActionCodeDefault = terminalActionCodeDefault;
        _TerminalActionCodeDenial = terminalActionCodeDenial;
        _TerminalActionCodeOnline = terminalActionCodeOnline;
    }

    #endregion
}