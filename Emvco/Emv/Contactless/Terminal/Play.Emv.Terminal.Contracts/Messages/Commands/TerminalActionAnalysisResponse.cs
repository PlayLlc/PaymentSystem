using Play.Emv.Ber.Enums;
using Play.Emv.Icc;

namespace Play.Emv.Terminal.Contracts.Messages.Commands;

public class TerminalActionAnalysisResponse
{
    #region Instance Values

    private readonly CryptogramTypes _CryptogramTypes;

    #endregion

    #region Constructor

    public TerminalActionAnalysisResponse(CryptogramTypes cryptogramTypes)
    {
        _CryptogramTypes = cryptogramTypes;
    }

    #endregion

    #region Instance Members

    public CryptogramTypes GetCryptogramType() => _CryptogramTypes;

    #endregion
}