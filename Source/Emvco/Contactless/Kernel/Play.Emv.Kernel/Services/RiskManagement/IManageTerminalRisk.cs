
using Play.Emv.Ber;
using Play.Emv.Configuration;

namespace Play.Emv.Kernel.Services;

public interface IManageTerminalRisk
{
    #region Instance Members

    public void Process(ITlvReaderAndWriter database, TerminalRiskManagementConfiguration configuration);

    #endregion
}