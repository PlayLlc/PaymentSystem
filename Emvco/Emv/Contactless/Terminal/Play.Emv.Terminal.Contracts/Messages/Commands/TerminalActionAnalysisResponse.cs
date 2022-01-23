using Play.Emv.DataElements;

namespace Play.Emv.Terminal.Contracts.Messages.Commands;

public class TerminalActionAnalysisResponse
{
    #region Instance Values

    private readonly CryptogramType _CryptogramType;

    #endregion

    #region Constructor

    public TerminalActionAnalysisResponse(CryptogramType cryptogramType)
    {
        _CryptogramType = cryptogramType;
    }

    #endregion

    #region Instance Members

    public CryptogramType GetCryptogramType() => _CryptogramType;

    #endregion
}